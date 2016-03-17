using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using OldW;
using rvtTools_zw;

namespace OldW
{
    namespace Modeling
    {
        /// <summary>
        /// 沉降测点建模
        /// </summary>
        [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
        public class cmd_SetFamilySettlement : IExternalCommand
        {
            #region IExternalCommand Members

            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                try
                {
                    UIDocument uidoc = commandData.Application.ActiveUIDocument;
                    ElementId foundId = null;

                    //是否存在族
                    Boolean found = FilterTools.existFamliyByName(uidoc.Document, GlobalSettings.InstrumentationType.地表隆沉.ToString(), out foundId);

                    Family family = null;
                    if (found == true)
                    {
                        //如果存在，获得文件该族
                        family = uidoc.Document.GetElement(foundId) as Family;
                    }
                    else
                    {
                        //如果不存在，载入族
                        Transaction trans = new Transaction(uidoc.Document, "trans");
                        trans.Start();
                        uidoc.Document.LoadFamily(Path.Combine(GlobalSettings.ProjectPath.Path_family, GlobalSettings.InstrumentationType.地表隆沉.ToString() + ".rfa"), out family);
                        trans.Commit();
                    }

                    //获得该族的族类型，并且放置族实例
                    FamilySymbol symbol = uidoc.Document.GetElement(family.GetFamilySymbolIds().ElementAt(0)) as FamilySymbol;
                    uidoc.PostRequestForElementTypePlacement(symbol);

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