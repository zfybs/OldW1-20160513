Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports std_ez


Namespace OldW.Instrumentation

    ''' <summary>
    ''' 监测测点：包括线测点（测斜管）或点测点（地表沉降、立柱隆起、支撑轴力）等
    ''' </summary>
    ''' <remarks>
    ''' 对于点测点而言，其监测数据是在不同的时间记录的，每一个时间上都只有一个数据。所以其监测数据是一个两列的表格，第一列为时间，第二列为监测数据。
    ''' 对于线测点而言（比如测斜管），在每一个时间上都有两列数据，用来记录这一时间上，线测点中每一个位置的监测值。
    ''' </remarks>
    Public MustInherit Class Instrumentation

#Region "   ---   Properties"

        Private F_Doc As Document
        Public ReadOnly Property Doc As Document
            Get
                Return F_Doc
            End Get
        End Property

        Private F_UIDoc As UIDocument
        Public ReadOnly Property UIDoc As UIDocument
            Get
                Return F_UIDoc
            End Get
        End Property

        Private F_Monitor As FamilyInstance
        ''' <summary>
        ''' 监测仪器，对于点测点，其包括地表沉降、立柱隆起、支撑轴力等；
        ''' 对于线测点，包括测斜管
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Monitor As FamilyInstance
            Get
                Return F_Monitor
            End Get
        End Property


        Private F_Type As InstrumentationType
        ''' <summary> 监测点的测点类型，也是测点所属的族的名称 </summary>
        Public ReadOnly Property Type As InstrumentationType
            Get
                Return F_Type
            End Get
        End Property

#End Region

#Region "   ---   构造函数"


        ''' <summary>
        ''' 构造函数
        ''' </summary>
        ''' <param name="Instrumentation">所有类型的监测仪器，包括线测点（测斜管）或点测点（地表沉降、立柱隆起、支撑轴力）等</param>
        ''' <param name="Type">监测点的测点类型，也是测点所属的族的名称</param>
        ''' <remarks></remarks>
        Friend Sub New(Instrumentation As FamilyInstance, ByVal Type As InstrumentationType)

            If Instrumentation IsNot Nothing Then
                Me.F_Monitor = Instrumentation
                Me.F_Doc = Instrumentation.Document
                Me.F_UIDoc = New UIDocument(Me.Doc)
                Me.F_Type = Type
                '

            Else
                Throw New NullReferenceException("The specified element is not valid as an instrumentation.")
            End If

        End Sub
#End Region


#Region "   ---   从Element集合中过滤出监测点对象"

        ''' <summary>
        ''' 从指定的Element集合中，找出所有的监测点元素
        ''' </summary>
        ''' <param name="Elements"> 要进行搜索过滤的Element集合</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FilterInstrumentations(ByVal Doc As Document, ByVal Elements As ICollection(Of ElementId)) As List(Of Instrumentation)
            Dim Instrus As New List(Of Instrumentation)
            Dim Coll As FilteredElementCollector = New FilteredElementCollector(Doc, Elements)
            ' 集合中的族实例
            Coll = Coll.OfClass(GetType(FamilyInstance))

            ' 找到指定的Element集合中，所有的族实例
            Dim FEI As FilteredElementIterator = Coll.GetElementIterator()
            Dim strName As String
            FEI.Reset()
            Do While FEI.MoveNext()
                'add level to list
                Dim fi As FamilyInstance = TryCast(FEI.Current, FamilyInstance)
                If fi IsNot Nothing Then
                    ' 一个Element所对应的族的名称
                    strName = fi.Symbol.FamilyName
                    Dim Tp As InstrumentationType
                    If [Enum].TryParse(value:=strName, result:=Tp) Then
                        Select Case Tp
                            Case InstrumentationType.墙体测斜
                                Instrus.Add(New Instrum_Incline(fi))
                            Case InstrumentationType.支撑轴力
                                Instrus.Add(New Instrum_StrutAxialForce(fi))
                            Case InstrumentationType.地表隆沉
                                Instrus.Add(New Instrum_GroundSettlement(fi))
                            Case InstrumentationType.立柱隆沉
                                Instrus.Add(New Instrum_ColumnHeave(fi))
                        End Select
                    End If
                End If
            Loop
            Return Instrus
        End Function

        ''' <summary>
        ''' 从指定的Element集合中，找出所有的点测点元素
        ''' </summary>
        ''' <param name="Elements"> 要进行搜索过滤的Element集合</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FilterInstru_Point(ByVal Doc As Document, ByVal Elements As ICollection(Of ElementId)) As List(Of Instrum_Point)
            Dim Instrus As New List(Of Instrum_Point)
            Dim Coll As FilteredElementCollector = New FilteredElementCollector(Doc, Elements)
            ' 找到指定的Element集合中，所有的族实例
            Dim FEI As FilteredElementIterator = Coll.OfClass(GetType(FamilyInstance)).GetElementIterator()
            Dim strName As String
            FEI.Reset()
            Do While FEI.MoveNext()
                'add level to list
                Dim fi As FamilyInstance = TryCast(FEI.Current, FamilyInstance)
                If fi IsNot Nothing Then
                    ' 一个Element所对应的族的名称
                    strName = fi.Symbol.FamilyName
                    Dim Tp As InstrumentationType
                    If [Enum].TryParse(value:=strName, result:=Tp) Then
                        Select Case Tp
                            Case InstrumentationType.支撑轴力
                                Instrus.Add(New Instrum_StrutAxialForce(fi))
                            Case InstrumentationType.地表隆沉
                                Instrus.Add(New Instrum_GroundSettlement(fi))
                            Case InstrumentationType.立柱隆沉
                                Instrus.Add(New Instrum_ColumnHeave(fi))
                        End Select
                    End If
                End If
            Loop
            Return Instrus
        End Function

#End Region

    End Class

End Namespace

