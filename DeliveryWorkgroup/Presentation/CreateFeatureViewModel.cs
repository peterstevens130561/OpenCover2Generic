using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryWorkgroup.Presentation
{
    class CreateFeatureViewModel : INotifyPropertyChanged
    {
        public string Feature { get; set; } 
        public string Sprints {
    get;
    set;
    }
        public event PropertyChangedEventHandler PropertyChanged;

        public CreateFeatureViewModel()
        {

        }
    }
}
