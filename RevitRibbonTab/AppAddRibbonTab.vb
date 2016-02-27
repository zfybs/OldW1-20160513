Imports System.IO
Imports Autodesk.Revit
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports System.IO.Directory

' -------------------------- OldWApplication.addin -----------------------------------------
' <?xml version="1.0" encoding="utf-8"?>
' <RevitAddIns>
'   <AddIn Type="Application">
'     <Name>ExternalApplication</Name>
'     <Assembly>F:\Software\Revit\RevitDevelop\OldW\bin\RevitRibbonTab.dll</Assembly>
'     <ClientId>32df6eb7-7dbc-45ff-b86e-b9c8bf0f8185</ClientId>
'     <FullClassName>OldW.AppAddRibbonTab</FullClassName>
'     <VendorId>OldW</VendorId>
'     <VendorDescription>http://naoce.sjtu.edu.cn/</VendorDescription>
'   </AddIn>
' </RevitAddIns>
' ------------------------------------------------------------------------------------------

Public Class AppAddRibbonTab
    Implements IExternalApplication

    Public Function OnStartup(ByVal application As UIControlledApplication) As Result Implements IExternalApplication.OnStartup
        'Create a custom ribbon tab
        Dim tabName As String = GlobalSettings.AppName
        application.CreateRibbonTab(tabName)

        ' 分析面板
        Dim ribbonPanelAnalysis As RibbonPanel = application.CreateRibbonPanel(tabName, "分析")
        AddPushButtonAnalysis(ribbonPanelAnalysis)

        ' 建模面板
        Dim ribbonPanelModeling As RibbonPanel = application.CreateRibbonPanel(tabName, "建模")
        AddSplitButtonModeling(ribbonPanelModeling)
        AddPushButtonSetWarning(ribbonPanelModeling)
        AddPushButtonDataEdit(ribbonPanelModeling)
        ' 关于面板
        Dim ribbonPanelAbout As RibbonPanel = application.CreateRibbonPanel(tabName, "关于")
        AddPushButtonAbout(ribbonPanelAbout) '添加关于
        '

        Return Result.Succeeded
    End Function

    Public Function OnShutdown(ByVal application As UIControlledApplication) As Result Implements IExternalApplication.OnShutdown
        Return Result.Succeeded
    End Function

#Region "  ---  添加按钮"

    ''' <summary> 添加建模的下拉记忆按钮 </summary>
    ''' <param name="panel"> 目标RibbonPanel </param>
    Private Sub AddSplitButtonModeling(ByVal panel As RibbonPanel)
        ' 创建一个SplitButton
        Dim splitButtonData As New SplitButtonData("ModelingMonitor", "监测建模")
        Dim splitButton As SplitButton = TryCast(panel.AddItem(splitButtonData), SplitButton)

        ' 创建一个沉降pushButton加到SplitButton的下拉列表里
        Dim pushButton As PushButton = splitButton.AddPushButton(New PushButtonData("ModelingSettlement", "沉降", Path.Combine(Path_Dlls, "Modeling.dll"), "OldW.Modeling.SetFamilySettlement"))
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorSet-36.png"))) '36*36的大小
        pushButton.Image = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorSet-16.png")))
        pushButton.ToolTip = "放置沉降测点模型"

        ' 创建一个轴力pushButton加到SplitButton的下拉列表里
        pushButton = splitButton.AddPushButton(New PushButtonData("ModelingForce", "轴力", Path.Combine(Path_Dlls, "Modeling.dll"), "OldW.Modeling.SetFamilyForce"))
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorForce-36.png"))) '36*36的大小
        pushButton.Image = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorForce-16.png")))
        pushButton.ToolTip = "放置轴力测点模型"

        ' 创建一个测斜pushButton加到SplitButton的下拉列表里
        pushButton = splitButton.AddPushButton(New PushButtonData("ModelingIncli", "测斜", Path.Combine(Path_Dlls, "Modeling.dll"), "OldW.Modeling.SetFamilyIncli"))
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorIncli-36.png"))) '36*36的大小
        pushButton.Image = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorIncli-16.png")))
        pushButton.ToolTip = "放置测斜测点模型"
    End Sub

    ''' <summary> 添加测点数据编辑的下拉记忆按钮 </summary>
    Private Sub AddPushButtonDataEdit(ByVal panel As RibbonPanel)
        ' Create a new push button
        Dim str As String = Path.Combine(Path_Dlls, "DataManager.dll")
        Dim pushButton As PushButton = TryCast(panel.AddItem(New PushButtonData("DataEdit", "导入数据", str, "OldW.DataManager.cmd_DataEdit")), PushButton)
        ' Set ToolTip    
        pushButton.ToolTip = "导入到导出监测数据"
        ' Set Contextual help
        Dim contextHelp As New ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com")
        pushButton.SetContextualHelp(contextHelp)
        ' Set Icon
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "DataEdit-32.png")))
    End Sub

    Private Sub AddPushButtonAbout(ByVal panel As RibbonPanel)
        ' Create a new push button
        Dim pushButton As PushButton = TryCast(panel.AddItem(New PushButtonData("About", "关于", Path.Combine(Path_Dlls, "RevitRibbonTab.dll"), "OldW.IECAbout")), PushButton)
        ' Set ToolTip 
        pushButton.ToolTip = "关于信息"
        ' Set Contextual help
        Dim contextHelp As New ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com")
        pushButton.SetContextualHelp(contextHelp)
        ' Set Icon
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "About-32.png")))
    End Sub

    Private Sub AddPushButtonSetWarning(ByVal panel As RibbonPanel)
        ' Create a new push button
        Dim str As String = Path.Combine(Path_Dlls, "Modeling.dll")
        Dim pushButton As PushButton = TryCast(panel.AddItem(New PushButtonData("SetWarning", "警戒值设定", str, "OldW.Modeling.SetWarning")), PushButton)
        ' Set ToolTip    
        pushButton.ToolTip = "警戒值设定"
        ' Set Contextual help
        Dim contextHelp As New ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com")
        pushButton.SetContextualHelp(contextHelp)
        ' Set Icon
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "SetWarning-32.png")))
    End Sub

    Private Sub AddPushButtonAnalysis(ByVal panel As RibbonPanel)
        ' Create a new push button
        Dim pushButton As PushButton = TryCast(panel.AddItem(New PushButtonData("Analysis", "分析", Path.Combine(Path_Dlls, "DataManager.dll"), "OldW.DataManager.cmd_Analyse")), PushButton)
        ' Set ToolTip 
        pushButton.ToolTip = "警戒值分析"
        ' Set Contextual help
        Dim contextHelp As New ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com")
        pushButton.SetContextualHelp(contextHelp)
        ' Set Icon
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "Analysis-32.png")))
    End Sub

#End Region

End Class
