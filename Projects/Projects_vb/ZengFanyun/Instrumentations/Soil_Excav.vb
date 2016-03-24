Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports rvtTools_ez

Namespace OldW.Soil

    ''' <summary> 用来模拟分块开挖的土体元素。 </summary>
    ''' <remarks></remarks>
    Public Class Soil_Excav

        Private F_Soil As FamilyInstance
        Public ReadOnly Property Soil As FamilyInstance
            Get
                Return F_Soil
            End Get
        End Property

        ''' <summary>
        ''' 构造函数：用来模拟分块开挖的土体元素。
        ''' </summary>
        ''' <param name="SoilRemove">用来模拟土体开挖的土体Element</param>
        ''' <remarks></remarks>
        Private Sub New(SoilRemove As FamilyInstance)
            Me.F_Soil = SoilRemove
        End Sub

    End Class

End Namespace