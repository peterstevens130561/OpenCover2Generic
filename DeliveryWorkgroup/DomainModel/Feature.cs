using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.MSProject;
namespace DeliveryWorkgroup.DomainModel
{
    public class Features : IFeatures
    {
        private Project _project;

        public Features(Project project)
        {
            _project = project;
        }

        Task Create(string name, string duration)
        {
            var feature = _project.Tasks.Add(name);
            feature.Type = PjTaskFixedType.pjFixedDuration;
            feature.Duration = int.Parse(duration) * 10 * 8 * 60;
            
            return feature;
        }

        void Fun()
        {
            _project.Change += changeHandler;
            _project.Application.ProjectBeforeTaskChange += bla;
            _project.Application.ProjectTaskNew += newTask;

        }

        private void newTask(Project pj, int ID)
        {
            throw new NotImplementedException();
        }

        private void bla(Task tsk, PjField Field, object NewVal, ref bool Cancel)
        {
            throw new NotImplementedException();
        }

        private void changeHandler(Project pj)
        {
            
        }
    }
}
