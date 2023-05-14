using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiHoYoStarter
{
    public partial class FormInput : Form
    {
        private string gameNameEN;
        public FormInput(string gameNameEN)
        {
            InitializeComponent();
            this.gameNameEN = gameNameEN;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAcctName.Text))
            {
                MessageBox.Show("请输入账号备注", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MiHoYoAccount acct = null;
            if (gameNameEN == "Genshin")
            {
                acct = new GenshinAccount();
                
            }
            else if (gameNameEN == "GenshinCloud")
            {
                acct = new GenshinCloudAccount();
            }
            else if (gameNameEN == "StarRail")
            {
                acct = new StarRailAccount();
            }
            else if (gameNameEN == "HonkaiImpact3")
            {
                acct = new HonkaiImpact3Account();
            }
            else
            {
                MessageBox.Show("未知的游戏账户类型", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            acct.ReadFromRegistry();
            acct.Name = txtAcctName.Text;
            acct.WriteToDisk();
            this.Close();
        }
    }
}
