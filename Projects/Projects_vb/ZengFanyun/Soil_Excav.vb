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

        ''' <summary>
        ''' 每一个开挖土体都有一个开挖完成的时间。由于记录的不完整，这个时间可能暂时不知道，但是后期要可以指定。
        ''' </summary>
        Public Property ExcavatedDate As DateTime

#End Region

#Region "   ---   构造函数，通过 OldWDocument.GetSoilModel，或者是Create静态方法"

        ''' <summary>
        ''' 构造函数：用来模拟分块开挖的土体元素。
        ''' </summary>
        ''' <param name="SoilRemove">用来模拟土体开挖的土体Element</param>
        ''' <param name="ExcavatedDate">每一个开挖土体都有一个开挖完成的时间。
        ''' 由于记录的不完整，这个时间可能暂时不知道，但是后期要可以指定。</param>
        ''' <remarks></remarks>
        Private Sub New(SoilRemove As FamilyInstance, ByVal ExcavatedDate As DateTime)

            MyBase.New(SoilRemove)
            Me.ExcavatedDate = ExcavatedDate
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
        ''' 创建开挖土体。除非是在API中创建出来，否则。在创建之前，请先通过静态方法IsExcavationModel来判断此族实例是否可以转换为Soil_Model对象。否则，在程序运行过程中可能会出现各种报错。
        ''' </summary>
        ''' <param name="SoilElement"></param>
        ''' <param name="ExcavatedDate">每一个开挖土体都有一个开挖完成的时间。
        ''' 由于记录的不完整，这个时间可能暂时不知道，但是后期要可以指定。</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Create(ByVal SoilElement As FamilyInstance, ByVal ExcavatedDate As DateTime) As Soil_Excav
            Return New Soil_Excav(SoilElement, ExcavatedDate)
        End Function

#End Region

#Region "   ---   为多个开挖土体设置对应的开挖完成的时间"
        ''' <summary>
        ''' 为多个开挖土体设置对应的开挖完成的时间
        ''' </summary>
        ''' <param name="Soil_Date">一个字典集合，其中包括要进行日期设置的所有开挖土体。土体开挖完成的时间，可以精确到分钟</param>
        ''' <remarks></remarks>
        Public Shared Sub SetExcavatedDate(ByVal doc As Document, Soil_Date As Dictionary(Of Soil_Excav, Date))
            Dim sF As FamilyInstance
            Using t As New Transaction(doc, "设置土体开挖完成的时间")
                t.Start()
                For Ind As Integer = 0 To Soil_Date.Count
                    sF = Soil_Date.Keys(Ind).Soil
                    Dim CompletedDate As Date = Soil_Date.Values(Ind)
                    Dim pa As Parameter = sF.Parameter(Constants.SP_ExcavationCompleted_Guid)
                    pa.Set(CompletedDate.ToString("yyyy/MM/dd h:mm"))
                Next
                t.Commit()
            End Using
        End Sub

        ''' <summary>
        ''' 为开挖土体设置对应的开挖完成的时间
        ''' </summary>
        ''' <param name="CompletedDate">土体开挖完成的时间，可以精确到分钟</param>
        ''' <remarks></remarks>
        Public Sub SetExcavatedDate(CompletedDate As Date)
            Using t As New Transaction(Doc, "设置土体开挖完成的时间")
                t.Start()
                Dim pa As Parameter = Soil.Parameter(Constants.SP_ExcavationCompleted_Guid)
                pa.Set(CompletedDate.ToString("yyyy/MM/dd h:mm"))
                t.Commit()
            End Using
        End Sub
#End Region



    End Class

End Namespace