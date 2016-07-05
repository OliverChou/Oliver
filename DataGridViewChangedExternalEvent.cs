using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Windows.Forms;

namespace CO2
{
    public class DataGridViewChangedExternalEvent : IExternalEventHandler
    {
        DataGridView dataGridView1;
        IList<Element> greenAreas;
        List<ElementData> greenElementData;
        DataGridViewCellEventArgs e;

        public void Execute(UIApplication app)
        {
            Document document = app.ActiveUIDocument.Document;
            UIDocument uidoc = app.ActiveUIDocument;
            SelElementSet set = uidoc.Selection.Elements;

            //找出被修改的元件的 ID
            string elementID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            //找出被修改的欄位名稱
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            //更新 elementData 資料
            foreach (ElementData elementData in greenElementData)
            {
                if (elementData.元件ID == elementID)
                {
                    //if (columnName == "元件名稱")
                    //{
                    //    elementData.元件名稱 = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    //    MessageBox.Show(elementData.元件名稱);
                    //}
                    if (columnName == "分類編碼")
                    {
                        elementData.分類編碼 = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        //MessageBox.Show(elementData.分類編碼);
                    }
                    if (columnName == "綠化形式")
                    {
                        elementData.綠化形式 = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                        //MessageBox.Show(elementData.綠化形式);
                    }
                }
            }
            //更新 Revit 模型中 greenAreaElements 資料
            Transaction trans = new Transaction(document);
            trans.Start("updateElementParametersInRevit");
            foreach (Element elem in greenAreas)
            {
                if (elem.Id.ToString() == elementID)
                {
                    //if (columnName == "元件名稱")
                    //{
                    //    elem.Name = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    //    MessageBox.Show(elem.Name);
                    //}
                    if (columnName == "分類編碼")
                    {
                        foreach (Parameter para in elem.Parameters)
                            if (para.Definition.Name == "綠化區域分類編碼")
                            {
                                para.Set(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                                //MessageBox.Show(para.AsString());
                            }
                    }
                    if (columnName == "綠化形式")
                    {
                        foreach (Parameter para in elem.Parameters)
                            if (para.Definition.Name == "綠化形式")
                            {
                                para.Set(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                                //MessageBox.Show(para.AsString());
                            }
                    }
                }
            }
            trans.Commit();
        }

        public void getDataGridViewAndGreenAreasAndElementData(DataGridView dataGridView1, DataGridViewCellEventArgs e, IList<Element> greenAreas, List<ElementData> greenElementData)
        {
            this.dataGridView1 = dataGridView1;
            this.greenAreas = greenAreas;
            this.greenElementData = greenElementData;
            this.e = e;
        }

        public string GetName()
        {
            return "ExternalEventCO2";
        }
    }
}
