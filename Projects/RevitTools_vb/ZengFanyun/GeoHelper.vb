Imports System
Imports System.Collections.Generic
Imports Autodesk.Revit
Imports Autodesk.Revit.DB

Namespace rvtTools_ez
    ''' <summary>
    ''' A object to help locating with geometry data.
    ''' </summary>
    Public Class GeoHelper
        'Defined the precision.
        Private Const Precision As Double = 0.0001

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

            ' The bottom face is consider as which's average elevation is the lowest, except vertical
            ' face.
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
            Dim isNotEqual As Boolean = (Precision < Math.Abs(vectorA.X - vectorB.X)) OrElse (Precision < Math.Abs(vectorA.Y - vectorB.Y))

            Return If(isNotEqual, False, True)
        End Function

    End Class
End Namespace