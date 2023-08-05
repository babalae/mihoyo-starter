using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace MiHoYoStarter
{
    [Serializable]
    public class MiHoYoAccount
    {
        public string Name { get; set; }

        /// <summary>
        /// 每个游戏保存的数据存在不同的目录
        /// </summary>
        public string SaveFolderName { get; set; }

        /// <summary>
        /// 注册表记住账户信息的键值位置
        /// </summary>
        public string AccountRegKeyName { get; set; }

        /// <summary>
        /// 注册表记住账户信息的键值名
        /// </summary>
        public string AccountRegValueName { get; set; }

        /// <summary>
        /// 注册表记住账户信息的键值数据
        /// </summary>
        public string AccountRegDataValue { get; set; }

        public MiHoYoAccount()
        {
        }


        public MiHoYoAccount(string saveFolderName, string accountRegKeyName, string accountRegValueName)
        {
            SaveFolderName = saveFolderName;
            AccountRegKeyName = accountRegKeyName;
            AccountRegValueName = accountRegValueName;
        }

        public void WriteToDisk()
        {
            File.WriteAllText(Path.Combine(Application.StartupPath, "UserData", SaveFolderName, Name),
                new JavaScriptSerializer().Serialize(this));
        }

        public static void DeleteFromDisk(string userDataPath, string name)
        {
            File.Delete(Path.Combine(userDataPath, name));
        }

        public static MiHoYoAccount CreateFromDisk(string userDataPath, string name)
        {
            string p = Path.Combine(userDataPath, name);
            string json = File.ReadAllText(p);
            return new JavaScriptSerializer().Deserialize<MiHoYoAccount>(json);
        }


        public void ReadFromRegistry()
        {
            AccountRegDataValue = GetStringFromRegistry(AccountRegValueName);
        }

        public void WriteToRegistry()
        {
            SetStringToRegistry(AccountRegValueName, AccountRegDataValue);
        }

        protected string GetStringFromRegistry(string key)
        {
            object value = Registry.GetValue(AccountRegKeyName, key, "");
            if (value == null)
            {
                throw new Exception($@"注册表{AccountRegKeyName}\{key}中没有找到账户信息");
            }
            return Encoding.UTF8.GetString((byte[])value);
        }

        protected void SetStringToRegistry(string key, string value)
        {
            Registry.SetValue(AccountRegKeyName, key, Encoding.UTF8.GetBytes(value));
        }
    }
}