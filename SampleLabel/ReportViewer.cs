using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
namespace SampleLabel
{
    public partial class ReportViewer : Form
    {
        public static LabelDataset _ld;
        public static Form1.SheetType _sheetType;
        public ReportViewer(LabelDataset ld, Form1.SheetType sheetType)
        {
            InitializeComponent();
            _ld = ld;
            _sheetType = sheetType;
        }
        private void ReportViewer_Load(object sender, EventArgs e)
        {
            ReportDocument report;
            if (_sheetType == Form1.SheetType.A56)
            {
                report = new A4_56();
            }
            else
            {
                report = new A4_65();
            }
            report.SetDataSource(_ld);
            crystalReportViewer1.ReportSource = report;
        }
    }
}