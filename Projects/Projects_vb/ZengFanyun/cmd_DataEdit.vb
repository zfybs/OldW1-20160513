Imports System
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI.Selection
Imports Autodesk.Revit.DB.Architecture


<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)>
Public Class cmd_DataEdit
    Implements IExternalCommand

    Public Function Execute(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result Implements IExternalCommand.Execute
        Dim uiApp As UIApplication = commandData.Application
        Dim doc As Document = uiApp.ActiveUIDocument.Document
        Dim eleIds As ICollection(Of ElementId) = uiApp.ActiveUIDocument.Selection.GetElementIds

        Dim frm As New ElementDataManager(eleIds, doc)
        frm.ShowDialog()
        Return Result.Succeeded
    End Function

End Class

<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)>
Public Class cmd_Analyze
    Implements IExternalCommand

    Public Function Execute(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result Implements IExternalCommand.Execute
        Dim uiApp As UIApplication = commandData.Application
        Dim doc As Document = uiApp.ActiveUIDocument.Document
        '
        Dim inclineEle As Element = doc.GetElement(New ElementId(460115))
        Dim Incline As New MP_Inclinometer(uiApp.ActiveUIDocument, inclineEle)
        '
        Incline.FindAdjacentEarthElevation()

        Return Result.Succeeded
    End Function


    Private Function 测点警戒值分析(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result
        Dim uiApp As UIApplication = commandData.Application

        Dim eleIds As List(Of ElementId) = uiApp.ActiveUIDocument.Selection.GetElementIds

        Dim frm As New Analysis(eleIds, uiApp.ActiveUIDocument)
        frm.CheckData()
        Return Result.Succeeded
    End Function

End Class

<System.Serializable()>
Friend Class MonitorData
    Public arrDate As Date()
    Public arrValue As Object()
    Public Sub New(ArrayDate As Date(), ArrayValue As Object())
        With Me
            .arrDate = ArrayDate
            .arrValue = ArrayValue
        End With
    End Sub
End Class