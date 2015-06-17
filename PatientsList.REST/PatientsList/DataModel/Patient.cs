using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using PatientsList.Annotations;

namespace PatientsList.DataModel
{
    public class Patient : INotifyPropertyChanged
    {
        private const int TIME_INTERVAL_IN_MILLISECONDS = 1000;
        private Timer _timer;

        public Patient()
        {
            _timer = new Timer(Callback, null, TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);
        }

        private void Callback(object state)
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    if (!Inside)
                    {
                        var ts = VisitTime.Subtract(DateTime.Now);
                        if (ts.Ticks <= 0)
                        {
                            TimerTime = TimeSpan.Zero;
                            Inside = true;
                            ImagePath = "Assets/doctor.jpeg";
                            TimerTime = Duration;
                        }
                        else TimerTime = ts;
                    }

                    if (Inside)
                    {
                        TimerTime = TimerTime.Subtract(TimeSpan.FromMilliseconds(TIME_INTERVAL_IN_MILLISECONDS));
                        if (TimerTime.Ticks <= 0)
                            return;
                    }
                    _timer.Change(TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);
                });
        }

        private int _id;
        private string _name;
        private bool _inside = false;
        private string _patientInfo = "Do wizyty pozostało:";
        private DateTime _visitTime;
        private TimeSpan _timerTime;
        private TimeSpan _duration = TimeSpan.FromSeconds(10);
        private string _imagePath = "Assets/person.jpg";

        public bool Inside
        {
            get { return _inside; }
            set
            {
                _inside = value;
                PatientInfo = "Trwa wizyta. Pozostało:";
                OnPropertyChanged("Inside");
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
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public DateTime VisitTime
        {
            get { return _visitTime; }
            set
            {
                _visitTime = value;
                OnPropertyChanged("VisitTime");
            }
        }
        public TimeSpan TimerTime
        {
            get { return _timerTime; }
            set
            {
                _timerTime = value;
                OnPropertyChanged("TimerTime");
            }
        }
        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged("Duration");
            }
        }
        public string ImagePath
        {
            get { return _imagePath; }
            set
            {
                _imagePath = value;
                OnPropertyChanged("ImagePath");
            }
        }
        public string PatientInfo
        {
            get { return _patientInfo; }
            set
            {
                _patientInfo = value;
                OnPropertyChanged("PatientInfo");
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
