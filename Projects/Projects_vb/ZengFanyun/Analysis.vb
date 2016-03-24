Imports System
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.DB
Imports OldW.GlobalSettings
Imports OldW.Instrumentation
Imports std_ez
Imports System.Windows.Forms
Imports rvtTools_ez.ExtensionMethods
Imports OldW.Modeling
Imports System.IO
Imports rvtTools_ez.rvtTools

Namespace OldW.DataManager
    Public Class Analysis

        Private uidoc As UIDocument
        Private doc As Document
        Private SelectedElements As List(Of ElementId)
        Private ViolateList As List(Of ElementId)
        Public Sub New(ByVal eleIds As ICollection(Of ElementId), UIDocument As UIDocument)

            ' --------------------
            Me.uidoc = UIDocument
            Me.doc = Me.uidoc.Document
            Me.SelectedElements = eleIds
        End Sub

        Public Sub CheckData()
            ViolateList = New List(Of ElementId)

            Dim fami As Family = doc.FindFamily(GlobalSettings.InstrumentationType.地表隆沉.ToString)
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
            Dim strData As String = ele.Parameter(Constants.SP_Guid_Monitor).AsString
            Dim Dt As MonitorData_Point = DirectCast(StringSerializer.Decode64(strData), MonitorData_Point)
            Dim v As Object
            With Dt
                For i As UInt32 = 0 To .arrDate.Length - 1
                    v = .arrValue(i)
                    Dim WV As WarningValue = GetWarningValue(GlobalSettings.ProjectPath.Path_WarningValueUsing)
                    '
                    If (v IsNot Nothing) AndAlso (v > WV.warningGSetle.sum) Then
                        Return True
                    End If
                Next
            End With
            Return blnIsViolated
        End Function

    End Class
End Namespace