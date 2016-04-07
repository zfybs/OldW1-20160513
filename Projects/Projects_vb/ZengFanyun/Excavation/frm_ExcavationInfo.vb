Imports System
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports Forms = System.Windows.Forms
Imports OldW.Soil
Imports System.ComponentModel

Public Class frm_ExcavationInfo
    Implements IExternalEventHandler

#Region "   ---   Declarations"

#Region "   ---   Types"

    ''' <summary>
    ''' 每一个外部事件调用时所提出的需求
    ''' </summary>
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
        ''' 从模型中提取开挖土体的信息，并显示在列表中
        ''' </summary>
        ''' <remarks></remarks>
        GetExcavationInfo

        ''' <summary> 将列表中的某一个开挖土体的信息同步到Revit文档中的对应元素中去。 </summary>
        SynToElement

        ''' <summary> 将列表中选中的多个开挖土体的信息同步到Revit文档中的对应元素中去。 </summary>
        SynToMultipleElements

        ''' <summary> 设置选定图元在当前视图中的可见性 </summary>
        SetVisibility
    End Enum

    ''' <summary>
    ''' 列表中的每一行数据所对应的类
    ''' </summary>
    ''' <remarks></remarks>
    Private Class ExcavSoilInfo

        ''' <summary> 开挖土体对象 </summary>
        <System.ComponentModel.Browsable(False)> _
        Public Property Soil As Soil_Excav
        ''' <summary> 土体单元的Id值 </summary>
        Public Property Id As ElementId

        ''' <summary> 开挖土体开挖完成的日期 </summary>
        Public Property StartedDate As Nullable(Of Date)
        ''' <summary> 开挖土体开挖完成的日期 </summary>
        Public Property CompletedDate As Nullable(Of Date)
        ''' <summary> 开挖土体在当前视图中是否可见 </summary>
        Public Property Visible As Boolean


        Public Sub SetToDocument(tran As Transaction, View As View)
            Soil.SetExcavatedDate(tran, True, StartedDate)
            Soil.SetExcavatedDate(tran, False, CompletedDate)
            'If Hide() Then
            '    View.HideElements({Soil.Soil.Id})
            'Else
            '    View.UnhideElements({Soil.Soil.Id})
            'End If
        End Sub


    End Class

#End Region

#Region "   ---   Properties"

    Private F_SoilModel As Soil_Model
    Private ReadOnly Property SoilModel As Soil_Model
        Get
            If F_SoilModel IsNot Nothing Then
                Return F_SoilModel
            Else
                Return ExcavDoc.FindSoilModel
            End If
        End Get
    End Property

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

    Public Document As Document
    Public UIDoc As UIDocument
    Public ExcavDoc As ExcavationDoc

    ''' <summary> 用来与列表框进行交互的开挖土体信息集合 </summary>
    ''' <remarks></remarks>
    Private WithEvents ExcavSoilInfos As BindingList(Of ExcavSoilInfo)

#End Region

#End Region

#Region "   ---   构造函数与窗口的打开关闭"

    Public Sub New(ByVal ExcavDoc As ExcavationDoc)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        '' ----------------------

        Me.TopMost = True
        Me.StartPosition = FormStartPosition.CenterScreen

        '
        Me.ExcavDoc = ExcavDoc
        Me.Document = ExcavDoc.Document
        Me.UIDoc = New UIDocument(Document)
        '
        SetupGrid()

        '' ------ 将所有的初始化工作完成后，执行外部事件的绑定 ----------------

        ' 新建一个外部事件实例
        Me.ExEvent = ExternalEvent.Create(Me)
    End Sub

    Protected Overrides Sub OnClosed(e As EventArgs)
        ' 保存的实例需要进行释放
        Me.ExEvent.Dispose()
        Me.ExEvent = Nothing
        ' 不关闭，只隐藏
        MyBase.Hide()
    End Sub

    Public Function GetName() As String Implements IExternalEventHandler.GetName
        Return "Revit External Event Example"
    End Function

#End Region

