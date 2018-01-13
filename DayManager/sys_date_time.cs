using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayManager
{
    class sys_date_time
    {
        public string Date;
        public string Time;
        DateTime D = DateTime.Today;
        public void get_date_time()
        {
            Date = D.ToString("yyyy-MM-dd");
            Time = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
