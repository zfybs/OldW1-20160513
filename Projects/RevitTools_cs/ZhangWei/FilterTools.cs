using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

using std_zw;

namespace rvtTools_zw
{
    public class FilterTools
    {

        #region "-------族的名称判断document是否包含该族"
        /// <summary>
        /// 通过族的名称判断document是否包含该族
        /// </summary>
        /// <param name="doc">搜索的document</param>
        /// <param name="name">族名称</param>
        /// <param name="foundId">输出族Id</param>
        /// <returns>返回是否包含</returns>
        static public Boolean existFamliyByName(Document doc, String name, out ElementId foundId)
        {
            String famName = name;
            Boolean found = false;
            foundId = null;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<ElementId> famIds = collector.OfClass(typeof(Family)).ToElementIds();
            foreach (ElementId famId in famIds)
            {
                if (doc.GetElement(famId).Name == famName)
                {
                    found = true;
                    foundId = famId;
                    return found;
                }
            }
            return found;
        }

        /// <summary>
        /// 通过族的名称判断document是否包含该族
        /// </summary>
        /// <param name="doc">搜索的document</param>
        /// <param name="name">族名称</param>
        /// <returns>返回是否包含</returns>
        static public Boolean existFamliyByName(Document doc, String name)
        {
            String famName = name;
            Boolean found = false;
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<ElementId> famIds = collector.OfClass(typeof(Family)).ToElementIds();
            foreach (ElementId famId in famIds)
            {
                if (doc.GetElement(famId).Name == famName)
                {
                    found = true;
                    return found;
                }
            }
            return found;
        }

        #endregion

        #region "------从族属性获得族"
        /// <summary>
        /// 通过族名称获得该族
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static public Family getFamilyByName(Document doc, String name)
        {
            ElementId foundId = null;
            existFamliyByName(doc, name, out foundId);
            Family fam = null;
            try
            {
                fam = foundId.getElementById(doc) as Family;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
            return fam;
        }
        #endregion

        #region "------从族获得族实例"
        /// <summary>
        /// 通过族名称获得该族的所有实例
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static public ICollection<ElementId> getFamilyInstancesByFamilyName(Document doc, String name)
        {
            ISet<ElementId> famSymbolIds = getFamilySymbolsByFamilyName(doc, name);
            ICollection<ElementId> familyInsanceIds = getFamilyInsancesByFamilySymbolIds(doc, famSymbolIds);
            return familyInsanceIds;
        }
        #endregion

        #region "------从族获得族类型"
        /// <summary>
        /// 通过族名称获得该族的类型
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        static public ISet<ElementId> getFamilySymbolsByFamilyName(Document doc, String name)
        {
            //得到族
            Family fam = getFamilyByName(doc, name);
            ISet<ElementId> famSymbolIds = fam.GetFamilySymbolIds();
            return famSymbolIds;
        }
        #endregion

        #region "------从族类型获得族实例"
        /// <summary>
        /// 通过族类型id获得该族类型的实例
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="FamilySymbolId"></param>
        /// <returns></returns>
        static public ICollection<ElementId> getFamilyInsancesByFamilySymbolId(Document doc, ElementId familySymbolId)
        {
            FamilyInstanceFilter familyInstanceFilter = new FamilyInstanceFilter(doc, familySymbolId);
            FilteredElementCollector filteredElements = new FilteredElementCollector(doc);
            filteredElements = filteredElements.WherePasses(familyInstanceFilter);
            ICollection<ElementId> familyInsanceIds = filteredElements.ToElementIds();
            return familyInsanceIds;
        }

        /// <summary>
        /// 通过族类型ids获得该族类型的实例
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="FamilySymbolIds"></param>
        /// <returns></returns>
        static public List<ElementId> getFamilyInsancesByFamilySymbolIds(Document doc, ISet<ElementId> familySymbolIds)
        {
            List<ElementId> familyInsanceIds = new List<ElementId>();
            foreach (ElementId familySymbolId in familySymbolIds)
            {
                ICollection<ElementId> familyInsanceIdstemp = getFamilyInsancesByFamilySymbolId(doc, familySymbolId);
                familyInsanceIds.AddRange(familyInsanceIdstemp);
            }
            return familyInsanceIds;
        }

        #endregion


        #region "------交"
        /// <summary>
        /// 返回两个集合交集
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <returns></returns>
        static public ICollection<ElementId> intersect(Document doc,ICollection<ElementId> col1, ICollection<ElementId> col2)
        {
            //过滤器
            FilteredElementCollector colFilter1 = new FilteredElementCollector(doc, col1);
            FilteredElementCollector colFilter2 = new FilteredElementCollector(doc, col2);
            FilteredElementCollector colResult = colFilter1.IntersectWith(colFilter2);
            return colResult.ToElementIds();          
        }
        #endregion

        #region "------并"
        /// <summary>
        /// 返回两个集合并集
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <returns></returns>
        static public ICollection<ElementId> union(Document doc, ICollection<ElementId> col1, ICollection<ElementId> col2)
        {
            //过滤器
            FilteredElementCollector colFilter1 = new FilteredElementCollector(doc, col1);
            FilteredElementCollector colFilter2 = new FilteredElementCollector(doc, col2);
            FilteredElementCollector colResult = colFilter1.UnionWith(colFilter2);
            return colResult.ToElementIds();
        }
        #endregion
    }
}