using PatientsList.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PatientsList.Annotations;
using PatientsList.DataModel;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace PatientsList
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class GroupedItemsPage : Page, INotifyPropertyChanged
    {

        private const int TIME_INTERVAL_IN_MILLISECONDS = 2000;
        private Timer _timer;

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ObservableCollection<Doctor> _doctors = new ObservableCollection<Doctor>();
        private CollectionViewSource _collVs;


        public ObservableCollection<Doctor> Doctors
        {
            get { return _doctors; }
            set
            {
                _doctors = value;
                OnPropertyChanged("Doctors");
            }
        } 
        public CollectionViewSource CollVs
        {
            get { return _collVs; }
            set
            {
                _collVs = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return defaultViewModel; }
            set
            {
                defaultViewModel = value;
                OnPropertyChanged("DefaultViewModel");
                this.DataContext = DefaultViewModel;
            }
        }

        public GroupedItemsPage()
        {
            InitializeComponent();
            navigationHelper = new NavigationHelper(this);
            navigationHelper.LoadState += navigationHelper_LoadState;

            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
            _timer = new Timer(AsynchronousRestActualization, null, TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);
        }

        private void AsynchronousRestActualization(object state)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, ActualizeUsingDispatcher);
        }

        private async void ActualizeUsingDispatcher()
        {
            Doctors = await DoctorsDataSource.ActualizeDoctors();
            DefaultViewModel["Groups"] = Doctors;
            CollVs.Source = Doctors;

            _timer.Change(TIME_INTERVAL_IN_MILLISECONDS, Timeout.Infinite);
        }

        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Doctors = await DoctorsDataSource.GetDoctors();
            DefaultViewModel["Groups"] = Doctors;
            CollVs = new CollectionViewSource
            {
                Source = Doctors,
                IsSourceGrouped = true,
                ItemsPath = new PropertyPath("PatientsList")
            };

            /*
             *
             * <!--<CollectionViewSource
            x:Name="groupedItemsViewSource"
            Source="{Binding Groups}"
            IsSourceGrouped="true"
            ItemsPath="PatientsList"/>-->
             */
        }

        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var group = ((Doctor)(sender as FrameworkElement).DataContext);
            Frame.Navigate(typeof(GroupDetailPage), group.Id);
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Dupa_OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
        }
    }
}