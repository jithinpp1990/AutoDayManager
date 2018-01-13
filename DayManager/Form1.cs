using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace DayManager
{
    public partial class Form1 : Form
    {

        ErrorTracker ET = new ErrorTracker();
        System.Windows.Forms.Timer tmr = new System.Windows.Forms.Timer();
        db_class dbc = new db_class();
        public string BOD_EOD_time;
        public string Recon_Gen_Time;
        public int interval_counter = 0;
        public int no_of_intervals = 0;
        public bool counter_flag = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string noti = null;
                tmr.Interval = 1000;
                tmr.Tick += new EventHandler(tmr_Tick);
                tmr.Enabled = true; //true;///sf_update_daybegin_dayclosing_entries
                BOD_EOD_time = dbc.BOD_EOD_Time("BOD_EOD");
                Recon_Gen_Time = dbc.BOD_EOD_Time("Recon");
                noti = dbc.notification();
                label4.Text = noti;
            }
            catch (Exception ex)
            {
                ET.Directory_Tracker();
                ET.Text_Tracker(ex.ToString());
            }

        }



        private void tmr_Tick(object sender, EventArgs e)
        {

            try
            {
                sys_date_time dt = new sys_date_time();
                dt.get_date_time();
                clock.Text = dt.Date + "     " + dt.Time;
                if (BOD_EOD_time == dt.Time)
                {
                    dbc.BOD_EOD(no_of_intervals);
                    label4.Text = dbc.notification();
                    counter_flag = true;
                }
                if (Recon_Gen_Time == dt.Time)
                {
                    DirectoryTracker DTrack = new DirectoryTracker();
                    string Dir_Path = DTrack.Directory_Tracker(dt.Date);
                    dbc.ReconFile_Gen(Dir_Path);
                }

                if (counter_flag == true)
                {
                    interval_counter++;
                    if (interval_counter == 1800 && no_of_intervals < 10)
                    {
                        no_of_intervals++;
                        dbc.BOD_EOD(no_of_intervals);
                        interval_counter = 0;
                    }
                    else if (no_of_intervals >= 10)
                    {
                        no_of_intervals = 0;
                        counter_flag = false;
                    }

                }
            }
            catch (Exception ex)
            {
                ET.Directory_Tracker();
                ET.Text_Tracker(ex.ToString());
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
