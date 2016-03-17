Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports System.IO

Namespace OldW.GlobalSettings


    ''' <summary>
    ''' 项目中各种文件的路径
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ProjectPath

        ''' <summary>
        ''' Application的Dll所对应的路径，也就是“bin”文件夹的目录。
        ''' </summary>
        ''' <remarks>等效于：Dim thisAssemblyPath As String = System.Reflection.Assembly.GetExecutingAssembly().Location</remarks>
        Public Shared ReadOnly Property Path_Dlls() As String
            Get
                ' 在最终的发布中，Path_Dlls的值是等于“ My.Application.Info.DirectoryPath”的，但是，在调试过程中，如果使用Revit的AddinManager插件进行调试，
                ' 则在每次调试时，AddinManager都会将对应的dll复制到一个新的临时文件夹中，如果将此复制后的dll的路作为Path_Dlls的值，那么其后面的Path_Solution等路径都是无效路径了。
                If Directory.Exists("F:\Software\Revit\RevitDevelop\OldW\bin") Then
                    Return "F:\Software\Revit\RevitDevelop\OldW\bin"
                Else
                    Return My.Application.Info.DirectoryPath
                End If
            End Get
        End Property


        ''' <summary>
        ''' Solution文件所在的文件夹
        ''' </summary>
        Public Shared Path_Solution As String = New DirectoryInfo(Path_Dlls).Parent.FullName
        ''' <summary>
        ''' 存放图标的文件夹
        ''' </summary>
        Public Shared Path_icons As String = Path.Combine(Path_Solution, "Resources\icons")
        ''' <summary>
        ''' 存放族与族共享参数的文件夹
        ''' </summary>
        Public Shared Path_family As String = Path.Combine(Path_Solution, "Resources\Family")
        ''' <summary>
        '''   族共享参数的文件的绝对路径（不是文件夹的路径）。
        ''' </summary>
        ''' <remarks>先通过application.SharedParametersFilename来设置当前的SharedParametersFilename的路径，
        ''' 然后通过application.OpenSharedParameterFile方法来返回对应的DefinitionFile对象。</remarks>
        Public Shared Path_SharedParametersFile As String = Path.Combine(Path_family, "global.txt")
        ''' <summary>
        ''' 存放数据文件
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Path_data As String = Path.Combine(Path_Solution, "Resources\Data")
        ''' <summary>
        ''' 存放不同项目的文件夹
        ''' </summary>
        Public Shared Path_Projects As String = Path.Combine(Path_Solution, "Projects")
        ''' <summary>
        ''' 监测警戒规范的绝对文件路径
        ''' </summary>
        Public Shared Path_WarningValueUsing As String = Path.Combine(Path_data, "WarningValueUsing.dat")

    End Class



End Namespace