using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiHoYoStarter.Utils
{
    internal class HuiUtils
    {
        public static string GetMyVersion()
        {
            string currentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (currentVersion.Length > 3)
            {
                return "v" +  currentVersion.Substring(0, 3);
            }
            return "";
        }
    }
}
