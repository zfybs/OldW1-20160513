Imports System
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI.Selection
Imports Autodesk.Revit.DB.Architecture
Imports OldW.Instrumentation
Imports OldW.Soil
Imports std_ez
Imports rvtTools_ez
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports System.Threading
Imports OldW.Excavation
Imports System.IO

Namespace OldW.Commands

    <Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)>
    Public Class cmd_DataEdit : Implements IExternalCommand

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
    Public Class cmd_Analyze : Implements IExternalCommand

        Public Function Execute(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result Implements IExternalCommand.Execute
            Dim uiApp As UIApplication = commandData.Application
            Dim WApp As OldWApplication = OldWApplication.Create(uiApp.Application)
            Dim WDoc As OldWDocument = OldWDocument.SearchOrCreate(WApp, uiApp.ActiveUIDocument.Document)



            Dim doc As Document = uiApp.ActiveUIDocument.Document
            '
            Dim inclineEle As Element = doc.GetElement(New ElementId(460115))
            Dim Incline As New Instrum_Incline(inclineEle)
            '
            Dim eleEarht As FamilyInstance = doc.GetElement(New ElementId(460116))
            Dim exca As New ExcavationDoc(WDoc)

            Dim soil As Soil_Model = exca.FindSoilModel()
            Incline.FindAdjacentEarthElevation(soil.Soil)

            Return Result.Succeeded
        End Function

    End Class

    <Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)>
    Public Class cmd_Excavation : Implements IExternalCommand

        Private Shared Frm As frm_DrawExcavation

        Public Function Execute(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result Implements IExternalCommand.Execute

            Dim dat As New DllActivator.DllActivator_Projects_vb
            dat.ActivateReferences()

            Dim uiApp As UIApplication = commandData.Application
            Dim doc As Document = uiApp.ActiveUIDocument.Document

            '
            Dim WApp As OldWApplication = OldWApplication.Create(uiApp.Application)
            Dim WDoc As OldWDocument = OldWDocument.SearchOrCreate(WApp, doc)
            '
            Dim ExcavDoc As New ExcavationDoc(WDoc)

            If Frm Is Nothing OrElse Frm.IsDisposed Then
                Frm = New frm_DrawExcavation(ExcavDoc)
            End If
            Frm.Show(Nothing)

            '
            Return Result.Succeeded
        End Function

    End Class

    ''' <summary> 提取模型中的开挖土体信息 </summary>
    <Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)>
    Public Class cmd_ExcavationInfo : Implements IExternalCommand

        Public Function Execute(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result Implements IExternalCommand.Execute

            Dim dat As New DllActivator.DllActivator_Projects_vb
            dat.ActivateReferences()

            Dim uiApp As UIApplication = commandData.Application
            Dim doc As Document = uiApp.ActiveUIDocument.Document
            '

            Dim WApp As OldWApplication = OldWApplication.Create(uiApp.Application)
            Dim WDoc As OldWDocument = OldWDocument.SearchOrCreate(WApp, doc)
            '
            Dim ExcavDoc As New ExcavationDoc(WDoc)

            Dim frm As New frm_ExcavationInfo(ExcavDoc)
            frm.Show(Nothing)

            Return Result.Succeeded
            '

        End Function

    End Class

    ''' <summary> 查看指定日期的开挖工况 </summary>
    <Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)>
    Public Class cmd_ViewStage : Implements IExternalCommand

        Public Function Execute(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result Implements IExternalCommand.Execute
            Dim uiApp As UIApplication = commandData.Application
            Dim doc As Document = uiApp.ActiveUIDocument.Document
            '

            Dim WApp As OldWApplication = OldWApplication.Create(uiApp.Application)
            Dim WDoc As OldWDocument = OldWDocument.SearchOrCreate(WApp, doc)
            '
            Dim f As New ConstructionReview
            f.Show(Nothing)

            Return Result.Succeeded
            '

        End Function

    End Class

End Namespace