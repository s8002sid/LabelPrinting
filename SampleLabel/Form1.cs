using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SampleLabel
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
        private void button1_Click(object sender, EventArgs e)
        {
            BlankLLabelConfig config = new BlankLLabelConfig(this);
            config.ShowDialog();

            LabelDataset ld = new LabelDataset();
            int k = 0;
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
                    while (k <= 64)
                    {
                        int row = k / 5;
                        int col = k % 5;
                        if (!Boolean.Parse(blankLabelConfig.Rows[row][col + 1].ToString()))
                        {
                            DataRow dr1 = ld.Tables["LabelData"].NewRow();
                            dr1["Name"] = "";
                            dr1["MRP"] = "";
                            dr1["VRP"] = ""; ;
                            dr1["Code1"] = "";
                            dr1["Code2"] = "";
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
                    ld.Tables["LabelData"].Rows.Add(dr);
                    k++;
                }
                
            }
            ReportViewer viewer = new ReportViewer(ld);
            viewer.Show();

            //dr
            //ld.Tables["LabelData"].Rows.Add(
        }
    }
}