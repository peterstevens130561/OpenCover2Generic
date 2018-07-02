using System;

using System.Windows.Forms;
using DeliveryWorkgroup.Application.Commands;
using Microsoft.Office.Interop.MSProject;

namespace DeliveryWorkgroup.Presentation
{
    public partial class UpdateStatusForm : Form
    {
        public UpdateStatusForm()
        {
            InitializeComponent();
        }

        public Task Task
        {
            get;
            set;
        }

        public string Feature
        {
            get { return FeatureTextBox.Text; }
            set { FeatureTextBox.Text = value; }
        }


        public double FractionSprintSpent
        {
            get { return Double.Parse(FractionSpentTextBox.Text)/100; }
            set { FractionSpentTextBox.Text = $"{value*100}"; }
        }

        public double RemainingSprints
        {
            get { return Double.Parse(PlannedSprintsTextBox.Text); }
            set { PlannedSprintsTextBox.Text = $"{value}"; }
        }

   
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            var command = new FeatureStatusUpdateCommand()
            {
                TaskUniqueId = Task.UniqueID,
                ResourceUniqueId = Task.Assignments[1].ResourceUniqueID,
                WorkedFraction = FractionSprintSpent,
                RemainingSprints = RemainingSprints
            };
            Task.Type = PjTaskFixedType.pjFixedUnits;
            var sprints = double.Parse(NewRemainingSprints.Text);
            int days =(int) (sprints * 10);
            var project = Globals.ThisAddIn.Application.ActiveProject;
            Task.Finish = project.StatusDate.AddDays(days - 1);
            Globals.ThisAddIn.Application.CalculateProject();
            Task.Type = PjTaskFixedType.pjFixedDuration;
            var handler = new FeatureStatusUpdateCommandHandler(Globals.ThisAddIn.Application.ActiveProject);
            handler.Execute(command);
            Task.Type = PjTaskFixedType.pjFixedWork;
            Globals.ThisAddIn.Application.CalculateProject();
        }



        private void Calculate(object sender, EventArgs e)
        {
            //We start with remaining work, as this gives us the previously planned work
            double remainingSprintsPreviousSprint = Task.RemainingDuration / 4800;
            PlannedSprintsTextBox.Text = $"{remainingSprintsPreviousSprint}";
            //now take the fraction. we should really look at hours planned in this sprint, but this is ok for now
            double workDone = 4800* Task.Assignments[1].Resource.MaxUnits * FractionSprintSpent;
            double futureRemainingWork = Task.RemainingWork - workDone;
            double futureRemainingSprints = futureRemainingWork / (4800 * Task.Assignments[1].Resource.MaxUnits);
            CalculatedRemainingSprintsTextBox.Text = $"{futureRemainingSprints}";
            NewRemainingSprints.Text = $"{futureRemainingSprints}";
        }
    }
}
