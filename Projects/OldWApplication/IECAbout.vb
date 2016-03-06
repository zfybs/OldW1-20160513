Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks

Imports Autodesk.Revit
Imports Autodesk.Revit.DB
Imports Autodesk.Revit.UI
Imports Autodesk.Revit.ApplicationServices

<Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)>
Public Class IECAbout
    Implements IExternalCommand

#Region "IExternalCommand Members"

    Public Function Execute(ByVal commandData As ExternalCommandData, ByRef message As String, ByVal elements As ElementSet) As Result Implements IExternalCommand.Execute
        Dim aboutString As String = "OldW" & ControlChars.Lf & "by Zw & Zengfy"
        TaskDialog.Show("About", aboutString)
        Return Result.Succeeded
    End Function

#End Region
End Class
