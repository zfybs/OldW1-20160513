Namespace OldW.DllActivator
    ''' <summary>
    ''' 用于在打开非模态窗口的的IExternalCommand.Execute方法中，
    ''' </summary>
    Public Interface IDllActivator

        ''' <summary> 激活本DLL所引用的那些DLLs </summary>
        Sub ActivateReferences()

    End Interface

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class DllActivator_std_vb
        Implements IDllActivator

        ''' <summary>
        ''' 激活本DLL所引用的那些DLLs
        ''' </summary>
        Public Sub ActivateReferences() Implements IDllActivator.ActivateReferences
        End Sub
    End Class
End Namespace