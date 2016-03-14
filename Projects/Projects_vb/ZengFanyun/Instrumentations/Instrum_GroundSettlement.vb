Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports rvtTools_ez.ExtensionMethods
Imports System.Math
Imports OldW.GlobalSettings


Namespace OldW.Instrumentation
    ''' <summary>
    ''' 测点_地表垂直位移
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Instrum_GroundSettlement
        Inherits Instrum_Point
        ''' <summary> 构造函数 </summary>
        ''' <param name="GroundSettlementElement">地表垂直位移测点所对应的图元</param>
        Public Sub New(GroundSettlementElement As FamilyInstance)
            MyBase.New(GroundSettlementElement, InstrumentationType.地表隆沉)


        End Sub

    End Class
End Namespace