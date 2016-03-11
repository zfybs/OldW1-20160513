Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports std_ez
Namespace OldW.DataManager

#Region "  ---   监测数据类"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    <System.Serializable()> Public Class MonitorData_Point
        Public Property arrDate As Date()
        Public Property arrValue As Object()
        Public Sub New(ArrayDate As Date(), ArrayValue As Object())
            With Me
                .arrDate = ArrayDate
                .arrValue = ArrayValue
            End With
        End Sub
    End Class

    ''' <summary>
    ''' 线测点中的每一天的监测数据
    ''' </summary>
    ''' <remarks></remarks>
    <System.Serializable()> Public Class MonitorData_Line

        Private AllData As SortedDictionary(Of Date, MonitorData_Length)
        Public ReadOnly Property Data As SortedDictionary(Of Date, MonitorData_Length)
            Get
                Return AllData
            End Get
        End Property

        Public Sub New(AllData As SortedDictionary(Of Date, MonitorData_Length))
            Me.AllData = AllData
        End Sub
    End Class

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    <System.Serializable()> Public Class MonitorData_Length
        Public Property arrDistance As Single()
        Public Property arrValue As Object()
        Public Sub New(ArrayDistance As Single(), ArrayValue As Object())
            With Me
                .arrDistance = ArrayDistance
                .arrValue = ArrayValue
            End With
        End Sub
    End Class
#End Region
End Namespace