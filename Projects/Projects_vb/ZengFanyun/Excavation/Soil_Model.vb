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
        Inherits Soil_Element

#Region "   ---   Properties"

        Private F_Group As Group
        ''' <summary>
        ''' 此模型土体所位于的组。
        ''' 注意：所有的土体开挖模型都会位于此组中，如果将开挖土体从此组中移除，则有会被识别为开挖土体。
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Group As Group
            Get
                If Not F_Group.IsValidObject Then
                    Throw New InvalidOperationException("请先将模型土体放置在一个组中。" & vbCrLf &
                                                      "提示：所有的土体开挖模型都会位于此组中，如果将开挖土体从此组中移除，则有会被识别为开挖土体。")
                End If
                Return F_Group
            End Get
        End Property

#End Region

#Region "   ---   构造函数，通过 OldWDocument.GetSoilModel，或者是Create静态方法"

        ''' <summary>
        ''' 构造函数：不要直接通过New Soil_Model来创建此对象，而应该用 OldWDocument.GetSoilModel，或者是Create静态方法来从模型中返回。
        ''' </summary>
        ''' <param name="ExcavDoc">模型土体单元所位于的文档</param>
        ''' <param name="ModelSoil">模型中的开挖土体单元</param>
        ''' <remarks></remarks>
        Private Sub New(ByVal ExcavDoc As ExcavationDoc, ModelSoil As FamilyInstance)
            MyBase.New(ExcavDoc, ModelSoil)
            ' 检查模型土体单元是否位于一个组中
            F_Group = ModelSoil.GroupId.Element(Doc)
            ExcavDoc.ModelSoil = Me
            If F_Group Is Nothing AndAlso Not F_Group.IsValidObject Then
                Throw New InvalidOperationException("请先将模型土体放置在一个组中。" & vbCrLf &
                                                    "提示：所有的土体开挖模型都会位于此组中，如果将开挖土体从此组中移除，则有会被识别为开挖土体。")
            End If
        End Sub

        ''' <summary>
        ''' 对于一个单元进行全面的检测，以判断其是否为一个模型土体单元。
        ''' </summary>
        ''' <param name="doc">进行土体单元搜索的文档</param>
        ''' <param name="SoilElementId">可能的土体单元的ElementId值</param>
        ''' <param name="FailureMessage">如果不是，则返回不能转换的原因。</param>
        ''' <returns>如果检查通过，则可以直接通过Create静态方法来创建对应的模型土体</returns>
        ''' <remarks></remarks>
        Public Shared Function IsSoildModel(ByVal doc As Document, ByVal SoilElementId As ElementId, Optional ByRef FailureMessage As String = Nothing) As Boolean
            Dim blnSucceed As Boolean = False
            If SoilElementId <> ElementId.InvalidElementId Then  ' 说明用户手动指定了土体单元的ElementId，此时需要检测此指定的土体单元是否是有效的土体单元
                Try
                    Dim Soil As FamilyInstance = DirectCast(SoilElementId.Element(doc), FamilyInstance)
                    ' 是否满足基本的土体单元的条件
                    If Not Soil_Element.IsSoildElement(doc, SoilElementId, FailureMessage) Then
                        Throw New InvalidCastException(FailureMessage)
                    End If
                    ' 进行细致的检测
                    ' 3. 是否在组中
                    Dim gp As Group = Soil.GroupId.Element(doc)
                    If Not gp.IsValidObject Then
                        Throw New InvalidCastException("指定的ElementId所对应的单元并不在任何一个组内。")
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
        ''' 创建模型土体。除非是在API中创建出来，否则。在创建之前，请先通过静态方法IsSoildModel来判断此族实例是否可以转换为Soil_Model对象。否则，在程序运行过程中可能会出现各种报错。
        ''' </summary>
        ''' <param name="doc"></param>
        ''' <param name="SoilElement"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Create(ByVal ExcavDoc As ExcavationDoc, ByVal SoilElement As FamilyInstance) As Soil_Model
            Return New Soil_Model(ExcavDoc, SoilElement)
        End Function

#End Region

        ''' <summary>
        ''' 从当前的开挖状态中，移除指定的一块土，用来模拟土体的开挖
        ''' </summary>
        ''' <param name="SoilToRemove"></param>
        ''' <remarks></remarks>
        Public Function RemoveSoil(ByVal SoilToRemove As Soil_Excav) As Boolean
            Dim blnSucceed As Boolean = False
            If SoilToRemove IsNot Nothing Then
                Dim removedSoil As FamilyInstance = SoilToRemove.Soil
                Using tran As New Transaction(Doc, "土体开挖")
                    Try
                        tran.Start()
                        ' 将要进行开挖的土体单元也添加进模型土体的组合中 
                        Dim gp As Group = Me.Group
                        Dim elems As List(Of ElementId) = gp.GetMemberIds
                        elems.Add(removedSoil.Id)
                        '
                        gp.UngroupMembers()  ' 将Group实例删除
                        Doc.Delete(gp.GroupType.Id)  ' 删除组类型

                        ' 进行开挖土体对于模型土体的剪切操作
                        Dim CFR As CutFailureReason
                        Dim blnCanCut As Boolean = SolidSolidCutUtils.CanElementCutElement(removedSoil, Me.Soil, CFR)
                        If blnCanCut Then
                            SolidSolidCutUtils.AddCutBetweenSolids(Doc, Me.Soil, removedSoil)
                            ' 将用来剪切的开挖土体在视图中进行隐藏
                            Dim V As View = Doc.ActiveView
                            If V Is Nothing Then
                                Throw New NullReferenceException("未找到有效的视图对象。")
                            End If
                            V.HideElements({removedSoil.Id})
                        Else
                            Throw New InvalidOperationException("开挖土体不能对模型土体进行剪切，其原因为：" & vbCrLf &
                                                                CFR.ToString)
                        End If

                        ' 重新构造Group，它将产生一个新的GroupType
                        Doc.Regenerate()
                        Me.F_Group = Doc.Create.NewGroup(elems) ' 	在通过NewGroup创建出组后，可以对组内的元素进行隐藏或移动等操作，但是最好不要再对组内的元素进行剪切，否则还是可能会在UI中出现“the group has changed outside group edit mode.”的警告。
                        Me.F_Group.GroupType.Name = "基坑土体"

                        tran.Commit()
                        blnSucceed = True
                    Catch ex As Exception
                        MessageBox.Show("土体开挖失败！" & vbCrLf & ex.Message, "出错", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        tran.RollBack()
                    End Try
                End Using
            End If
            Return blnSucceed
        End Function

        ''' <summary>
        ''' 从当前的开挖状态中，添加进指定的一块土，用来模拟土方的回填，或者反向回滚开挖状态
        ''' </summary>
        ''' <param name="SoilToRemove"></param>
        ''' <remarks></remarks>      
        Public Sub FillSoil(ByVal SoilToRemove As Soil_Excav)


        End Sub

    End Class
End Namespace