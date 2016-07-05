using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace CO2
{
    public class ElementData
    {
        string _id;
        string _elementName;
        string _greenType;
        string _classificationCode;
        double _area;

        public ElementData() { }

        public ElementData(string elementName, string classificationCode, string greenType, string id, double area)
        {
            _elementName = elementName;
            _greenType = greenType;
            _classificationCode = classificationCode;
            _id = id;
            _area = area;
        }
        public string 元件ID
        {
            set { _id = value; }
            get { return _id; }
        }
        public string 元件名稱
        {
            set { _elementName = value; }
            get { return _elementName; }
        }
        public string 綠化形式
        {
            set { _greenType = value; }
            get { return _greenType; }
        }
        public string 分類編碼
        {
            set { _classificationCode = value; }
            get { return _classificationCode; }
        }
        public double 區域面積
        {
            set { _area = value; }
            get { return _area; }
        }
    }
}
