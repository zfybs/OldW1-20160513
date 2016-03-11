Imports OldW.DataManager
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports std_ez

''' <summary>
''' 所有类型的监测点，包括地表沉降、立柱隆起、支撑轴力等，但不包括测斜管
''' </summary>
''' <remarks></remarks>
Public Class Instru_Point
    Inherits Instrumentation

#Region "   ---   Properties"

    Private F_MonitorData As MonitorData_Point
    ''' <summary>
    ''' 点测点的整个施工阶段中的监测数据
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MonitorData As MonitorData_Point
        Get
            If F_MonitorData Is Nothing Then
                F_MonitorData = GetData()
            End If
            Return Me.F_MonitorData
        End Get
        Set(value As MonitorData_Point)
            Me.F_MonitorData = value
        End Set
    End Property


#End Region
    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="MonitorPoint">所有类型的监测点，包括地表沉降、立柱隆起、支撑轴力等，但不包括测斜管</param>
    ''' <param name="Type">监测点的测点类型，也是测点所属的族的名称</param>
    ''' <remarks></remarks>
    Public Sub New(MonitorPoint As FamilyInstance, Optional ByVal Type As InstrumentationType = InstrumentationType.地表隆沉)
        MyBase.New(MonitorPoint, Type)

    End Sub


    Private Function GetData() As MonitorData_Point
        Dim strData As String = Monitor.Parameter(Guid_Monitor).AsString
        Dim MData As MonitorData_Point = Nothing
        If strData.Length > 0 Then
            Try
                MData = DirectCast(BinarySerializer.Decode64(strData), MonitorData_Point)
                If (MData.arrDate Is Nothing) OrElse (MData.arrValue Is Nothing) Then
                    Throw New Exception
                End If
                Return MData
            Catch ex As Exception
                TaskDialog.Show("Error", Me.Name & " (" & Me.Monitor.Id.IntegerValue.ToString & ")" & " 中的监测数据不能正确地提取。", TaskDialogCommonButtons.Ok, TaskDialogResult.Ok)
                Return Nothing
            End Try
        End If
        Return MData
    End Function


    ''' <summary>
    ''' 将监测数据以序列化字符串保存到对应的Parameter对象中。
    ''' </summary>
    ''' <remarks></remarks>
    Public Function SaveData() As Boolean
        If Me.MonitorData IsNot Nothing Then
            ' 将数据序列化为字符串
            Dim strData As String = BinarySerializer.Encode64(Me.MonitorData)
            Dim para As Parameter = Monitor.Parameter(Guid_Monitor) ' ActiveElement.Parameter_MonitorData
            Using Tran As New Transaction(Doc, "保存表格中的数据到Element的参数中")
                Try
                    Tran.Start()
                    para.Set(strData)
                    Tran.Commit()
                    Return True
                Catch ex As Exception
                    Tran.RollBack()
                    Return False
                End Try
            End Using
        Else

            Return False
        End If
    End Function

End Class