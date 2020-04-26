using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VTBarcode
{
    public partial class Form1 : Form
    {
        DataTable blankLabelConfig;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        public void SetBlankLabelConfig(DataTable dt)
        {
            blankLabelConfig = dt;
        }
        private string GenerateCode(string value, string code, int incVal)
        {
            if (value == "")
                return "";
            double d = Double.Parse(value);
            string retVal = "";
            int convVal = (int)d;
            int intVal = convVal + incVal;
            if (d - convVal == 0.5)
            {
                retVal = code + intVal.ToString() + "50" + code;
            }
            else
            {
                retVal = code + intVal.ToString() + code;
            }
            return retVal;
        }
        public enum SheetType { A65, A56 };
        public SheetType GetSheetType()
        {
            if (comboBox1.Text == "A4-56")
                return SheetType.A56;
            else
                return SheetType.A65;
        }
        public struct SheetTypeData
        {
            public int rows;
            public int cols;
        };
        public SheetTypeData GetSheetTypeData()
        {
            SheetTypeData data = new SheetTypeData();
            switch (GetSheetType())
            {
                case SheetType.A56:
                    data.rows = 14;
                    data.cols = 4;
                    break;
                case SheetType.A65:
                    data.rows = 13;
                    data.cols = 5;
                    break;
            }
            return data;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            BlankLLabelConfig config = new BlankLLabelConfig(this);
            config.ShowDialog();
            SheetTypeData sheetData = GetSheetTypeData();
            LabelDataset ld = new LabelDataset();
            int k = 0;
            Code128 barcodeGenerator = new Code128();
            byte[] emptyByte = new byte[1];
            emptyByte[0] = 0;
            for (int i = 0; i <   dataGridView1.Rows.Count-1; i++)
            {
                string Code1 = "";
                if (dataGridView1.Rows[i].Cells[0].Value != null)
                    Code1 = dataGridView1.Rows[i].Cells[0].Value.ToString();
                string Code2 = "";
                if (dataGridView1.Rows[i].Cells[1].Value != null)
                Code2 = dataGridView1.Rows[i].Cells[1].Value.ToString();
                string VRP = "";
                if (dataGridView1.Rows[i].Cells[2].Value != null)
                    VRP = dataGridView1.Rows[i].Cells[2].Value.ToString();
                string MRP = "";
                if (dataGridView1.Rows[i].Cells[3].Value != null)
                    MRP = dataGridView1.Rows[i].Cells[3].Value.ToString();
                string barcode = "";
                if (dataGridView1.Rows[i].Cells[5].Value != null)
                    barcode = dataGridView1.Rows[i].Cells[5].Value.ToString();
                byte[] barcode_bitmap = barcodeGenerator.Generate(barcode);
                Code1 = GenerateCode(Code1, "5", 100);
                Code2 = GenerateCode(Code2, "6", 100);
                VRP = GenerateCode(VRP, "7", 0);
                if (!MRP.Contains(".") && MRP != "")
                {
                    MRP = MRP + ".00";
                }
                int numLabels = Int32.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                for (int j = 0; j < numLabels; j++)
                {
                    while (k <= sheetData.rows*sheetData.cols-1)
                    {
                        int row = k / sheetData.cols;
                        int col = k % sheetData.cols;
                        if (!Boolean.Parse(blankLabelConfig.Rows[row][col + 1].ToString()))
                        {
                            DataRow dr1 = ld.Tables["LabelData"].NewRow();
                            dr1["Name"] = "";
                            dr1["MRP"] = "";
                            dr1["VRP"] = ""; ;
                            dr1["Code1"] = "";
                            dr1["Code2"] = "";
                            dr1["barcode"] = "";
                            dr1["barcode_bitmap"] = emptyByte;
                            ld.Tables["LabelData"].Rows.Add(dr1);
                            k++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    DataRow dr = ld.Tables["LabelData"].NewRow();
                    dr["Name"] = "";
                    dr["MRP"] = MRP;
                    dr["VRP"] = VRP;;
                    dr["Code1"] = Code1;
                    dr["Code2"] = Code2;
                    dr["barcode"] = barcode;
                    dr["barcode_bitmap"] = barcode_bitmap;
                    ld.Tables["LabelData"].Rows.Add(dr);
                    k++;
                }
                
            }
            ReportViewer viewer = new ReportViewer(ld, GetSheetType());
            viewer.Show();

            //dr
            //ld.Tables["LabelData"].Rows.Add(
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            LabelReport1.Dispose();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SheetTypeData data = GetSheetTypeData();
            Int32 numSticker = data.rows * data.cols;
            label2.Text = numSticker.ToString() + " Stickers in 1 Sheet";
        }

        private void dataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 4)
            {
                Int32 sum = 0;
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    if (dataGridView1.Rows[i].Cells[e.ColumnIndex].Value != null)
                    {
                        int result;
                        Int32.TryParse(dataGridView1.Rows[i].Cells[e.ColumnIndex].Value.ToString(), out result);
                        sum += result;
                    }
                }
                label3.Text = sum.ToString() + " Stickers will be printed.";
            }
            else if (e.ColumnIndex == 5)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value =
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToUpper();
            }
        }
        private DataGridViewCell GetStartCell(DataGridView dgView)
        {
            //get the smallest row,column index
            if (dgView.SelectedCells.Count == 0)
                return null;

            int rowIndex = dgView.Rows.Count - 1;
            int colIndex = dgView.Columns.Count - 1;

            foreach (DataGridViewCell dgvCell in dgView.SelectedCells)
            {
                if (dgvCell.RowIndex < rowIndex)
                    rowIndex = dgvCell.RowIndex;
                if (dgvCell.ColumnIndex < colIndex)
                    colIndex = dgvCell.ColumnIndex;
            }

            return dgView[colIndex, rowIndex];
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count == 0)
            {
                return;
            }

            if (e.KeyCode == System.Windows.Forms.Keys.C && e.Control)
            {
                // copy logic
                DataGridView dgv = sender as DataGridView;
                dgv.Select();
                DataObject o = dgv.GetClipboardContent();
                Clipboard.SetDataObject(o);
            }
            else if (e.KeyCode == System.Windows.Forms.Keys.V && e.Control)
            {
                DataGridViewCell startCell = GetStartCell(dataGridView1);
                //Get the clipboard value in a dictionary
                 Dictionary<int, Dictionary<int, string>> cbValue =
                        ClipBoardValues(Clipboard.GetText());

                int iRowIndex = startCell.RowIndex;
                foreach (int rowKey in cbValue.Keys)
                {
                    int iColIndex = startCell.ColumnIndex;
                    if (iRowIndex >= dataGridView1.Rows.Count - 1)
                    {
                        DataGridViewRow dr = (DataGridViewRow)dataGridView1.Rows[dataGridView1.Rows.Count - 1].Clone();
                        dataGridView1.Rows.Add(dr);
                    }
                    foreach (int cellKey in cbValue[rowKey].Keys)
                    {
                        //Check if the index is within the limit
                        if (iColIndex <= dataGridView1.Columns.Count - 1
                        && iRowIndex < dataGridView1.Rows.Count - 1)
                        {
                            DataGridViewCell cell = dataGridView1[iColIndex, iRowIndex];

                            //Copy to selected cells if 'chkPasteToSelectedCells' is checked
                            //if ((chkPasteToSelectedCells.Checked && cell.Selected) ||
                            //    (!chkPasteToSelectedCells.Checked))
                            cell.Value = cbValue[rowKey][cellKey];
                        }
                        iColIndex++;
                    }
                    iRowIndex++;
                }
            }
        }
        private Dictionary<int, Dictionary<int, string>> ClipBoardValues(string clipboardValue)
        {
            Dictionary<int, Dictionary<int, string>>
            copyValues = new Dictionary<int, Dictionary<int, string>>();

            String[] lines = clipboardValue.Split('\n');

            for (int i = 0; i < lines.Length - 1; i++)
            {
                copyValues[i] = new Dictionary<int, string>();
                String[] lineContent = lines[i].Split('\t');

                //if an empty cell value copied, then set the dictionary with an empty string
                //else Set value to dictionary
                if (lineContent.Length == 0)
                    copyValues[i][0] = string.Empty;
                else
                {
                    for (int j = 0; j <= lineContent.Length - 1; j++)
                        copyValues[i][j] = lineContent[j];
                }
            }
            return copyValues;
        }
    }
}