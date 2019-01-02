using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SampleLabel
{
    public partial class ReportViewer : Form
    {
        public static LabelDataset _ld;
        public ReportViewer(LabelDataset ld)
        {
            InitializeComponent();
            _ld = ld;
        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            LabelReport lr = new LabelReport();
            lr.SetDataSource(_ld);
            crystalReportViewer1.ReportSource = lr;
        }
    }
}