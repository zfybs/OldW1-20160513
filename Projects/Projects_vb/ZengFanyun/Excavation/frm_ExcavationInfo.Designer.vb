Namespace OldW.Excavation
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frm_ExcavationInfo
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
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_ExcavationInfo))
            Me.DataGridView1 = New System.Windows.Forms.DataGridView()
            Me.btn_SyncMultiple = New System.Windows.Forms.Button()
            Me.Label1 = New System.Windows.Forms.Label()
            Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
            Me.btnGetExcavInfo = New System.Windows.Forms.Button()
            Me.BtnClearEmpty = New System.Windows.Forms.Button()
            Me.CheckBox1 = New System.Windows.Forms.CheckBox()
            Me.CheckBox_MultiVisible = New System.Windows.Forms.CheckBox()
            CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'DataGridView1
            '
            Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.DataGridView1.Location = New System.Drawing.Point(12, 41)
            Me.DataGridView1.Name = "DataGridView1"
            Me.DataGridView1.RowTemplate.Height = 23
            Me.DataGridView1.Size = New System.Drawing.Size(536, 459)
            Me.DataGridView1.TabIndex = 1
            '
            'btn_SyncMultiple
            '
            Me.btn_SyncMultiple.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btn_SyncMultiple.Location = New System.Drawing.Point(562, 477)
            Me.btn_SyncMultiple.Name = "btn_SyncMultiple"
            Me.btn_SyncMultiple.Size = New System.Drawing.Size(75, 23)
            Me.btn_SyncMultiple.TabIndex = 0
            Me.btn_SyncMultiple.Text = "同步"
            Me.btn_SyncMultiple.UseVisualStyleBackColor = True
            '
            'Label1
            '
            Me.Label1.AutoSize = True
            Me.Label1.Location = New System.Drawing.Point(12, 16)
            Me.Label1.Name = "Label1"
            Me.Label1.Size = New System.Drawing.Size(137, 12)
            Me.Label1.TabIndex = 2
            Me.Label1.Text = "当前模型中的开挖土体："
            '
            'btnGetExcavInfo
            '
            Me.btnGetExcavInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnGetExcavInfo.Location = New System.Drawing.Point(562, 41)
            Me.btnGetExcavInfo.Name = "btnGetExcavInfo"
            Me.btnGetExcavInfo.Size = New System.Drawing.Size(75, 23)
            Me.btnGetExcavInfo.TabIndex = 0
            Me.btnGetExcavInfo.Text = "开挖信息"
            Me.ToolTip1.SetToolTip(Me.btnGetExcavInfo, "提取模型中的开挖土体的信息，并显示在列表中。")
            Me.btnGetExcavInfo.UseVisualStyleBackColor = True
            '
            'BtnClearEmpty
            '
            Me.BtnClearEmpty.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.BtnClearEmpty.Location = New System.Drawing.Point(562, 417)
            Me.BtnClearEmpty.Name = "BtnClearEmpty"
            Me.BtnClearEmpty.Size = New System.Drawing.Size(75, 23)
            Me.BtnClearEmpty.TabIndex = 4
            Me.BtnClearEmpty.Text = "清理"
            Me.ToolTip1.SetToolTip(Me.BtnClearEmpty, "清理模型中，没有实例对象的开挖土体族。")
            Me.BtnClearEmpty.UseVisualStyleBackColor = True
            '
            'CheckBox1
            '
            Me.CheckBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.CheckBox1.AutoSize = True
            Me.CheckBox1.Location = New System.Drawing.Point(562, 455)
            Me.CheckBox1.Name = "CheckBox1"
            Me.CheckBox1.Size = New System.Drawing.Size(48, 16)
            Me.CheckBox1.TabIndex = 3
            Me.CheckBox1.Text = "全选"
            Me.CheckBox1.UseVisualStyleBackColor = True
            '
            'CheckBox_MultiVisible
            '
            Me.CheckBox_MultiVisible.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.CheckBox_MultiVisible.AutoSize = True
            Me.CheckBox_MultiVisible.Checked = True
            Me.CheckBox_MultiVisible.CheckState = System.Windows.Forms.CheckState.Indeterminate
            Me.CheckBox_MultiVisible.Location = New System.Drawing.Point(562, 379)
            Me.CheckBox_MultiVisible.Name = "CheckBox_MultiVisible"
            Me.CheckBox_MultiVisible.Size = New System.Drawing.Size(48, 16)
            Me.CheckBox_MultiVisible.TabIndex = 5
            Me.CheckBox_MultiVisible.Text = "可见"
            Me.CheckBox_MultiVisible.ThreeState = True
            Me.CheckBox_MultiVisible.UseVisualStyleBackColor = True
            '
            'frm_ExcavationInfo
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(649, 512)
            Me.Controls.Add(Me.CheckBox_MultiVisible)
            Me.Controls.Add(Me.BtnClearEmpty)
            Me.Controls.Add(Me.CheckBox1)
            Me.Controls.Add(Me.Label1)
            Me.Controls.Add(Me.DataGridView1)
            Me.Controls.Add(Me.btnGetExcavInfo)
            Me.Controls.Add(Me.btn_SyncMultiple)
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "frm_ExcavationInfo"
            Me.Text = "土体开挖模型"
            CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
        Friend WithEvents btn_SyncMultiple As System.Windows.Forms.Button
        Friend WithEvents Label1 As System.Windows.Forms.Label
        Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
        Friend WithEvents btnGetExcavInfo As System.Windows.Forms.Button
        Friend WithEvents CheckBox1 As System.Windows.Forms.CheckBox
        Friend WithEvents BtnClearEmpty As System.Windows.Forms.Button
        Friend WithEvents CheckBox_MultiVisible As CheckBox
    End Class
End Namespace