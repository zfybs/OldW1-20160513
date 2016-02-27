using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

using zwRevitTools;
using eZstd;
using OldW;

namespace OldW
{
    namespace Modeling
    {
        /// <summary>
        /// test command
        /// </summary>
        [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
        public class SetWarning : IExternalCommand
        {
            #region IExternalCommand Members

            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                try
                {
                    bool b = Test.testFun();
                    FormSetWarning formSet = new FormSetWarning();
                    formSet.Show();
                }
                catch (Exception e)
                {
                    message = e.Message;
                    return Autodesk.Revit.UI.Result.Failed;
                }
                return Result.Succeeded;
            }

            #endregion
        }
    }
}
