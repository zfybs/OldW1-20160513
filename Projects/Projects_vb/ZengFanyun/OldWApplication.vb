Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports rvtTools_ez
Imports OldW.Soil
Imports System.Linq
Imports OldW

''' <summary>
''' OldW程序中的应用程序级的操作。通常用来存储或者处理Document或者Element之上的对象。比如程序中所有打开的OldWDocument对象。
''' </summary>
''' <remarks></remarks>
Public Class OldWApplication
    Private WithEvents uip As UIApplication
#Region "   ---   Properties"

    ''' <summary>
    ''' 当前正在运行的Revit的Application程序对象
    ''' </summary>
    ''' <remarks></remarks>
    Private WithEvents F_Application As Autodesk.Revit.ApplicationServices.Application
    ''' <summary>
    ''' 当前正在运行的Revit的Application程序对象
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>在每一次通过IExternalCommand接口执行的外部命令中，都可以从中提取出一个Application对象，
    ''' 从变量上来说，每次的这个Application之间都是 not equal的，但是，这些Application对象都是代表Revit当前正在运行的应用程序，即其本质上是相同的。</remarks>
    Public ReadOnly Property Application As Autodesk.Revit.ApplicationServices.Application
        Get
            Return F_Application
        End Get
    End Property

    ''' <summary>
    ''' 整个系统中，所有打开的 OldWDocument 文档
    ''' </summary>
    ''' <remarks></remarks>
    Private F_OpenedDocuments As New List(Of OldWDocument)
    ''' <summary>
    ''' 整个系统中，所有打开的 OldWDocument 文档
    ''' </summary>
    ''' <remarks></remarks>
    Public ReadOnly Property OpenedDocuments As List(Of OldWDocument)
        Get
            Return Me.F_OpenedDocuments
        End Get
    End Property

    Private F_ActiveDocument As OldWDocument
    Public ReadOnly Property ActiveDocument As OldWDocument
        Get
            Return F_ActiveDocument
        End Get
    End Property

#End Region

#Region "   ---   Fields"


#End Region

#Region "   ---   构造函数：构造唯一实例的经典思路。其实例可以通过OldWApplication.Create方法获得。"

    ''' <summary>
    ''' 程序中已经加载进来的唯一的OldWApplication实例
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared LoadedApplication As OldWApplication
    ''' <summary>
    ''' 程序中是否已经有加载过OldWApplication对象。
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared IsLoaded As Boolean

    ''' <summary>
    ''' OldWApplication类在整个程序中只有一个实例，为了保证这一点，会在Create方法中进行判断，
    ''' 看在程序中是否已经存在对应的OldWApplication实例，如果有，则直接返回，如果没有，则创建一个新的。
    ''' </summary>
    ''' <param name="App"></param>
    ''' <returns>返回程序中唯一的那一个OldWApplication对象，如果不能正常返回，则抛出异常。</returns>
    ''' <remarks>由于OldWApplication中，对Revit的Application对象进行了很多事件处理，
    ''' 所以，如果程序中有n个OldWApplication实例，那么，每一次触发Revit的Application中的事件，
    ''' 在OldWApplication中，每一个实例都会对此事件进行一次操作，这会极大地造成程序的混乱。</remarks>
    Friend Shared Function Create(ByVal App As Autodesk.Revit.ApplicationServices.Application) As OldWApplication
        If Not IsLoaded Then
            LoadedApplication = New OldWApplication(App)
            IsLoaded = True
            Return LoadedApplication
        Else  ' 直接返回现有的
            If LoadedApplication.Application.IsValidObject Then
                Return LoadedApplication
            Else
                MessageBox.Show("在程序测试中，出现上一个外部命令的Application对象的IsValidObject属性为False的情况，请及时调试并解决", "warnning")
                LoadedApplication = New OldWApplication(App)
                Return LoadedApplication
            End If
        End If
    End Function

    ''' <summary>
    ''' 为了确保程序中只有唯一的OldWApplication实例，应该将其New方法设置为Private，然后将Create设置为Shared。
    ''' </summary>
    ''' <param name="App"></param>
    ''' <remarks></remarks>
    Private Sub New(ByVal App As Autodesk.Revit.ApplicationServices.Application)
        If App.IsValidObject Then
            Me.F_Application = App
        Else
            Throw New ArgumentException("The specified application to construct an instance of OldWApplication is not a valid object")
        End If
    End Sub

#End Region

#Region "   ---    整个程序中打开的 OldWDocument 文档的集合"

    ''' <summary> 在整个系统的集合中，搜索是否有与指定的Document相对应的OldWDocument对象，如果没有，则返回Nothing。 </summary>
    ''' <param name="Doc"></param>
    ''' <param name="ClosingDocumentIndex">即将删除的Document文档中程序的 OpenedDocuments 集合中的下标位置，如果此Document文档不在OpenedDocuments集合中，则其值为-1。</param>
    ''' <returns>有相对应的对象，则返回之，否则返回Nothing。</returns>
    ''' <remarks></remarks>
    Public Function SearchOldWDocument(ByVal Doc As Document, Optional ByRef ClosingDocumentIndex As Integer = -1) As OldWDocument
        Dim OldWD As OldWDocument = Nothing
        ' 搜索是否有对应的 OldWDocument 对象
        Dim od As OldWDocument = Nothing
        For ClosingDocumentIndex = 0 To OpenedDocuments.Count - 1
            od = OpenedDocuments.Item(ClosingDocumentIndex)
            If od.Equals(Doc) Then
                OldWD = od
                Exit For
            End If
        Next
        If OldWD Is Nothing Then
            ClosingDocumentIndex = -1
        End If
        Return OldWD
    End Function

    ''' <summary>
    ''' 在 Application.DocumentClosing 事件中，即将要关闭的那个Document的Id值。
    ''' 对于同一个Revit文档，在其每次触发 Application.DocumentClosing事件时，其对应的DocumentId值都是不一样的。
    ''' 这个DocumentId值只是为了与对应的Application.DocumentClosed事件中的DocumentId值进行匹配。
    ''' </summary>
    ''' <remarks></remarks>
    Private DocumentIdTobeClosed As Integer
    ''' <summary>
    ''' 即将删除的Document文档中程序的 OpenedDocuments 集合中的下标位置，如果此Document文档不在OpenedDocuments集合中，则其值为-1。
    ''' </summary>
    Private DocumentIndexTobeClosed As Integer = -1

    Private Sub App_DocumentClosing(sender As Object, e As Autodesk.Revit.DB.Events.DocumentClosingEventArgs) Handles F_Application.DocumentClosing
        DocumentIdTobeClosed = e.DocumentId
        SearchOldWDocument(e.Document, DocumentIndexTobeClosed)
    End Sub

    Private Sub App_DocumentClosed(sender As Object, e As Autodesk.Revit.DB.Events.DocumentClosedEventArgs) Handles F_Application.DocumentClosed
        If e.Status = Autodesk.Revit.DB.Events.RevitAPIEventStatus.Succeeded Then
            Dim ClosedDocumentId As Integer = e.DocumentId
            ' This Id is only used to identify the document for the duration of this event and the DocumentClosing event which preceded it. 
            'It serves as synchronization means for the pair of events. 
            If e.DocumentId = DocumentIdTobeClosed AndAlso (DocumentIndexTobeClosed > 0) Then
                ' 如果这个文档确实是被删除了，而且这个文档确实是位于OldWDocument集合中，
                ' 则就要要 从全局集合 OpenedDocuments 集合中删除对应的元素
                OpenedDocuments.RemoveAt(DocumentIndexTobeClosed)
            End If
            '
        End If
    End Sub

#End Region

End Class
