Imports System
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI.Selection
Imports Autodesk.Revit.DB.Architecture
Imports OldW.Instrumentation
Imports OldW.Soil

Namespace OldW.DataManager

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

            Dim app As OldWApplication = OldWApplication.Create(uiApp.Application)
            Dim od As OldWDocument = OldWDocument.Create(app, uiApp.ActiveUIDocument.Document)

            Dim bln As Boolean = OldWDocument.IsOldWDocument(uiApp.ActiveUIDocument.Document)

            Return Result.Succeeded

            Dim doc As Document = uiApp.ActiveUIDocument.Document
            '
            Dim inclineEle As Element = doc.GetElement(New ElementId(460115))
            Dim Incline As New Instrum_Incline(inclineEle)
            '
            Dim eleEarht As FamilyInstance = doc.GetElement(New ElementId(460116))

            Dim soil As Soil_Model = Soil_Model.FindSoilModel(doc)
            Incline.FindAdjacentEarthElevation(soil.Soil)

            Return Result.Succeeded
        End Function

    End Class

End Namespace