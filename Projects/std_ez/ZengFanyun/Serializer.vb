Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Namespace std_ez

    ''' <summary>
    ''' 将任意一个有 Serializable标记的类以二进制转换器将类中所有数据与字符串间的相互序列化。
    ''' 即可以将类中的数据（包括数组）序列化为字符，还可以将序列化的字符反序列化为一个类。
    ''' </summary>
    Public Class StringSerializer

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

            ' 将二进制数据编码为base64的字符串
            Dim n As Integer = CInt(stream.Length)
            Dim buf(n - 1) As Byte
            stream.Read(buf, 0, n)
            ' 如果想将二进制字节数组转直接换成字符串，可以使用具有8位编码的字符集转换，但不能使用其它字符集，比如Unicode、GB2312.
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

        ''' <summary>
        ''' 为了解决SerializationException，方法之一是确保此assembly放置在与acad.exe 或 revit.exe相同的文件夹中，
        ''' 另一个方法就是实现一个像这样的类。
        ''' </summary>
        ''' <remarks>
        '''  Resolve System.Runtime.Serialization.SerializationException, Message = 
        ''' "Unable to find assembly 'StoreData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'."
        ''' One solution is to ensure that assembly resides in same directory as acad.exe or revit.exe,
        ''' the other is to implement a class such as this, cf. 
        ''' http://www.codeproject.com/soap/Serialization_Samples.asp
        ''' </remarks>
        Private NotInheritable Class ZengfyLinkBinder
            Inherits System.Runtime.Serialization.SerializationBinder

            Public Overrides Function BindToType(ByVal assemblyName As String, ByVal typeName As String) As System.Type
                Return Type.GetType(String.Format("{0}, {1}", typeName, assemblyName))
            End Function

        End Class

    End Class

    ''' <summary>
    ''' 在.NET中，我们可以将对象序列化从而保存对象的状态到内存或者磁盘文件中，或者分布式应用程序中用于系统通信，，这样就有可能做出一个“对象数据库”了。
    ''' 一般来说，二进制序列化的效率要高，所获得的字节数最小。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class BinarySerializer

        ''' <summary>
        ''' 将任意一个声明为Serializable的类或者其List等集合中的数据，以二进制的格式保存到对应的流文件中。
        ''' </summary>
        ''' <param name="fs">推荐使用FileStream对象。此方法中不会对Stream对象进行Close。</param>
        ''' <param name="Data">要进行保存的可序列化对象</param>
        ''' <remarks></remarks>
        Public Shared Sub EnCode(ByVal fs As Stream, ByVal Data As Object)
            Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter ' 最关键的对象，用来进行类到二进制的序列化与反序列化操作
            bf.Serialize(fs, Data)
        End Sub

        ''' <summary>
        ''' 从二进制流文件中，将其中的二进制数据反序列化为对应的类或集合对象。
        ''' </summary>
        ''' <param name="fs">推荐使用FileStream对象。此方法中不会对Stream对象进行Close。</param>
        ''' <returns>此二进制流文件所对应的可序列化对象</returns>
        ''' <remarks></remarks>
        Public Shared Function DeCode(ByVal fs As Stream) As Object
            Dim bf As New System.Runtime.Serialization.Formatters.Binary.BinaryFormatter
            Dim dt As Object = bf.Deserialize(fs)
            Return dt
        End Function

    End Class

End Namespace