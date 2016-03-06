Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI

Namespace rvtTools_ez.Test_Addin

    <Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)>
    Public Class ExternalCommand
        Implements IExternalCommand

        Public Function Execute(commandData As ExternalCommandData, ByRef message As String, elements As ElementSet) As Result Implements IExternalCommand.Execute
            'TaskDialog.Show("Revit", "ExternalCommand1")
            Return Result.Succeeded
        End Function
    End Class


    Public Class ExternalApplication
        Implements IExternalApplication

        Public Function OnShutdown(application As UIControlledApplication) As Result Implements IExternalApplication.OnShutdown
            'TaskDialog.Show("Revit", "ExternalApplication")
            Return Result.Succeeded
        End Function

        Public Function OnStartup(application As UIControlledApplication) As Result Implements IExternalApplication.OnStartup
            'Call NewRibbon(application)
            Return Result.Succeeded
        End Function

        ''' <summary>
        ''' 创建一个新的 Ribbon
        ''' </summary>
        Public Sub NewRibbon(application As UIControlledApplication)
            '添加一个新的Ribbon面板
            'Dim ribbonPanel As RibbonPanel = application.CreateRibbonPanel("NewRibbonPanel")

            ' ''在新的Ribbon面板上添加一个按钮
            ' ''点击这个按钮，前一个例子“HelloRevit”这个插件将被运行。
            'Dim pushButton As PushButton = ribbonPanel.AddItem(New PushButtonData("HelloRevit",
            '        "HelloRevit", "F:\Software\Revit\RevitDevelop\eZRevtiTools\eZrvt_ExApp\eZrvt_ExApp\bin\Debug\eZrvt_ExApp.dll", "eZrvt_ExApp.test.ExternalCommand"))

            '给按钮添加一个图片
            'Dim uriImage As Uri = New Uri("C:\Users\tt\Desktop\11.png")
            'Dim largeImage As BitmapImage = New BitmapImage(uriImage)
            'pushButton.LargeImage = largeImage

        End Sub

    End Class

    Public Class ExternalDBApplication
        Implements IExternalDBApplication


        Public Function OnShutdown(application As Autodesk.Revit.ApplicationServices.ControlledApplication) As ExternalDBApplicationResult Implements IExternalDBApplication.OnShutdown
            'TaskDialog.Show("Revit", "ExternalDBApplication")
            Return ExternalDBApplicationResult.Succeeded
        End Function

        Public Function OnStartup(application As Autodesk.Revit.ApplicationServices.ControlledApplication) As ExternalDBApplicationResult Implements IExternalDBApplication.OnStartup
            'TaskDialog.Show("Revit", "ExternalDBApplication")
            Return ExternalDBApplicationResult.Succeeded
        End Function

    End Class



End Namespace

'三种不同类型的插件的 XML 调用方式：
'<?xml version="1.0" encoding="utf-8"?>
'<RevitAddIns>
'  <AddIn Type="Application">
'    <Name>ExternalApplication</Name>
'    <Assembly>F:\Software\Revit\RevitDevelop\eZrvt\eZrvt_ExApp\eZrvt_ExApp\bin\Debug\eZrvt_ExApp.dll</Assembly>
'    <ClientId>1a706721-ae9e-41ba-a783-cb4092401317</ClientId>
'    <FullClassName>eZrvt_ExApp.test.ExternalApplication</FullClassName>
'    <VendorId>ADSK</VendorId>
'    <VendorDescription>Autodesk, www.autodesk.com</VendorDescription>
'  </AddIn>
'  <AddIn Type="Command">
'    <Assembly>F:\Software\Revit\RevitDevelop\eZrvt\eZrvt_ExApp\eZrvt_ExApp\bin\Debug\eZrvt_ExApp.dll</Assembly>
'    <ClientId>089df578-a98e-492c-a7e1-299f15572a42</ClientId>
'    <FullClassName>eZrvt_ExApp.test.ExternalCommand</FullClassName>
'    <Text>ExternalCommand</Text>
'    <Description>""</Description>
'    <VisibilityMode>AlwaysVisible</VisibilityMode>
'    <VendorId>ADSK</VendorId>
'    <VendorDescription>Autodesk, www.autodesk.com</VendorDescription>
'  </AddIn>
'
'   <AddIn Type="DBApplication">
'      <Assembly>F:\Software\Revit\RevitDevelop\eZrvt\eZrvt_ExApp\eZrvt_ExApp\bin\Debug\eZrvt_ExApp.dll</Assembly>
'      <ClientId>bae83b6a-36e4-47e5-99b3-666a1060716b</ClientId>
'      <FullClassName>eZrvt_ExApp.test.ExternalDBApplication</FullClassName>
'      <Name>Revit LookupA</Name>
'      <VendorId>ADSK</VendorId>
'      <VendorDescription>Autodesk, www.autodesk.com</VendorDescription>	  
'   </AddIn>
'
'</RevitAddIns>