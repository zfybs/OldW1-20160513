using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

using rvtTools_zw;
using std_ez;
using OldW;

namespace OldW
{
    namespace Modeling
    {
        /// <summary>
        /// test command
        /// </summary>
        [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
        public class cmd_SetWarning : IExternalCommand
        {
            #region IExternalCommand Members

            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                try
                {
                    MessageBox.Show("进入");
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
