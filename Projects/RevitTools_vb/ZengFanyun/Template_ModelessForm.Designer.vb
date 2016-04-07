
Namespace rvtTools_ez.Test
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class Template_ModelessForm
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
            Me.BtnPick = New System.Windows.Forms.Button()
            Me.BtnDelete = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'BtnPick
            '
            Me.BtnPick.Location = New System.Drawing.Point(32, 12)
            Me.BtnPick.Name = "BtnPick"
            Me.BtnPick.Size = New System.Drawing.Size(75, 23)
            Me.BtnPick.TabIndex = 0
            Me.BtnPick.Text = "选择"
            Me.BtnPick.UseVisualStyleBackColor = True
            '
            'BtnDelete
            '
            Me.BtnDelete.Location = New System.Drawing.Point(122, 12)
            Me.BtnDelete.Name = "BtnDelete"
            Me.BtnDelete.Size = New System.Drawing.Size(75, 23)
            Me.BtnDelete.TabIndex = 0
            Me.BtnDelete.Text = "删除"
            Me.BtnDelete.UseVisualStyleBackColor = True
            '
            'ModelessForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(227, 51)
            Me.Controls.Add(Me.BtnDelete)
            Me.Controls.Add(Me.BtnPick)
            Me.Name = "ModelessForm"
            Me.Text = "ModelessForm"
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents BtnPick As System.Windows.Forms.Button
        Friend WithEvents BtnDelete As System.Windows.Forms.Button
    End Class
End Namespace