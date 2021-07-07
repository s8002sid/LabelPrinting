using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VTBarcode
{
    public partial class BlankLLabelConfig : Form
    {
        Form1 form1Ref;
        public BlankLLabelConfig(Form1 frm)
        {
            form1Ref = frm;
            InitializeComponent();
        }

        private void BlankLLabelConfig_Load(object sender, EventArgs e)
        {
            if (form1Ref.GetSheetType() == Form1.SheetType.A65)
            {
                for (int i = 0; i < 13; i++)
                    dataGridView1.Rows.Add("ToggleCheck", true, true, true, true, true);
            }
            else if (form1Ref.GetSheetType() == Form1.SheetType.A56 || form1Ref.GetSheetType() == Form1.SheetType.A56_New)
            {
                dataGridView1.Columns.Remove("Col5");
                for (int i = 0; i < 14; i++)
                    dataGridView1.Rows.Add("ToggleCheck", true, true, true, true);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                for (int i = 1; i < dataGridView1.Columns.Count; i++)
                {
                    bool curVal = Boolean.Parse(dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString());
                    dataGridView1.Rows[e.RowIndex].Cells[i].Value = !curVal;
                }
            }
        }

        private void setValue (bool value)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 1; j < dataGridView1.Columns.Count; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = value;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setValue(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setValue(true);
        }

        private void BlankLLabelConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Check");
            dt.Columns.Add("Col1");
            dt.Columns.Add("Col2");
            dt.Columns.Add("Col3");
            dt.Columns.Add("Col4");
            dt.Columns.Add("Col5");
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataRow dr = dt.NewRow();
                for (int j = 1; j < dataGridView1.Columns.Count; j++)
                {
                    dr[j] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
                dt.Rows.Add(dr);
            }
            form1Ref.SetBlankLabelConfig(dt);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}