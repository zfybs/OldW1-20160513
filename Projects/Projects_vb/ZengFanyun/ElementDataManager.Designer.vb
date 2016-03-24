Namespace OldW.DataManager
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class ElementDataManager
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.btnSaveChange = New System.Windows.Forms.Button()
            Me.cmbx_elements = New System.Windows.Forms.ComboBox()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.eZDataGridView1 = New std_ez.eZDataGridView()
            Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.btnDraw = New System.Windows.Forms.Button()
            CType(Me.eZDataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'btnSaveChange
            '
            Me.btnSaveChange.Location = New System.Drawing.Point(298, 68)
            Me.btnSaveChange.Name = "btnSaveChange"
            Me.btnSaveChange.Size = New System.Drawing.Size(75, 23)
            Me.btnSaveChange.TabIndex = 1
            Me.btnSaveChange.Text = "保存"
            Me.btnSaveChange.UseVisualStyleBackColor = True
            '
            'cmbx_elements
            '
            Me.cmbx_elements.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbx_elements.FormattingEnabled = True
            Me.cmbx_elements.Location = New System.Drawing.Point(12, 33)
            Me.cmbx_elements.Name = "cmbx_elements"
            Me.cmbx_elements.Size = New System.Drawing.Size(165, 20)
            Me.cmbx_elements.TabIndex = 2
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(12, 9)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(65, 12)
            Me.Label1.TabIndex = 3
            Me.Label1.Text = "操作的测点"
            '
            'eZDataGridView1
            '
            DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
            DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle1.Font = New System.Drawing.Font("SimSun", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
            DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            Me.eZDataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
            Me.eZDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.eZDataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
            Me.eZDataGridView1.Location = New System.Drawing.Point(12, 68)
            Me.eZDataGridView1.Name = "eZDataGridView1"
            DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
            DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
            DataGridViewCellStyle2.Font = New System.Drawing.Font("SimSun", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
            DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
            DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
            DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.eZDataGridView1.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
            Me.eZDataGridView1.RowHeadersWidth = 52
            Me.eZDataGridView1.RowTemplate.Height = 23
            Me.eZDataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
            Me.eZDataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            Me.eZDataGridView1.Size = New System.Drawing.Size(280, 359)
            Me.eZDataGridView1.TabIndex = 0
            '
            'Column1
            '
            Me.Column1.HeaderText = "日期"
            Me.Column1.Name = "Column1"
            Me.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
            '
            'Column2
            '
            Me.Column2.HeaderText = "数据"
            Me.Column2.Name = "Column2"
            Me.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
            '
            'btnDraw
            '
            Me.btnDraw.Location = New System.Drawing.Point(298, 97)
            Me.btnDraw.Name = "btnDraw"
            Me.btnDraw.Size = New System.Drawing.Size(75, 23)
            Me.btnDraw.TabIndex = 1
            Me.btnDraw.Text = "绘图"
            Me.btnDraw.UseVisualStyleBackColor = True
            '
            'ElementDataManager
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(382, 434)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.cmbx_elements)
            Me.Controls.Add(Me.btnDraw)
            Me.Controls.Add(Me.btnSaveChange)
            Me.Controls.Add(Me.eZDataGridView1)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Name = "ElementDataManager"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "测点数据编辑"
            CType(Me.eZDataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents eZDataGridView1 As std_ez.eZDataGridView
        Friend WithEvents btnSaveChange As System.Windows.Forms.Button
        Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents cmbx_elements As System.Windows.Forms.ComboBox
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents btnDraw As System.Windows.Forms.Button
    End Class
End Namespace