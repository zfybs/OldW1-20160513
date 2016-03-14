Imports OldW.DataManager
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings

Namespace OldW.Instrumentation

    ''' <summary>
    ''' 所有类型的线监测，包括测斜管
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Instrum_Line
        Inherits Instrumentation
#Region "   ---   Properties"


        ''' <summary>
        ''' 线测点的整个施工阶段中的监测数据
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MonitorData As SortedDictionary(Of Date, Dictionary(Of Single, Object))

#End Region

        ''' <summary>
        ''' 构造函数
        ''' </summary>
        ''' <param name="MonitorLine">所有类型的监测管线，包括测斜管，但不包括地表沉降、立柱隆起、支撑轴力等</param>
        ''' <param name="Type">监测点的测点类型，也是测点所属的族的名称</param>
        ''' <remarks></remarks>
        Public Sub New(MonitorLine As FamilyInstance, Optional ByVal Type As InstrumentationType = InstrumentationType.墙体测斜)
            MyBase.New(MonitorLine, Type)

        End Sub


        Public Function GetData() As MonitorData_Line
            Return Nothing
        End Function


    End Class
End Namespace