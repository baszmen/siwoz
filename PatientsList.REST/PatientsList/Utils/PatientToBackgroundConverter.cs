using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using PatientsList.DataModel;

namespace PatientsList.Utils
{
    public class PatientToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var patient = value as Patient;
            if (patient == null) return new SolidColorBrush(Colors.White);
            return (patient.Inside) ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
