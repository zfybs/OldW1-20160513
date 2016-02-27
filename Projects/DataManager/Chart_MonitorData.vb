Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Windows.Forms
Imports System.Drawing
Imports OldW.GlobalConstant
Public Class Chart_MonitorData

#Region "  ---  Properties"

#End Region

#Region "  ---  Fields"

    ''' <summary> 数据图表 </summary>
    Friend WithEvents Chart As Chart
    Friend WithEvents Series As Series

#End Region

#Region "  ---  构造函数与窗口的开启与关闭"

    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="Title">窗口的标题：监测类型</param>
    ''' <remarks></remarks>
    Public Sub New(Type As OldW.GlobalConstant.FamilyName)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        '
        Me.KeyPreview = True
        '
        Me.Text = Type.ToString
        Call SetupChart()

        Select Case Type  ' 对于不同的监测数据类型，设置不同的图表格式
            Case FamilyName.测斜测点
                Chart.Size = New Size(400, 650)

            Case FamilyName.沉降测点
                Chart.Size = New Size(650, 400)

            Case FamilyName.轴力测点
                Chart.Size = New Size(650, 400)

        End Select

    End Sub

    ''' <summary>
    ''' 图表的初始化
    ''' </summary>
    Private Sub SetupChart()
        With Chart
            Me.Series = .Series("Series1")


            ' 设置空数据点的格式
            Dim pst As New DataPointCustomProperties
            With pst

            End With
            '  Me.Series.EmptyPointStyle = pst


        End With
    End Sub

#End Region

#Region "  ---  事件"

    Private Sub Chart_MonitorData_KeyDown(sender As Object, e As Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Escape Then
            Me.Close()
        End If
    End Sub

#End Region

End Class