#Region "   ---   界面效果与事件响应"

    ''' <summary> 将列表框进行初始化 </summary>
    Private Sub SetupGrid()

        ExcavSoilInfos = New BindingList(Of ExcavSoilInfo)

        With Me.DataGridView1
            .AutoGenerateColumns = False
            .DataSource = ExcavSoilInfos
            .AllowUserToAddRows = False

            '-------- 将已有的数据源集合中的每一个元素的不同属性在不同的列中显示出来 -------

            '-------- 为DataGridView控件中添加一列，此列与DataSource没有任何绑定关系 -------
            'Add an Unbound Column to a Data-Bound Windows Forms DataGridView Control
            ' 注意，添加此列后，DataGridView.DataSource的值并不会发生改变。
            Dim buttonColumn As New DataGridViewButtonColumn
            buttonColumn.HeaderText = "Sync"
            buttonColumn.Name = "Set"
            buttonColumn.Text = "同步"
            buttonColumn.Width = 50
            ' Use the Text property for the button text for all cells rather
            ' than using each cell's value as the text for its own button.
            buttonColumn.UseColumnTextForButtonValue = True
            DataGridView1.Columns.Add(buttonColumn)

            ' Initialize and add a text box column.
            ' 先创建一个列，然后将列中的数据与数据源集合中的某个属性相关联即可。
            Dim column As DataGridViewColumn = New DataGridViewTextBoxColumn()
            column.DataPropertyName = "Id" ' 此列所对应的数据源中的元素中的哪一个属性的名称
            column.Name = "Id"
            column.Width = 50
            DataGridView1.Columns.Add(column)

            ' Initialize and add a text box column.
            ' 先创建一个列，然后将列中的数据与数据源集合中的某个属性相关联即可。
            column = New DataGridViewTextBoxColumn()
            column.DataPropertyName = "StartedDate" ' 此列所对应的数据源中的元素中的哪一个属性的名称
            column.Name = "开始日期"
            DataGridView1.Columns.Add(column)

            ' Initialize and add a text box column.
            ' 先创建一个列，然后将列中的数据与数据源集合中的某个属性相关联即可。
            column = New DataGridViewTextBoxColumn()
            column.DataPropertyName = "CompletedDate" ' 此列所对应的数据源中的元素中的哪一个属性的名称
            column.Name = "完成日期"
            DataGridView1.Columns.Add(column)

            ' Initialize and add a check box column.
            column = New DataGridViewCheckBoxColumn()
            column.DataPropertyName = "Visible" ' 此列所对应的数据源中的元素中的哪一个属性的名称
            column.Name = "可见"
            column.Width = 50
            DataGridView1.Columns.Add(column)

        End With
    End Sub

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

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            Me.DataGridView1.SelectAll()
        Else
            Me.DataGridView1.ClearSelection()
        End If
    End Sub

#End Region

#Region "   ---   DataGridView控件的事件与处理"

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

        Select Case e.ColumnIndex
            Case 0  ' 将表格信息同步到Revit文档
                If e.RowIndex >= 0 Then ' 说明不是点击的表头位置
                    Me.RequestPara = New RequestParameter(Request.SynToElement, e, sender)
                    Me.ExEvent.Raise()
                    Me.DozeOff()
                End If

            Case 4  ' 点击复选框，控制图元是否可见的那一列
                If e.RowIndex >= 0 Then ' 说明不是点击的表头位置
                    Me.RequestPara = New RequestParameter(Request.SetVisibility, e, sender)
                    Me.ExEvent.Raise()
                    Me.DozeOff()
                End If

        End Select
    End Sub

    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged

    End Sub

    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError
        If e.Context And DataGridViewDataErrorContexts.Parsing Then
            '说明单元格中输入的数据类型不能转换为DataGridView中某列指定的ValueType类型。
            MessageBox.Show("数据类型转换出错！")
        End If
        e.ThrowException = False
    End Sub

#End Region

#Region "   ---   执行操作 ExternalEvent.Raise 与 IExternalEventHandler.Execute"

    Private Sub btnGetExcavInfo_Click(sender As Object, e As EventArgs) Handles btnGetExcavInfo.Click
        Me.RequestPara = New RequestParameter(Request.GetExcavationInfo, e, sender)
        Me.ExEvent.Raise()
        Me.DozeOff()
    End Sub
    Private Sub btn_SyncMultiple_Click(sender As Object, e As EventArgs) Handles btn_SyncMultiple.Click
        Me.RequestPara = New RequestParameter(Request.SynToMultipleElements, e, sender)
        Me.ExEvent.Raise()
        Me.DozeOff()
    End Sub

    ''' <summary>
    ''' 在执行ExternalEvent.Raise()方法之前，请先将操作需求信息赋值给其RequestHandler对象的RequestId属性。
    ''' 当ExternalEvent.Raise后，Revit会在下个闲置时间（idling time cycle）到来时调用IExternalEventHandler.Execute方法的实现。
    ''' </summary>
    ''' <param name="app">此属性由Revit自动提供，其值不是Nothing，而是一个真实的UIApplication对象</param>
    ''' <remarks>由于在通过外部程序所引发的操作中，如果出现异常，Revit并不会给出任何提示或者报错，
    ''' 而是直接退出函数。所以要将整个操作放在一个Try代码块中，以处理可能出现的任何报错。</remarks>
    Public Sub Execute(app As UIApplication) Implements IExternalEventHandler.Execute
        Try  ' 由于在通过外部程序所引发的操作中，如果出现异常，Revit并不会给出任何提示或者报错，而是直接退出函数。所以这里要将整个操作放在一个Try代码块中，以处理可能出现的任何报错。
            ' 开始执行具体的操作
            Select Case RequestPara.Id  ' 判断具体要干什么
                Case Request.GetExcavationInfo
                    Me.DataGridView1.DataSource = GetExcavSoilInfo()

                    ' ------------------------------------------------------------------------------------------------------------
                Case Request.SynToElement
                    Dim e As DataGridViewCellEventArgs = DirectCast(RequestPara.e, DataGridViewCellEventArgs)
                    Dim exsI As ExcavSoilInfo = DirectCast(DataGridView1.Rows.Item(e.RowIndex).DataBoundItem, ExcavSoilInfo)
                    Using t As New Transaction(Document, "将单个元素的信息从表格中同步到文档中")
                        t.Start()
                        exsI.SetToDocument(t, UIDoc.ActiveView)
                        t.Commit()
                    End Using


                    ' ------------------------------------------------------------------------------------------------------------
                Case Request.SynToMultipleElements
                    Dim exsI As ExcavSoilInfo
                    Using t As New Transaction(Document, "将单个元素的信息从表格中同步到文档中")
                        t.Start()
                        For Each r In Me.DataGridView1.SelectedRows
                            exsI = DirectCast(r.DataBoundItem, ExcavSoilInfo)
                            exsI.SetToDocument(t, UIDoc.ActiveView)
                        Next
                        t.Commit()
                    End Using

                    ' -------------------------------------------------------------------------------------------------------------
                Case Request.SetVisibility
                    Dim e As DataGridViewCellEventArgs = DirectCast(RequestPara.e, DataGridViewCellEventArgs)
                    Dim exsI As ExcavSoilInfo = DirectCast(DataGridView1.Rows.Item(e.RowIndex).DataBoundItem, ExcavSoilInfo)
                    Using t As New Transaction(Document, "将设置单个元素的可见性")
                        t.Start()
                        If Not exsI.Visible Then
                            UIDoc.ActiveView.HideElements({exsI.Soil.Soil.Id})
                        Else
                            UIDoc.ActiveView.UnhideElements({exsI.Soil.Soil.Id})
                        End If
                        t.Commit()
                    End Using
            End Select
        Catch ex As Exception
            MessageBox.Show("出错" & vbCrLf & ex.Message & vbCrLf & ex.TargetSite.Name & vbCrLf & ex.StackTrace,
                            "外部事件执行出错", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' 刷新Form，将Form中的Controls的Enable属性设置为True
            Me.WarmUp()
        End Try
    End Sub

    ''' <summary> 将模型中的开挖土体信息同步到列表中 </summary>
    Private Function GetExcavSoilInfo() As BindingList(Of ExcavSoilInfo)

        Dim ExcSoils As New BindingList(Of ExcavSoilInfo)
        Dim V As View = New UIDocument(Document).ActiveView
        If V IsNot Nothing Then
            Dim Ses = Me.ExcavDoc.FindExcavSoils(SoilModel)
            For Each es As Soil_Excav In Ses
                Dim EsI As New ExcavSoilInfo
                With EsI
                    .Soil = es
                    .Id = es.Soil.Id
                    .StartedDate = es.StartedDate
                    .CompletedDate = es.CompletedDate
                    .Visible = Not es.Soil.IsHidden(V)
                End With
                ExcSoils.Add(EsI)
            Next
        Else
            Throw New NullReferenceException("当前视图不可用！")
        End If
        Return ExcSoils
    End Function

#End Region


End Class