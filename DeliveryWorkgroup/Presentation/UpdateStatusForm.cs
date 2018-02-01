using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeliveryWorkgroup.Presentation
{
    public partial class UpdateStatusForm : Form
    {
        public UpdateStatusForm()
        {
            InitializeComponent();
        }

        public string Feature
        {
            get { return FeatureTextBox.Text; }
            set { FeatureTextBox.Text = value; }
        }

        public string RemainingSprints
        {
            get { return RemainingEffortTextBox.Text; }
            set { RemainingEffortTextBox.Text = value; }
        }

        public double FractionSprintSpent
        {
            get { return Double.Parse(FractionSpentTextBox.Text)/100; }
            set { FractionSpentTextBox.Text = $"{value*100}"; }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {

        }
    }
}
