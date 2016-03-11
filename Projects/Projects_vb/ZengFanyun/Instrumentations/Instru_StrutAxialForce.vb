Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports rvtTools_ez.ExtensionMethods
Imports System.Math
Imports OldW.GlobalSettings


Namespace OldW.DataManager
    ''' <summary>
    ''' 测点_支撑轴力
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Instru_StrutAxialForce
        Inherits Instru_Point
        ''' <summary> 构造函数 </summary>
        ''' <param name="StrutAxialForceElement"> 支撑轴力测点所对应的图元</param>
        Public Sub New(StrutAxialForceElement As FamilyInstance)
            MyBase.New(StrutAxialForceElement, Global.OldW.GlobalSettings.InstrumentationType.支撑轴力)


        End Sub

    End Class
End Namespace