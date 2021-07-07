using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
namespace VTBarcode
{
    public partial class ReportViewer : Form
    {
        public static LabelDataset _ld;
        public static Form1.SheetType _sheetType;
        private ReportDocument report;
        public ReportViewer(LabelDataset ld, Form1.SheetType sheetType)
        {
            InitializeComponent();
            _ld = ld;
            _sheetType = sheetType;
        }
        private void ReportViewer_Load(object sender, EventArgs e)
        {
            if (_sheetType == Form1.SheetType.A56)
            {
                report = new A4_56();
            }
            else if (_sheetType == Form1.SheetType.A56_New)
            {
                report = new A4_56_New();
            }
            else
            {
                report = new A4_65();
            }
            //report.ReportOptions.EnableSaveDataWithReport = false;
            report.SetDataSource(_ld);
            crystalReportViewer1.ReportSource = report;
        }

        private void ReportViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Table t in report.Database.Tables)
                t.Dispose();
            _ld.Dispose();
            _ld = null;
            report.Close();
            report.Dispose();
            crystalReportViewer1.ReportSource = null;
            crystalReportViewer1.Dispose();
            crystalReportViewer1 = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            //crystalReportViewer1.Refresh();
        }
    }
}