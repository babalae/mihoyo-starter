using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiHoYoStarter
{
    [Serializable]
    public class ZZZOverseaAccount : MiHoYoAccount
    {
        public ZZZOverseaAccount() : base("ZZZOversea", @"HKEY_CURRENT_USER\Software\miHoYo\ZenlessZoneZero", "MIHOYOSDK_ADL_PROD_OVERSEA_h1158948810")
        {
        }
    }
}
