using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace CO2
{
    class GetGreenAreaElements
    {
        public static IList<Element> getElements(Document document)
        {
            // 使用provider 和evaluator 來創建篩選器
            BuiltInParameter typeNameParam = BuiltInParameter.ALL_MODEL_TYPE_NAME;
            ParameterValueProvider pvp = new ParameterValueProvider(new ElementId((int)typeNameParam));
            FilterStringRuleEvaluator fsre = new FilterStringContains();
            string ruleString = "綠化區域";
            FilterStringRule fRule = new FilterStringRule(pvp, fsre, ruleString, true);
            FilteredElementCollector collector = new FilteredElementCollector(document);
            ElementParameterFilter epf = new ElementParameterFilter(fRule);
            return collector.OfClass(typeof(Floor)).WherePasses(epf).ToElements();
        }
    }
}
