using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using DeliveryWorkgroup.Application.Commands;
using DeliveryWorkgroup.DomainModel;
using DeliveryWorkgroup.Presentation;
using Microsoft.Office.Interop.MSProject;
using Office = Microsoft.Office.Core;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon1();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace DeliveryWorkgroup
{
    [ComVisible(true)]
    public class Ribbon1 : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public Ribbon1()
        {
        }

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("DeliveryWorkgroup.Ribbon1.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit http://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            this.ribbon = ribbonUI;
        }

        public void OnAction_AddFeature(Office.IRibbonControl control)
        {
            var form = new FeatureCreateForm();
            form.ShowDialog();

        }

        public void OnAction_UpdateFeature(Office.IRibbonControl control)
        {
            Tasks tasks = Globals.ThisAddIn.Application.ActiveSelection.Tasks;

            var form = new UpdateStatusForm();
            if (tasks.Count != 1)
            {
                MessageBox.Show("Select one task");
                return;
            }
            var task = tasks[1];
            form.Feature = task.Name;
            form.FractionSprintSpent = 1;
            var command = new FeatureStatusUpdateCommand();
            command.TaskUniqueId = task.UniqueID;
            command.ResourceUniqueId = task.Assignments[1].ResourceUniqueID;
            command.WorkedFraction = form.FractionSprintSpent;
            form.ShowDialog();
            var handler = new FeatureStatusUpdateCommandHandler(Globals.ThisAddIn.Application.ActiveProject);
            handler.Execute(command);

        }
        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
