Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports rvtTools_ez
Imports OldW.Excavation

Namespace OldW.Soil

    ''' <summary>
    ''' 土体单元对象。一个土体单元的族实例，必须满足的条件有：1. 族的名称限制；2. 实例类别为“场地”。
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class Soil_Element

#Region "   ---   Properties"

        Private F_Soil As FamilyInstance
        ''' <summary> 土体单元所对应的族实例对象 </summary>
        Public ReadOnly Property Soil As FamilyInstance
            Get
                Return F_Soil
            End Get
        End Property

        Protected F_ExcavDoc As ExcavationDoc
        Public ReadOnly Property ExcavDoc As ExcavationDoc
            Get
                Return F_ExcavDoc
            End Get
        End Property

        Protected Doc As Document
        Public ReadOnly Property Document As Document
            Get
                Return Doc
            End Get
        End Property


#End Region

#Region "   ---   Fields"

#End Region

#Region "   ---   构造函数，通过 OldWDocument.GetSoilModel，或者是Create静态方法"

        Protected Sub New(ByVal ExcavDoc As ExcavationDoc, ByVal SoilElement As FamilyInstance)
            If SoilElement IsNot Nothing AndAlso SoilElement.IsValidObject Then
                Me.F_ExcavDoc = ExcavDoc
                Me.F_Soil = SoilElement
                Me.Doc = ExcavDoc.Document
            Else
                Throw New NullReferenceException("The specified element is not valid as soil.")
            End If
        End Sub
#End Region

        ''' <summary>
        ''' 对于一个单元进行全面的检测，以判断其是否为一个模型土体单元或者开挖土体单元。
        ''' </summary>
        ''' <param name="doc">进行土体单元搜索的文档</param>
        ''' <param name="SoilElementId">可能的土体单元的ElementId值</param>
        ''' <param name="FailureMessage">如果不是，则返回不能转换的原因。</param>
        ''' <returns>如果检查通过，则可以直接通过Create静态方法来创建对应的模型土体</returns>
        ''' <remarks>一个土体单元的族实例，必须满足的条件有：1. 族的名称限制；2. 实例类别为“场地”。</remarks>
        Protected Shared Function IsSoildElement(ByVal doc As Document, ByVal SoilElementId As ElementId, Optional ByRef FailureMessage As String = Nothing) As Boolean
            Dim blnSucceed As Boolean = False
            If SoilElementId <> ElementId.InvalidElementId Then  ' 说明用户手动指定了土体单元的ElementId，此时需要检测此指定的土体单元是否是有效的土体单元
                Try
                    Dim Soil As FamilyInstance = DirectCast(SoilElementId.Element(doc), FamilyInstance)
                    ' 进行细致的检测
                    '1. 族实例的族名称
                    If String.Compare(Soil.Symbol.FamilyName, Constants.FamilyName_Soil, True) <> 0 Then
                        Throw New TypeUnloadedException(String.Format("指定的ElementId所对应的单元的族名称与全局的土体族的名称""{0}""不相同。", Constants.FamilyName_Soil))
                    End If
                    '2. 族实例的类别
                    If Not Soil.Category.Id.Equals(doc.Settings.Categories.Item(BuiltInCategory.OST_Site).Id) Then
                        Throw New InvalidCastException("指定的ElementId所对应的单元的族类别不是""场地""类别。")
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

    End Class
End Namespace