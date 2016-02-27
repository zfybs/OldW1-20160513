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
Imports eZRevtiTools.rvtTools

Public Class Analysis

    Private uidoc As UIDocument
    Private doc As Document
    Private SelectedElements As List(Of ElementId)
    Private ViolateList As List(Of ElementId)
    Public Sub New(ByVal eleIds As ICollection(Of ElementId), document As UIDocument)

        ' --------------------
        Me.uidoc = document
        Me.doc = Me.uidoc.Document
        Me.SelectedElements = eleIds
    End Sub

    Public Sub CheckData()
        ViolateList = New List(Of ElementId)

        Dim fami As Family = doc.FindFamily(GlobalConstant.FamilyName.沉降测点.ToString)
        '
        Dim Monitor As New List(Of ElementId)
        If fami IsNot Nothing Then
            Monitor = fami.Instances.ToElementIds
        End If

        For Each eleId As ElementId In SelectedElements
            ' 判断图形是否是测点
            If Monitor.Contains(eleId) Then

                If IsViolated(eleId) Then
                    ViolateList.Add(eleId)
                End If
            End If

        Next
        ' 选择单元
        With uidoc.Selection
            .SetElementIds(ViolateList)
        End With

        ' 将结果显示在表格中
        Dim f As New System.Windows.Forms.Form
        Dim dgv As New DataGridView
        f.Controls.Add(dgv)
        f.StartPosition = FormStartPosition.CenterScreen
        Dim col As New DataGridViewTextBoxColumn
        With dgv
            .AutoGenerateColumns = False
            .Columns.Clear()
            .Dock = DockStyle.Fill
            .Columns.Add(col)
            .DataSource = ViolateList
        End With
        With col
            .DataPropertyName = "IntegerValue"
            Dim a As Integer = 20
        End With
        f.ShowDialog()
    End Sub

    Private Function IsViolated(ByVal eleId As ElementId) As Boolean
        Dim blnIsViolated As Boolean = False
        '
        Dim ele As Element = doc.GetElement(eleId)
        Dim strData As String = ele.Parameter(GuidMonitor).AsString
        Dim Dt As MonitorData = DirectCast(BinarySerializer.Decode64(strData), MonitorData)
        Dim v As Object
        With Dt
            For i As UInt32 = 0 To .arrDate.Length - 1
                v = .arrValue(i)
                Dim WV As WarningValue = GetWarningValue(GlobalSettings.Path_WarningValueUsing)
                '
                If (v IsNot Nothing) AndAlso (v > WV.warningGSetle.sum) Then
                    Return True
                End If
            Next
        End With
        Return blnIsViolated
    End Function

End Class
