Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports rvtTools_ez

Namespace OldW.Soil

    ''' <summary> 用来模拟分块开挖的土体元素。 </summary>
    ''' <remarks></remarks>
    Public Class Soil_Excav
        Inherits Soil_Element

#Region "   ---   Properties"

        ''' <summary> 开挖土体单元所附着的模型土体 </summary>
        Private F_ModelSoil As Soil_Model
        ''' <summary> 开挖土体单元所附着的模型土体 </summary>
        Public ReadOnly Property ModelSoil As Soil_Model
            Get
                Return F_ModelSoil
            End Get
        End Property

        Private F_CompletedDate As Nullable(Of DateTime)
        ''' <summary>
        ''' 每一个开挖土体都有一个开挖完成的时间。由于记录的不完整，这个时间可能暂时不知道，但是后期要可以指定。
        ''' </summary>
        Public ReadOnly Property CompletedDate As Nullable(Of DateTime)
            Get
                If F_CompletedDate Is Nothing Then
                    F_CompletedDate = GetExcavatedDate(False)
                End If
                Return F_CompletedDate
            End Get
        End Property

        Private F_StartedDate As Nullable(Of DateTime)
        ''' <summary>
        ''' 每一个开挖土体都有一个开始开挖的时间。由于记录的不完整，这个时间可能暂时不知道，但是后期要可以指定。
        ''' </summary>
        Public ReadOnly Property StartedDate As Nullable(Of DateTime)
            Get
                If F_StartedDate Is Nothing Then
                    F_StartedDate = GetExcavatedDate(True)
                End If
                Return F_StartedDate
            End Get
        End Property

#End Region

