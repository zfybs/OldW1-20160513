Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI

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
    ''' <param name="FamilySymbol"></param>
    <System.Runtime.CompilerServices.Extension()>
    Function Instances(Family As Family) As FilteredElementCollector
        Dim doc As Document = Family.Document
        Dim SymbolsId As ISet(Of ElementId) = Family.GetFamilySymbolIds
        Dim Collector1 As New FilteredElementCollector(doc)

        If SymbolsId.Count > 0 Then
            ' 创建过滤器 
            Dim Filter As New FamilyInstanceFilter(doc, SymbolsId(0))
            ' 执行过滤条件
            Collector1.WherePasses(Filter)
        End If
        ' 当族类型多于一个时，才进行相交
        If SymbolsId.Count > 1 Then
            For index As Integer = 1 To SymbolsId.Count - 1
                ' 创建过滤器   
                Dim Filter As New FamilyInstanceFilter(doc, SymbolsId(index))
                Dim Collector2 As New FilteredElementCollector(doc)
                ' 执行过滤条件
                Collector2.WherePasses(Filter)

                ' 将此FamilySymbol的实例添加到集合中
                Collector1.UnionWith(Collector2)
            Next
        End If
        Return Collector1
    End Function

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

End Module
