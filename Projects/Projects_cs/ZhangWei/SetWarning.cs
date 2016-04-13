using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.IO;
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
                //Utils.SetDllDirectory(System.Reflection.Assembly.GetExecutingAssembly().Location);
                //MessageBox.Show(System.Reflection.Assembly.GetExecutingAssembly().Location);
                try
                {
                    FormSetWarning formSet = new FormSetWarning();
                    formSet.Show(null);
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
