Imports System
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports System.Windows.Forms
Imports std_ez
Imports OldW.GlobalSettings
Imports System.ComponentModel

Namespace OldW.DataManager
    Public Class ElementDataManager

#Region "  ---  Properties"

#End Region

#Region "  ---  Fields"
        Private doc As Document

        ''' <summary> 当前活动的图元 </summary>
        Private ActiveElement As Element
        ''' <summary> 当前活动的图元中所保存的监测数据 </summary>
        Private ActiveMonitorData As MonitorData_Point

#End Region

        ''' <summary>
        ''' 构造函数
        ''' </summary>
        ''' <param name="eleidCollection">所有要进行处理的测点元素的Id集合</param>
        ''' <param name="document"></param>
        ''' <remarks></remarks>
        Public Sub New(eleIdCollection As ICollection(Of ElementId), document As Document)
            Call InitializeComponent()
            ' --------------------------------
            Me.doc = document

            With Me.cmbx_elements
                .DisplayMember = LstbxDisplayAndItem.DisplayMember
                .ValueMember = LstbxDisplayAndItem.ValueMember
            End With
            If eleIdCollection.Count > 0 Then
                Me.cmbx_elements.DataSource = fillCombobox(eleIdCollection)
            End If
            With Me.eZDataGridView1
                .Columns.Item(0).ValueType = GetType(Date)
                .Columns.Item(1).ValueType = GetType(Object)
            End With
        End Sub

        Private Function fillCombobox(elementCollection As ICollection(Of ElementId)) As LstbxDisplayAndItem()
            Dim c As Integer = elementCollection.Count
            Dim arr(0 To c - 1) As LstbxDisplayAndItem
            Dim i As Integer = 0
            Dim ele As Element
            For Each eleid As ElementId In elementCollection
                ele = doc.GetElement(eleid)
                arr(i) = New LstbxDisplayAndItem(ele.Name & ":" & ele.Id.IntegerValue.ToString, ele)
                i += 1
            Next
            Return arr
        End Function

        Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbx_elements.SelectedIndexChanged
            Dim ele As Element = DirectCast(cmbx_elements.SelectedValue, Element)
            Me.ActiveElement = ele
            Call FillTableWithElementData(ele, Me.eZDataGridView1)
        End Sub

#Region "   ---   本地子方法"

        ''' <summary>
        ''' 将表格中的数据保存到Element的对应参数中。
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SaveTableToElement(sender As Object, e As EventArgs) Handles btnSaveChange.Click
            Dim strData As String = ""
            With Me.eZDataGridView1
                Dim RowsCount As Integer = .Rows.Count
                Dim ColsCount As Integer = .Columns.Count
                Dim arrDate(0 To RowsCount - 2) As Date, arrValue(0 To RowsCount - 2) As Object  ' 默认不读取最行一行，因为一般情况下，DataGridView中的最后一行是一行空数据
                '
                Dim row As DataGridViewRow
                For r = 0 To RowsCount - 2
                    row = .Rows.Item(r)
                    For c = 0 To ColsCount - 1
                        If Not DateTime.TryParse(row.Cells(0).Value, arrDate(r)) Then
                            TaskDialog.Show("Error", "第" & r + 1 & "行数据不能正确地转换为DateTime。")
                            Exit Sub
                        End If
                        arrValue(r) = row.Cells(1).Value
                    Next
                Next
                Dim moniData As New MonitorData_Point(arrDate, arrValue)
                strData = BinarySerializer.Encode64(moniData)
            End With

            Dim aa As Parameter = ActiveElement.Parameter(Guid_Monitor)

            Dim para As Parameter = ActiveElement.Parameter(Guid_Monitor) ' ActiveElement.Parameter_MonitorData
            Using Tran As New Transaction(doc, "保存表格中的数据到Element的参数中")
                Tran.Start()
                Try
                    para.Set(strData)
                    doc.Regenerate()
                    Tran.Commit()
                Catch ex As Exception
                    Tran.RollBack()
                End Try
            End Using
        End Sub

        ''' <summary>
        ''' 将元素的数据写入表格
        ''' </summary>
        ''' <param name="ele"></param>
        ''' <param name="Table"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function FillTableWithElementData(ele As Element, Table As DataGridView) As Boolean
            Dim blnSucceed As Boolean = True
            Dim strData As String = ele.Parameter(Guid_Monitor).AsString
            If strData.Length > 0 Then
                Dim Dt As MonitorData_Point
                Try
                    Dt = DirectCast(BinarySerializer.Decode64(strData), MonitorData_Point)
                    If (Dt.arrDate Is Nothing) OrElse (Dt.arrValue Is Nothing) Then
                        Throw New Exception
                    End If
                    Me.ActiveMonitorData = Dt
                Catch ex As Exception
                    TaskDialog.Show("Error", "参数中的数据不能正确地提取。", TaskDialogCommonButtons.Ok, TaskDialogResult.Ok)
                    Return False
                End Try
                ' 将此测点的监测数据写入表格
                Dim arrDate As Date() = Dt.arrDate
                Dim arrValue As Object() = Dt.arrValue
                Dim DataCount As Integer = Dt.arrDate.Length
                '
                If DataCount > 0 Then
                    With Me.eZDataGridView1
                        Dim originalRowsCount As Integer = .Rows.Count

                        If DataCount >= originalRowsCount Then ' 添加不够的行
                            .Rows.Add(DataCount - originalRowsCount + 1)
                        Else  ' 删除多余行
                            For i As Integer = originalRowsCount To DataCount + 2 Step -1
                                .Rows.RemoveAt(0)  ' 不能直接移除最后那一行，因为那一行是未提交的
                            Next
                        End If

                        ' 将数据写入表格
                        For r As Integer = 0 To DataCount - 1
                            .Rows.Item(r).SetValues({arrDate(r), arrValue(r)})
                        Next
                        ' 将最后一行的数据清空，即使某个单元格的ValueType为DateTime，也可以设置其值为Nothing，此时这个单元格中会显示为空。
                        .Rows.Item(.Rows.Count - 1).SetValues({Nothing, Nothing})
                    End With
                End If
            Else
                Table.Rows.Clear()
            End If
            Return blnSucceed
        End Function

        ''' <summary>
        ''' 绘制图表
        ''' </summary>
        ''' <param name="Data"></param>
        Private Function DrawData(ByVal Data As MonitorData_Point) As Chart_MonitorData
            Dim Chart1 As New Chart_MonitorData(GlobalSettings.InstrumentationType.地表隆沉)
            With Chart1
                .Series.Points.DataBindXY(Data.arrDate, Data.arrValue)
            End With
            Chart1.Show()
            Return Chart1
        End Function

#End Region

#Region "   ---   事件处理"

        Private Sub MyDataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles eZDataGridView1.DataError
            If e.Context And DataGridViewDataErrorContexts.Parsing Then
                MessageBox.Show("输入的数据不能转换为日期数据。", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                e.ThrowException = False
            End If
        End Sub

        Private Sub btnDraw_Click(sender As Object, e As EventArgs) Handles btnDraw.Click
            Call DrawData(ActiveMonitorData)
        End Sub
#End Region

    End Class
End Namespace