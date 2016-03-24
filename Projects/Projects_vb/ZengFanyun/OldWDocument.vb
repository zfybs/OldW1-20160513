Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports std_ez
Imports rvtTools_ez.ExtensionMethods
Imports rvtTools_ez.rvtTools
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

    ''' <summary> 每一个OldWDocument对象都绑定了一个Revit的Document对象。 </summary>
    Private Doc As Document
    ''' <summary> 每一个OldWDocument对象都绑定了一个Revit的Document对象。 </summary>
    Public ReadOnly Property Document As Document
        Get
            Return Me.Doc
        End Get
    End Property

    ''' <summary>
    ''' OldWDocument中保存的与基坑开挖有关的信息
    ''' </summary>
    ''' <remarks></remarks>
    Private ProjectInfo As OldWProjectInfo

#End Region

#Region "   ---   构造函数：通过静态函数 Create "

    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="Doc"></param>
    ''' <remarks></remarks>
    Protected Friend Sub New(ByVal Doc As Document)
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
        If OldWDocument.IsOldWDocument(Doc) Then
            ' 先搜索系统集合中是否有对应的 OldWDocument 对象
            OldWD = OldWApp.SearchOldWDocument(Doc)
            If OldWD Is Nothing Then   ' 说明此文档在OldWApp的集合中没有找到对应的项。
                OldWD = New OldWDocument(Doc)
                OldWApp.OpenedDocuments.Add(OldWD)
            End If
        Else   ' 说明这个文档不符合OldWDocument的特征，此时为其添加对应的项目参数，使其成为一个OldWDocument对象
            If Doc.IsFamilyDocument Then
                Throw New InvalidOperationException("此文档为一个族文档，不能用来创建OldWDocument对象")
                ' MessageBox.Show("此文档为一个族文档，不能用来创建OldWDocument对象", "Warnning", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else     ' 在项目信息中添加一个新的参数
                OldWD = CreateNew(OldWApp, Doc)
                OldWApp.OpenedDocuments.Add(OldWD)
            End If
        End If
        Return OldWD
    End Function

    ''' <summary> 不在OldWApplication.OpenedDocuments 集合中进行搜索，而直接创建一个OldWDocument对象，并添加到OpenedDocuments集合中。 </summary>
    ''' <param name="OldWApp">整个系统的OldWApplication对象</param>
    ''' <param name="Doc">用户应该非常确信此Doc并不在OldWApplication的OpenedDocuments集合中，
    ''' 否则，会将一个重复的OldWDocument对象再次添加进OpenedDocuments集合。
    ''' 这样虽然不会报错，但是不利于程序的高效。</param>
    ''' <returns>有相对应的对象，则返回之，否则则抛出异常。</returns>
    ''' <remarks></remarks>
    Friend Shared Function Create(ByVal OldWApp As OldWApplication, ByVal Doc As Document) As OldWDocument
        Dim OldWD As OldWDocument = Nothing
        ' 创建一个新的OldW文档
        If OldWDocument.IsOldWDocument(Doc) Then
            OldWD = New OldWDocument(Doc)
            OldWApp.OpenedDocuments.Add(OldWD)

        Else  ' 说明这个文档不符合OldWDocument的特征，此时为其添加对应的项目参数，使其成为一个OldWDocument对象

            If Doc.IsFamilyDocument Then
                Throw New InvalidOperationException("此文档为一个族文档，不能用来创建OldWDocument对象")
                ' MessageBox.Show("此文档为一个族文档，不能用来创建OldWDocument对象", "Warnning", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else     ' 在项目信息中添加一个新的参数
                OldWD = OldWDocument.CreateNew(OldWApp, Doc)
                OldWApp.OpenedDocuments.Add(OldWD)
            End If
        End If
        Return OldWD
    End Function

    ''' <summary>
    ''' 将一个非OldWDocument类型的文档创建成为一个OldWDocument对象，即在其项目信息中添加参数OldW_Project
    ''' </summary>
    ''' <param name="OldWApp"></param>
    ''' <param name="Doc">注意默认OldWDocument.IsOldWDocument(Doc)为false，如果此Doc的项目信息中有参数OldW_Project，则重新进行绑定可能会出错。</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function CreateNew(ByVal OldWApp As OldWApplication, ByVal Doc As Document) As OldWDocument
        Dim OldWD As OldWDocument = Nothing

        Dim app = OldWApp.Application
        Dim myGroup As DefinitionGroup = GetOldWDefinitionGroup(app)
        Dim OldWDefi As ExternalDefinition = DirectCast(myGroup.Definitions.Item(Constants.SP_OldWProjectInfo), ExternalDefinition)

        ' 创建一个“项目信息”类别，用来提供给共享参数进行绑定
        Dim myCategories As CategorySet = app.Create.NewCategorySet()
        ' use BuiltInCategory to get category of wall
        Dim myCategory As Category = Doc.Settings.Categories.Item(BuiltInCategory.OST_ProjectInformation)
        myCategories.Insert(myCategory)

        '将外部共享参数绑定到项目信息中
        Dim instanceBinding As InstanceBinding = app.Create.NewInstanceBinding(myCategories)
        Dim bindingMap As BindingMap = Doc.ParameterBindings
        ' Bind the definitions to the document
        Using resource As New Transaction(Doc, "添加项目参数")
            resource.Start()
            Dim instanceBindOK As Boolean = bindingMap.Insert(OldWDefi, instanceBinding, BuiltInParameterGroup.PG_TEXT)
            resource.Commit()
        End Using
        OldWD = New OldWDocument(Doc)
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
            Dim pa As Parameter = proInfo.Parameter(Constants.SP_Guid_OldWProjectInfo)
            If pa IsNot Nothing Then
                bln = True
            End If
        End If
        Return bln
    End Function

#End Region

#Region "   ---    OldW项目信息的保存与提取"

    ''' <summary>
    ''' 将与基坑开挖有关的信息保存到Document的相关参数中
    ''' </summary>
    ''' <param name="ProjInfo"></param>
    ''' <remarks></remarks>
    Public Sub SetProjectInfo(ByVal ProjInfo As OldWProjectInfo)
        Dim pa As Parameter = Me.Doc.ProjectInformation.Parameter(Constants.SP_Guid_OldWProjectInfo)
        Dim Info As String = StringSerializer.Encode64(Me.ProjectInfo)
        Using tran As New Transaction(Doc, "将OldW项目信息保存到Document中")
            tran.Start()
            pa.Set(Info)
            tran.Commit()
            tran.Commit()
        End Using
    End Sub

    ''' <summary>
    ''' 从Document中提取OldWDocument中保存的与基坑开挖有关的信息
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProjectInfo() As OldWProjectInfo
        Dim pa As Parameter = Me.Doc.ProjectInformation.Parameter(Constants.SP_Guid_OldWProjectInfo)
        Dim info As String = pa.AsString()
        Dim proinfo As OldWProjectInfo = DirectCast(StringSerializer.Decode64(info), OldWProjectInfo)
        Return proinfo
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

