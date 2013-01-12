Option Explicit On
Option Strict Off
Imports System.Math
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.Civil.Corridor
Imports Autodesk.Civil
Imports Autodesk.Civil.Alignment
Imports Autodesk.Civil.Profile
Imports Shape = Autodesk.Civil.Corridor.Shape
Imports DBTransactionManager = Autodesk.AutoCAD.DatabaseServices.TransactionManager


' **************************************************************************************************
'          名称: BasicLaneTransition
'
'          描述: Creates a simple cross-sectional representation of a corridor
'                lane composed of a single closed shape.  Attachment origin
'                is at top, most inside portion of lane.  The lane can
'                transition its width and cross-slope based on the position
'                supplied by an optional horizontal and vertical alignment.
'
' 逻辑名称: 名称        类型     是否可选   缺省值         描述
'                ---------------------------------------------------------------------------------------------------
'                TargetHA    路线          yes                  none             用于过度的水平方向的路线
'                TargetVA    纵断面        yes                  none             用于过度的垂直方向的纵断面
'
'    参数: 名称          类型     是否可选    缺省值          描述
'                ----------------------------------------------------------------------------------------------------
'                Side                    long                 yes                  Right               指定要将部件放置在哪一侧
'                Width                  double             yes                  12.0                车道宽度
'                Depth                  double             yes                  0.667              从建造面到底基的深度
'                Slope                  double            yes                  -0.02                车道的百分比坡度
'                TransitionType   long                yes                  2                       描述将路线和/或纵断面用作运行时逻辑指定时，部件的行为方式
' **************************************************************************************************

