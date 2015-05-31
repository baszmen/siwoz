using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PatientsList.Annotations;

namespace PatientsList.DataModel
{
    public class Doctor : INotifyPropertyChanged
    {
        private int _id;
        private string _titles;
        private string _name;
        private string _surname;
        private IList<Patient> _patientsList;

        public string Name
        {
            get { return _name;}
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Titles
        {
            get { return _titles;}
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
        public IList<Patient> PatientsList
        {
            get { return _patientsList; }
            set
            {
                _patientsList = value;
                foreach (Patient t in _patientsList)
                    t.TimesUp += OnTimesUp;
                OnPropertyChanged("PatientsList");
            }
        }

        private void OnTimesUp(object sender, Patient patient)
        {
            PatientsList.Remove(patient);
            OnPropertyChanged("PatientsList");
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
