using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.MSProject;
using System.Linq;

namespace DeliveryWorkgroup.Application.Commands
{
    class FeatureStatusUpdateCommandHandler
    {
        private readonly Project _project;

        public FeatureStatusUpdateCommandHandler(Project project)
        {
            _project = project;
        }

        public void Execute(IFeatureStatusUpdateCommand command)
        {
            var task = _project.Tasks.UniqueID[command.TaskUniqueId];
            var team = _project.Resources.UniqueID[command.ResourceUniqueId];

            
            DateTime statusDate = _project.StatusDate;
            DateTime endDate = statusDate.AddDays(-1);
            var startDate = statusDate.AddDays(-14);
            Assignment assignment= task.Assignments[1];
            //start is first date
            //end is last date
            AssignActualWorked(command, assignment, startDate, endDate, team);
        }

        private void AssignActualWorked(IFeatureStatusUpdateCommand command, Assignment assignment, DateTime startDate, DateTime endDate, Resource team)
        {
            double fractionWorked = command.WorkedFraction;
            TimeScaleValues values = assignment.TimeScaleData(ToProjectDate(startDate), ToProjectDate(endDate),
                PjAssignmentTimescaledData.pjAssignmentTimescaledActualWork, PjTimescaleUnit.pjTimescaleDays, 1);
            foreach (TimeScaleValue value in values)
            {
                DateTime date = value.StartDate;
                DayOfWeek dayOfWeek = date.DayOfWeek;
                if (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday)
                {
                    var units = GetAvailability(value.StartDate, team);
                    var minutes = GetWorkedMinutesPerDay(fractionWorked, units);
                    value.Value = minutes;
                }
            }
        }

        private int GetActualWorked(IFeatureStatusUpdateCommand command, Assignment assignment, DateTime startDate, DateTime endDate, Resource team)
        {
            int minutes = 0;
            double fractionWorked = command.WorkedFraction;
            TimeScaleValues values = assignment.TimeScaleData(ToProjectDate(startDate), ToProjectDate(endDate),
                PjAssignmentTimescaledData.pjAssignmentTimescaledActualWork, PjTimescaleUnit.pjTimescaleDays, 1);
            foreach (TimeScaleValue value in values)
            {
                DateTime date = value.StartDate;
                DayOfWeek dayOfWeek = date.DayOfWeek;
                if (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday)
                {
                    var units = GetAvailability(value.StartDate, team);
                    minutes += GetWorkedMinutesPerDay(fractionWorked, units);
                }
            }
            return minutes;
        }

        private List<TimeScaleValue> GetTimeScaleValuesList(Assignment assignment, DateTime startDate, DateTime endDate, Resource team)
        {

            TimeScaleValues values = assignment.TimeScaleData(ToProjectDate(startDate), ToProjectDate(endDate),
                PjAssignmentTimescaledData.pjAssignmentTimescaledActualWork, PjTimescaleUnit.pjTimescaleDays, 1);
            return values.Cast<TimeScaleValue>().ToList();
        }

        private int GetWorkedMinutesPerDay(double workedFraction,double maxUnits)
        {
            return  (int)( 8 * 60 * workedFraction * maxUnits);
        }

        private string ToProjectDate(DateTime dateTime)
        {
            return _project.Application.DateFormat(dateTime);
        }

        private double GetAvailability(DateTime day, Resource team)
        {
            double units = team.MaxUnits;
            Availabilities availabilities = team.Availabilities;
            foreach (Availability availability in availabilities)
            {
                DateTime start = availability.AvailableFrom;
                DateTime end = availability.AvailableTo;
                
                if (start <= day && end >= day)
                {
                    units = availability.AvailableUnit / 100;
                    break;
                }
            }
            return units;
        }
    }
}
