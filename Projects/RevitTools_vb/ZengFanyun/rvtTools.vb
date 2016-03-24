Imports System.IO
Imports std_ez
Imports OldW
Imports OldW.Modeling
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.ApplicationServices
Imports OldW.GlobalSettings


Namespace rvtTools_ez

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class rvtTools

        ''' <summary>
        ''' 提取WarningValueUsing.dat中的WarningValue类
        ''' </summary>
        ''' <param name="FullPath">WarningValueUsing.dat的绝对路径</param>
        Public Shared Function GetWarningValue(ByVal FullPath As String) As WarningValue
            Dim fsr As New StreamReader(FullPath)
            Dim strData1 As String = fsr.ReadLine
            fsr.Close()
            '
            Dim a = GlobalSettings.InstrumentationType.墙体测斜
            Dim WV As WarningValue = StringSerializer.Decode64(strData1)
            '
            Return WV
        End Function

        ''' <summary>
        ''' 获取外部共享参数文件中的参数组“OldW”，然后可以通过DefinitionGroup.Definitions.Item(name As String)来索引其中的共享参数，
        ''' 也可以通过DefinitionGroup.Definitions.Create(name As String)来创建新的共享参数。
        ''' </summary>
        ''' <param name="App"></param>
        Public Shared Function GetOldWDefinitionGroup(ByVal App As Application) As DefinitionGroup

            ' 打开共享文件
            Dim OriginalSharedFileName As String = App.SharedParametersFilename   ' Revit程序中，原来的共享文件路径
            App.SharedParametersFilename = ProjectPath.Path_SharedParametersFile
            Dim myDefinitionFile As DefinitionFile = App.OpenSharedParameterFile()  ' 如果没有找到对应的文件，则打开时不会报错，而是直接返回Nothing
            App.SharedParametersFilename = OriginalSharedFileName  ' 将Revit程序中的共享文件路径还原，以隐藏插件程序中的共享文件路径

            If myDefinitionFile Is Nothing Then
                Throw New NullReferenceException("The specified shared parameter file """ & ProjectPath.Path_SharedParametersFile & """ is not found!")
            End If

            ' 索引到共享文件中的指定共享参数
            Dim myGroups As DefinitionGroups = myDefinitionFile.Groups
            Dim myGroup As DefinitionGroup = myGroups.Item(Constants.SP_GroupName)
            Return myGroup
        End Function

        Public Shared Sub ShowEnumerable(ByVal V As IEnumerable)
            Dim str As String = ""
            For Each o As Object In V
                str = str & o.ToString & vbCrLf
            Next
            TaskDialog.Show("集合", str, TaskDialogCommonButtons.Ok)
        End Sub
    End Class


End Namespace