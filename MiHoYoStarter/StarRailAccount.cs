using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiHoYoStarter
{
    [Serializable]
    public class StarRailAccount : MiHoYoAccount
    {
        public StarRailAccount() : base("StarRail", @"HKEY_CURRENT_USER\Software\miHoYo\崩坏：星穹铁道", "MIHOYOSDK_ADL_PROD_CN_h3123967166")
        {
        }
    }
}
