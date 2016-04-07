Imports System
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports Forms = System.Windows.Forms

Namespace rvtTools_ez.Test

    ''' <summary>
    ''' 无模态窗口的模板
    ''' 此窗口可以直接通过Form.Show来进行调用
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Template_ModelessForm
        Implements IExternalEventHandler

#Region "   ---   Declarations"

#Region "   ---   Types"

        ''' <summary>
        ''' 每一个外部事件调用时所提出的需求，为了在Execute方法中充分获取窗口的需求，
        ''' 所以将调用外部事件的窗口控件以及对应的触发事件参数也传入Execute方法中。
        ''' </summary>
        ''' <remarks></remarks>
        Private Class RequestParameter

            Private F_sender As Object
            ''' <summary> 引发Form事件控件对象 </summary>
            Public ReadOnly Property sender As Object
                Get
                    Return F_sender
                End Get
            End Property

            Private F_e As EventArgs
            ''' <summary> Form中的事件所对应的事件参数 </summary>
            Public ReadOnly Property e As EventArgs
                Get
                    Return F_e
                End Get
            End Property

            Private F_Id As Request
            ''' <summary> 具体的需求 </summary>
            Public ReadOnly Property Id As Request
                Get
                    Return F_Id
                End Get
            End Property


            ''' <summary>
            ''' 定义事件需求与窗口中引发此事件的控件对象及对应的事件参数
            ''' </summary>
            ''' <param name="RequestId">具体的需求</param>
            ''' <param name="e">Form中的事件所对应的事件参数</param>
            ''' <param name="sender">引发Form事件控件对象</param>
            ''' <remarks></remarks>
            Public Sub New(ByVal RequestId As Request, Optional e As EventArgs = Nothing, Optional ByVal sender As Object = Nothing)
                With Me
                    .F_sender = sender
                    .F_e = e
                    .F_Id = RequestId
                End With
            End Sub
        End Class

        ''' <summary>
        ''' ModelessForm的操作需求，用来从窗口向IExternalEventHandler对象传递需求。
        ''' </summary>
        ''' <remarks></remarks>
        Private Enum Request
            ''' <summary>
            ''' 与Revit用户界面进行交互。弥补了Form.ShowDialog不能进行Selection.PickObjects等操作的缺陷。
            ''' </summary>
            ''' <remarks></remarks>
            Pick
            ''' <summary>
            ''' 开启Revit事务以修改Revit文档。弥补了Form.Show后开启事务时给出报错：
            ''' “Starting a transaction from an external application running outside of APIcontext is not allowed.”的问题。
            ''' </summary>
            ''' <remarks></remarks>
            Delete
        End Enum

#End Region

#Region "   ---   Fields"

        ''' <summary>用来触发外部事件（通过其Raise方法） </summary>
        ''' <remarks>ExEvent属性是必须有的，它用来执行Raise方法以触发事件。</remarks>
        Private ExEvent As ExternalEvent

        ''' <summary> Execute方法所要执行的需求 </summary>
        ''' <remarks>在Form中要执行某一个操作时，先将对应的操作需求信息赋值为一个RequestId枚举值，然后再执行ExternalEvent.Raise()方法。
        ''' 然后Revit会在会在下个闲置时间（idling time cycle）到来时调用IExternalEventHandler.Excute方法，在这个Execute方法中，
        ''' 再通过RequestId来提取对应的操作需求，</remarks>
        Private RequestPara As RequestParameter

        Private Doc As Document

#End Region

#Region "   ---   Properties"

#End Region

#End Region

#Region "   ---   构造函数与窗口的打开关闭"

        Public Sub New(ByVal Doc As Document)
            ' This call is required by the designer.
            InitializeComponent()
            ' Add any initialization after the InitializeComponent() call.
            '' ----------------------

            Me.TopMost = True
            Me.StartPosition = FormStartPosition.CenterScreen
            Me.Doc = Doc

            '' ------ 将所有的初始化工作完成后，执行外部事件的绑定 ----------------
            ' 新建一个外部事件实例
            Me.ExEvent = ExternalEvent.Create(Me)
        End Sub

        Protected Overrides Sub OnClosed(e As EventArgs)
            ' 保存的实例需要进行释放
            Me.ExEvent.Dispose()
            Me.ExEvent = Nothing
            '
            MyBase.OnClosed(e)
        End Sub

        Public Function GetName() As String Implements IExternalEventHandler.GetName
            Return "Revit External Event & ModelessForm"
        End Function

#End Region

#Region "   ---   界面效果与事件响应"

        ''' <summary> 在Revit执行相关操作时，禁用窗口中的控件 </summary>
        Private Sub DozeOff()
            For Each c As Forms.Control In Me.Controls
                c.Enabled = False
            Next
        End Sub

        ''' <summary> 在外部事件RequestHandler中的Execute方法执行完成后，用来激活窗口中的控件 </summary>
        Private Sub WarmUp()
            For Each c As Forms.Control In Me.Controls
                c.Enabled = True
            Next
        End Sub

#End Region

#Region "   ---   执行操作 ExternalEvent.Raise 与 IExternalEventHandler.Execute"

        Private Sub Button1_Click(sender As Object, e As EventArgs) Handles BtnPick.Click
            Me.RequestPara = New RequestParameter(Request.Pick, e, sender)
            Me.ExEvent.Raise()
            Me.DozeOff()
        End Sub

        Private Sub Button2_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
            Me.RequestPara = New RequestParameter(Request.Delete, e, sender)
            Me.ExEvent.Raise()
            Me.DozeOff()
        End Sub

        ''为每一项操作执行具体的实现
        ''' <summary>
        ''' 在执行ExternalEvent.Raise()方法之前，请先将操作需求信息赋值给其RequestHandler对象的RequestId属性。
        ''' 当ExternalEvent.Raise后，Revit会在下个闲置时间（idling time cycle）到来时调用IExternalEventHandler.Execute方法的实现。
        ''' </summary>
        ''' <param name="app">此属性由Revit自动提供，其值不是Nothing，而是一个真实的UIApplication对象</param>
        ''' <remarks>由于在通过外部程序所引发的操作中，如果出现异常，Revit并不会给出任何提示或者报错，
        ''' 而是直接退出函数。所以要将整个操作放在一个Try代码块中，以处理可能出现的任何报错。</remarks>
        Public Sub Execute(app As UIApplication) Implements IExternalEventHandler.Execute
            Try  ' 由于在通过外部程序所引发的操作中，如果出现异常，Revit并不会给出任何提示或者报错，而是直接退出函数。所以这里要将整个操作放在一个Try代码块中，以处理可能出现的任何报错。

                Dim uiDoc As UIDocument = New UIDocument(Doc)

                ' 开始执行具体的操作
                Select Case RequestPara.Id  ' 判断具体要干什么
                    Case Request.Pick

                        Dim a = uiDoc.Selection.PickObject(Selection.ObjectType.Element)
                        MessageBox.Show(a.ElementId.IntegerValue.ToString)

                    Case Request.Delete

                        Dim ids As List(Of ElementId) = uiDoc.Selection.GetElementIds.ToList
                        If ids.Count = 0 Then
                            MessageBox.Show("请先选择一个元素")
                        Else
                            Dim id As ElementId = ids.First

                            Using tr As New Transaction(Doc, "删除对象")
                                If tr.Start() = TransactionStatus.Started Then
                                    Doc.Delete(id)
                                    tr.Commit()
                                End If
                            End Using
                        End If
                    Case Else
                End Select
            Catch ex As Exception
                MessageBox.Show("出错" & vbCrLf & ex.Message & vbCrLf & ex.TargetSite.Name,
                                "外部事件执行出错", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                ' 刷新Form，将Form中的Controls的Enable属性设置为True
                Me.WarmUp()
            End Try
        End Sub

#End Region

    End Class
End Namespace