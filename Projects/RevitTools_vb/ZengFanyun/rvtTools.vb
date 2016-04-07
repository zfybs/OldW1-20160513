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
    ''' Revit中的一些常规性操作工具
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

#Region "   ---   搜索文档中的元素"



        ''' <summary>
        ''' Helper function: find a list of element with the given Class, Name and Category (optional). 
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function FindElements(ByVal rvtDoc As Document, _
                              ByVal targetType As Type, Optional ByVal targetCategory As BuiltInCategory = Nothing, _
                              Optional ByVal targetName As String = Nothing) As List(Of Element)

            ''  first, narrow down to the elements of the given type and category 
            Dim collector = New FilteredElementCollector(rvtDoc).OfClass(targetType)

            ' 是否要按类别搜索
            If Not (targetCategory = Nothing) Then
                collector.OfCategory(targetCategory)
            End If

            ' 是否要按名称搜索
            If targetName IsNot Nothing Then
                ''  using LINQ query here.
                Dim elems = _
                    From element In collector _
                    Where element.Name.Equals(targetName) _
                    Select element

                ''  put the result as a list of element for accessibility. 
                Return elems.ToList()
            End If
            Return collector.ToElements
        End Function

        ''' <summary>
        '''  Helper function: find a list of element with the given Class, Name and Category (optional). 
        ''' </summary>
        ''' <param name="rvtDoc">要进行搜索的Revit文档</param>
        ''' <param name="SourceElements">要从文档中的哪个集合中来进行搜索</param>
        ''' <param name="targetType"></param>
        ''' <param name="targetCategory"></param>
        ''' <param name="targetName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FindElements(ByVal rvtDoc As Document, ByVal SourceElements As ICollection(Of ElementId), _
                              ByVal targetType As Type, Optional ByVal targetCategory As BuiltInCategory = Nothing, _
                              Optional ByVal targetName As String = Nothing) As List(Of Element)


            Dim collector = New FilteredElementCollector(rvtDoc, SourceElements)

            ' 搜索类型
            collector.OfClass(targetType)

            ' 是否要搜索类别
            If Not (targetCategory = Nothing) Then
                collector.OfCategory(targetCategory)
            End If

            ' 是否要搜索名称
            If targetName IsNot Nothing Then
                Dim elems As IEnumerable(Of Element)
                ''  parse the collection for the given names
                ''  using LINQ query here.
                elems = _
                    From element In collector _
                    Where element.Name.Equals(targetName) _
                    Select element
                Return elems.ToList
            End If

            Return collector.ToElements
        End Function

        Public Shared Function FindElement(ByVal rvtDoc As Document, _
                              ByVal targetType As Type, Optional ByVal targetCategory As BuiltInCategory = Nothing, _
                              Optional ByVal targetName As String = Nothing) As Element

            ''  find a list of elements using the overloaded method. 
            Dim elems As IList(Of Element) = FindElements(rvtDoc, targetType, targetCategory, targetName)

            ''  return the first one from the result. 
            If elems.Count > 0 Then
                Return elems.Item(0)
            End If
            Return Nothing
        End Function
        Public Shared Function FindElement(ByVal rvtDoc As Document, ByVal SourceElements As ICollection(Of ElementId), _
                      ByVal targetType As Type, Optional ByVal targetCategory As BuiltInCategory = Nothing, _
                      Optional ByVal targetName As String = Nothing) As Element
            ''  find a list of elements using the overloaded method. 
            Dim elems As IList(Of Element) = FindElements(rvtDoc, targetType, targetCategory, targetName)

            ''  return the first one from the result. 
            If elems.Count > 0 Then
                Return elems.Item(0)
            End If
            Return Nothing
        End Function


#End Region

    End Class
End Namespace