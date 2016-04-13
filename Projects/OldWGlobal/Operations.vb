Imports System
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI.Selection
Imports Autodesk.Revit.DB.Architecture
Imports OldW.GlobalSettings
Imports System.Threading


Namespace OldW.GlobalSettings
    Public Class Operations

        ''' <summary>
        ''' 将程序插件中的所有面板禁用
        ''' </summary>
        Public Shared Sub DeActivateControls(ByVal uiApp As UIApplication)
            Dim Panels = uiApp.GetRibbonPanels(Constants.AppName)
            For Each p As RibbonPanel In Panels
                p.Enabled = False
            Next
        End Sub

        ''' <summary>
        ''' 将程序插件中的所有面板激活
        ''' </summary>
        Public Shared Sub ActivateControls(ByVal uiApp As UIApplication)
            Dim Panels = uiApp.GetRibbonPanels(Constants.AppName)
            For Each p As RibbonPanel In Panels
                p.Enabled = True
            Next
        End Sub

    End Class
End Namespace