using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using  System.Data;
using System.IO;




namespace DayManager
{
    class db_class
    {
        ErrorTracker ET = new ErrorTracker();
        private string first="#Busopasnosty";
        private string middle="123*Emecartnace";
        private string last="noon321*";
        private string _con_string = null;
        public static OdbcConnection _conn;
        public string responseCode = null;
        //database connection open and close////////////////

        public void db_Connection_open()
        {
            _con_string="DSN=CCBank;uid=DayManager;pwd="+first+middle+last;
            try
            {
                _conn = new OdbcConnection(_con_string);
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
                _conn.Open();
            }
            catch (Exception err)
            {
                ET.Directory_Tracker();
                ET.Text_Tracker(err.ToString());
            }

        }
        private void db_Connection_close()
        {
            _conn.Close();
        }


        public void db_InsertUpdateDelete_Operations(string _sql_string)
        {
            try
            {
                db_Connection_open();
                OdbcCommand _command_obj = new OdbcCommand(_sql_string, _conn);
                _command_obj.ExecuteNonQuery();
            }
            catch (Exception err)
            {
                ET.Directory_Tracker();
                ET.Text_Tracker(err.ToString());
            }
        }
        public object db_select_operation(string _sql_string, string return_param)
        {
            try
            {
                db_Connection_open();
                string ret_val = null;
                DataTable _DT_obj = new DataTable();
                OdbcCommand _command_obj = new OdbcCommand(_sql_string, _conn);
                //OdbcDataAdapter _DA_obj = new OdbcDataAdapter(_command_obj);
                using (OdbcDataReader reader = _command_obj.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        //Console.WriteLine(String.Format("{0}", reader[return_param]));
                        ret_val = String.Format("{0}", reader[return_param]);
                    }
                }
                //_DA_obj.Fill(_DT_obj);
                //return _DT_obj;
                db_Connection_close();
                return ret_val;
            }
            catch(Exception ex)
            {
                ET.Directory_Tracker();
                ET.Text_Tracker(ex.ToString());
                return "";
            }

        }

        public void BOD_EOD(int interval)
        {
            int return_val;

            try
            {
                string ss = "select cc.sf_update_daybegin_dayclosing_process('"+interval+"') return_val ";
                return_val = Convert.ToInt16(db_select_operation(ss, "return_val"));

            }
            catch (Exception exx)
            {
                ET.Directory_Tracker();
                ET.Text_Tracker(exx.ToString());

            }
         }
        public void ReconFile_Gen(string path)
        {
            int return_val;

            try
            {
                string ss = "select cc.sf_generate_ATM_recon_file('"+path+"') return_val ";
                return_val = Convert.ToInt16(db_select_operation(ss, "return_val"));
            }
            catch (Exception exx)
            {
                ET.Directory_Tracker();
                ET.Text_Tracker(exx.ToString());
            }
        }
        public string BussinessDate()
        {
            string return_val=null;

            try
            {
                string ss = "select dateformat(max(bank_date),'dd-MM-yyyy') return_val from cc.day_details where closed_by is null";
                return_val = (string)db_select_operation(ss, "return_val");
            }
            catch (Exception exx)
            {
                ET.Directory_Tracker();
                ET.Text_Tracker(exx.ToString());
            }
            return return_val;

        }
        public string BOD_EOD_Time(string flag)
        {
            string date_time = "00:01:00";
            
            try
            {
                if (flag == "BOD_EOD")
                {
                    string ss = "select parameter_value from cc_parameters where parameter_name='DayClose_DayBegin_Time'";
                    date_time = Convert.ToString(db_select_operation(ss, "parameter_value"));
                }
                else if(flag=="Recon")
                {
                    string ss = "select parameter_value from cc_parameters where parameter_name='Recon File Gen Time'";
                    date_time = Convert.ToString(db_select_operation(ss, "parameter_value"));
                }

            }
            catch (Exception exx)
            {
                ET.Directory_Tracker();
                ET.Text_Tracker(exx.ToString());
                return date_time = "00:01:00";
            }
            return date_time;
        }
        public string notification()
        {
            string closed_opened = null;
            try
            {
                string ss = "select distinct(dateformat(bank_date,'dd/mm/yyyy')) Bday from day_details where closed_by is null";
                closed_opened = Convert.ToString(db_select_operation(ss, "Bday"));

            }
            catch (Exception exx)
            {
                ET.Directory_Tracker();
                ET.Text_Tracker(exx.ToString());
            }
           
            return closed_opened;
        }

    }
}
