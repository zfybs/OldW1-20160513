Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports rvtTools_ez.ExtensionMethods
Imports System.Math
Imports OldW.GlobalSettings


Namespace OldW.Instrumentation
    ''' <summary>
    ''' 测点_立柱垂直位移
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Instrum_ColumnHeave
        Inherits Instrum_Point
        ''' <summary> 构造函数 </summary>
        ''' <param name="ColumnHeaveElement">立柱垂直位移测点所对应的图元</param>
        Public Sub New(ColumnHeaveElement As FamilyInstance)
            MyBase.New(ColumnHeaveElement, InstrumentationType.立柱隆沉)


        End Sub

    End Class
End Namespace