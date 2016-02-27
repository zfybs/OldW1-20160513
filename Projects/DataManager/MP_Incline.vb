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
Public Class MP_Incline

    ''' <summary>
    ''' 测斜管所对应的图元
    ''' </summary>
    Public Property Inclinometer As FamilyInstance

    Public Property UIDocument As UIDocument

    Private Doc As Document

    Public eleEarht As FamilyInstance

    ''' <summary>
    ''' 构造函数
    ''' </summary>
    ''' <param name="Ele">测斜管所对应的图元</param>
    ''' <remarks></remarks>
    Public Sub New(UIDoc As UIDocument)
        '  Me.Inclinometer = Ele
        Me.UIDocument = UIDoc
        Me.Doc = UIDoc.Document
    End Sub


    Public Function FindAdjacentEarthElevation() As Element
        FindSupporting()

        Exit Function
        '
        Dim Options As New Options()
        ' 测斜
        Dim GeInclininometer As GeometryElement = Inclinometer.Geometry(Options)
        '
        Dim arrGeInclininometer As GeometryObject() = GeInclininometer.ToArray
        '
        Dim ins As FamilyInstance
        ' 土体
        Dim GeEarth As GeometryElement = eleEarht.Geometry(Options)
        '
        Dim arrGeEarth As GeometryObject() = GeEarth.ToArray


    End Function


    Public Sub GetGeometry()

        '选择一根柱子
        Dim ref1 As Reference = UIDocument.Application.ActiveUIDocument.Selection.PickObject(ObjectType.Element, "Please pick a column")
        Dim elem As Element = Doc.GetElement(ref1)
        Dim column As FamilyInstance = TryCast(elem, FamilyInstance)


        Dim opt As New Options()
        opt.ComputeReferences = True
        opt.DetailLevel = Autodesk.Revit.DB.ViewDetailLevel.Medium

        Dim geoElem_Colu As GeometryElement = column.Geometry(opt)

        ' 柱子中的各种图形
        For Each obj As GeometryObject In geoElem_Colu
            ' 如果柱子被切割了，那么此柱子的几何信息与定义此柱子的族的几何信息就不一样了，所以，被切割的柱子的GeometryElement之中就直接包含有Solid
            If TypeOf obj Is Solid Then
                Dim solid As Solid = TryCast(obj, Solid)

                ' 如果柱子未被切割，则GeometryElement之中就包含的就是GeometryInstance
            ElseIf TypeOf obj Is GeometryInstance Then
                Dim geoInstance As GeometryInstance = TryCast(obj, GeometryInstance)
                ' 返回族实例的几何数据
                Dim geoElement As GeometryElement = geoInstance.GetInstanceGeometry()
                '
                For Each obj2 As GeometryObject In geoElement
                    If TypeOf obj2 Is Solid Then
                        Dim solid2 As Solid = TryCast(obj2, Solid)
                        '对象几何数据中可能包含没有属性的Solid，需要排除它。
                        If solid2.Volume > 0 Then  ' 判断此Solid是否有效

                        End If
                    End If
                Next obj2
            End If
        Next obj

    End Sub



    ''' <summary> 找到与梁相交的所有墙对象 </summary>
    Public Function FindSupporting() As Result
        Dim app As UIApplication = UIDocument.Application
        Dim trans As New Transaction(Doc, "ExComm")
        trans.Start()

        ' 在界面中选择一个梁
        Dim sel As Selection = app.ActiveUIDocument.Selection
        Dim ref1 As Reference = sel.PickObject(ObjectType.Element, "Please pick a beam")
        Dim beam As FamilyInstance = TryCast(Doc.GetElement(ref1), FamilyInstance)

        'Read the beam's location line
        Dim lc As LocationCurve = TryCast(beam.Location, LocationCurve)
        Dim curve As Curve = lc.Curve

        ' 将梁的起点和端点作为射线的原点与方向
        Dim ptStart As XYZ = curve.GetEndPoint(0)
        Dim ptEnd As XYZ = curve.GetEndPoint(1)

        'move the two point a little bit lower, so the ray can go through the wall
        ' 将这两个点向下移动一点点，来让引射线可以穿过墙
        Dim offset As New XYZ(0, 0, 0.01)
        ptStart = ptStart - offset
        ptEnd = ptEnd - offset

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
        Dim references As IList(Of ReferenceWithContext) = ReferenceIntersector1.Find(origin:=ptStart, direction:=(ptEnd - ptStart))

        ' 清除已经选择的对象
        sel.SetElementIds(New List(Of ElementId))
        ' 返回已经选择的对象
        Dim Ge As ICollection(Of ElementId) = sel.GetElementIds()

        ' 找到所有相交对象中，与梁相交的墙对象
        Dim tolerate As Double = 0.00001
        For Each reference As ReferenceWithContext In references
            Dim ref2 As Reference = reference.GetReference()
            Dim id As ElementId = ref2.ElementId
            Dim elem As Element = Doc.GetElement(id)
            '
            If TypeOf elem Is Wall Then
                ' 如果与射线相交的对象到射线原点的距离比梁的长度小，说明，此对象是与梁相交的
                ' 如果相交面与梁的端面重合，则可以设置一个tolerate，这样的话，那个与之重合的面也可以被选中了。
                If reference.Proximity < (beamLen + tolerate) Then
                    Ge.Add(elem.Id)
                End If
            End If
        Next reference
        ' 整体选择所有与梁相交的墙
        sel.SetElementIds(Ge)
        trans.Commit()

        Return Result.Succeeded
    End Function
End Class
