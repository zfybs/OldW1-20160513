Namespace OldW.DllActivator
    Public Class DllActivator_OldWGlobal
        Implements IDllActivator
        ''' <summary>
        ''' 激活本DLL所引用的那些DLLs
        ''' </summary>
        Public Sub ActivateReferences() Implements IDllActivator.ActivateReferences
            Dim dat As IDllActivator
            dat = New DllActivator_std_vb
            dat.ActivateReferences()
        End Sub

    End Class
End Namespace