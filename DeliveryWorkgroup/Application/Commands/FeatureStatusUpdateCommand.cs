using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWorkgroup.Application.Commands
{
    class FeatureStatusUpdateCommand : IFeatureStatusUpdateCommand
    {
        public double AvailabilityFraction { get; set; }
        public double RemainingSprints { get; internal set; }
        public int ResourceUniqueId { get; set; }

        public int TaskUniqueId { get; set; }

        public double WorkedFraction { get; set; }

    }
}
