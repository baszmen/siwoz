using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Windows.UI.Xaml;
using PatientsList.DataModel;

namespace PatientsList.Utils
{
    public class PatientsSelector : DataTemplateSelector
    {
        public DataTemplate Insider { get; set; }
        public DataTemplate Waiting { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var menuItem = item as Patient;
            if (menuItem != null)
            {
                if (menuItem.Inside)
                    return Insider;
                return Waiting;
            }
            return null;
        }
    }
}
