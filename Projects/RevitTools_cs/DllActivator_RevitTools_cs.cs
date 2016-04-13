using System;
namespace OldW.DllActivator
{
    public class DllActivator_RevitTools_cs : IDllActivator
    {
        /// <summary>
        /// 激活本DLL所引用的那些DLLs
        /// </summary>
        void IDllActivator.ActivateReferences()
        {
            IDllActivator dat;
            dat = new DllActivator_std_vb();
            dat.ActivateReferences();
            //
            dat = new DllActivator_std_cs();
            dat.ActivateReferences();
            //
            dat = new DllActivator_std_vb();
            dat.ActivateReferences();
            //
        }
    }
}