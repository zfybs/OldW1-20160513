Imports System.Windows.Forms


''' <summary>
''' 自定义控件：DataGridView，向其中增加了：插入行、删除行、显示行号等功能
''' </summary>
''' <remarks></remarks>
Public Class eZDataGridView
    Inherits System.Windows.Forms.DataGridView

    Private components As System.ComponentModel.IContainer
    Private WithEvents ToolStripMenuItemInsert As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents ToolStripMenuItemRemove As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents CMS_DeleteRows As System.Windows.Forms.ContextMenuStrip
    Private WithEvents ToolStripMenuItemRemoveRows As System.Windows.Forms.ToolStripMenuItem
    Private WithEvents CMS_RowHeader As System.Windows.Forms.ContextMenuStrip

#Region "  ---  控件的初始属性"

    Public Sub New()
        Call InitializeComponent()
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.CMS_RowHeader = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemInsert = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItemRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.CMS_DeleteRows = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItemRemoveRows = New System.Windows.Forms.ToolStripMenuItem()
        Me.CMS_RowHeader.SuspendLayout()
        Me.CMS_DeleteRows.SuspendLayout()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CMS_RowHeader
        '
        Me.CMS_RowHeader.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemInsert, Me.ToolStripMenuItemRemove})
        Me.CMS_RowHeader.Name = "CMS_RowHeader"
        Me.CMS_RowHeader.ShowImageMargin = False
        Me.CMS_RowHeader.Size = New System.Drawing.Size(76, 48)
        '
        'ToolStripMenuItemInsert
        '
        Me.ToolStripMenuItemInsert.Name = "ToolStripMenuItemInsert"
        Me.ToolStripMenuItemInsert.Size = New System.Drawing.Size(75, 22)
        Me.ToolStripMenuItemInsert.Text = "插入"
        '
        'ToolStripMenuItemRemove
        '
        Me.ToolStripMenuItemRemove.Name = "ToolStripMenuItemRemove"
        Me.ToolStripMenuItemRemove.Size = New System.Drawing.Size(75, 22)
        Me.ToolStripMenuItemRemove.Text = "移除"
        '
        'CMS_DeleteRows
        '
        Me.CMS_DeleteRows.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItemRemoveRows})
        Me.CMS_DeleteRows.Name = "CMS_RowHeader"
        Me.CMS_DeleteRows.ShowImageMargin = False
        Me.CMS_DeleteRows.Size = New System.Drawing.Size(112, 26)
        '
        'ToolStripMenuItemRemoveRows
        '
        Me.ToolStripMenuItemRemoveRows.Name = "ToolStripMenuItemRemoveRows"
        Me.ToolStripMenuItemRemoveRows.Size = New System.Drawing.Size(111, 22)
        Me.ToolStripMenuItemRemoveRows.Text = "删除所选行"
        '
        'myDataGridView
        '
        Me.ColumnHeadersHeight = 25
        Me.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.RowTemplate.Height = 23
        Me.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.Size = New System.Drawing.Size(346, 110)
        Me.CMS_RowHeader.ResumeLayout(False)
        Me.CMS_DeleteRows.ResumeLayout(False)
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region "  ---  显示行号"

    ''' <summary>
    ''' 行数改变时的事件：显示行号
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub myDataGridView_RowsNumberChanged(sender As Object, e As Object) Handles Me.RowsAdded, Me.RowsRemoved
        Dim longRow As Long
        For longRow = e.RowIndex + e.RowCount - 1 To Me.Rows.GetLastRow(DataGridViewElementStates.Displayed)
            Me.Rows(longRow).HeaderCell.Value = (longRow + 1).ToString
        Next
    End Sub

    ''' <summary>
    ''' 设置新添加的一行的Resizable属性为False
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RowsResizable(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles Me.RowsAdded
        Me.Rows.Item(e.RowIndex).Resizable = DataGridViewTriState.False
    End Sub

    Private Sub myDataGridView_RowStateChanged(sender As Object, e As DataGridViewRowStateChangedEventArgs) Handles Me.RowStateChanged
        e.Row.HeaderCell.Value = (e.Row.Index + 1).ToString
    End Sub

#End Region

#Region "  ---  右键菜单的关联与显示"

    Private Sub myDataGridView_RowHeaderMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Me.RowHeaderMouseClick
        '如果是右击
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If e.RowIndex <> Me.Rows.Count Then
                '如果行数只有一行
                With Me.ToolStripMenuItemRemove
                    If Me.Rows.Count <= 1 Then
                        .Enabled = False
                    Else
                        .Enabled = True
                    End If
                End With
                '选择右击项的那一行
                Me.ClearSelection()
                Me.Rows.Item(e.RowIndex).Selected = True
                '显示菜单栏
                With CMS_RowHeader
                    .Show()
                    .Left = MousePosition.X
                    .Top = MousePosition.Y
                End With
            End If
        End If
    End Sub

    Private Sub myDataGridView_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles Me.CellMouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then

            Dim R As Integer = e.RowIndex
            Dim C As Integer = e.ColumnIndex
            If R >= 0 And C >= 0 Then
                '显示菜单栏
                If Me.SelectedRows.Count = 0 OrElse Me.Rows.Count < 2 Then
                    Me.ToolStripMenuItemRemoveRows.Enabled = False
                Else
                    Me.ToolStripMenuItemRemoveRows.Enabled = True
                End If
                With CMS_DeleteRows
                    .Show()
                    .Left = MousePosition.X
                    .Top = MousePosition.Y
                End With
            End If
        End If

    End Sub

#End Region

#Region "  ---  行的插入与删除"

    ''' <summary>
    ''' 插入一行
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub InsertRow(sender As Object, e As EventArgs) Handles ToolStripMenuItemInsert.Click
        With Me
            Dim SelectedIndex As Integer = .SelectedRows(0).Index
            .Rows.Insert(SelectedIndex, 1)
        End With
    End Sub

    ''' <summary>
    ''' 移除一行
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RemoveRow(sender As Object, e As EventArgs) Handles ToolStripMenuItemRemove.Click
        With Me
            Dim Row = .SelectedRows.Item(0)
            If Row.Index < .Rows.Count - 1 Then
                '当删除最后一行（不带数据，自动添加的行）时会报错：无法删除未提交的新行。
                .Rows.Remove(Row)
            End If
        End With
    End Sub

    ''' <summary>
    ''' 移除多行
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripMenuItemRemoveRows_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItemRemoveRows.Click
        With Me
            '下面的 For Each 是从下往上索引的，即前面的Row对象的index的值大于后面的Index的值
            For Each Row As DataGridViewRow In .SelectedRows
                If Row.Index < .Rows.Count - 1 Then
                    '当删除最后一行（不带数据，自动添加的行）时会报错：无法删除未提交的新行。
                    .Rows.Remove(Row)
                End If
            Next
        End With
    End Sub

#End Region

#Region "  ---  数据的复制与粘贴"

    ''' <summary>
    ''' 如下按下Ctrl+V，则将表格中的数据粘贴到DataGridView控件中
    ''' </summary>
    ''' <remarks>DataGridView表格的索引：行号：表头为-1，第一行为0，列号：表示行编号的列为-1，第一个数据列的列号为0.
    ''' DataGridView.Rows.Count与DataGridView.Columns.Count均只计算数据区域，而不包含表头与列头。</remarks>
    Private Sub myDataGridView_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Delete Then
            ' 删除选择的单元格中的数据
            For Each c As DataGridViewCell In Me.SelectedCells
                c.Value = Nothing
            Next

        ElseIf e.Control And e.KeyCode = Keys.V Then
            Dim a = Me.SelectedCells
            Dim count = a.Count
            'Dim s As String = "行号" & vbTab & "列号" & vbCrLf
            'For i = 0 To count - 1
            '    s = s & a.Item(i).RowIndex & vbTab & a.Item(i).ColumnIndex & vbCrLf
            'Next
            'MessageBox.Show(s)
            If count <> 1 Then
                MessageBox.Show("请选择某一个单元格，来作为粘贴的起始位置。", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            Dim c As DataGridViewCell = Me.SelectedCells.Item(0)
            Dim rownum As Integer = c.RowIndex
            Call PasteFromTable(c.RowIndex, c.ColumnIndex)
        End If
    End Sub

    ''' <summary> 将表格中的数据粘贴到DataGridView控件中 </summary>
    ''' <param name="startRow">粘贴的起始单元格的行位置</param>
    ''' <param name="startCol">粘贴的起始单元格的列位置</param>
    ''' <remarks>DataGridView表格的索引：行号：表头为-1，第一行为0，列号：表示行编号的列为-1，第一个数据列的列号为0.
    ''' DataGridView.Rows.Count与DataGridView.Columns.Count均只计算数据区域，而不包含表头与列头。总行数包括最后一行空数据行。</remarks>
    Private Sub PasteFromTable(startRow As Integer, startCol As Integer)
        Dim pastTest As String = Clipboard.GetText
        If String.IsNullOrEmpty(pastTest) Then Exit Sub
        'excel中是以"空格"和"换行"来当做字段和行，所以用"\r\n"来分隔，即"回车+换行"
        Dim lines As String() = pastTest.Split(New Char() {Chr(13), Chr(10)}, _
                                               StringSplitOptions.RemoveEmptyEntries)
        Try
            'For Each line As String In lines
            '    '在每一行的单元格间，作为单元格的分隔的字符为"\t",即水平换行符
            '    Dim strs As String() = line.Split(Chr(9))
            '    Me.Rows.Add(strs)
            'Next
            ' MessageBox.Show(startRow & startCol & Me.Rows.Count & Me.Columns.Count)
            '
            Dim WriteRowsCount As Integer = lines.Length  '要写入多少行数据
            Dim WriteColsCount As Integer = lines(0).Split(Chr(9)).Length  '要写入的每一行数据中有多少列
            '
            Dim endRow As Integer = startRow + WriteRowsCount - 1  ' 要修改的最后一行的行号
            If endRow > Me.Rows.Count - 2 Then  ' 说明要额外添加这么多行才能放置要粘贴进来的数据
                Me.Rows.Add(endRow + 2 - Me.Rows.Count)
            End If
            Dim endCol As Integer  ' 要修改的最后面的那一列的列号
            endCol = If(startCol + WriteColsCount <= Me.Columns.Count, startCol + WriteColsCount - 1, Me.Columns.Count - 1)
            '
            Dim strline As String
            Dim strs As String()
            For r As Integer = startRow To endRow
                strline = lines(r - startRow)
                strs = strline.Split(Chr(9)) '在每一行的单元格间，作为单元格的分隔的字符为"\t",即水平换行符
                For c As Integer = startCol To endCol
                    Me.Rows(r).Cells(c).Value = strs(c - startCol)
                Next
            Next
        Catch ex As Exception

        End Try
    End Sub

#End Region

End Class
