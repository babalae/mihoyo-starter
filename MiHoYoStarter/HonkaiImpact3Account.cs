using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiHoYoStarter
{
    [Serializable]
    public class HonkaiImpact3Account : MiHoYoAccount
    {
        public HonkaiImpact3Account() : base("HonkaiImpact3", @"HKEY_CURRENT_USER\Software\miHoYo\崩坏3", "MIHOYOSDK_ADL_PROD_CN_h3123967166")
        {
        }
    }
}
