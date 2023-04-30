using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiHoYoStarter
{
    [Serializable]
    public class GenshinCloudAccount : MiHoYoAccount
    {
        public GenshinCloudAccount() : base("GenshinCloud", @"HKEY_CURRENT_USER\Software\miHoYo\云·原神", "MIHOYOSDK_ADL_0")
        {
        }
    }
}
