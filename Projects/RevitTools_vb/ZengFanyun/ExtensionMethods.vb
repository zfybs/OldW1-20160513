Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Namespace rvtTools_ez

    ''' <summary>
    ''' Revit中对象的扩展方法
    ''' </summary>
    ''' <remarks></remarks>
    Public Module ExtensionMethods

#Region "  ---  elementId"

        ''' <summary> 从ElementId返回其所在的Document中的Element对象 </summary>
        ''' <param name="elementId"></param>
        ''' <param name="Doc">此elementId所位于的文档</param>
        <System.Runtime.CompilerServices.Extension()>
        Function Element(elementId As ElementId, Doc As Document) As Element
            Return Doc.GetElement(elementId)
        End Function

#End Region

#Region "  ---  DB.Document"

        ''' <summary> 返回项目文档中指定名称的族Family对象 </summary>
        ''' <param name="FamilyName">在此文档中，所要搜索的族对象的名称</param>
        <System.Runtime.CompilerServices.Extension()>
        Function FindFamily(Doc As Document, FamilyName As String) As Family
            Dim fam As Family = Nothing
            ' 文档中所有的族对象
            Dim cols As New FilteredElementCollector(Doc)
            Dim Familys As IList(Of Element) = cols.OfClass(GetType(Family)).ToElements
            ' 按名称搜索族（Linq语句）
            Dim Q = From ff As Family In Familys
                  Where ff.Name = FamilyName
                  Select ff
            If Q.Count > 0 Then
                fam = Q.First
            End If
            Return fam
        End Function

#End Region

#Region "  ---  Family"

        ''' <summary> 返回项目文档中某族Family的所有实例 </summary>
        ''' <param name="Category">此族所属的 BuiltInCategory 类别，如果不确定，就不填。</param>
        ''' <param name="Family"></param>
        <System.Runtime.CompilerServices.Extension()>
        Function Instances(Family As Family, Optional ByVal Category As BuiltInCategory = BuiltInCategory.INVALID) As FilteredElementCollector
            Dim doc As Document = Family.Document
            Dim SymbolsId As ISet(Of ElementId) = Family.GetFamilySymbolIds
            Dim Collector1 As New FilteredElementCollector(doc)

            If SymbolsId.Count > 0 Then
                ' 创建过滤器 
                Dim Filter As New FamilyInstanceFilter(doc, SymbolsId(0))
                ' 执行过滤条件
                If Category <> BuiltInCategory.INVALID Then
                    Collector1 = Collector1.OfCategory(Category)
                End If
                Collector1.WherePasses(Filter)
            End If
            ' 当族类型多于一个时，才进行相交
            If SymbolsId.Count > 1 Then
                For index As Integer = 1 To SymbolsId.Count - 1
                    ' 创建过滤器   
                    Dim Filter As New FamilyInstanceFilter(doc, SymbolsId(index))
                    Dim Collector2 As New FilteredElementCollector(doc)
                    ' 执行过滤条件
                    If Category <> BuiltInCategory.INVALID Then
                        Collector2 = Collector2.OfCategory(Category)
                    End If
                    Collector2.WherePasses(Filter)

                    ' 将此FamilySymbol的实例添加到集合中
                    Collector1.UnionWith(Collector2)
                Next
            End If
            Return Collector1
        End Function

        ''' <summary> 对Document中加载的族进行重命名 </summary>
        ''' <param name="Family"></param>
        ''' <param name="NewName">要重新命名的新名称</param>
        <System.Runtime.CompilerServices.Extension()>
        Sub ReName(Family As Family, ByVal NewName As String)
            Dim doc As Document = Family.Document
            Using tran As New Transaction(doc, "Rename family")
                tran.Start()
                Family.Name = NewName
                tran.Commit()
            End Using
        End Sub

#End Region

#Region "  ---  FamilySymbol"

        ''' <summary> 返回项目文档中某族类型FamilySymbol的所有实例 </summary>
        ''' <param name="FamilySymbol"></param>
        <System.Runtime.CompilerServices.Extension()>
        Function Instances(FamilySymbol As FamilySymbol) As FilteredElementCollector
            Dim doc As Document = FamilySymbol.Document
            Dim FamilySymbolId As ElementId = FamilySymbol.Id
            Dim InsancesColl As New FilteredElementCollector(doc)
            Dim FIFilter As New FamilyInstanceFilter(doc, FamilySymbolId)
            InsancesColl.WherePasses(FIFilter)
            Return InsancesColl
        End Function

#End Region

#Region "  ---  Transform"
        ''' <summary> 以矩阵的形式返回变换矩阵，仅作显示之用 </summary>
        ''' <param name="Trans"></param>
        <System.Runtime.CompilerServices.Extension()>
        Function ToString_Matrix(Trans As Transform) As String
            Dim str As String = ""
            With Trans
                str = "(" & .BasisX.X.ToString("0.000") & "  ,  " & .BasisY.X.ToString("0.000") & "  ,  " & .BasisZ.X.ToString("0.000") & "  ,  " & .Origin.X.ToString("0.000") & ")" & vbCrLf &
                      "(" & .BasisX.Y.ToString("0.000") & "  ,  " & .BasisY.Y.ToString("0.000") & "  ,  " & .BasisZ.Y.ToString("0.000") & "  ,  " & .Origin.Y.ToString("0.000") & ")" & vbCrLf &
                      "(" & .BasisX.Z.ToString("0.000") & "  ,  " & .BasisY.Z.ToString("0.000") & "  ,  " & .BasisZ.Z.ToString("0.000") & "  ,  " & .Origin.Z.ToString("0.000") & ")"
            End With
            Return str
            Return str
        End Function

#End Region

#Region "  ---  Double"
        ''' <summary> 长度单位转换：将英尺转换为毫米 1英尺=304.8mm </summary>
        ''' <param name="value_foot"></param>
        ''' <remarks> 1 foot = 12 inches = 304.8 mm</remarks>
        <System.Runtime.CompilerServices.Extension()>
        Function Foot2mm(value_foot As Double) As Double
            ' 1 foot = 12 inches = 304.8 mm
            Return value_foot * 304.8
        End Function
#End Region



    End Module
End Namespace