Imports System.IO
Imports Autodesk.Revit
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports System.IO.Directory

' -------------------------- OldWApplication.addin -----------------------------------------
' <?xml version="1.0" encoding="utf-8"?>
' <RevitAddIns>
'   <AddIn Type="Application">
'     <Name>ExternalApplication</Name>
'     <Assembly>F:\Software\Revit\RevitDevelop\OldW\bin\OldWApplication.dll</Assembly>
'     <ClientId>32df6eb7-7dbc-45ff-b86e-b9c8bf0f8185</ClientId>
'     <FullClassName>OldW.AppAddRibbonTab</FullClassName>
'     <VendorId>OldW</VendorId>
'     <VendorDescription>http://naoce.sjtu.edu.cn/</VendorDescription>
'   </AddIn>
' 
' </RevitAddIns>
' ------------------------------------------------------------------------------------------

Public Class AppAddRibbonTab
    Implements IExternalApplication


#Region "  ---  文件路径"

    ''' <summary> Application的Dll所对应的路径，也就是“bin”文件夹的目录。 </summary>
    Private Path_Dlls As String = My.Application.Info.DirectoryPath
    ''' <summary> 存放图标的文件夹 </summary>
    Private Path_icons As String = Path.Combine(New DirectoryInfo(Path_Dlls).Parent.FullName, "Resources\icons")

#End Region

#Region "  ---  常数"

    ''' <summary> 整个程序的标志性名称 </summary>
    Private Const AppName As String = "OldW"

    ''' <summary> VB.NET项目的Dll的名称 </summary>
    Private Const Dll_vb As String = "Projects_vb.dll"
    ''' <summary> C#项目的Dll的名称 </summary>
    Private Const Dll_cs As String = "Projects_cs.dll"
    ''' <summary> 本程序集的Dll的名称 </summary>
    Private Const Dll_RibbonTab As String = "OldWRibbonTab.dll"

