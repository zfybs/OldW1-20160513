using System;

namespace OldW.DllActivator
{
public class DllActivator_Projects_cs : IDllActivator
{
        /// <summary>
        /// 激活本DLL所引用的那些DLLs
        /// </summary>
        void IDllActivator.ActivateReferences()
    {
        IDllActivator dat;
        //
        dat = new DllActivator_std_vb();
        dat.ActivateReferences();
        //
        dat = new DllActivator_OldWGlobal();
        dat.ActivateReferences();
        //
        dat = new DllActivator_RevitTools_cs();
        dat.ActivateReferences();
    }
} 
}