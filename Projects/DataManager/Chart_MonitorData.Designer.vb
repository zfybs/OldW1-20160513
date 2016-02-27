Imports System.Windows.Forms.DataVisualization.Charting
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Chart_MonitorData
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
        Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
        Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
        Me.Chart = New System.Windows.Forms.DataVisualization.Charting.Chart()
        CType(Me.Chart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Chart
        '
        ChartArea1.Area3DStyle.WallWidth = 1
        ChartArea1.Name = "ChartArea1"
        Me.Chart.ChartAreas.Add(ChartArea1)
        Me.Chart.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Chart.Location = New System.Drawing.Point(0, 0)
        Me.Chart.Name = "Chart"
        Series1.ChartArea = "ChartArea1"
        Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line
        Series1.CustomProperties = "EmptyPointValue=Zero"
        Series1.EmptyPointStyle.IsValueShownAsLabel = True
        Series1.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle
        Series1.Name = "Series1"
        Me.Chart.Series.Add(Series1)
        Me.Chart.Size = New System.Drawing.Size(634, 362)
        Me.Chart.TabIndex = 0
        Me.Chart.Text = "Chart1"
        '
        'Chart_MonitorData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 362)
        Me.Controls.Add(Me.Chart)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "Chart_MonitorData"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Chart_MonitorData"
        CType(Me.Chart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

End Class