#Region "   ---    基坑开挖与回筑"

    Private F_SoilElement As Soil_Model
    ''' <summary>
    ''' 模型中的土体单元，此单元与土体
    ''' </summary>
    ''' <param name="Doc">进行土体单元搜索的文档</param>
    ''' <param name="SoilElementId">可能的土体单元的ElementId值，如果没有待选的，可以不指定，此时程序会在整个Document中进行搜索。</param>
    ''' <returns>如果成功搜索到，则返回对应的土体单元，如果没有找到，则返回Nothing</returns>
    ''' <remarks></remarks>
    Public Function FindSoilModel(Optional SoilElementId As Integer = -1) As Soil_Model
        If F_SoilElement Is Nothing Then
            Dim SE As FamilyInstance
            SE = FindSoilElement(Doc, SoilElementId)
            If SE IsNot Nothing Then
                F_SoilElement = New Soil_Model(SE)
            Else
                F_SoilElement = Nothing
            End If
        Else
            If Not F_SoilElement.Soil.IsValidObject Then
                MessageBox.Show("土体单元无效，请重新构造土体单元。")
                F_SoilElement = Nothing
            End If
        End If
        Return F_SoilElement
    End Function

    ''' <summary>
    ''' 找到模型中的开挖土体单元
    ''' </summary>
    ''' <param name="doc">进行土体单元搜索的文档</param>
    ''' <param name="SoilElementId">可能的土体单元的ElementId值</param>
    ''' <returns>如果找到有效的土体单元，则返回对应的FamilyInstance，否则返回Nothing</returns>
    ''' <remarks></remarks>
    Private Function FindSoilElement(ByVal doc As Document, SoilElementId As Integer) As FamilyInstance
        Dim Soil As FamilyInstance = Nothing
        If SoilElementId <> -1 Then  ' 说明用户手动指定了土体单元的ElementId，此时需要检测此指定的土体单元是否是有效的土体单元
            Try
                Soil = DirectCast(New ElementId(SoilElementId).Element(doc), FamilyInstance)
                ' 进行更细致的检测
                If String.Compare(Soil.Symbol.FamilyName, Constants.FamilyName_Soil, True) <> 0 Then
                    Throw New TypeUnloadedException(String.Format("指定的ElementId所对应的单元的族名称与全局的土体族的名称""{0}""不相同。", Constants.FamilyName_Soil))
                End If
            Catch ex As Exception
                MessageBox.Show(String.Format("指定的元素Id ({0})不是有效的土体单元。", SoilElementId), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Soil = Nothing
            End Try
        Else  ' 说明用户根本没有指定任何可能的土体单元，此时需要在模型中按特定的方式来搜索出土体单元
            Dim SoilFamily As Family = doc.FindFamily(Constants.FamilyName_Soil)
            If SoilFamily IsNot Nothing Then
                Dim soils As List(Of ElementId) = SoilFamily.Instances(BuiltInCategory.OST_Site).ToElementIds

                ' 整个模型中只能有一个模型土体对象
                If soils.Count = 0 Then
                    MessageBox.Show("模型中没有土体单元", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ElseIf soils.Count > 1 Then
                    Dim UIDoc As UIDocument = New UIDocument(doc)
                    UIDoc.Selection.SetElementIds(soils)
                    MessageBox.Show(String.Format("模型中的土体单元数量多于一个，请删除多余的土体单元 ( 族""{0}""的实例对象 )。", Constants.FamilyName_Soil), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    Soil = DirectCast(soils.Item(0).Element(doc), FamilyInstance)   ' 找到有效且唯一的土体单元 ^_^
                End If

            End If
        End If
        Return Soil
    End Function


    Public Function FindSoilRemove() As List(Of Family)
        Dim collector As New FilteredElementCollector(Doc)

        collector.OfClass(GetType(FamilySymbol)).OfCategory(BuiltInCategory.OST_Site)
        For Each f As FamilySymbol In collector
            MessageBox.Show(f.Name & vbCrLf & f.FamilyName)
        Next
    End Function

#End Region

End Class
