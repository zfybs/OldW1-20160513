Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports rvtTools_ez
Imports OldW.Soil
Imports System.Linq

''' <summary>
''' 将Revit中与基坑开挖相关的Document对象转换为OldW程序中的OldWDocument对象。
''' 此OldWDocument对象的标志性特征在于：此Document对象所对应的项目，在其“管理-项目信息”中，有一个参数：OldW_Project。
''' 在此OldWDocument中，可以在Revit的Document中进行与基坑相关的操作，比如搜索基坑开挖土体，记录测点信息等。
''' 此对象可以通过静态函数 Create进行构造。
''' </summary>
Public Class OldWDocument

#Region "   ---    Properties"

    Private Doc As Document
    ''' <summary>
    ''' 每一个OldWDocument对象都绑定了一个Revit的Document对象。
    ''' </summary>
    Public ReadOnly Property Document As Document
        Get
            Return Me.Doc
        End Get
    End Property


#End Region

#Region "   ---   构造函数：通过静态函数 Create "

    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="Doc"></param>
    ''' <remarks></remarks>
    Private Sub New(ByVal Doc As Document)
        If Doc.IsValidObject Then
            Me.Doc = Doc
        Else
            Throw New ArgumentException("The specified document to construct an instance of OldWDocument is not a valid object")
        End If
    End Sub


    ''' <summary> 在OldWApplication.OpenedDocuments 集合中，搜索是否有与指定的Document相对应的OldWDocument对象 </summary>
    ''' <param name="OldWApp">整个系统的OldWApplication对象，可以通过OldWApplication.Create方法获得。</param>
    ''' <returns>有相对应的对象，则返回之，否则则抛出异常。</returns>
    ''' <remarks></remarks>
    Friend Shared Function SearchOrCreate(ByVal OldWApp As OldWApplication, ByVal Doc As Document) As OldWDocument
        Dim OldWD As OldWDocument = Nothing
        ' 先搜索系统集合中是否有对应的 OldWDocument 对象
        OldWD = OldWApp.FindOldWDocument(Doc)
        If OldWD Is Nothing Then
            ' 创建一个新的OldW文档
            OldWD = OldWDocument.Create(OldWApp, Doc)
        End If
        Return OldWD
    End Function

    ''' <summary> 不在OldWApplication.OpenedDocuments 集合中进行搜索，而直接创建一个OldWDocument对象，并添加到OpenedDocuments集合中。 </summary>
    ''' <param name="OldWApp">整个系统的OldWApplication对象，可以通过OldWApplication.Create方法获得。</param>
    ''' <param name="Doc"></param>
    ''' <returns>有相对应的对象，则返回之，否则则抛出异常。</returns>
    ''' <remarks></remarks>
    Friend Shared Function Create(ByVal OldWApp As OldWApplication, ByVal Doc As Document) As OldWDocument
        Dim OldWD As OldWDocument = Nothing
        ' 创建一个新的OldW文档
        If OldWDocument.IsOldWDocument(Doc) Then
            Dim PossibleDocument As OldWDocument = OldWApp.FindOldWDocument(Doc)
            If PossibleDocument Is Nothing Then
                OldWD = New OldWDocument(Doc)
                OldWApp.OpenedDocuments.Add(OldWD)
            Else
                OldWD = PossibleDocument
            End If

        Else  ' 说明这个文档不符合OldWDocument的特征，此时为其添加对应的项目参数，使其成为一个OldWDocument对象
            If Doc.IsFamilyDocument Then
                MessageBox.Show("此文档为一个族文档，不能用来创建OldWDocument对象", "Warnning", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else     ' 在项目信息中添加一个新的参数


                ' 打开共享文件
                Dim app = New UIDocument(Doc).Application.Application
                app.SharedParametersFilename = ProjectPath.Path_SharedParametersFile
                Dim myDefinitionFile As DefinitionFile = app.OpenSharedParameterFile()

                ' create a new group in the shared parameters file
                Dim myGroups As DefinitionGroups = myDefinitionFile.Groups
                Dim myGroup As DefinitionGroup = myGroups.Item("OldW")
                Dim OldWDefi As ExternalDefinition = DirectCast(myGroup.Definitions.Item("OldW_Project"), ExternalDefinition)

                '------------ 
                ' create a category set and insert category of wall to it
                Dim myCategories As CategorySet = app.Create.NewCategorySet()
                ' use BuiltInCategory to get category of wall
                Dim myCategory As Category = Doc.Settings.Categories.Item(BuiltInCategory.OST_ProjectInformation)
                myCategories.Insert(myCategory)

                'Create an instance of InstanceBinding
                Dim instanceBinding As InstanceBinding = app.Create.NewInstanceBinding(myCategories)

                ' Get the BingdingMap of current document.
                Dim bindingMap As BindingMap = Doc.ParameterBindings

                ' Bind the definitions to the document
                Using resource As New Transaction(Doc, "添加项目参数")
                    resource.Start()
                    Dim instanceBindOK As Boolean = bindingMap.Insert(OldWDefi, instanceBinding, BuiltInParameterGroup.PG_TEXT)
                    resource.Commit()
                End Using
                OldWD = New OldWDocument(Doc)
                OldWApp.OpenedDocuments.Add(OldWD)
            End If
        End If
        MessageBox.Show(OldWApp.OpenedDocuments.Count)
        Return OldWD
    End Function

    ''' <summary>
    ''' 判断一个Document文档是否是一个OldW的项目文档，其判断的依据是：：此Document对象所对应的项目，在其“管理-项目信息”中，有一个参数：OldW_Project。
    ''' </summary>
    ''' <param name="Doc">要进行判断的Revit的Document文档</param>
    ''' <returns></returns>
    Public Shared Function IsOldWDocument(ByVal Doc As Document) As Boolean
        Dim bln As Boolean = False
        Dim proInfo As ProjectInfo = Doc.ProjectInformation  ' 族文件中的ProjectInformation属性为Nothing
        If proInfo IsNot Nothing Then
            Dim pa As Parameter = proInfo.Parameter(Constants.Guid_OldWProject)
            If pa IsNot Nothing Then
                bln = True
            End If
        End If
        Return bln
    End Function

#End Region

#Region "   ---    不同Documente对象之间的比较"

    ''' <summary>
    ''' 比较指定的Document对象与此OldWDocument对象中的Document对象是否是同一个Revit文档
    ''' </summary>
    ''' <returns>如果这两个Document对象是同一个Revit文档，则返回True，否则返回False。</returns>
    Public Shadows Function Equals(ComparedDocument As Document) As Boolean
        If OldWDocument.IsOldWDocument(Doc) Then
            Return CompareDocuments(Me.Document, ComparedDocument)
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' 比较两个Document对象是否是同一个Revit文档。
    ''' 如果这两个Document对象是同一个Revit文档，则返回True，否则返回False。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CompareDocuments(ByVal Doc1 As Document, ByVal Doc2 As Document) As Boolean
        Dim IsEqual As Boolean = False
        If Doc1.Equals(Doc2) Then
            IsEqual = True
        Else  ' 比较两个 Document 的绝对路径是否相同
            Dim path1 As String = Doc1.PathName
            Dim path2 As String = Doc2.PathName
            If path1.CompareTo(path2) = 0 Then
                IsEqual = True
            End If
        End If
        Return IsEqual
    End Function

#End Region

End Class
