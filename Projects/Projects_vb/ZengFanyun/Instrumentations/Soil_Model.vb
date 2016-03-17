Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports rvtTools_ez
Namespace OldW.Soil

    ''' <summary>
    ''' 基坑中的开挖土体，整个模型中，只有一个土体元素
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Soil_Model

#Region "   ---   Properties"

        Private F_Soil As FamilyInstance
        Public ReadOnly Property Soil As FamilyInstance
            Get
                Return F_Soil
            End Get
        End Property

#End Region

#Region "   ---   构造函数"


        ''' <summary>
        ''' 构造函数：不要直接通过New Soil_Model来创建此对象，而应该用 FindSoilModel 方法来从模型中返回。
        ''' </summary>
        ''' <param name="ModelSoil">模型中的开挖土体单元</param>
        ''' <remarks></remarks>
        Private Sub New(ModelSoil As FamilyInstance)

            If ModelSoil IsNot Nothing Then
                Me.F_Soil = ModelSoil
            Else
                Throw New NullReferenceException("The specified element is not valid as soil.")
            End If

        End Sub
#End Region

        ''' <summary>
        ''' 从当前的开挖状态中，移除指定的一块土，用来模拟土体的开挖
        ''' </summary>
        ''' <param name="SoilToRemove"></param>
        ''' <remarks></remarks>
        Public Sub RemoveSoil(ByVal SoilToRemove As Soil_Remove)





        End Sub

        ''' <summary>
        ''' 从当前的开挖状态中，添加进指定的一块土，用来模拟土方的回填，或者反向回滚开挖状态
        ''' </summary>
        ''' <param name="SoilToRemove"></param>
        ''' <remarks></remarks>
        Public Sub FillSoil(ByVal SoilToRemove As Soil_Remove)





        End Sub


#Region "   ---    找出Document中的土体单元"

        Private Shared F_SoilElement As Soil_Model
        ''' <summary>
        ''' 模型中的土体单元，此单元与土体
        ''' </summary>
        ''' <param name="Doc">进行土体单元搜索的文档</param>
        ''' <param name="SoilElementId">可能的土体单元的ElementId值，如果没有待选的，可以不指定，此时程序会在整个Document中进行搜索。</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FindSoilModel(ByVal Doc As Document, Optional SoilElementId As Integer = -1) As Soil_Model
            If F_SoilElement Is Nothing Then
                Dim SE As FamilyInstance
                SE = FindSoilElement(Doc, SoilElementId)
                If SE IsNot Nothing Then
                    F_SoilElement = New Soil_Model(SE)
                Else
                    F_SoilElement = Nothing
                End If
            End If
            Return F_SoilElement
        End Function

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
                    If String.Compare(Soil.Symbol.FamilyName, Constants.FamilyName_Soil, True) <> 0 Then
                        Throw New TypeUnloadedException(String.Format("指定的ElementId所对应的单元的族名称与全局的土体族的名称""{0}""不相同。", Constants.FamilyName_Soil))
                    End If
                Catch ex As Exception
                    MessageBox.Show(String.Format("指定的元素Id ({0})不是有效的土体单元。", SoilElementId), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Soil = Nothing
                End Try
            Else  ' 说明用户根本没有指定任何可能的土体单元，此时需要在模型中按特定的方式来搜索出土体单元
                Dim SoilFamily As Family = doc.FindFamily(Constants.FamilyName_Soil)
                If SoilFamily IsNot Nothing Then
                    Dim soils As List(Of ElementId) = SoilFamily.Instances.ToElementIds

                    ' 整个模型中只能有一个模型土体对象
                    If soils.Count = 0 Then
                        MessageBox.Show("模型中没有土体单元", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    ElseIf soils.Count > 1 Then
                        Dim UIDoc As UIDocument = New UIDocument(doc)
                        UIDoc.Selection.SetElementIds(soils)
                        MessageBox.Show(String.Format("模型中的土体单元数量多于一个，请删除多余的土体单元 ( 族""{0}""的实例对象 )。", Constants.FamilyName_Soil), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        Soil = DirectCast(soils.Item(0).Element(doc), FamilyInstance)   ' 找到有效且唯一的土体单元 ^_^
                    End If

                End If
            End If
            Return Soil
        End Function

        ' ''' <summary> 判断模型中是否删除了模型土体单元 </summary>
        'Private Sub App_DocumentChanged(sender As Object, e As Autodesk.Revit.DB.Events.DocumentChangedEventArgs) Handles App.DocumentChanged

        '    If F_SoilElement IsNot Nothing Then
        '        Dim DE = e.GetDeletedElementIds
        '        MessageBox.Show(DE.Count.ToString, "删除的Element的数量")
        '        If DE.Count > 0 Then
        '            Dim Q = From id As ElementId In DE
        '                  Where id = F_SoilElement.Soil.Id
        '                  Select id
        '            If Q.Count > 0 Then  ' 说明此模型中的模型土体单元已经被删除了。
        '                F_SoilElement = Nothing
        '            End If
        '        End If
        '    End If
        'End Sub

#End Region
    End Class

End Namespace