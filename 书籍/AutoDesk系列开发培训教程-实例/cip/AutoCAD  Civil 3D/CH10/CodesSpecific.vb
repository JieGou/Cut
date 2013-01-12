Module CodesSpecific

    'in case you change the relative path of the codes file, please modify here
    Private Const constCodesFile = "C3DStockSubassemblyScripts.codes"

    'default values, in case the codes file not found
    Dim CodesDefault(0 To 148) As String

    Public Structure CodeType
        Public Code As String
        Public Index As Integer
        Public Description As String
    End Structure
    Public Structure AllCodes
        Public CodesStructureFilled As Boolean 'it will tell me if this content was garbage collected or not
        Public Crown As CodeType
        Public CrownPave1 As CodeType
        Public CrownPave2 As CodeType
        Public CrownBase As CodeType
        Public CrownSub As CodeType
        Public ETW As CodeType
        Public ETWPave1 As CodeType
        Public ETWPave2 As CodeType
        Public ETWBase As CodeType
        Public ETWSub As CodeType
        Public Lane As CodeType
        Public LanePave1 As CodeType
        Public LanePave2 As CodeType
        Public LaneBase As CodeType
        Public LaneSub As CodeType
        Public EPS As CodeType
        Public EPSPave1 As CodeType
        Public EPSPave2 As CodeType
        Public EPSBase As CodeType
        Public EPSSub As CodeType
        Public EPSBaseIn As CodeType
        Public EPSSubIn As CodeType
        Public ESUnpaved As CodeType
        Public DaylightSub As CodeType
        Public Daylight As CodeType
        Public DaylightFill As CodeType
        Public DaylightCut As CodeType
        Public DitchIn As CodeType
        Public DitchOut As CodeType
        Public BenchIn As CodeType
        Public BenchOut As CodeType
        Public FlowlineDitch As CodeType
        Public LMedDitch As CodeType
        Public RMedDitch As CodeType
        Public Flange As CodeType
        Public Flowline_Gutter As CodeType
        Public TopCurb As CodeType
        Public BottomCurb As CodeType
        Public BackCurb As CodeType
        Public SidewalkIn As CodeType
        Public SideWalkOut As CodeType
        Public HingeCut As CodeType
        Public HingeFill As CodeType
        Public Top As CodeType
        Public Datum As CodeType
        Public Pave As CodeType
        Public Pave1 As CodeType
        Public Pave2 As CodeType
        Public Base As CodeType
        Public SubBase As CodeType
        Public Gravel As CodeType
        Public TopCurbNew As CodeType
        Public BackCurbNew As CodeType
        Public Curb As CodeType
        Public Sidewalk As CodeType
        Public Hinge As CodeType
        Public EOV As CodeType
        Public EOVOverlay As CodeType
        Public Level As CodeType
        Public Mill As CodeType
        Public Overlay As CodeType
        Public CrownOverlay As CodeType
        Public Barrier As CodeType
        Public EBD As CodeType
        Public CrownDeck As CodeType
        Public Deck As CodeType
        Public Girder As CodeType
        'add for RailSingle
        Public EBS As CodeType
        Public ESL As CodeType
        Public DaylightBallast As CodeType
        Public ESBS As CodeType
        Public DaylightSubballast As CodeType
        Public Ballast As CodeType
        Public Sleeper As CodeType
        Public Subballast As CodeType
        Public Rail As CodeType
        Public R1 As CodeType
        Public R2 As CodeType
        Public R3 As CodeType
        Public R4 As CodeType
        Public R5 As CodeType
        Public R6 As CodeType
        Public Bridge As CodeType
        Public Ditch As CodeType
        Public CrownFin As CodeType
        Public CrownSubBase As CodeType
        Public ETWSubBase As CodeType
        Public MarkedPoint As CodeType
        Public Guardrail As CodeType
        Public Median As CodeType
        Public ETWOverlay As CodeType
        Public TrenchBottom As CodeType
        Public TrenchDaylight As CodeType
        Public TrenchBedding As CodeType
        Public TrenchBackfill As CodeType
        Public Trench As CodeType
        Public LaneBreak As CodeType
        Public LaneBreakOverlay As CodeType
        Public Sod As CodeType
        Public DaylightStrip As CodeType
        Public sForeslopeStripping As CodeType
        Public Stripping As CodeType
        Public ChannelFlowline As CodeType
        Public Channe_Bottom As CodeType
        Public ChannelTop As CodeType
        Public ChannelExtension As CodeType
        Public ChannelBackslope As CodeType
        Public LiningMaterial As CodeType
        Public DitchBack As CodeType
        Public DitchFace As CodeType
        Public DitchTop As CodeType
        Public DitchBottom As CodeType
        Public Backfill As CodeType
        Public BackfillFace As CodeType
        Public DitchLidFace As CodeType
        Public LidTop As CodeType
        Public DitchBackFill As CodeType
        Public Lid As CodeType
        Public DrainBottom As CodeType
        Public DrainBottomOutside As CodeType
        Public DrainTopOutside As CodeType
        Public DrainTopInside As CodeType
        Public DrainBottomInside As CodeType
        Public DrainCenter As CodeType
        Public FlowLine As CodeType
        Public DrainTop As CodeType
        Public DrainStructure As CodeType
        Public DrainArea As CodeType
        Public RWFront As CodeType
        Public RWTop As CodeType
        Public RWBack As CodeType
        Public RWHinge As CodeType
        Public RWInside As CodeType
        Public RWOutside As CodeType
        Public Wall As CodeType
        Public RWall As CodeType
        Public RWallB1 As CodeType
        Public RWallB2 As CodeType
        Public RWallB3 As CodeType
        Public RWallB4 As CodeType
        Public RWallK1 As CodeType
        Public RWallK2 As CodeType
        Public FootingBottom As CodeType
        Public WalkEdge As CodeType
        Public Lot As CodeType
        Public Slope_Link As CodeType
        Public Channel_Side As CodeType
        Public Bench As CodeType
    End Structure

    Public Codes As AllCodes

    Private Sub InitializeDefaults()
        CodesDefault(1) = "Crown"
        CodesDefault(2) = "Crown_Pave1"
        CodesDefault(3) = "Crown_Pave2"
        CodesDefault(4) = "Crown_Base"
        CodesDefault(5) = "Crown_Sub"
        CodesDefault(6) = "ETW"
        CodesDefault(7) = "ETW_Pave1"
        CodesDefault(8) = "ETW_Pave2"
        CodesDefault(9) = "ETW_Base"
        CodesDefault(10) = "ETW_Sub"
        CodesDefault(11) = "Lane"
        CodesDefault(12) = "Lane_Pave1"
        CodesDefault(13) = "Lane_Pave2"
        CodesDefault(14) = "Lane_Base"
        CodesDefault(15) = "Lane_Sub"
        CodesDefault(16) = "EPS"
        CodesDefault(17) = "EPS_Pave1"
        CodesDefault(18) = "EPS_Pave2"
        CodesDefault(19) = "EPS_Base"
        CodesDefault(20) = "EPS_Sub"
        CodesDefault(21) = "EPS_Base_In"
        CodesDefault(22) = "EPS_Sub_In"
        CodesDefault(23) = "ES_Unpaved"
        CodesDefault(24) = "Daylight_Sub"
        CodesDefault(25) = "Daylight"
        CodesDefault(26) = "Daylight_Fill"
        CodesDefault(27) = "Daylight_Cut"
        CodesDefault(28) = "Ditch_In"
        CodesDefault(29) = "Ditch_Out"
        CodesDefault(30) = "Bench_In"
        CodesDefault(31) = "Bench_Out"
        CodesDefault(32) = "Flowline_Ditch"
        CodesDefault(33) = "LMedDitch"
        CodesDefault(34) = "RMedDitch"
        CodesDefault(35) = "Flange"
        CodesDefault(36) = "Flowline_Gutter"
        CodesDefault(37) = "Top_Curb"
        CodesDefault(38) = "Bottom_Curb"
        CodesDefault(39) = "Back_Curb"
        CodesDefault(40) = "Sidewalk_In"
        CodesDefault(41) = "SideWalk_Out"
        CodesDefault(42) = "Hinge_Cut"
        CodesDefault(43) = "Hinge_Fill"
        CodesDefault(44) = "Top"
        CodesDefault(45) = "Datum"
        CodesDefault(46) = "Pave"
        CodesDefault(47) = "Pave1"
        CodesDefault(48) = "Pave2"
        CodesDefault(49) = "Base"
        CodesDefault(50) = "SubBase"
        CodesDefault(51) = "Gravel"
        CodesDefault(52) = "Top_Curb"
        CodesDefault(53) = "Back_Curb"
        CodesDefault(54) = "Curb"
        CodesDefault(55) = "Sidewalk"
        CodesDefault(56) = "Hinge"
        CodesDefault(57) = "EOV"
        CodesDefault(58) = "EOV_Overlay"
        CodesDefault(59) = "Level"
        CodesDefault(60) = "Mill"
        CodesDefault(61) = "Overlay"
        CodesDefault(62) = "Crown_Overlay"
        CodesDefault(63) = "Barrier"
        CodesDefault(64) = "EBD"
        CodesDefault(65) = "Crown_Deck"
        CodesDefault(66) = "Deck"
        CodesDefault(67) = "Girder"
        CodesDefault(68) = "EBS"
        CodesDefault(69) = "ESL"
        CodesDefault(70) = "Daylight_Ballast"
        CodesDefault(71) = "ESBS"
        CodesDefault(72) = "Daylight_Subballast"
        CodesDefault(73) = "Ballast"
        CodesDefault(74) = "Sleeper"
        CodesDefault(75) = "Subballast"
        CodesDefault(76) = "Rail"
        CodesDefault(77) = "R1"
        CodesDefault(78) = "R2"
        CodesDefault(79) = "R3"
        CodesDefault(80) = "R4"
        CodesDefault(81) = "R5"
        CodesDefault(82) = "R6"
        CodesDefault(83) = "Bridge"
        CodesDefault(84) = "Ditch"
        CodesDefault(85) = "Crown_Fin"
        CodesDefault(86) = "Crown_SubBase"
        CodesDefault(87) = "ETW_SubBase"
        CodesDefault(88) = "MarkedPoint"
        CodesDefault(89) = "Guardrail"
        CodesDefault(90) = "Median"
        CodesDefault(91) = "ETW_Overlay"
        CodesDefault(92) = "Trench_Bottom"
        CodesDefault(93) = "Trench_Daylight"
        CodesDefault(94) = "Trench_Bedding"
        CodesDefault(95) = "Trench_Backfill"
        CodesDefault(96) = "Trench"
        CodesDefault(97) = "LaneBreak"
        CodesDefault(98) = "LaneBreak_Overlay"
        CodesDefault(99) = "Sod"
        CodesDefault(100) = "Daylight_Strip"
        CodesDefault(101) = "Foreslope_Stripping"
        CodesDefault(102) = "Stripping"
        CodesDefault(103) = "Channel_Flowline"
        CodesDefault(104) = "Channel_Bottom"
        CodesDefault(105) = "Channel_Top"
        CodesDefault(106) = "Channel_Extension"
        CodesDefault(107) = "Channel_Backslope"
        CodesDefault(108) = "Lining_Material"
        CodesDefault(109) = "Ditch_Back"
        CodesDefault(110) = "Ditch_Face"
        CodesDefault(111) = "Ditch_Top"
        CodesDefault(112) = "Ditch_Bottom"
        CodesDefault(113) = "Backfill"
        CodesDefault(114) = "Backfill_Face"
        CodesDefault(115) = "Ditch_Lid_Face"
        CodesDefault(116) = "Lid_Top"
        CodesDefault(117) = "Ditch_Back_Fill"
        CodesDefault(118) = "Lid"
        CodesDefault(119) = "Drain_Bottom"
        CodesDefault(120) = "Drain_Top_Outside"
        CodesDefault(121) = "Drain_Top_Outside"
        CodesDefault(122) = "Drain_Top_Inside"
        CodesDefault(123) = "Drain_Bottom_Inside"
        CodesDefault(124) = "Drain_Center"
        CodesDefault(125) = "Flow_Line"
        CodesDefault(126) = "Drain_Top"
        CodesDefault(127) = "Drain_Structure"
        CodesDefault(128) = "Drain_Area"
        CodesDefault(129) = "RW_Front"
        CodesDefault(130) = "RW_Top"
        CodesDefault(131) = "RW_Back"
        CodesDefault(132) = "RW_Hinge"
        CodesDefault(133) = "RW_Inside"
        CodesDefault(134) = "RW_Outside"
        CodesDefault(135) = "Wall"
        CodesDefault(136) = "RWall"
        CodesDefault(137) = "RWall_B1"
        CodesDefault(138) = "RWall_B2"
        CodesDefault(139) = "RWall_B3"
        CodesDefault(140) = "RWall_B4"
        CodesDefault(141) = "RWall_K1"
        CodesDefault(142) = "RWall_K2"
        CodesDefault(143) = "Footing_Bottom"
        CodesDefault(144) = "Walk_Edge"
        CodesDefault(145) = "Lot"
        CodesDefault(146) = "Slope_Link"
        CodesDefault(147) = "Channel_Side"
        CodesDefault(148) = "Bench"
    End Sub
    Private Sub FillDefaults(ByVal colCodesAndDescriptionHashtable As Collection)
        Dim n As Integer
        On Error Resume Next 'leave inline
        InitializeDefaults()
        For n = 1 To UBound(CodesDefault)
            colCodesAndDescriptionHashtable.Add(CodesDefault(n), "I" & n)
        Next
    End Sub
    Private Function GetCodesFilePath() As String
        'the commented code doesn't work....
        'Dim oAeccApp As AeccCorridorApplication
        'Set oAeccApp = ThisDrawing.Application.GetInterfaceObject("AeccXUiCorridor.AeccCorridorApplication")
        'GetCodesFilePath = oAeccApp.Path & "\Codes.txt"
        'Exit Sub

        'I'll read the path from registries, if the code above will be fixed, just enable it


        GetCodesFilePath = Environ("AeccContent_Dir")
        'GetCodesFilePath = Left(GetCodesFilePath, InStrRev(GetCodesFilePath, "\")) & constCodesFile
        Dim lastBackSlashPosition As Integer
        lastBackSlashPosition = InStrRev(GetCodesFilePath, "\")
        If (lastBackSlashPosition < GetCodesFilePath.Length()) Then
            GetCodesFilePath = GetCodesFilePath & "\" & constCodesFile
        Else
            GetCodesFilePath = GetCodesFilePath & constCodesFile
        End If
    End Function
    Public Sub FillCodeStructure()
        On Error Resume Next 'leave it in-line

        Dim n As Integer, strIndex As String, sCodeAndDescription As String
        Dim colCodesAndDescriptionHashtable As Collection
        Dim sCodesFilePath As String
        sCodesFilePath = GetCodesFilePath()
        colCodesAndDescriptionHashtable = New Collection
        n = 0

        If Len(Dir(sCodesFilePath)) <> 0 Then
            Dim parser As New FileIO.TextFieldParser(sCodesFilePath)
            parser.SetDelimiters(",")
            Dim currentRow As String
            While Not parser.EndOfData

                currentRow = parser.ReadLine()
                If currentRow.IndexOf(",") <> -1 Then
                    strIndex = currentRow.Substring(0, currentRow.IndexOf(","))
                    sCodeAndDescription = currentRow.Substring(currentRow.IndexOf(",") + 1)
                    colCodesAndDescriptionHashtable.Add(sCodeAndDescription, "I" & strIndex)
                End If
            End While
        Else
            FillDefaults(colCodesAndDescriptionHashtable)
        End If



        With Codes
            .CodesStructureFilled = True

            GetFromCollection(colCodesAndDescriptionHashtable, n, .Crown)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .CrownPave1)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .CrownPave2)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .CrownBase)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .CrownSub)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ETW)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ETWPave1)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ETWPave2)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ETWBase)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ETWSub)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Lane)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .LanePave1)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .LanePave2)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .LaneBase)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .LaneSub)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .EPS)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .EPSPave1)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .EPSPave2)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .EPSBase)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .EPSSub)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .EPSBaseIn)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .EPSSubIn)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ESUnpaved)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DaylightSub)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Daylight)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DaylightFill)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DaylightCut)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DitchIn)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DitchOut)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .BenchIn)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .BenchOut)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .FlowlineDitch)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .LMedDitch)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RMedDitch)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Flange)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Flowline_Gutter)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .TopCurb)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .BottomCurb)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .BackCurb)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .SidewalkIn)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .SideWalkOut)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .HingeCut)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .HingeFill)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Top)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Datum)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Pave)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Pave1)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Pave2)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Base)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .SubBase)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Gravel)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .TopCurbNew)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .BackCurbNew)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Curb)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Sidewalk)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Hinge)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .EOV)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .EOVOverlay)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Level)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Mill)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Overlay)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .CrownOverlay)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Barrier)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .EBD)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .CrownDeck)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Deck)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Girder)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .EBS)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ESL)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DaylightBallast)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ESBS)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DaylightSubballast)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Ballast)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Sleeper)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Subballast)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Rail)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .R1)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .R2)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .R3)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .R4)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .R5)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .R6)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Bridge)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Ditch)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .CrownFin)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .CrownSubBase)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ETWSubBase)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .MarkedPoint)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Guardrail)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Median)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ETWOverlay)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .TrenchBottom)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .TrenchDaylight)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .TrenchBedding)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .TrenchBackfill)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Trench)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .LaneBreak)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .LaneBreakOverlay)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Sod)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DaylightStrip)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .sForeslopeStripping)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Stripping)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ChannelFlowline)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Channe_Bottom)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ChannelTop)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ChannelExtension)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .ChannelBackslope)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .LiningMaterial)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DitchBack)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DitchFace)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DitchTop)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DitchBottom)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Backfill)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .BackfillFace)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DitchLidFace)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .LidTop)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DitchBackFill)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Lid)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DrainBottom)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DrainBottomOutside)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DrainTopOutside)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DrainTopInside)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DrainBottomInside)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DrainCenter)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .FlowLine)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DrainTop)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DrainStructure)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .DrainArea)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWFront)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWTop)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWBack)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWHinge)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWInside)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWOutside)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Wall)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWall)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWallB1)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWallB2)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWallB3)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWallB4)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWallK1)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .RWallK2)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .FootingBottom)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .WalkEdge)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Lot)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Slope_Link)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Channel_Side)
            GetFromCollection(colCodesAndDescriptionHashtable, n, .Bench)
        End With

        colCodesAndDescriptionHashtable = Nothing
    End Sub
    Private Sub GetFromCollection(ByVal colCodesAndDescriptionHashtable As Collection, ByRef n As Integer, ByRef g_sEachCode As CodeType)
        On Error GoTo ErrH
        Dim sCode As String, sDescription As String, sCodesAndDes As String
        n = n + 1
        g_sEachCode.Index = n
        sCodesAndDes = colCodesAndDescriptionHashtable("I" & n)

        Dim firstCommaPos As Integer
        firstCommaPos = InStr(1, sCodesAndDes, ",")

        If firstCommaPos <> 0 Then
            sCode = Left(sCodesAndDes, firstCommaPos - 1)

            Dim SecondCommaPos As Integer
            SecondCommaPos = InStr(firstCommaPos + 1, sCodesAndDes, ",")
            If (SecondCommaPos <> 0) Then
                sDescription = Mid(sCodesAndDes, SecondCommaPos + 1)
            Else
                sDescription = ""
            End If
        Else
            sCode = sCodesAndDes
            sDescription = ""
        End If
        g_sEachCode.Code = sCode
        g_sEachCode.Description = sDescription
ErrH:
        If Err.Number <> 0 Then
            Debug.Print("Error for code " & n)
        End If
    End Sub
End Module
