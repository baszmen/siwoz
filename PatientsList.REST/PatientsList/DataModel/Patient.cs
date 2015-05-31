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
                    OnPropertyChanged("LeftTime");

                });
            _timer.Change(TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);
        }

        private int _id;
        private string _name;
        private DateTime _checkTime;
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
        public DateTime CheckTime
        {
            get { return _checkTime; }
            set
            {
                _checkTime = value;
                OnPropertyChanged("CheckTime");
            }
        }

        public TimeSpan LeftTime
        {
            get
            {
                var ts = CheckTime.Subtract(DateTime.Now);
                if (ts.Ticks < 0)
                {
                    if (TimesUp != null)
                        TimesUp(this, this);
                    return TimeSpan.Zero;
                }
                else return ts;
            }
        }

        public event EventHandler<Patient> TimesUp;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
