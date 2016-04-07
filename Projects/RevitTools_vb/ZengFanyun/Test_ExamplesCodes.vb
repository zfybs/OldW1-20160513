Imports System
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports OldW.GlobalSettings
Imports System.IO
Imports Autodesk.Revit.UI.Selection
Imports Autodesk.Revit.DB.Architecture


Namespace rvtTools_ez.Test

    ''' <summary>
    ''' 一些效果实现的示例代码
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Test_ExamplesCodes

        Private UIDoc As UIDocument
        Private Doc As Document
        Private App As Autodesk.Revit.ApplicationServices.Application

        ''' <summary>
        ''' 返回Element的几何数据
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub GetGeometry()

            '选择一根柱子
            Dim ref1 As Reference = UIDoc.Selection.PickObject(ObjectType.Element, "Please pick a column")
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

        ''' <summary> 射线法：找到与梁相交的所有墙对象 </summary>
        Public Function FindSupporting() As Result
            Dim trans As New Transaction(Doc, "ExComm")
            trans.Start()

            ' 在界面中选择一个梁
            Dim sel As Selection = UIDoc.Selection
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
            ' Change
            Return Result.Succeeded
        End Function

        ''' <summary> 找到与某Element的几何实体相交的Elements </summary>
        Public Sub FindIntersectWallsByElement()

            Dim trans As New Transaction(Doc, "ExComm")
            trans.Start()

            ' 选择一个柱子
            Dim sel As Selection = UIDoc.Selection
            Dim ref1 As Reference = sel.PickObject(ObjectType.Element, "Please pick a column")
            Dim column As Element = Doc.GetElement(ref1)

            ' 应用过滤器，在整个文档中搜索与指定Element相交的Element
            Dim collector As New FilteredElementCollector(Doc)
            Dim elementFilter As New ElementIntersectsElementFilter(column, False)
            collector.WherePasses(elementFilter)

            ' 排除柱子本身
            Dim excludes As New List(Of ElementId)()
            excludes.Add(column.Id)
            collector.Excluding(excludes)

            '将最终的相交结果选中
            Dim selEle As New List(Of ElementId)
            For Each elem As Element In collector
                selEle.Add(elem.Id)
            Next elem
            sel.SetElementIds(selEle)

            trans.Commit()

        End Sub

        ''' <summary>ElementIntersectsSolidFilter: 找到与某Solid相交的Elements </summary>
        Public Sub FindIntersectWallsByGeometry()

            Dim trans As New Transaction(Doc, "ExComm")
            trans.Start()

            ' ---------------- 在API内存中创建一个拉伸Solid的定义（但是并不包含在Document中） ----------------
            'pick a point to draw solid
            Dim sel As Selection = UIDoc.Selection
            Dim pt As XYZ = sel.PickPoint("Please pick a point to get the close walls")

            Dim pttemp1 As XYZ = sel.PickPoint(Autodesk.Revit.UI.Selection.ObjectSnapTypes.None, "Pick leader end...")
            Dim pttemp2 As XYZ = sel.PickPoint(Autodesk.Revit.UI.Selection.ObjectSnapTypes.None, "Pick leader elbow...")

            Dim dBoxLength As Double = 3
            ' 创建进行拉伸的闭合曲线
            Dim pt1 As New XYZ(pt.X - dBoxLength / 2, pt.Y - dBoxLength / 2, pt.Z)
            Dim pt2 As New XYZ(pt.X + dBoxLength / 2, pt.Y - dBoxLength / 2, pt.Z)
            Dim pt3 As New XYZ(pt.X + dBoxLength / 2, pt.Y + dBoxLength / 2, pt.Z)
            Dim pt4 As New XYZ(pt.X - dBoxLength / 2, pt.Y + dBoxLength / 2, pt.Z)

            ' 闭合曲线的四条边
            Dim lineBottom As Line = Line.CreateBound(pt1, pt2)
            Dim lineRight As Line = Line.CreateBound(pt2, pt3)
            Dim lineTop As Line = Line.CreateBound(pt3, pt4)
            Dim lineLeft As Line = Line.CreateBound(pt4, pt1)

            ' 将四条边连接起来
            Dim profile As New CurveLoop()
            profile.Append(lineBottom)
            profile.Append(lineRight)
            profile.Append(lineTop)
            profile.Append(lineLeft)

            ' 创建闭合的连续曲线段
            Dim loops As New List(Of CurveLoop)()
            loops.Add(profile)

            ' 创建出一个Solid：此Solid只是为了与其他API交互，并未保存在Document中，所以也不可见。
            Dim vector As New XYZ(0, 0, 1)
            Dim solid As Solid = GeometryCreationUtilities.CreateExtrusionGeometry(loops, vector, 10)

            ' 在整个文档中搜索与此虚拟定义的Solid相交的Element
            Dim collector As New FilteredElementCollector(Doc)
            Dim solidFilter As New ElementIntersectsSolidFilter(solid)
            collector.WherePasses(solidFilter)

            ' 将最终的相交结果选中
            Dim selEle As New List(Of ElementId)
            For Each elem As Element In collector
                selEle.Add(elem.Id)
            Next elem
            sel.SetElementIds(selEle)
            '
            trans.Commit()
        End Sub

        ''' <summary>
        ''' ElementParameterFilter 参数过滤器：找到一个房间内的所有对象
        ''' </summary>
        ''' <remarks>提示: 参数过滤条件可能比其他的类型过滤条件要快，但是这要视条件而定。毕竟这是一个慢过，使用时请按照过滤标准的复杂程度而异。</remarks>
        Public Sub ElementParameterFilter_FindOjbectsInSpecificRoom(commandData As ExternalCommandData)
            Dim app As UIApplication = commandData.Application
            Dim document As Document = app.ActiveUIDocument.Document
            'pick a room
            Dim sel As Selection = app.ActiveUIDocument.Selection
            Dim ref1 As Reference = sel.PickObject(ObjectType.Element, "Please pick a room")
            Dim room As Room = TryCast(document.GetElement(ref1), Room)

            ' 定义要过滤哪个参数 ParameterValueProvider ：表示Element的房间Id的参数。
            Dim provider As New ParameterValueProvider(New ElementId(BuiltInParameter.ELEM_ROOM_ID))
            ' 定义过滤规则的表达式 FilterNumericRuleEvaluator 或者 FilterStringRuleEvaluator
            Dim evaluator As FilterNumericRuleEvaluator = New FilterNumericEquals()
            ' 定义要过滤的类型 FilterRule
            Dim rule As New FilterElementIdRule(provider, evaluator, room.Id)
            ' 最终得到参数过滤器 ElementParameterFilter
            Dim filter As New ElementParameterFilter(rule)

            ' 在什么范围内进行过滤
            Dim collector As New FilteredElementCollector(document)
            ' 执行过滤操作，并得到过滤后的结果
            collector.WherePasses(filter)

        End Sub


        ''' <summary>
        ''' ElementParameterFilter参数过滤器 Creates an ElementParameter filter to find rooms whose area is greater than specified value
        ''' </summary>
        ''' <remarks>提示: 参数过滤条件可能比其他的类型过滤条件要快，但是这要视条件而定。毕竟这是一个慢过，使用时请按照过滤标准的复杂程度而异。</remarks>
        Public Sub ElementParameterFilter_FindRooms(Document As Document)

            ' 以下5个步骤演示了要进行参数过滤的完整过程。
            ' provider
            Dim pvp As New ParameterValueProvider(New ElementId(BuiltInParameter.ROOM_AREA))
            ' evaluator
            Dim fnrv As FilterNumericRuleEvaluator = New FilterNumericGreater()

            ' 过滤规则 : 参数“房间面积”的值“大于”“100 Square Foot”。（浮点数比较的容差为0.000001）
            Dim fRule As FilterRule = New FilterDoubleRule(valueProvider:=pvp, evaluator:=fnrv, ruleValue:=100, epsilon:=0.000001)

            ' Create an ElementParameter filter
            Dim filter As New ElementParameterFilter(fRule)

            ' Apply the filter to the elements in the active document
            Dim collector As New FilteredElementCollector(Document)
            Dim rooms As IList(Of Element) = collector.WherePasses(filter).ToElements()

            ' 反转过滤条件
            ' Find rooms whose area is less than or equal to 100: 
            ' Use inverted filter to match elements
            Dim lessOrEqualFilter As New ElementParameterFilter(filterRule:=fRule, inverted:=True)
            collector = New FilteredElementCollector(Document)
            Dim lessOrEqualFounds As IList(Of Element) = collector.WherePasses(lessOrEqualFilter).ToElements()

        End Sub

        Public Sub Test()

        End Sub
    End Class
End Namespace

