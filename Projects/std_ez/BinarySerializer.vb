Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Namespace std_ez
    ''' <summary>
    ''' 将任意一个有 Serializable标记的类以二进制转换器进行类中所有数据与字符串间的相互序列化。
    ''' 即可以将类中的数据（包括数组）序列化为字符，还可以将序列化的字符反序列化为一个类。
    ''' </summary>
    Public Class BinarySerializer

        ''' <summary>
        ''' Resolve System.Runtime.Serialization.SerializationException, Message = 
        ''' "Unable to find assembly 'StoreData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'."
        ''' One solution is to ensure that assembly resides in same directory as acad.exe or revit.exe,
        ''' the other is to implement a class such as this, cf. 
        ''' http://www.codeproject.com/soap/Serialization_Samples.asp
        ''' </summary>
        Private NotInheritable Class ZengfyLinkBinder
            Inherits System.Runtime.Serialization.SerializationBinder

            Public Overrides Function BindToType(ByVal assemblyName As String, ByVal typeName As String) As System.Type
                Return Type.GetType(String.Format("{0}, {1}", typeName, assemblyName))
            End Function

        End Class

        ''' <summary>
        ''' Encode arbitrary .NET serialisable object 
        ''' into binary data encodes as base64 string.
        ''' </summary>
        Public Shared Function Encode64(ByVal obj As Object) As String
            ' serialize into binary stream
            Dim f As New BinaryFormatter()
            Dim stream As New MemoryStream()
            f.Serialize(stream, obj)
            stream.Position = 0

            ' encode binary data to base64 string
            Dim n As Integer = CInt(stream.Length)
            Dim buf(n - 1) As Byte
            stream.Read(buf, 0, n)
            Return Convert.ToBase64String(buf)
        End Function

        ''' <summary>
        ''' Decode arbitrary .NET serialisable object 
        ''' from binary data encoded as base64 string.
        ''' </summary>
        Public Shared Function Decode64(ByVal s64 As String) As Object
            ' decode string back to binary data:
            Dim s As New MemoryStream(Convert.FromBase64String(s64))
            s.Position = 0

            ' deserialize:
            Dim f As New BinaryFormatter()
            'f.AssemblyFormat = FormatterAssemblyStyle.Simple;
            ' add this line below to avoid the "unable to find assembly" issue:
            f.Binder = New ZengfyLinkBinder()
            Return f.Deserialize(s)
        End Function

    End Class

    Public Class Test
        Public Shared Function testFun() As Boolean
            Return True
        End Function
    End Class

    ''' <summary>
    ''' 一个测试序列化与反序列化的模块，可以删除。
    ''' </summary>
    ''' <remarks></remarks>
    Friend Module BinarySerializerTest

        Private Sub main()
            ' 创建一个类
            Dim b As New DataB("字符", 123, 456)
            ' serialise and encode data into string: 将一个类及其中的所有值序列化为字符串
            Dim s64 As String = BinarySerializer.Encode64(b)
            ' decode and deserialise data back:  从字符串反序列化为一个类，其中包含此类中的所有数据，甚至是数组数据。
            Dim res As Object = BinarySerializer.Decode64(s64)
            '
            Dim b2 As DataB = TryCast(res, DataB)
            ' Add a breakpoint here.
        End Sub

        <Serializable()>
        Private Class DataA
            Public I As Integer
            Public D As Double
            Public Sub New(ByVal i As Integer, ByVal d As Double)
                Me.I = i
                Me.D = d
            End Sub
        End Class

        <Serializable()>
        Private Class DataB
            Public S As String
            Public A As DataA
            Private arr(0 To 100) As Double
            Public Sub New(ByVal s As String, ByVal i As Integer, ByVal d As Double)
                Me.S = s
                A = New DataA(i, d)
                For i = 0 To 100
                    arr(i) = i
                Next
            End Sub
        End Class

    End Module

End Namespace