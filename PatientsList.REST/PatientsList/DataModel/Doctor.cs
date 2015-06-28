using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using PatientsList.Annotations;

namespace PatientsList.DataModel
{
    public class Doctor : INotifyPropertyChanged
    {
        private int _id;
        private string _titles;
        private string _name;
        private string _surname;
        private ObservableCollection<Patient> _patientsList;

        public TimeSpan TimeStuck(Patient patient)
        {
            var p = PatientsList.FirstOrDefault(x => x.Id == patient.Id);
            var index = PatientsList.IndexOf(p);
            if (index == -1) return TimeSpan.Zero;
            if (index == 0) return TimeSpan.Zero;

            TimeSpan duration;
            for (var i = 0; i < index; i++)
                duration += PatientsList[i].Duration;

            return duration; //patient.VisitTime.Subtract(DateTime.Now) <= duration;
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Titles
        {
            get { return _titles; }
            set
            {
                _titles = value;
                OnPropertyChanged("Titles");
            }
        }
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged("Surname");
            }
        }
        public ObservableCollection<Patient> PatientsList
        {
            get { return _patientsList; }
            set
            {
                _patientsList = value;
                OnPropertyChanged("PatientsList");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
