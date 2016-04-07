Imports System.Text.RegularExpressions

Namespace std_ez

    ''' <summary>
    ''' 提供一些基础性的操作工具
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Utils

        ''' <summary>
        ''' 将集合中的每一个元素的ToString函数的结果组合到一个字符串中进行显示
        ''' </summary>
        ''' <param name="V"></param>
        ''' <remarks></remarks>
        Public Shared Sub ShowEnumerable(ByVal V As IEnumerable)
            Dim str As String = ""
            For Each o As Object In V
                str = str & o.ToString & vbCrLf
            Next
            MessageBox.Show(str, "集合", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Sub

        ''' <summary>
        ''' 将字符转换为日期。除了.NET能够识别的日期格式外，
        ''' 还增加了20160406（ 即 2016/04/06），以及 201604061330（即 2016/04/06 13:30）
        ''' </summary>
        ''' <param name="text">要转换为日期的字符。</param>
        ''' <returns></returns>
        Public Shared Function String2Date(ByVal text As String, ByRef ResultedDate As Date) As Boolean
            Dim blnSucceed As Boolean
            ' 模式1. 正常的日期格式
            If Date.TryParse(text, ResultedDate) Then
                Return True
            End If

            ' 模式2. 20160406 ， 即 2016/04/06
            If text.Length = 8 Then
                Try
                    ResultedDate = New Date(Integer.Parse(text.Substring(0, 4)),
                                            Integer.Parse(text.Substring(4, 2)),
                                            Integer.Parse(text.Substring(6, 2)))
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End If

            ' 模式3. 201604061330 ， 即 2016/04/06 13:30
            If text.Length = 12 Then
                Try
                    ResultedDate = New Date(Integer.Parse(text.Substring(0, 4)),
                                            Integer.Parse(text.Substring(4, 2)),
                                            Integer.Parse(text.Substring(6, 2)),
                                            Integer.Parse(text.Substring(8, 2)),
                                            Integer.Parse(text.Substring(10, 2)), 0)
                    Return True
                Catch ex As Exception
                    Return False
                End Try
            End If
            Return blnSucceed
        End Function
    End Class
End Namespace