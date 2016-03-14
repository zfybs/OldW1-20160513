Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports rvtTools_ez
Namespace OldW.Instrumentation

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Soil

#Region "   ---   Properties"

        Private F_Model As FamilyInstance
        Public ReadOnly Property Model As FamilyInstance
            Get
                Return F_Model
            End Get
        End Property

#End Region

#Region "   ---   构造函数"


        ''' <summary>
        ''' 构造函数
        ''' </summary>
        ''' <param name="Soil">模型中的开挖土体单元</param>
        ''' <remarks></remarks>
        Friend Sub New(Soil As FamilyInstance)

            If Soil IsNot Nothing Then
                Me.F_Model = Soil
            Else
                Throw New NullReferenceException("The specified element is not valid as soil.")
            End If

        End Sub
#End Region

#Region "   ---   Shared Methods : 找出Document中的土体单元"

        Private Shared Shared_SoilElement As Soil
        ''' <summary>
        ''' 模型中的土体单元，此单元与土体
        ''' </summary>
        ''' <param name="doc">进行土体单元搜索的文档</param>
        ''' <param name="SoilElementId">可能的土体单元的ElementId值</param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Shared Property SoilModel(ByVal doc As Document, Optional SoilElementId As Integer = -1) As Soil
            Get
                If Shared_SoilElement Is Nothing Then
                    Dim SE As FamilyInstance
                    SE = FindSoilElement(doc, SoilElementId)
                    If SE IsNot Nothing Then
                        Shared_SoilElement = New Soil(SE)
                    Else
                        Shared_SoilElement = Nothing
                    End If
                End If
                Return Shared_SoilElement
            End Get
            Set(value As Soil)
                Shared_SoilElement = value
            End Set
        End Property

        ''' <summary>
        ''' 找到模型中的开挖土体单元
        ''' </summary>
        ''' <param name="doc">进行土体单元搜索的文档</param>
        ''' <param name="SoilElementId">可能的土体单元的ElementId值</param>
        ''' <returns>如果找到有效的土体单元，则返回对应的FamilyInstance，否则返回Nothing</returns>
        ''' <remarks></remarks>
        Private Shared Function FindSoilElement(ByVal doc As Document, SoilElementId As Integer) As FamilyInstance
            Dim Soil As FamilyInstance = Nothing
            If SoilElementId <> -1 Then  ' 说明用户手动指定了土体单元的ElementId，此时需要检测此指定的土体单元是否是有效的土体单元
                Try
                    Soil = DirectCast(New ElementId(SoilElementId).Element(doc), FamilyInstance)
                    ' 进行更细致的检测
                    If String.Compare(Soil.Symbol.FamilyName, FamilyName_Soil, True) <> 0 Then
                        Throw New TypeUnloadedException(String.Format("指定的ElementId所对应的单元的族名称与全局的土体族的名称""{0}""不相同。", FamilyName_Soil))
                    End If
                Catch ex As Exception
                    MessageBox.Show(String.Format("指定的元素Id ({0})不是有效的土体单元。", SoilElementId), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Soil = Nothing
                End Try
            Else  ' 说明用户根本没有指定任何可能的土体单元，此时需要在模型中按特定的方式来搜索出土体单元
                Dim SoilFamily As Family = doc.FindFamily(FamilyName_Soil)
                If SoilFamily IsNot Nothing Then
                    Dim soils As List(Of ElementId) = SoilFamily.Instances.ToElementIds
                    If soils.Count = 0 Then
                        MessageBox.Show("模型中没有土体单元", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ElseIf soils.Count > 1 Then
                        Dim UIDoc As UIDocument = New UIDocument(doc)
                        UIDoc.Selection.SetElementIds(soils)
                        MessageBox.Show(String.Format("模型中的土体单元数量多于一个，请删除多余的土体单元 ( 族""{0}""的实例对象 )。", FamilyName_Soil), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        Soil = DirectCast(soils.Item(0).Element(doc), FamilyInstance)   ' 找到有效且唯一的土体单元 ^_^
                    End If
                End If
            End If
            Return Soil
        End Function

#End Region

    End Class

End Namespace