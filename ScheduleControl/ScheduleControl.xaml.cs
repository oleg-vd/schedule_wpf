using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScheduleControl
{

    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class ScheduleControl : UserControl, INotifyPropertyChanged
    {

        
        public ScheduleControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        public int CurrentHeight { get; set; }


        public void Init(DateTime startTime, DateTime endTime, List<JobbsGroup> jobbs)
        {
            if (TimesHeaders != null) TimesHeaders.Clear();
            if (Jobbs != null) Jobbs.Clear();
            if (TimesGridLines != null) TimesGridLines.Clear();
            OnPropertyChanged("TimesHeaders");
            OnPropertyChanged("Jobbs");
            OnPropertyChanged("TimesGridLines");
            WaitOpen = true;
            Task.Run(()=> { 

                _Init(startTime, endTime, jobbs);
                WaitOpen = false;
            });
        }
        void _Init(DateTime startTime, DateTime endTime, List<JobbsGroup> jobbs)
        {
            MinTime = startTime;
            MaxTime = endTime;
            OnPropertyChanged("MinTime");
            OnPropertyChanged("MaxTime");

            var h0 = GenHeaderTimes();
            var h1 = GenHeaderTimes_Day();
            TimesHeaders = new ObservableCollection<TimesHeader>
            {
                new TimesHeader() { Items = new List<TimeItem>(h1) },
                new TimesHeader() { Items = new List<TimeItem>(h0) }
            };

            TimesGridLines = new ObservableCollection<TimeItem>(h0);

            Jobbs = new ObservableCollection<JobbsGroup>(jobbs);

            OnPropertyChanged("TimesHeaders");
            OnPropertyChanged("Jobbs");
            OnPropertyChanged("TimesGridLines");

            

        }


        public bool WaitOpen { get { return _WaitOpen; } set { _WaitOpen = value; OnPropertyChanged(); } }
        bool _WaitOpen;
        public double ScheduleHeight { get; set; }
        public int MinRowHeight { get; private set; } = 30;
        public DateTime MinTime { get; private set; }
        public DateTime MaxTime { get; private set; }
        public ObservableCollection<TimesHeader> TimesHeaders { get; private set; }
        public ObservableCollection<JobbsGroup> Jobbs { get; private set; }
        public ObservableCollection<TimeItem> TimesGridLines { get; private set; }

        internal List<TimeItem> GenHeaderTimes()
        {
            List<TimeItem> lst = new List<TimeItem>();
            if (MinTime != DateTime.MinValue && MaxTime != DateTime.MaxValue)
            {
                TimeItem curr = new TimeItem() { 
                    StartTime = MinTime,
                    EndTime = new DateTime(MinTime.Year, MinTime.Month, MinTime.Day, MinTime.Hour+1, 0, 0)
                };
                bool f = true;

                while (curr.EndTime < MaxTime)
                {
                    lst.Add(new TimeItem() { StartTime = curr.StartTime, EndTime = curr.EndTime, Name = f ? "" : curr.StartTime.Hour.ToString() + ":00" });
                    curr.StartTime = curr.EndTime;
                    curr.EndTime = curr.EndTime.AddHours(2);
                    f = false;
                }
                lst.Add(new TimeItem() { StartTime = curr.StartTime, EndTime = MaxTime });
            }
            return lst;
        }
        internal List<TimeItem> GenHeaderTimes_Day()
        {
            List<TimeItem> lst = new List<TimeItem>();
            if (MinTime != DateTime.MinValue && MaxTime != DateTime.MaxValue)
            {
                TimeItem curr = new TimeItem()
                {
                    StartTime = MinTime,
                    EndTime = new DateTime(MinTime.Year, MinTime.Month, MinTime.Day + 1, 0, 0, 0)
                };
                while (curr.EndTime < MaxTime)
                {
                    lst.Add(new TimeItem() { StartTime = curr.StartTime, EndTime = curr.EndTime, Name = curr.StartTime.ToString("dd.MM.yyyy")});
                    curr.StartTime = curr.EndTime;
                    curr.EndTime = curr.EndTime.AddDays(1);
                }
                lst.Add(new TimeItem() { StartTime = curr.StartTime, EndTime = MaxTime, Name = curr.StartTime.ToString("dd.MM.yyyy") });
            }
            return lst;
        }



        private void SV_Jobbs_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            SV_TimesHeaders.ScrollToHorizontalOffset(e.HorizontalOffset);
            SV_GridLines.ScrollToHorizontalOffset(e.HorizontalOffset);
            SV_Groups.ScrollToVerticalOffset(e.VerticalOffset);
        }
        private void SV_Groups_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            SV_Jobbs.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void ItemsControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SV_Jobbs.UpdateLayout();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }

}
