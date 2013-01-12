Option Explicit On
Option Strict Off

Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.Civil.Corridor
Imports Autodesk.Civil.ApplicationServices
Imports DBTransactionManager = Autodesk.AutoCAD.DatabaseServices.TransactionManager

Public MustInherit Class SATemplate
    ' --------------------------------------------------------------------------
    ' Returns logical names used by this script
    Public Sub GetLogicalNames()
        Dim trans As Transaction = Nothing
        Dim corridorState As CorridorState = Nothing

        Try

            ' Start transaction
            trans = StartTransaction()
            ' Get the corridor stateobject
            corridorState = CorridorApplication.Application.ActiveDocument.CorridorState

            ' Retrieve parameter buckets from the corridor state
            GetLogicalNamesImplement(corridorState)

            trans.Commit()

        Catch e As System.Exception
            Utilities.RecordError(corridorState, e)
            If trans IsNot Nothing Then trans.Abort()
        Finally
            If trans IsNot Nothing Then trans.Dispose()
        End Try

    End Sub

    ' --------------------------------------------------------------------------
    ' Returns input parameters required by this script
    Public Sub GetInputParameters()

        Dim trans As Transaction = Nothing
        Dim corridorState As CorridorState = Nothing

        Try
            trans = StartTransaction()
            corridorState = CorridorApplication.Application.ActiveDocument.CorridorState

            GetInputParametersImplement(corridorState)

            trans.Commit()
        Catch e As System.Exception
            Utilities.RecordError(corridorState, e)
            If trans IsNot Nothing Then trans.Abort()
        Finally
            If trans IsNot Nothing Then trans.Dispose()
        End Try

    End Sub

    ' --------------------------------------------------------------------------
    ' Returns output parameters returned by this script
    Public Sub GetOutputParameters()

        Dim trans As Transaction = Nothing
        Dim corridorState As CorridorState = Nothing

        Try
            ' Start transaction
            trans = StartTransaction()
            corridorState = CorridorApplication.Application.ActiveDocument.CorridorState

            GetOutputParametersImplement(corridorState)

            trans.Commit()
        Catch e As System.Exception
            Utilities.RecordError(corridorState, e)
            If trans IsNot Nothing Then trans.Abort()
        Finally
            If trans IsNot Nothing Then trans.Dispose()
        End Try

    End Sub

    Public Sub Draw()
        Dim corridorState As CorridorState = Nothing

        Try
            corridorState = CorridorApplication.Application.ActiveDocument.CorridorState

            DrawImplement(corridorState)
        Catch e As System.Exception
            Utilities.RecordError(corridorState, e)
        End Try
    End Sub



    Protected Overridable Sub GetLogicalNamesImplement(ByVal corridorState As CorridorState)
        'default do nothing
    End Sub
    Protected Overridable Sub GetInputParametersImplement(ByVal corridorState As CorridorState)
        'default do nothing
    End Sub
    Protected Overridable Sub GetOutputParametersImplement(ByVal corridorState As CorridorState)
        'default do nothing
    End Sub


    Protected MustOverride Sub DrawImplement(ByVal corridorState As CorridorState)


    Protected Function StartTransaction() As Transaction
        Dim db As Database = HostApplicationServices.WorkingDatabase
        Dim tm As DBTransactionManager = db.TransactionManager
        Dim trans As Transaction = tm.StartTransaction()
        Return trans
    End Function

    '' *************************************************************************
    '' *************************************************************************
    '' *************************************************************************
    ''          Name: GetCorridorState
    ''
    ''   Description:  return the corridor state .NET object.
    ''                It has been opened for writing.
    ''                Attention: it method should be called only once under a transaction opened by transactionManager
    ''
    'Protected Function GetCorridorState(ByVal trans As Transaction) As CorridorState
    '    'If trans Is Nothing Then
    '    '    Throw New ArgumentNullException("trans")
    '    'End If
    '    'Dim oId As ObjectId
    '    'oId = CorridorApplication.Application.ActiveDocument.CorridorStateId
    '    'Return trans.GetObject(oId, OpenMode.ForWrite, False)
    '    Return CorridorApplication.Application.ActiveDocument.CorridorState
    'End Function

    Protected Sub New()
        If Not Codes.CodesStructureFilled Then
            FillCodeStructure()
        End If
    End Sub
End Class
