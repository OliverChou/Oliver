using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace CO2
{
    public partial class CO2Form : System.Windows.Forms.Form
    {
        DataGridViewChangedExternalEvent cellValueChangedHandler;
        ExternalEvent cellValueChangedExEvent;
        List<ElementData> greenElementData = new List<ElementData>();
        Document document;
        UIDocument uiDocument;
        IList<Element> greenAreas;
        SelElementSet selElements;
        List<ElementId> selectedIDs = new List<ElementId>();

        public CO2Form(ExternalCommandData commandData, ElementSet elements)
        {
            InitializeComponent();
            cellValueChangedHandler = new DataGridViewChangedExternalEvent();
            cellValueChangedExEvent = ExternalEvent.Create(cellValueChangedHandler);
            document = commandData.Application.ActiveUIDocument.Document;
            uiDocument = new UIDocument(document);
            selElements = uiDocument.Selection.Elements;

            #region 取得綠化區域及其參數
            //取得綠化區域元件及其參數
            greenAreas = GetGreenAreaElements.getElements(document);
            foreach (Element e in greenAreas)
            {
                ElementData elemData = new ElementData();
                elemData.元件ID = e.Id.ToString();
                elemData.元件名稱 = e.Name;
                foreach (Parameter p in e.Parameters)
                {
                    if (p.Definition.Name == "綠化區域分類編碼")
                        elemData.分類編碼 = p.AsString();
                    if (p.Definition.Name == "綠化形式")
                        elemData.綠化形式 = p.AsString();// 缺參數"綠化區域備註"
                    if (p.Definition.Name == "面積")
                        elemData.區域面積 = p.AsDouble();
                }
                greenElementData.Add(elemData);
            }
            dataGridView1.DataSource = greenElementData;
            #endregion


            //亮顯綠化區域
            //清除可能選擇的元件
            foreach (Element elem in greenAreas)
            {
                //顯示已定義的綠化區域
                selElements.Insert(elem);
                //prompt += "\n" + elem.Name;
            }
            uiDocument.Selection.Elements = selElements;
            //TaskDialog.Show("Revit", prompt);
        }

        // datagridview 中資料被更改時，將資料同步至 elementData 與 模型中元件參數
        public void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            cellValueChangedHandler.getDataGridViewAndGreenAreasAndElementData(dataGridView1, e, greenAreas, greenElementData);
            cellValueChangedExEvent.Raise();
        }


        //將在 datagridview 中被挑選的元件的綠化形式改為綠屋頂
        private void button1_Click(object sender, EventArgs e)
        {

        }

        // 在模型中展示並選擇在 datagridview 中被挑選的元件
        private void button5_Click(object sender, EventArgs e)
        {
            selectedIDs.Clear();
            selElements.Clear();
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.SelectedRows.Contains(dataGridView1.Rows[i]))
                    selectedIDs.Add(new ElementId(Convert.ToInt32(dataGridView1.Rows[i].Cells[0].Value.ToString())));
            }
            //顯示選擇的元件
            foreach (ElementId id in selectedIDs)
            {
                selElements.Add(document.GetElement(id));
            }
            //展示選擇的元件
            uiDocument.ShowElements(selectedIDs);
        }

        //關閉 Form 時，釋放記憶體資源
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            cellValueChangedExEvent.Dispose();
        }
    }
}