#End Region


    ''' <summary> Ribbon界面设计 </summary>
    Public Function OnStartup(ByVal application As UIControlledApplication) As Result Implements IExternalApplication.OnStartup
        'Create a custom ribbon tab
        Dim tabName As String = AppName
        application.CreateRibbonTab(tabName)

        ' 建模面板
        Dim ribbonPanelModeling As RibbonPanel = application.CreateRibbonPanel(tabName, "建模")
        AddSplitButtonModeling(ribbonPanelModeling)
        AddPushButtonExcavation(ribbonPanelModeling)

        ' 监测数据面板
        Dim ribbonPanelData As RibbonPanel = application.CreateRibbonPanel(tabName, "监测")
        AddPushButtonDataEdit(ribbonPanelData)


        ' 分析面板
        Dim ribbonPanelAnalysis As RibbonPanel = application.CreateRibbonPanel(tabName, "分析")
        AddPushButtonSetWarning(ribbonPanelAnalysis)
        AddPushButtonAnalysis(ribbonPanelAnalysis)

        ' 关于面板
        Dim ribbonPanelAbout As RibbonPanel = application.CreateRibbonPanel(tabName, "关于")
        AddPushButtonAbout(ribbonPanelAbout) '添加关于
        '

        Return Result.Succeeded
    End Function

    Public Function OnShutdown(ByVal application As UIControlledApplication) As Result Implements IExternalApplication.OnShutdown
        Return Result.Succeeded
    End Function

#Region "  ---  添加按钮  （如果LargeImage所对应的图片不能在Ribbon中显示，请尝试先下载128*128的，然后通过画图工具将其大小调整为32*32.）"

    ''' <summary> 添加“关于”的下拉记忆按钮 </summary>
    Private Sub AddPushButtonAbout(ByVal panel As RibbonPanel)
        ' Create a new push button
        Dim pushButton As PushButton = TryCast(panel.AddItem(New PushButtonData("About", "关于", Path.Combine(Path_Dlls, Dll_RibbonTab), "OldW.IECAbout")), PushButton)
        ' Set ToolTip 
        pushButton.ToolTip = "关于信息"
        ' Set Contextual help
        Dim contextHelp As New ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com")
        pushButton.SetContextualHelp(contextHelp)
        ' Set Icon
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "About-32.png")))
    End Sub

    ''' <summary> 添加“放置监测点”的下拉记忆按钮 </summary>
    ''' <param name="panel"> 目标RibbonPanel </param>
    Private Sub AddSplitButtonModeling(ByVal panel As RibbonPanel)
        ' 创建一个SplitButton
        Dim splitButtonData As New SplitButtonData("ModelingMonitor", "监测建模")
        Dim splitButton As SplitButton = TryCast(panel.AddItem(splitButtonData), SplitButton)

        ' 创建一个沉降pushButton加到SplitButton的下拉列表里
        Dim pushButton As PushButton = splitButton.AddPushButton(New PushButtonData("ModelingSettlement", "沉降", Path.Combine(Path_Dlls, Dll_cs), "OldW.Modeling.cmd_SetFamilySettlement"))
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorSet-32.png"))) '36*36的大小
        pushButton.Image = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorSet-16.png")))
        pushButton.ToolTip = "放置沉降测点模型"

        ' 创建一个轴力pushButton加到SplitButton的下拉列表里
        pushButton = splitButton.AddPushButton(New PushButtonData("ModelingForce", "轴力", Path.Combine(Path_Dlls, Dll_cs), "OldW.Modeling.cmd_SetFamilyForce"))
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorForce-32.png"))) '36*36的大小
        pushButton.Image = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorForce-16.png")))
        pushButton.ToolTip = "放置轴力测点模型"

        ' 创建一个测斜pushButton加到SplitButton的下拉列表里
        pushButton = splitButton.AddPushButton(New PushButtonData("ModelingIncli", "测斜", Path.Combine(Path_Dlls, Dll_cs), "OldW.Modeling.cmd_SetFamilyIncli"))
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorIncli-32.png"))) '36*36的大小
        pushButton.Image = New BitmapImage(New Uri(Path.Combine(Path_icons, "MonitorIncli-16.png")))
        pushButton.ToolTip = "放置测斜测点模型"
    End Sub

    ''' <summary> 添加“测点数据编辑”的下拉记忆按钮 </summary>
    Private Sub AddPushButtonDataEdit(ByVal panel As RibbonPanel)
        ' Create a new push button
        Dim str As String = Path.Combine(Path_Dlls, Dll_vb)
        Dim pushButton As PushButton = TryCast(panel.AddItem(New PushButtonData("DataEdit", "导入数据", str, "OldW.Commands.cmd_DataEdit")), PushButton)
        ' Set ToolTip    
        pushButton.ToolTip = "导入到导出监测数据"
        ' Set Contextual help
        Dim contextHelp As New ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com")
        pushButton.SetContextualHelp(contextHelp)
        ' Set Icon
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "DataEdit-32.png")))
    End Sub

    ''' <summary> 添加“警戒值设定”的下拉记忆按钮 </summary>
    Private Sub AddPushButtonSetWarning(ByVal panel As RibbonPanel)
        ' Create a new push button
        Dim str As String = Path.Combine(Path_Dlls, Dll_cs)
        Dim pushButton As PushButton = TryCast(panel.AddItem(New PushButtonData("SetWarning", "警戒值设定", str, "OldW.Modeling.cmd_SetWarning")), PushButton)
        ' Set ToolTip    
        pushButton.ToolTip = "警戒值设定"
        ' Set Contextual help
        Dim contextHelp As New ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com")
        pushButton.SetContextualHelp(contextHelp)
        ' Set Icon
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "SetWarning-32.png")))
    End Sub

    ''' <summary> 添加“分析”的下拉记忆按钮 </summary>
    Private Sub AddPushButtonAnalysis(ByVal panel As RibbonPanel)
        ' Create a new push button
        Dim pushButton As PushButton = TryCast(panel.AddItem(New PushButtonData("Analysis", "分析", Path.Combine(Path_Dlls, Dll_vb), "OldW.Commands.cmd_Analyze")), PushButton)
        ' Set ToolTip 
        pushButton.ToolTip = "警戒值分析"
        ' Set Contextual help
        Dim contextHelp As New ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com")
        pushButton.SetContextualHelp(contextHelp)
        ' Set Icon
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "Analysis-32.png")))
    End Sub

    ''' <summary> 添加“开挖”的下拉记忆按钮 </summary>
    Private Sub AddPushButtonExcavation(ByVal panel As RibbonPanel)
        ' Create a new push button
        Dim pushButton As PushButton = TryCast(panel.AddItem(New PushButtonData("Excavation", "开挖", Path.Combine(Path_Dlls, Dll_vb), "OldW.Commands.cmd_Excavation")), PushButton)
        ' Set ToolTip 
        pushButton.ToolTip = "基坑开挖与回筑"
        ' Set Contextual help
        Dim contextHelp As New ContextualHelp(ContextualHelpType.Url, "http://www.autodesk.com")
        pushButton.SetContextualHelp(contextHelp)
        ' Set Icon
        pushButton.LargeImage = New BitmapImage(New Uri(Path.Combine(Path_icons, "Excavation-32.png"))) ' "Excavation-32.png"

    End Sub

#End Region

End Class
