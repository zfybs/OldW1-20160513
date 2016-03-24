Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit
Imports OldW.GlobalSettings
Imports System.IO
Imports Autodesk.Revit.UI.Selection
Imports rvtTools_ez

Public Class Excavation

#Region "   ---   Fields"

    Private WDoc As OldWDocument
    Private Doc As Document
    Private App As Autodesk.Revit.ApplicationServices.Application

#End Region

    Public Sub New(ByVal OldWDoc As OldWDocument)
        Me.WDoc = OldWDoc
        Me.Doc = OldWDoc.Document
        Me.App = Doc.Application
    End Sub

#Region "   ---   创建与设置土体"

#Region "   ---   模型土体"

    ''' <summary>
    ''' 创建模型土体，此土体单元在模型中应该只有一个。
    ''' </summary>
    ''' <param name="CreateMultipli">是否要在一个族中创建多个实体</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateModelSoil(Optional ByVal CreateMultipli As Boolean = False) As Extrusion

        ' 获得用来创建实体的模型线
        Dim CurveArrArr As CurveArrArray = GetLoopedCurveArray(CreateMultipli)
        If (Not CurveArrArr.IsEmpty) Then
            ' 构造一个族文档
            Dim FamilyDoc As Document = CreateSoilFamily()

            Dim sp As SketchPlane
            ' 在族文档中绘制实体
            Using t As New Transaction(FamilyDoc, "添加实体")
                t.Start()
                sp = SketchPlane.Create(FamilyDoc, New Plane())
                Dim FamilyCreation As Creation.FamilyItemFactory = FamilyDoc.FamilyCreate
                FamilyCreation.NewExtrusion(True, CurveArrArr, sp, -102)
                t.Commit()
            End Using

            ' 将族加载到项目文档中
            Dim f As Family = FamilyDoc.LoadFamily(Doc, UIDocument.GetRevitUIFamilyLoadOptions)
            FamilyDoc.Close(False)
            ' 获取一个族类型，以加载一个族实例到项目文档中
            Dim fs As FamilySymbol = f.GetFamilySymbolIds.First.Element(Doc)

            Using t As New Transaction(Doc, "添加族实例")
                t.Start()
                ' 为族重命名
                f.Name = Constants.FamilyName_Soil

                If Not fs.IsActive Then
                    fs.Activate()
                End If
                Doc.Create.NewFamilyInstance(New XYZ(), fs, [Structure].StructuralType.NonStructural)
                t.Commit()
            End Using

        Else

        End If
        Return Nothing
    End Function


#End Region

