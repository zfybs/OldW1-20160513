using System;
namespace OldW.DllActivator
{
    public class DllActivator_std_cs : IDllActivator
    {
        /// <summary>
        /// 激活本DLL所引用的那些DLLs
        /// </summary>
        void IDllActivator.ActivateReferences()
        {
            IDllActivator dat = new DllActivator_std_vb();
            dat.ActivateReferences();
        }
    }
}