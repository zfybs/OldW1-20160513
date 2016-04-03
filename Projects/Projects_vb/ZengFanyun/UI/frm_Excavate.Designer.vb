<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Excavate
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
        Me.btn_DrawExcav = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.ElementId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ExcavatedDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BottomElevation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btn_ModelSoil = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TopElevation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.btn_SpecifyExcav = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btn_DrawExcav
        '
        Me.btn_DrawExcav.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_DrawExcav.Location = New System.Drawing.Point(483, 70)
        Me.btn_DrawExcav.Name = "btn_DrawExcav"
        Me.btn_DrawExcav.Size = New System.Drawing.Size(75, 23)
        Me.btn_DrawExcav.TabIndex = 0
        Me.btn_DrawExcav.Text = "绘制开挖"
        Me.ToolTip1.SetToolTip(Me.btn_DrawExcav, "在Revit界面中通过模型线来绘制开挖土体单元")
        Me.btn_DrawExcav.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ElementId, Me.ExcavatedDate, Me.BottomElevation, Me.TopElevation})
        Me.DataGridView1.Location = New System.Drawing.Point(12, 41)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowTemplate.Height = 23
        Me.DataGridView1.Size = New System.Drawing.Size(457, 410)
        Me.DataGridView1.TabIndex = 1
        '
        'ElementId
        '
        Me.ElementId.HeaderText = "Id"
        Me.ElementId.Name = "ElementId"
        '
        'ExcavatedDate
        '
        Me.ExcavatedDate.HeaderText = "开挖日期"
        Me.ExcavatedDate.Name = "ExcavatedDate"
        '
        'BottomElevation
        '
        Me.BottomElevation.HeaderText = "底部标高"
        Me.BottomElevation.Name = "BottomElevation"
        '
        'btn_ModelSoil
        '
        Me.btn_ModelSoil.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_ModelSoil.Location = New System.Drawing.Point(483, 41)
        Me.btn_ModelSoil.Name = "btn_ModelSoil"
        Me.btn_ModelSoil.Size = New System.Drawing.Size(75, 23)
        Me.btn_ModelSoil.TabIndex = 0
        Me.btn_ModelSoil.Text = "基坑土体"
        Me.btn_ModelSoil.UseVisualStyleBackColor = True
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
        'TopElevation
        '
        Me.TopElevation.HeaderText = "顶部标高"
        Me.TopElevation.Name = "TopElevation"
        '
        'btn_SpecifyExcav
        '
        Me.btn_SpecifyExcav.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btn_SpecifyExcav.Location = New System.Drawing.Point(483, 99)
        Me.btn_SpecifyExcav.Name = "btn_SpecifyExcav"
        Me.btn_SpecifyExcav.Size = New System.Drawing.Size(75, 23)
        Me.btn_SpecifyExcav.TabIndex = 0
        Me.btn_SpecifyExcav.Text = "指定开挖"
        Me.ToolTip1.SetToolTip(Me.btn_SpecifyExcav, "在Revit界面中指定要作为开挖土体的单元")
        Me.btn_SpecifyExcav.UseVisualStyleBackColor = True
        '
        'frm_Excavate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(570, 463)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.btn_ModelSoil)
        Me.Controls.Add(Me.btn_SpecifyExcav)
        Me.Controls.Add(Me.btn_DrawExcav)
        Me.Name = "frm_Excavate"
        Me.Text = "土体开挖模型"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btn_DrawExcav As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents btn_ModelSoil As System.Windows.Forms.Button
    Friend WithEvents ElementId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ExcavatedDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BottomElevation As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TopElevation As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btn_SpecifyExcav As System.Windows.Forms.Button
End Class
