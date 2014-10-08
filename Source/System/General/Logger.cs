using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace POSystem
{
    public enum Log_Type
    {
        ERROR = 0,
        WARNING = 1,
        INFO = 2
    }

    public static class Logger
    {
        static private bool              _active;  // In case you want to deactivate the logger

        static public void init()
        {
            _active = true;

            try
            {
                StreamWriter textOut = new StreamWriter(new FileStream("log.html", FileMode.Create, FileAccess.Write));
                textOut.WriteLine("<span style=\"font-family: &quot;Arial&quot;; color: #000000;font-weight:bold;\">");
                textOut.WriteLine("Log started at " + System.DateTime.Now.ToLongTimeString() + "</span><hr />");
                textOut.Close();
            }
            catch (Exception ex)
            {
                _active = false;
                string error = ex.Message;
            }
        }
 
        static public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }
 
        static public void log(Log_Type type, string text)
        {
            if (!_active) return;
            string begin = "";
            string prefix = "";
            switch (type)
            {
                case Log_Type.ERROR: begin = "<span style=\"font-family: &quot;Arial&quot;;color: #d41616;\">"; prefix = "ERROR"; break;
                case Log_Type.INFO: begin = "<span style=\"font-family: &quot;Arial&quot;;color: #167dd4;\">"; prefix = "INFO"; break;
                case Log_Type.WARNING: begin = "<span style=\"font-family: &quot;Arial&quot;;color: #00ff00;\">"; prefix = "WARNING"; break;
            }
            text = begin + System.DateTime.Now.ToLongTimeString() + " | (<strong>" + prefix + "</strong>)&nbsp;" + text + "</span><br />";
            Output(text);
        }
 
        static private void Output(string text)
        {
            try
            {
                StreamWriter textOut = new StreamWriter(new FileStream("log.html", FileMode.Append, FileAccess.Write));
                textOut.WriteLine(text);
                textOut.Close();
            }
            catch (System.Exception e)
            {
                string error = e.Message;
            }
        }
    }
}
