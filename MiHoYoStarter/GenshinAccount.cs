using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiHoYoStarter
{
    [Serializable]
    public class GenshinAccount : MiHoYoAccount
    {
        public GenshinAccount() : base("Genshin", @"HKEY_CURRENT_USER\Software\miHoYo\原神", "MIHOYOSDK_ADL_PROD_CN_h3123967166")
        {
        }
    }
}