#Region "   ---   开挖土体"


    ''' <summary>
    ''' 创建开挖土体，此土体单元在模型中可以有很多个。
    ''' </summary>
    ''' <param name="CreateMultipli">是否要在一个族中创建多个实体</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateExcavationSoil(ByVal CreateMultipli As Boolean) As Extrusion

        ' 获得用来创建实体的模型线
        Dim CurveArrArr As CurveArrArray = GetLoopedCurveArray(CreateMultipli)
        If (Not CurveArrArr.IsEmpty) Then
            ' 构造一个族文档
            Dim FamDoc As Document = CreateSoilFamily()

            Dim sp As SketchPlane

            Using t As New Transaction(FamDoc, "添加参数与拉伸实体")
                t.Start()

                ' 添加参数
                Dim FM As FamilyManager = FamDoc.FamilyManager
                Dim ExDef As ExternalDefinition = rvtTools.GetOldWDefinitionGroup(App).Definitions.Item(Constants.SP_ExcavationCompleted)
                FM.AddParameter(ExDef, BuiltInParameterGroup.PG_DATA, True)

                Try  ' 在族文档中绘制实体
                    sp = SketchPlane.Create(FamDoc, New Plane())
                    Dim FamilyCreation As Creation.FamilyItemFactory = FamDoc.FamilyCreate
                    FamilyCreation.NewExtrusion(True, CurveArrArr, sp, -10)
                    t.Commit()
                Catch ex As Exception
                    Dim res As DialogResult = MessageBox.Show("创建拉伸实体失败！" & vbCrLf & ex.Message, "出错", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    t.RollBack()
                End Try
            End Using

            ' 将族加载到项目文档中
            Dim fam As Family = FamDoc.LoadFamily(Doc, UIDocument.GetRevitUIFamilyLoadOptions)
            FamDoc.Close(False)
            ' 获取一个族类型，以加载一个族实例到项目文档中
            Dim fs As FamilySymbol = fam.GetFamilySymbolIds.First.Element(Doc)
            Using t As New Transaction(Doc, "添加族实例")
                t.Start()
                ' 族或族类型的重命名


                ' 创建实例
                If Not fs.IsActive Then
                    fs.Activate()
                End If
                Doc.Create.NewFamilyInstance(New XYZ(), fs, [Structure].StructuralType.NonStructural)
                t.Commit()
            End Using
        Else

        End If
        Return Nothing
    End Function


#End Region

    ' 几何创建
    ''' <summary>
    ''' 从模型中获取要创建开挖土体的边界线
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetLoopedCurveArray(ByVal CreateMultipli As Boolean) As CurveArrArray
        Dim Profiles As New CurveArrArray  ' 每一次创建开挖土体时，在NewExtrusion方法中，要创建的实体的轮廓
        Dim blnStop As Boolean
        Do
            blnStop = True
            Dim cvLoop As CurveLoop = GetLoopCurve()
            ' 检验并添加
            If cvLoop.Count > 0 Then
                Dim cvArr As New CurveArray
                For Each c As Curve In cvLoop
                    cvArr.Append(c)
                Next
                Profiles.Append(cvArr)

                ' 是否要继续添加
                If CreateMultipli Then
                    Dim res As DialogResult = MessageBox.Show("曲线添加成功，是否还要继续添加？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If res = DialogResult.Yes Then
                        blnStop = False
                    End If
                End If
            End If
        Loop Until blnStop
        Return Profiles
    End Function
    ''' <summary>
    ''' 获取一组连续封闭的模型线
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetLoopCurve() As CurveLoop
        Dim Boundaries As List(Of Reference) = New UIDocument(Doc).Selection.PickObjects(ObjectType.Element, New CurveSelectionFilter, "选择一组封闭的模型线。")
        Dim cvLoop As New CurveLoop
        Try
            If Boundaries.Count = 1 Then  ' 要么是封闭的圆或圆弧，要么就不封闭
                Dim c As Curve = DirectCast(Doc.GetElement(Boundaries.Item(0)), ModelCurve).GeometryCurve
                If ((TypeOf c Is Arc) OrElse (TypeOf c Is Ellipse)) AndAlso (Not c.IsBound) Then
                    cvLoop.Append(c)
                Else
                    Throw New Exception("选择的一条圆弧线或者椭圆线并不封闭。")
                End If
            Else
                ' 对于选择了多条曲线的情况
                Dim cs As List(Of Curve) = GeoHelper.GetContiguousCurvesFromSelectedCurveElements(Doc, Boundaries)
                For Each c As Curve In cs
                    cvLoop.Append(c)
                Next
                If cvLoop.IsOpen OrElse Not cvLoop.HasPlane Then
                    Throw New Exception
                Else
                    Return cvLoop
                End If
            End If
        Catch ex As Exception
            Dim res As DialogResult = MessageBox.Show("所选择的曲线不在同一个平面上，或者不能形成封闭的曲线，请重新选择。" & vbCrLf & ex.Message, "Warnning", MessageBoxButtons.OKCancel)
            If res = DialogResult.OK Then
                cvLoop = GetLoopCurve()
            Else
                cvLoop = New CurveLoop
                Return cvLoop
            End If
        End Try
        Return cvLoop
    End Function

    ''' <summary>
    ''' 创建一个模型土体（或者是开挖土体）的族文档，并将其打开。
    ''' </summary>
    Private Function CreateSoilFamily() As Document
        Dim app = Doc.Application
        Dim TemplateName As String = Path.Combine(ProjectPath.Path_family, Constants.FamilyTemplateName_Soil)
        If Not File.Exists(TemplateName) Then
            Throw New FileNotFoundException("在创建土体时，族样板文件没有找到", TemplateName)
        Else
            Dim FamDoc As Document = app.NewFamilyDocument(TemplateName)
            Dim F As Family = FamDoc.OwnerFamily
            Using T As New Transaction(FamDoc, "设置族类别与名称")
                T.Start()
                ' 设置族的族类别
                F.FamilyCategory = FamDoc.Settings.Categories.Item(BuiltInCategory.OST_Site)
                ' 设置族的名称
                T.Commit()
            End Using
            Return FamDoc
        End If
    End Function
    ''' <summary>
    ''' 曲线选择过滤器
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CurveSelectionFilter
        Implements ISelectionFilter
        Public Function AllowElement(element As Element) As Boolean Implements ISelectionFilter.AllowElement
            Dim bln As Boolean = False
            If (TypeOf element Is ModelCurve) Then
                Return True
            End If
            Return bln
        End Function

        Public Function AllowReference(refer As Reference, point As XYZ) As Boolean Implements ISelectionFilter.AllowReference
            Return False
        End Function
    End Class

    ' 参数添加与关联
    ''' <summary>
    ''' 将土体单元中的深度值与相应的参数进行关联
    ''' </summary>
    ''' <param name="FamDoc"></param>
    ''' <remarks></remarks>
    Private Sub ConnectElevation(ByVal FamDoc As Document)



    End Sub

#End Region

End Class
