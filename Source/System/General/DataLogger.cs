using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace POSystem
{
    public static class DataLogger
    {
        static private bool _active;  // In case you want to deactivate the logger

        static public void init(string _firstLine)
        {
#if WINDOWS
            _active = true;

            try
            {
                StreamWriter textOut = new StreamWriter(new FileStream("data.csv", FileMode.Create, FileAccess.Write));
                textOut.WriteLine(_firstLine);
                textOut.Close();
            }
            catch (Exception ex)
            {
                _active = false;
                string error = ex.Message;
            }
#endif
        }

        static public void Output(string text)
        {
#if WINDOWS
            if (!_active) return;
            try {
                StreamWriter textOut = new StreamWriter(new FileStream("data.csv", FileMode.Append, FileAccess.Write));
                textOut.WriteLine(text);
                textOut.Close();
            } catch (System.Exception e) {
                string error = e.Message;
            }
#endif
        }
    }
}
