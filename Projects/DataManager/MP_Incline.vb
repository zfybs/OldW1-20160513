Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports eZRevtiTools.ExtensionMethods
Imports System.Math

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

    ''' <summary> 测斜管的位置是在模型中的开挖土体的内部还是外部，即测斜管与开挖土体的Element是否相交。 </summary>
    ''' <remarks>True if the inclinometer is inside the excavation earth, 
    ''' and False if the inclinometer is outside the excavation earth.</remarks>
    Private IsInsideEarth As Boolean

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

        ' 将测斜管的底部端点作为原点
        Dim lc As LocationPoint = TryCast(Me.Inclinometer.Location, LocationPoint)
        Dim ptOrigin As XYZ = lc.Point

        '
        ' 土体
        Dim eleEarht As FamilyInstance = FindAdjacentEarth()

        ' 将当前3D视图作为ReferenceIntersector的构造参数
        Dim view3d As View3D = Nothing
        view3d = TryCast(Doc.ActiveView, View3D)
        If view3d Is Nothing Then
            TaskDialog.Show("3D view", "current view should be 3D view")
            Return Result.Failed
        End If

        ' 将土体单元作为搜索相交面的目标，搜索其中所有的相交面
        Dim IntersectedEarth As New ReferenceIntersector(eleEarht.Id, FindReferenceTarget.Face, view3d)


        ' Dim references As IList(Of ReferenceWithContext) = ReferenceIntersector1.Find(origin:=ptStart, direction:=(ptEnd - ptStart))
        Dim PtBottom = FindBottomPoint(ptOrigin, Me.IsInsideEarth, eleEarht, IntersectedEarth)

        ' 将测点附近的那个底部点（开挖土体内部）向上发出一条射线，以得到真实的土体开挖面
        Dim refContext As ReferenceWithContext = IntersectedEarth.FindNearest(origin:=PtBottom, direction:=New XYZ(0, 0, 1))
        Dim ExcavFace As Face = eleEarht.GetGeometryObjectFromReference(refContext.GetReference)
        Dim Elevation As XYZ = refContext.GetReference.GlobalPoint
        TaskDialog.Show("哈哈", "找到了开挖面 " & Elevation.ToString)
        Return 0

    End Function

    Private Function FindAdjacentEarth() As FamilyInstance
        Dim eleEarht As FamilyInstance

        eleEarht = Doc.GetElement(New ElementId(460116))

        Return eleEarht
    End Function


    ''' <summary>
    ''' 搜索一个底部坐标点，有了此点后，只要向上发射一条射线，即可以找到此时的开挖面
    ''' </summary>
    ''' <param name="ptInclinometerBottom">测斜管的底部坐标点</param>
    ''' <param name="IsInside">测斜管是否在开挖土体Element的内部</param>
    ''' <param name="Earth">开挖墙体单元</param>
    ''' <param name="IntersectedEarth">用来搜索相交面的开挖土体</param>
    ''' <returns></returns>
    ''' <remarks>  如果测斜管就在土体内部，那么测斜管的底部点就可以直接用来向上发射射线了。
    ''' 如果测斜管在土体外部，那么需要以测斜管的底部点为中心，向四周发射多条射线，
    ''' 这些射线分别都与土体相交，找到距离土体最近的那一条射线所对应的相交点与相交面，然后将相交点向面内偏移一点点，即可以作为寻找开挖面的射线的原点了。</remarks>
    Private Function FindBottomPoint(ptInclinometerBottom As XYZ, ByVal IsInside As Boolean, Earth As FamilyInstance, IntersectedEarth As ReferenceIntersector) As XYZ
        Dim PtBottom As New XYZ
        If IsInside Then
            ' 如果测斜管就在土体内部，那么测斜管的底部点就可以直接用来向上发射射线了。
            Return ptInclinometerBottom
        Else
            ' 如果测斜管在土体外部，那么需要以测斜管的底部点为中心，向四周发射多条射线，
            ' 这些射线分别都与土体相交，找到距离土体最近的那一条射线所对应的相交点与相交面，然后将相交点向面内偏移一点点，即可以作为寻找开挖面的射线的原点了。
            Dim NearestDist As Double = Double.MaxValue
            Dim NearestRef As Reference = Nothing
            Dim NearestDir As XYZ
            Dim dire As XYZ
            For angle As Single = 0 To 2 * Math.PI Step Math.PI / 6
                ' 创建一个水平面上的方向向量，此向量与x轴的夹角为angle
                dire = New XYZ(Cos(angle), Sin(angle), 0)
                Dim refContext As ReferenceWithContext = IntersectedEarth.FindNearest(origin:=ptInclinometerBottom, direction:=dire)
                If (refContext IsNot Nothing) AndAlso (refContext.Proximity < NearestDist) Then
                    NearestDist = refContext.Proximity
                    NearestRef = refContext.GetReference()
                    NearestDir = dire
                End If
            Next

            If NearestRef IsNot Nothing Then
                ' 找到最近的那个相交射线所对应的相交面，此面即为此离测斜管最近的那个土体的侧面（竖向）
                '
                Dim VerticalFace As Face = Earth.GetGeometryObjectFromReference(NearestRef)

                ' 测斜管底部点到找到的最近的面的垂足点
                Dim Normal As XYZ = VerticalFace.ComputeNormal(NearestRef.UVPoint)  ' 此面的法向

                ' 将垂足点沿着面的法向的反方向延长 0.001 英尺，以进入土体内部
                PtBottom = ptInclinometerBottom - Normal * (NearestDist + 0.001)
            Else
                TaskDialog.Show("出错！", "未找到离此测斜点最近的开挖面")
                Return Nothing
            End If

        End If
        Return PtBottom
    End Function
End Class
