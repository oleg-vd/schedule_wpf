using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleControl
{
    public class TimeItem 
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Name { get; set; }
    }

    public class Jobb : TimeItem
    {
        public TimeSpan Duration { get { return EndTime - StartTime; } }
    }

    public class JobbsGroup
    {
        public string Name { get; set; }
        public List<Jobb> Jobbs { get; set; }
    }
}
