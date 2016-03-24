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
        Friend Sub New(ModelSoil As FamilyInstance)

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
        Public Sub RemoveSoil(ByVal SoilToRemove As Soil_Excav)


        End Sub

        ''' <summary>
        ''' 从当前的开挖状态中，添加进指定的一块土，用来模拟土方的回填，或者反向回滚开挖状态
        ''' </summary>
        ''' <param name="SoilToRemove"></param>
        ''' <remarks></remarks>
        Public Sub FillSoil(ByVal SoilToRemove As Soil_Excav)





        End Sub


#Region "   ---    找出Document中的土体单元"


#End Region
    End Class

End Namespace