#Region "   ---   构造函数，通过 OldWDocument.GetSoilModel，或者是Create静态方法"

        ''' <summary>
        ''' 构造函数：用来模拟分块开挖的土体元素。
        ''' </summary>
        ''' <param name="SoilRemove">用来模拟土体开挖的土体Element</param>
        ''' <param name="BindedModelSoil">开挖土体单元所附着的模型土体。</param>
        ''' <remarks></remarks>
        Private Sub New(SoilRemove As FamilyInstance, ByVal BindedModelSoil As Soil_Model)
            MyBase.New(BindedModelSoil.ExcavDoc, SoilRemove)
            Me.F_ModelSoil = BindedModelSoil
        End Sub

        ''' <summary>
        ''' 对于一个单元进行全面的检测，以判断其是否为一个开挖土体单元。
        ''' </summary>
        ''' <param name="doc">进行土体单元搜索的文档</param>
        ''' <param name="SoilElementId">可能的开挖土体单元的ElementId值</param>
        ''' <param name="FailureMessage">如果不是，则返回不能转换的原因。</param>
        ''' <returns>如果检查通过，则可以直接通过Create静态方法来创建对应的模型土体</returns>
        ''' <remarks></remarks>
        Public Shared Function IsExcavationModel(ByVal doc As Document, ByVal SoilElementId As ElementId, Optional ByRef FailureMessage As String = Nothing) As Boolean
            Dim blnSucceed As Boolean = False
            If SoilElementId <> ElementId.InvalidElementId Then  ' 说明用户手动指定了土体单元的ElementId，此时需要检测此指定的土体单元是否是有效的土体单元
                Try
                    Dim Soil As FamilyInstance = DirectCast(SoilElementId.Element(doc), FamilyInstance)
                    ' 是否满足基本的土体单元的条件
                    If Not Soil_Element.IsSoildElement(doc, SoilElementId, FailureMessage) Then
                        Throw New InvalidCastException(FailureMessage)
                    End If
                    ' 进行细致的检测
                    Dim pa As Parameter = Soil.Parameter(Constants.SP_ExcavationCompleted_Guid)
                    If pa IsNot Nothing Then
                        Throw New InvalidCastException("族实例中没有参数：""" & Constants.SP_ExcavationCompleted & """")
                    End If
                    blnSucceed = True
                Catch ex As Exception
                    FailureMessage = ex.Message
                    'MessageBox.Show(String.Format("指定的元素Id ({0})不是有效的土体单元。", SoilElementId) & vbCrLf &
                    '                ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
            ' 
            Return blnSucceed
        End Function

        ''' <summary>
        ''' 创建开挖土体。除非是在API中创建出来，否则。在创建之前，请先通过静态方法IsExcavationModel来判断此族实例是否可以转换为Soil_Model对象。
        ''' 否则，在程序运行过程中可能会出现各种报错。
        ''' </summary>
        ''' <param name="SoilElement">开挖土体单元</param>
        ''' <param name="BindedModelSoil">开挖土体单元所附着的模型土体。</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Create(ByVal SoilElement As FamilyInstance, ByVal BindedModelSoil As Soil_Model) As Soil_Excav
            Return New Soil_Excav(SoilElement, BindedModelSoil)
        End Function

#End Region

#Region "   ---   为开挖土体设置与读取对应的开挖完成的时间"

        ''' <summary>
        ''' 为多个开挖土体设置对应的开挖完成的时间
        ''' </summary>
        ''' <param name="Started">如果要设置土体开始开挖的时间，则设置为True，反之则是设置土体开挖完成的时间</param>
        ''' <param name="Soil_Date">一个字典集合，其中包括要进行日期设置的所有开挖土体。土体开挖完成的时间，可以精确到分钟</param>
        ''' <remarks></remarks>
        Public Shared Sub SetExcavatedDate(ByVal doc As Document, ByVal Started As Boolean, Soil_Date As Dictionary(Of Soil_Excav, Date))
            Dim Exs As Soil_Excav
            Using t As New Transaction(doc, "设置土体开挖完成的时间")
                t.Start()
                For Ind As Integer = 0 To Soil_Date.Count
                    Exs = Soil_Date.Keys(Ind)
                    Exs.SetExcavatedDate(t, Started, Soil_Date.Values(Ind))
                Next
                t.Commit()
            End Using
        End Sub

        ''' <summary>
        ''' 为开挖土体设置对应的开挖完成的时间
        ''' </summary>
        ''' <param name="Tran">Revit事务对象，在此函数中此事务并不会Start或者Commit，所以在调用此函数时，请确保此事务对象已经Started了。</param>
        ''' <param name="Started">如果要设置土体开始开挖的时间，则设置为True，反之则是设置土体开挖完成的时间</param>
        ''' <param name="ResDate">土体开挖开始或者完成的时间，可以精确到分钟。如果要清空日期字符，则设置其为Nothing。</param>
        ''' <remarks></remarks>
        Public Sub SetExcavatedDate(ByVal Tran As Transaction, ByVal Started As Boolean, ResDate As Nullable(Of Date))
            Dim pa As Parameter
            If Started Then
                pa = Me.Soil.Parameter(Constants.SP_ExcavationStarted_Guid)
            Else
                pa = Me.Soil.Parameter(Constants.SP_ExcavationCompleted_Guid)
            End If

            If pa IsNot Nothing Then
                If ResDate Is Nothing Then
                    pa.Set("")
                Else
                    If ResDate.Value.Hour = 0 Then
                        pa.Set(ResDate.Value.ToString("yyyy/MM/dd"))
                    Else
                        pa.Set(ResDate.Value.ToString("yyyy/MM/dd hh:mm"))
                    End If
                End If

                If Started Then
                    Me.F_StartedDate = ResDate
                Else
                    Me.F_CompletedDate = ResDate
                End If
            Else
                Throw New NullReferenceException(String.Format("土体单元中未找到指定的参数""{0}""", Constants.SP_ExcavationCompleted))
            End If
        End Sub

        ''' <summary>
        ''' 从开挖土体的单元中读取开挖完成的日期
        ''' </summary>
        ''' <param name="Started">如果要提取土体开始开挖的时间，则设置为True，反之则是提取土体开挖完成的时间</param>
        Private Function GetExcavatedDate(ByVal Started As Boolean) As Nullable(Of Date)
            Dim pa As Parameter
            Dim paName As String
            If Started Then
                pa = Me.Soil.Parameter(Constants.SP_ExcavationStarted_Guid)
                paName = Constants.SP_ExcavationStarted
            Else
                pa = Me.Soil.Parameter(Constants.SP_ExcavationCompleted_Guid)
                paName = Constants.SP_ExcavationCompleted
            End If
            '
            Dim strDate As String
            If pa IsNot Nothing Then
                strDate = pa.AsString()
                Dim dt As Date
                If Date.TryParse(strDate, dt) Then
                    Return dt
                Else  ' 说明此字符为空或者不能转换为日期
                    'MessageBox.Show(String.Format("土体单元{0}中的参数""{1}""的值""{2}""不能正确地转换为日期。",
                    '                                          Me.Soil.Id, Constants.SP_ExcavationCompleted, strDate))
                    Return Nothing
                End If
            Else
                Throw New NullReferenceException(String.Format("土体单元中未找到指定的参数""{0}""", paName))
            End If

        End Function

#End Region

        ''' <summary>
        ''' 得到开挖土体的顶面或者底面的在模型中的标高
        ''' </summary>
        ''' <param name="Top">If true, obtain the elevation of the top surface. If false, obtain the elevation of the bottom surface.  </param>
        ''' <returns>指定表面的标高值（单位为m）。the elevation of the specified surface, in the unit of meter.</returns>
        ''' <remarks>不用Element.Geometry（）方法，因为此方法包含大量的数据结构转换，太消耗CPU。而应使用GetBoundingBox与GetTransform等方法。</remarks>
        Public Function GetElevation(ByVal Top As Boolean) As Double
            Dim Z As Double
            If Top Then
                Z = Soil.BoundingBox(Doc.ActiveView).Max.Z
            Else
                Z = Soil.BoundingBox(Doc.ActiveView).Min.Z
            End If
            Return UnitUtils.ConvertFromInternalUnits(Z, DisplayUnitType.DUT_CUBIC_METERS)
        End Function

    End Class

End Namespace