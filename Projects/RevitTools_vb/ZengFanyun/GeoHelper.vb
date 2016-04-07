Imports System
Imports System.Collections.Generic
Imports Autodesk.Revit
Imports Autodesk.Revit.DB
Imports System.Math

Namespace rvtTools_ez
    ''' <summary>
    ''' A object to help locating with geometry data.
    ''' </summary>
    Public Class GeoHelper
        ''' <summary>
        ''' 进行点的距离比较时的容差
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PointPrecision As Double = 0.00001

        ''' <summary>
        ''' Find the bottom face of a face array.
        ''' </summary>
        ''' <param name="faces">A face array.</param>
        ''' <returns>The bottom face of a face array.</returns>
        Public Shared Function GetBottomFace(ByVal faces As FaceArray) As Face
            Dim face As Face = Nothing
            Dim elevation As Double = 0
            Dim tempElevation As Double = 0
            Dim mesh As Mesh = Nothing

            For Each f As Face In faces
                If IsVerticalFace(f) Then
                    ' If this is a vertical face, it cannot be a bottom face to a certainty.
                    Continue For
                End If

                tempElevation = 0
                mesh = f.Triangulate()

                For Each xyz As Autodesk.Revit.DB.XYZ In mesh.Vertices
                    tempElevation = tempElevation + xyz.Z
                Next xyz

                tempElevation = tempElevation / mesh.Vertices.Count

                If elevation > tempElevation OrElse Nothing Is face Then
                    ' Update the bottom face to which's elevation is the lowest.
                    face = f
                    elevation = tempElevation
                End If
            Next f

            ' The bottom face is consider as which's average elevation is the lowest, except vertical face.
            Return face
        End Function

        ''' <summary>
        ''' Find out the three points which made of a plane.
        ''' </summary>
        ''' <param name="mesh">A mesh contains many points.</param>
        ''' <param name="startPoint">Create a new instance of ReferencePlane.</param>
        ''' <param name="endPoint">The free end apply to reference plane.</param>
        ''' <param name="thirdPnt">A third point needed to define the reference plane.</param>
        Public Shared Sub Distribute(ByVal mesh As Mesh, ByRef startPoint As Autodesk.Revit.DB.XYZ, ByRef endPoint As Autodesk.Revit.DB.XYZ, ByRef thirdPnt As Autodesk.Revit.DB.XYZ)
            Dim count As Integer = mesh.Vertices.Count
            startPoint = mesh.Vertices(0)
            endPoint = mesh.Vertices(CInt(count \ 3))
            thirdPnt = mesh.Vertices(CInt(count \ 3 * 2))
        End Sub

        ''' <summary>
        ''' Calculate the length between two points.
        ''' </summary>
        ''' <param name="startPoint">The start point.</param>
        ''' <param name="endPoint">The end point.</param>
        ''' <returns>The length between two points.</returns>
        Public Shared Function GetLength(ByVal startPoint As Autodesk.Revit.DB.XYZ, ByVal endPoint As Autodesk.Revit.DB.XYZ) As Double
            Return Math.Sqrt(Math.Pow((endPoint.X - startPoint.X), 2) + Math.Pow((endPoint.Y - startPoint.Y), 2) + Math.Pow((endPoint.Z - startPoint.Z), 2))
        End Function

        ''' <summary>
        ''' The distance between two value in a same axis.
        ''' </summary>
        ''' <param name="start">start value.</param>
        ''' <param name="end">end value.</param>
        ''' <returns>The distance between two value.</returns>
        Public Shared Function GetDistance(ByVal start As Double, ByVal [end] As Double) As Double
            Return Math.Abs(start - [end])
        End Function

        ''' <summary>
        ''' Get the vector between two points.
        ''' </summary>
        ''' <param name="startPoint">The start point.</param>
        ''' <param name="endPoint">The end point.</param>
        ''' <returns>The vector between two points.</returns>
        Public Shared Function GetVector(ByVal startPoint As Autodesk.Revit.DB.XYZ, ByVal endPoint As Autodesk.Revit.DB.XYZ) As Autodesk.Revit.DB.XYZ
            Return New Autodesk.Revit.DB.XYZ(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y, endPoint.Z - startPoint.Z)
        End Function

        ''' <summary>
        ''' Determines whether a face is vertical.
        ''' </summary>
        ''' <param name="face">The face to be determined.</param>
        ''' <returns>Return true if this face is vertical, or else return false.</returns>
        Private Shared Function IsVerticalFace(ByVal face As Face) As Boolean
            For Each ea As EdgeArray In face.EdgeLoops
                For Each e As Edge In ea
                    If IsVerticalEdge(e) Then
                        Return True
                    End If
                Next e
            Next ea

            Return False
        End Function

        ''' <summary>
        ''' Determines whether a edge is vertical.
        ''' </summary>
        ''' <param name="edge">The edge to be determined.</param>
        ''' <returns>Return true if this edge is vertical, or else return false.</returns>
        Private Shared Function IsVerticalEdge(ByVal edge As Edge) As Boolean
            Dim polyline As List(Of XYZ) = TryCast(edge.Tessellate(), List(Of XYZ))
            Dim verticalVct As New Autodesk.Revit.DB.XYZ(0, 0, 1)
            Dim pointBuffer As Autodesk.Revit.DB.XYZ = polyline(0)

            For i As Integer = 1 To polyline.Count - 1
                Dim temp As Autodesk.Revit.DB.XYZ = polyline(i)
                Dim vector As Autodesk.Revit.DB.XYZ = GetVector(pointBuffer, temp)
                If Equal(vector, verticalVct) Then
                    Return True
                Else
                    Continue For
                End If
            Next i

            Return False
        End Function

        ''' <summary>
        ''' Determines whether two vector are equal in x and y axis.
        ''' </summary>
        ''' <param name="vectorA">The vector A.</param>
        ''' <param name="vectorB">The vector B.</param>
        ''' <returns>Return true if two vector are equals, or else return false.</returns>
        Private Shared Function Equal(ByVal vectorA As Autodesk.Revit.DB.XYZ, ByVal vectorB As Autodesk.Revit.DB.XYZ) As Boolean
            Dim isNotEqual As Boolean = (PointPrecision < Math.Abs(vectorA.X - vectorB.X)) OrElse (PointPrecision < Math.Abs(vectorA.Y - vectorB.Y))

            Return If(isNotEqual, False, True)
        End Function

        ''' <summary>
        ''' 从选择的Curve Elements中，获得连续多段曲线。此函数经测试，并不能将顺序的连续线转换为从头至尾的连续线。
        ''' Gets a list of curves which are ordered correctly and oriented correctly to form a closed loop.
        ''' </summary>
        ''' <param name="doc">The document.</param>
        ''' <param name="boundaries">The list of curve element references which are the boundaries.
        ''' 注意，boundaries中每一条曲线都必须是有界的（IsBound），否则，其GetEndPoint会报错。</param>
        ''' <returns>The list of curves.</returns>
        Public Shared Function GetContiguousCurvesFromSelectedCurveElementsWithGug(ByVal doc As Document, ByVal boundaries As IList(Of Reference)) As IList(Of Curve)
            Dim curves As New List(Of Curve)()

            ' Build a list of curves from the curve elements
            For Each reference As Reference In boundaries
                Dim curveElement As CurveElement = TryCast(doc.GetElement(reference), CurveElement)
                curves.Add(curveElement.GeometryCurve.Clone())
            Next reference

            ' Walk through each curve (after the first) to match up the curves in order
            For i As Integer = 0 To curves.Count - 1
                Dim curve As Curve = curves(i)
                Dim endPoint As XYZ = curve.GetEndPoint(1) ' 第i条线的终点

                ' 从剩下的曲线中找出起点与上面的终点重合的曲线 。 find curve with start point = end point
                For j As Integer = i + 1 To curves.Count - 1
                    Dim tmpCurve As Curve = curves(i + 1)
                    ' Is there a match end->start, if so this is the next curve
                    If curves(j).GetEndPoint(0).IsAlmostEqualTo(endPoint, 0.00001) Then
                        curves(i + 1) = curves(j)
                        curves(j) = tmpCurve
                        Continue For
                        ' Is there a match end->end, if so, reverse the next curve
                    ElseIf curves(j).GetEndPoint(1).IsAlmostEqualTo(endPoint, 0.00001) Then
                        curves(i + 1) = curves(j).CreateReversed() ' CreateReversedCurve(curves(j))
                        curves(j) = tmpCurve
                        Continue For
                    End If
                Next j
            Next i

            Return curves
        End Function

        ''' <summary>
        ''' 从选择的Curve Elements中，获得连续排列的多段曲线（不一定要封闭）。
        ''' </summary>
        ''' <param name="doc">曲线所在文档</param>
        ''' <param name="SelectedCurves">多条曲线元素所对应的Reference，可以通过Selection.PickObjects返回。
        ''' 注意，SelectedCurves中每一条曲线都必须是有界的（IsBound），否则，其GetEndPoint会报错。</param>
        ''' <returns>如果输入的曲线可以形成连续的多段线，则返回重新排序后的多段线集合；
        ''' 如果输入的曲线不能形成连续的多段线，则返回Nothing！</returns>
        Public Shared Function GetContiguousCurvesFromSelectedCurveElements(ByVal doc As Document, ByVal SelectedCurves As IList(Of Reference)) As IList(Of Curve)
            Dim curves As New List(Of Curve)()

            ' Build a list of curves from the curve elements
            For Each reference As Reference In SelectedCurves
                Dim curveElement As CurveElement = TryCast(doc.GetElement(reference), CurveElement)
                curves.Add(curveElement.GeometryCurve.Clone())
            Next reference

            Dim endPoint As XYZ  ' 每一条线的终点，用来与剩下的线段的起点或者终点进行比较
            Dim blnHasCont As Boolean  ' 此终点是否有对应的点与之对应，如果没有，则说明所有的线段中都不能形成连续的多段线

            ' Walk through each curve (after the first) to match up the curves in order
            For ThisCurveId As Integer = 0 To curves.Count - 2
                Dim ThisCurve As Curve = curves(ThisCurveId)
                blnHasCont = False
                endPoint = ThisCurve.GetEndPoint(1) ' 第i条线的终点
                Dim tmpCurve As Curve = curves(ThisCurveId + 1)  ' 当有其余的曲线放置在当ThisCurveId + 1位置时，要将当前状态下的ThisCurveId + 1位置的曲线与对应的曲线对调。

                ' 从剩下的曲线中找出起点与上面的终点重合的曲线 。 find curve with start point = end point
                For NextCurveId As Integer = ThisCurveId + 1 To curves.Count - 1

                    ' Is there a match end->start, if so this is the next curve
                    If IsAlmostEqualTo(curves(NextCurveId).GetEndPoint(0), endPoint, PointPrecision) Then
                        blnHasCont = True
                        ' 向上对换
                        curves(ThisCurveId + 1) = curves(NextCurveId)
                        curves(NextCurveId) = tmpCurve
                        Continue For

                        ' Is there a match end->end, if so, reverse the next curve
                    ElseIf IsAlmostEqualTo(curves(NextCurveId).GetEndPoint(1), endPoint, PointPrecision) Then
                        blnHasCont = True
                        ' 向上对换
                        curves(ThisCurveId + 1) = curves(NextCurveId).CreateReversed()
                        If NextCurveId <> ThisCurveId + 1 Then
                            ' 如果 NextCurveId = ThisCurveId + 1 ，说明 ThisCurveId + 1 就是接下来的那条线，只不过方向反了。
                            ' 这样就不可以将原来的那条线放回去，而只需要执行上面的反转操作就可以了。
                            curves(NextCurveId) = tmpCurve
                        End If

                        Continue For
                    End If

                Next NextCurveId
                If Not blnHasCont Then  ' 说明不可能形成连续的多段线了
                    Return Nothing
                End If
            Next ThisCurveId

            Return curves
        End Function

        ''' <summary> 比较两个点之间的距离是否小于指定的容差 </summary>
        ''' <remarks>对于Revit中的XYZ对象，其也有一个IsAlmostEqualTo函数，但是要注意，
        ''' 那个函数是用来比较两个向量的方向是否小于指定的弧度容差。</remarks>
        Private Shared Function IsAlmostEqualTo(Point1 As XYZ, Point2 As XYZ, ByVal Precision As Double) As Boolean
            Dim D As Double = Sqrt((Point1.X - Point2.X) ^ 2 +
                                   (Point1.Y - Point2.Y) ^ 2 +
                                   (Point1.Z - Point2.Z) ^ 2)
            If D <= Precision Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' 找到Extrusion中指定法向的平面。如果有多个平面的法向都是指定的法向，则返回第一个找到的平面。
        ''' Given a solid, find a planar face with the given normal (version 2)
        ''' this is a slightly enhanced version which checks if the face is on the given reference plane.
        ''' </summary>
        ''' <param name="refPlane">除了验证平面的法向外，还可以额外验证一下指定法向的平面是否是在指定的参考平面上。即要同时满足normal与ReferencePlane两个条件。
        ''' additionally, we want to check if the face is on the reference plane</param>
        ''' <remarks></remarks>
        Public Shared Function FindFace(ByVal aSolid As Extrusion, ByVal normal As XYZ, Optional ByVal refPlane As ReferencePlane = Nothing) As PlanarFace

            '' get the geometry object of the given element
            ''
            Dim op As New Options
            op.ComputeReferences = True
            ' Dim geomObjs As GeometryObjectArray = aSolid.Geometry(op).Objects

            '' loop through the array and find a face with the given normal
            ''
            For Each geomObj As GeometryObject In aSolid.Geometry(op)

                If TypeOf geomObj Is Solid Then  ''  solid is what we are interested in.

                    Dim pSolid As Solid = geomObj
                    Dim faces As FaceArray = pSolid.Faces

                    For Each pFace As Face In faces
                        If TypeOf pFace Is PlanarFace Then
                            Dim pPlanarFace As PlanarFace = pFace
                            If Not (pPlanarFace Is Nothing) Then
                                ''  check to see if they have same normal
                                If pPlanarFace.ComputeNormal(New UV(0, 0)).IsAlmostEqualTo(normal) Then

                                    If refPlane Is Nothing Then
                                        Return pPlanarFace  '' we found the face. 
                                    Else
                                        ''  additionally, we want to check if the face is on the reference plane
                                        ''  get a point on the face. Any point will do.
                                        Dim pEdge As Edge = pPlanarFace.EdgeLoops.Item(0).Item(0)
                                        Dim pt As XYZ = pEdge.Evaluate(0.0)
                                        ''  is the point on the reference plane? 
                                        Dim res As Boolean = IsPointOnPlane(pt, refPlane)
                                        If res Then
                                            Return pPlanarFace  '' we found the face 
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next

                ElseIf TypeOf geomObj Is GeometryInstance Then
                    '' will come back later as needed.

                ElseIf TypeOf geomObj Is Curve Then
                    '' will come nack later as needed.

                ElseIf TypeOf geomObj Is Mesh Then
                    '' will come back later as needed.

                Else
                    '' what else do we have?

                End If
            Next

            '' if we come here, we did not find any.
            Return Nothing

        End Function
        Private Shared Function IsPointOnPlane(ByVal p1 As XYZ, ByVal plane As ReferencePlane)

            ''  get the plane equation 
            Dim n As XYZ = plane.Normal
            Dim p0 As XYZ = plane.GetPlane.Origin

            Dim dt As Double = n.DotProduct(p1 - p0)

            If IsAlmostEqual(dt, 0.0) Then Return True
            Return False

        End Function
        ''' <summary>
        ''' test if a given point lies on the given reference plane. 
        ''' linear equation of plane: ax + by + cz = d 
        ''' the normal is orthogonal to any vector on the plane: 
        '''     n.(p1 - p0) = 0 
        ''' compare two double values and judges if it is "almost equal".
        ''' </summary>
        ''' <remarks>Note: you may need to adjust the tolorance to fit your needs. </remarks>
        Private Shared Function IsAlmostEqual(ByVal val1 As Double, ByVal val2 As Double)

            Const tol As Double = 0.0001 '' hard coding the tolerance here. 

            If Math.Abs(val1 - val2) < tol Then Return True
            Return False

        End Function


    End Class

End Namespace