using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace zwRevitTools
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    public static class ExtensionRevitLib
    {
        /// <summary>
        /// 从elementId获得element
        /// </summary>
        /// <param name="eleId"></param>
        /// <param name="uidoc"></param>
        /// <returns></returns>
        public static Element getElementById(this ElementId eleId, Document doc)
        {
            Element ele = null;
            ele = doc.GetElement(eleId) as Element;
            return ele;
        }
    }
}
