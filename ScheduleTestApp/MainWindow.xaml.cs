using ScheduleControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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

namespace ScheduleTestApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        void Gen(int groupsCount, int jobbsCount)
        {
            var rnd = new Random();
            var JobbsGroups = new List<JobbsGroup>();
            DateTime startTime = DateTime.Now.AddMinutes(-22);
            DateTime endTime = DateTime.MinValue;

            for (int i = 0; i < groupsCount; i++)
            {
                DateTime lastTime = startTime;
                List<Jobb> jobbs = new List<Jobb>();
                for (int j = 0; j < jobbsCount; j++)
                {
                    var jobb_c = new Jobb() { Name = "Jobb N " + i + ":" + j };
                    jobb_c.StartTime = lastTime;
                    jobb_c.EndTime = jobb_c.StartTime + new TimeSpan(0, rnd.Next(40, 100), 0);
                    jobbs.Add(jobb_c);
                    lastTime = jobb_c.EndTime + new TimeSpan(0, rnd.Next(20, 50), 0);

                    endTime = endTime < lastTime ? lastTime : endTime;
                }
                JobbsGroups.Add(new JobbsGroup() { Jobbs = jobbs, Name = "JobbGroup " + i });
            }
            StartTime = startTime;
            EndTime = endTime;
            OnPropertyChanged("StartTime");
            OnPropertyChanged("EndTime");

            SCH.Init(startTime, endTime, JobbsGroups);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Gen(5, 100);
        }
        private void ButtonLarge_Click(object sender, RoutedEventArgs e)
        {
            Gen(20, 2000);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
