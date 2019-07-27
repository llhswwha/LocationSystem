using Location.BLL.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationServices.Tools
{
    public class PositionEngineLog
    {
        public string LogLeft = "";
        public string LogRight = "";

        public void WriteLogLeft(string txt)
        {
            //Log.Info("WriteLogLeft:" + txt);
            LogLeft = txt + "\n" + LogLeft;
            if (LogLeft.Length > 3000)
            {
                LogLeft = txt;
            }
        }

        public void WriteLogRight(string txt,bool isError)
        {
            if (isError)
            {
                Log.Error(LogTags.Engine2Db, txt);
            }
            
            //Log.Info("WriteLogRight:" + txt);
            LogRight = txt + "\n" + LogRight;
            if (LogRight.Length > 3000)
            {
                LogRight = txt;
            }
        }
    }
}
