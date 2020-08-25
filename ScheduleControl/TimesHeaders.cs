using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleControl
{

    public class TimesHeader : INotifyPropertyChanged
    {
        public List<TimeItem> Items { get { return _Items; } set { _Items = value; OnPropertyChanged(); } }
        List<TimeItem> _Items;


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