Public Class MyBasicLaneTransition
    Inherits SATemplate


    Private Enum TransitionTypes ' Transition types supported
        kHoldOffsetAndElevation = 0
        kHoldElevationChangeOffset = 1
        kHoldGradeChangeOffset = 2
        kHoldOffsetChangeElevation = 3
        kChangeOffsetAndElevation = 4
    End Enum

    ' --------------------------------------------------------------------------
    ' Default values for input parameters
    Private Const SideDefault = Utilities.Right
    Private Const LaneWidthDefault = 12.0#
    Private Const LaneDepthDefault = 0.667
    Private Const LaneSlopeDefault = -0.02    '0.25 inch per foot
    Private Const HoldOriginalPositionDefault = TransitionTypes.kHoldOffsetAndElevation

    '从SATemplate类派生出自定义部件类


    Protected Overrides Sub GetLogicalNamesImplement(ByVal corridorState As CorridorState)
        MyBase.GetLogicalNamesImplement(corridorState)


        ' Retrieve parameter buckets from the corridor state
        Dim oParamsLong As ParamLongCollection
        oParamsLong = corridorState.ParamsLong

        ' Add the logical names we use in this script
        Dim oParamLong As ParamLong
        oParamLong = oParamsLong.Add("TargetHA", ParamLogicalNameType.Alignment)
        oParamLong.DisplayName = "690"

        oParamLong = oParamsLong.Add("TargetVA", ParamLogicalNameType.Profile)
        oParamLong.DisplayName = "691"


    End Sub
    ' 重载GetInputParametersImplement子例程
    Protected Overrides Sub GetInputParametersImplement(ByVal corridorState As CorridorState)
        MyBase.GetInputParametersImplement(corridorState)
        ' '从传入的CorridorState对象中获取参数
        Dim oParamsLong As ParamLongCollection
        oParamsLong = corridorState.ParamsLong

        Dim oParamsDouble As ParamDoubleCollection
        oParamsDouble = corridorState.ParamsDouble

        ' 
        oParamsLong.Add(Utilities.Side, SideDefault)
        oParamsDouble.Add("Width", LaneWidthDefault)
        oParamsDouble.Add("Depth", LaneDepthDefault)
        oParamsDouble.Add("Slope", LaneSlopeDefault)
        oParamsLong.Add("TransitionType", HoldOriginalPositionDefault)
    End Sub

    ' 重载DrawImplement子例程

    Protected Overrides Sub DrawImplement(ByVal corridorState As CorridorState)


        ''从传入的CorridorState对象中获取参数
        Dim oParamsDouble As ParamDoubleCollection
        oParamsDouble = corridorState.ParamsDouble

        Dim oParamsLong As ParamLongCollection
        oParamsLong = corridorState.ParamsLong

        Dim oParamsAlignment As ParamAlignmentCollection
        oParamsAlignment = corridorState.ParamsAlignment

        Dim oParamsProfile As ParamProfileCollection
        oParamsProfile = corridorState.ParamsProfile

        '---------------------------------------------------------
        ' 判断左侧还是右侧（flip参数）
        Dim vSide As Long
        Try
            vSide = oParamsLong.Value(Utilities.Side)
        Catch
            vSide = SideDefault
        End Try

        Dim dFlip As Double
        dFlip = 1.0#
        If vSide = Utilities.Left Then
            dFlip = -1.0#
        End If

        '---------------------------------------------------------
        ' 车道过度类型
        Dim vTransitionType As Long
        Try
            vTransitionType = oParamsLong.Value("TransitionType")
        Catch
            vTransitionType = HoldOriginalPositionDefault
        End Try

        '---------------------------------------------------------
        '基本车道过度参数 

        Dim vWidth As Double
        Try
            vWidth = oParamsDouble.Value("Width")
        Catch
            vWidth = LaneWidthDefault
        End Try



        Dim vDepth As Double
        Try
            vDepth = oParamsDouble.Value("Depth")
        Catch
            vDepth = LaneDepthDefault
        End Try



        Dim vSlope As Double
        Try
            vSlope = oParamsDouble.Value("Slope")
        Catch
            vSlope = LaneSlopeDefault
        End Try

        '-------------------------------------------------------
        ' 版本判断，如果可能做必要的测试
        Dim sVersion As String
        sVersion = Utilities.GetVersion(corridorState)
        If sVersion <> Utilities.R2005 Then
            '不需要改变
        Else
            'R2005
            '转换坡度 %slope为正切值
            vSlope = vSlope / 100
        End If

        '---------------------------------------------------------
        ' 检查用户输入
        If vWidth <= 0 Then
            Utilities.RecordError(corridorState, CorridorError.ValueShouldNotBeLessThanOrEqualToZero, "Width", "BasicLaneTransition")
            vWidth = LaneWidthDefault
        End If

        If vDepth <= 0 Then
            Utilities.RecordError(corridorState, CorridorError.ValueShouldNotBeLessThanOrEqualToZero, "Depth", "BasicLaneTransition")
            vDepth = LaneDepthDefault
        End If


        '计算当前的路线和装配偏移
        Dim oCurrentAlignmentId As ObjectId
        Dim oOrigin As New PointInMem

        Utilities.GetAlignmentAndOrigin(corridorState, oCurrentAlignmentId, oOrigin)

        '---------------------------------------------------------
        ' 定义点、链接和造型的编码
        Dim sPointCodeArray(0 To 4, 0) As String
        Dim sLinkCodeArray(0 To 2, 0 To 1) As String
        Dim sShapeCodeArray(0 To 1) As String

        FillCodesFromTable(sPointCodeArray, sLinkCodeArray, sShapeCodeArray)

        '---------------------------------------------------------
        ' 获取操作所需的路线和纵剖面

        Dim oHAId As ObjectId '路线对象 Id
        Dim oVAId As ObjectId '纵剖面对象 Id

        Dim dOffsetToTargetHA As Double
        Dim dOffsetElev As Double

        If corridorState.Mode = CorridorMode.Layout Then
            vTransitionType = TransitionTypes.kHoldOffsetAndElevation
        End If

        Dim dTempStation As Double
        Dim dTempOffset As Double
        Dim oProfileAlignmentId As ObjectId

        Select Case vTransitionType
            Case TransitionTypes.kHoldOffsetAndElevation

            Case TransitionTypes.kHoldElevationChangeOffset
                '
                Try
                    oHAId = oParamsAlignment.Value("TargetHA")
                Catch
                    Utilities.RecordError(corridorState, CorridorError.ParameterNotFound, "Edge Offset", "BasicLaneTransition")
                    'Exit Sub
                End Try
                '获取到 targetHA的偏移量
                If False = Utilities.CalcAlignmentOffsetToThisAlignment(oCurrentAlignmentId, corridorState.CurrentStation, oHAId, Utilities.GetSide(vSide), dOffsetToTargetHA, dTempStation) Then
                    Utilities.RecordWarning(corridorState, CorridorError.LogicalNameNotFound, "TargetHA", "BasicLaneTransition")
                    dOffsetToTargetHA = vWidth + oOrigin.Offset
                    dTempStation = corridorState.CurrentStation
                End If


            Case TransitionTypes.kHoldGradeChangeOffset
                'oHA must exist
                Try
                    oHAId = oParamsAlignment.Value("TargetHA")
                Catch
                    Utilities.RecordError(corridorState, CorridorError.ParameterNotFound, "Edge Offset", "BasicLaneTransition")
                    'Exit Sub
                End Try
                '获取到 targetHA的偏移量
                If False = Utilities.CalcAlignmentOffsetToThisAlignment(oCurrentAlignmentId, corridorState.CurrentStation, oHAId, Utilities.GetSide(vSide), dOffsetToTargetHA, dTempStation) Then
                    Utilities.RecordWarning(corridorState, CorridorError.LogicalNameNotFound, "TargetHA", "BasicLaneTransition")
                    dOffsetToTargetHA = vWidth + oOrigin.Offset
                    dTempStation = corridorState.CurrentStation
                End If

            Case TransitionTypes.kHoldOffsetChangeElevation
                'oVA must exist
                Try
                    oVAId = oParamsProfile.Value("TargetVA")
                Catch
                    Utilities.RecordError(corridorState, CorridorError.ParameterNotFound, "Edge Elevation", "BasicLaneTransition")
                    'Exit Sub
                End Try

                Dim db As Database = HostApplicationServices.WorkingDatabase
                Dim tm As DBTransactionManager = db.TransactionManager
                Dim oProfile As Profile = Nothing

                '获取纵断面上的高程
                Try
                    oProfile = tm.GetObject(oVAId, OpenMode.ForRead, False, False)
                    oProfileAlignmentId = oProfile.AlignmentId
                Catch
                End Try

                If False = Utilities.CalcAlignmentOffsetToThisAlignment(oCurrentAlignmentId, corridorState.CurrentStation, oProfileAlignmentId, Utilities.GetSide(vSide), dTempOffset, dTempStation) Then
                    Utilities.RecordWarning(corridorState, CorridorError.LogicalNameNotFound, "TargetHA", "BasicLaneTransition")
                    dOffsetElev = corridorState.CurrentElevation + vWidth * vSlope
                Else
                    dOffsetElev = oProfile.ElevationAt(dTempStation)
                End If


            Case TransitionTypes.kChangeOffsetAndElevation
                ' oHA 和 oVA 必须存在
                Try
                    oHAId = oParamsAlignment.Value("TargetHA")
                Catch
                    Utilities.RecordError(corridorState, CorridorError.ParameterNotFound, "Edge Offset", "BasicLaneTransition")
                    'Exit Sub
                End Try

                Try
                    oVAId = oParamsProfile.Value("TargetVA")
                Catch
                    Utilities.RecordError(corridorState, CorridorError.ParameterNotFound, "Edge Elevation", "BasicLaneTransition")
                    'Exit Sub
                End Try


                '获取纵断面上的高程
                Dim db As Database = HostApplicationServices.WorkingDatabase
                Dim tm As DBTransactionManager = db.TransactionManager
                Dim oProfile As Profile = Nothing

                Try
                    oProfile = tm.GetObject(oVAId, OpenMode.ForRead, False, False)
                    oProfileAlignmentId = oProfile.AlignmentId
                Catch
                End Try

                If False = Utilities.CalcAlignmentOffsetToThisAlignment(oCurrentAlignmentId, corridorState.CurrentStation, oProfileAlignmentId, Utilities.GetSide(vSide), dTempOffset, dTempStation) Then
                    Utilities.RecordWarning(corridorState, CorridorError.LogicalNameNotFound, "TargetHA", "BasicLaneTransition")
                    dOffsetElev = corridorState.CurrentElevation + vWidth * vSlope
                Else
                    dOffsetElev = oProfile.ElevationAt(dTempStation)
                End If

                '获取到 targetHA的偏移量
                If False = Utilities.CalcAlignmentOffsetToThisAlignment(oCurrentAlignmentId, corridorState.CurrentStation, oHAId, Utilities.GetSide(vSide), dOffsetToTargetHA, dTempStation) Then
                    Utilities.RecordWarning(corridorState, CorridorError.LogicalNameNotFound, "TargetHA", "BasicLaneTransition")
                    dOffsetToTargetHA = vWidth + oOrigin.Offset
                End If
        End Select

        '---------------------------------------------------------
        ' 创建部件点
        Dim corridorPoints As PointCollection
        corridorPoints = corridorState.Points

        Dim dX As Double
        Dim dy As Double

        dX = 0.0#
        dy = 0.0#
        Dim oPoint1 As Point
        oPoint1 = corridorPoints.Add(dX, dy, "")

        ' 计算车道的外边位置 
        Select Case vTransitionType

            Case TransitionTypes.kHoldOffsetAndElevation

                ' 
                dX = vWidth
                dy = Abs(vWidth) * vSlope

            Case TransitionTypes.kHoldElevationChangeOffset

                ' 
                'dX = Abs(dOffsetToTargetHA - corridorState.CurrentSubassemblyOffset)
                dX = Abs(dOffsetToTargetHA - oOrigin.Offset)
                dy = Abs(vWidth) * vSlope

            Case TransitionTypes.kHoldGradeChangeOffset

                dX = Abs(dOffsetToTargetHA - oOrigin.Offset)
                dy = Abs(dX) * vSlope

            Case TransitionTypes.kHoldOffsetChangeElevation

                ' 
                dX = vWidth
                'dY = dOffsetElev - corridorState.CurrentSubassemblyElevation
                dy = dOffsetElev - oOrigin.Elevation

            Case TransitionTypes.kChangeOffsetAndElevation

                ' 
                dX = Abs(dOffsetToTargetHA - oOrigin.Offset)
                dy = dOffsetElev - oOrigin.Elevation

        End Select

        Dim oPoint2 As Point
        oPoint2 = corridorPoints.Add(dX * dFlip, dy, "")

        dX = dX - 0.001
        dy = dy - vDepth
        Dim oPoint3 As Point
        oPoint3 = corridorPoints.Add(dX * dFlip, dy, "")

        dX = 0.0#
        dy = -vDepth
        Dim oPoint4 As Point
        oPoint4 = corridorPoints.Add(dX, dy, "")

        Utilities.AddCodeToPoint(1, corridorPoints, oPoint1.Index, sPointCodeArray)
        Utilities.AddCodeToPoint(2, corridorPoints, oPoint2.Index, sPointCodeArray)
        Utilities.AddCodeToPoint(3, corridorPoints, oPoint3.Index, sPointCodeArray)
        Utilities.AddCodeToPoint(4, corridorPoints, oPoint4.Index, sPointCodeArray)


        '---------------------------------------------------------
        ' 创建创配链接

        Dim oCorridorLinks As LinkCollection
        oCorridorLinks = corridorState.Links

        Dim oPoint(1) As Point
        Dim oLink(3) As Link

        oPoint(0) = oPoint1
        oPoint(1) = oPoint2
        oLink(0) = oCorridorLinks.Add(oPoint, "") 'L1

        oPoint(0) = oPoint2
        oPoint(1) = oPoint3
        oLink(1) = oCorridorLinks.Add(oPoint, "") 'L2

        oPoint(0) = oPoint3
        oPoint(1) = oPoint4
        oLink(2) = oCorridorLinks.Add(oPoint, "") 'L3

        oPoint(0) = oPoint4
        oPoint(1) = oPoint1
        oLink(3) = oCorridorLinks.Add(oPoint, "") 'L4

        Utilities.AddCodeToLink(1, oCorridorLinks, oLink(0).Index, sLinkCodeArray)
        Utilities.AddCodeToLink(2, oCorridorLinks, oLink(2).Index, sLinkCodeArray)

        '---------------------------------------------------------
        ' 创建装配造型

        Dim corridorShapes As ShapeCollection
        corridorShapes = corridorState.Shapes


        corridorShapes.Add(oLink, sShapeCodeArray(1))

    End Sub

    Protected Sub FillCodesFromTable(ByVal sPointCodeArray(,) As String, ByVal sLinkCodeArray(,) As String, ByVal sShapeCodeArray() As String)
        sPointCodeArray(1, 0) = Codes.Crown.Code
        sPointCodeArray(2, 0) = Codes.ETW.Code
        sPointCodeArray(3, 0) = Codes.ETWSubBase.Code 'P4
        sPointCodeArray(4, 0) = Codes.CrownSubBase.Code 'P3

        sLinkCodeArray(1, 0) = Codes.Top.Code
        sLinkCodeArray(1, 1) = Codes.Pave.Code
        sLinkCodeArray(2, 0) = Codes.Datum.Code
        sLinkCodeArray(2, 1) = Codes.SubBase.Code

        sShapeCodeArray(1) = Codes.Pave1.Code
    End Sub
End Class
