Imports System
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports OldW.GlobalSettings
Imports OldW.DataManager
Imports eZstd
Imports System.Windows.Forms
Imports eZRevtiTools.ExtensionMethods
Imports OldW.Modeling
Imports System.IO
Imports Autodesk.Revit.UI.Selection


''' <summary>
''' 测点_测斜管
''' </summary>
''' <remarks></remarks>
Public Class MP_Inclinometer

    ''' <summary>
    ''' 测斜管所对应的图元
    ''' </summary>
    Public Property Inclinometer As FamilyInstance

    Public Property UIDoc As UIDocument

    Private Doc As Document


    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="InclinometerElement">测斜管所对应的图元</param>
    ''' <remarks></remarks>
    Public Sub New(UIDoc As UIDocument, InclinometerElement As FamilyInstance)
        Me.Inclinometer = InclinometerElement
        Me.UIDoc = UIDoc
        Me.Doc = UIDoc.Document
        '
   
    End Sub

    ''' <summary>
    ''' 找到距离此测斜管最近的土体开挖面的标高值
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FindAdjacentEarthElevation() As Double

        Dim lc As LocationCurve = TryCast(Me.Inclinometer.Location, LocationCurve)
        Dim curve As Curve = lc.Curve

        ' 将测斜管的底部端点作为原点
        Dim ptOrigin As XYZ
        With curve
            Dim ptStart As XYZ = .GetEndPoint(0)
            Dim ptEnd As XYZ = .GetEndPoint(1)
            If ptStart.Z < ptEnd.Z Then
                ptOrigin = ptStart
            Else
                ptOrigin = ptEnd
            End If
        End With

        '

        ' 将当前3D视图作为ReferenceIntersector的构造参数
        Dim view3d As View3D = Nothing
        view3d = TryCast(Doc.ActiveView, View3D)
        If view3d Is Nothing Then
            TaskDialog.Show("3D view", "current view should be 3D view")
            Return Result.Failed
        End If

        ' 执行射线相交。注意此射线是无限延长的，如果没有指定ReferenceIntersector中的搜索范围，则会在整个项目中的所有Element中进行相交运算。
        Dim beamLen As Double = curve.Length
        Dim ReferenceIntersector1 As New ReferenceIntersector(view3d)
        '   Dim references As IList(Of ReferenceWithContext) = ReferenceIntersector1.Find(origin:=ptStart, direction:=(ptEnd - ptStart))


        '
        Dim Options As New Options()
        ' 测斜
        Dim GeInclininometer As GeometryElement = Inclinometer.Geometry(Options)
        '
        Dim arrGeInclininometer As GeometryObject() = GeInclininometer.ToArray
        '
        Dim ins As FamilyInstance
        ' 土体
        Dim eleEarht As FamilyInstance = FindAdjacentEarth()

        Dim GeEarth As GeometryElement = eleEarht.Geometry(Options)
        '
        Dim arrGeEarth As GeometryObject() = GeEarth.ToArray


    End Function

    Private Function FindAdjacentEarth() As FamilyInstance
        Dim eleEarht As FamilyInstance

        eleEarht = Doc.GetElement(New ElementId(460116))

        Return eleEarht
    End Function

End Class
