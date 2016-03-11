Imports System.IO
Imports std_ez
Imports OldW
Imports OldW.Modeling

Namespace rvtTools_ez

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class rvtTools

        ''' <summary>
        ''' 提取WarningValueUsing.dat中的WarningValue类
        ''' </summary>
        ''' <param name="FullPath">WarningValueUsing.dat的绝对路径</param>
        Public Shared Function GetWarningValue(ByVal FullPath As String) As WarningValue
            Dim fsr As New StreamReader(FullPath)
            Dim strData1 As String = fsr.ReadLine
            fsr.Close()
            '
            Dim a = GlobalSettings.InstrumentationType.墙体测斜
            Dim WV As WarningValue = BinarySerializer.Decode64(strData1)
            '
            Return WV
        End Function

    End Class


End Namespace