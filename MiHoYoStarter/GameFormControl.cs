using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiHoYoStarter
{
    public class GameFormControl
    {
        private string userDataPath;
        public string GameNameCN { get; set; }

        /// <summary>
        /// 游戏简称
        /// </summary>
        public string GameNameShortCN { get; set; }

        public string GameNameEN { get; set; }

        public string ProcessName { get; set; }

        private FormMain formMain;

        private TextBox txtPath;
        private TextBox txtStartParam;
        private ListView lvwAcct;
        private Button btnChoosePath;
        private Button btnAdd;
        private Button btnSwitch;
        private Button btnDelete;
        private Button btnStart;

        private CheckBox chkAutoStart;

        public List<ToolStripMenuItem> AcctMenuItemList { get; set; }

        public GameFormControl(string nameCN, string nameShortCN, string nameEN, string processName)
        {
            GameNameCN = nameCN;
            GameNameShortCN = nameShortCN;
            GameNameEN = nameEN;
            ProcessName = processName;
            AcctMenuItemList = new List<ToolStripMenuItem>();
        }

        public void InitControl(FormMain formMain, TabPage tabPage, string pathSetting)
        {
            this.formMain = formMain;
            // 用户数据路径
            userDataPath = Path.Combine(Application.StartupPath, "UserData", GameNameEN);
            if (!Directory.Exists(userDataPath))
            {
                Directory.CreateDirectory(userDataPath);
            }

            // 初始化控件
            FindControl(tabPage, $"txt{GameNameEN}Path", ref txtPath);
            FindControl(tabPage, $"txt{GameNameEN}StartParam", ref txtStartParam);
            FindControl(tabPage, $"lvw{GameNameEN}Acct", ref lvwAcct);
            FindControl(tabPage, $"btn{GameNameEN}ChoosePath", ref btnChoosePath);
            FindControl(tabPage, $"btn{GameNameEN}Add", ref btnAdd);
            FindControl(tabPage, $"btn{GameNameEN}Switch", ref btnSwitch);
            FindControl(tabPage, $"btn{GameNameEN}Delete", ref btnDelete);
            FindControl(tabPage, $"btn{GameNameEN}Start", ref btnStart);
            FindControl(tabPage, $"chk{GameNameEN}AutoStart", ref chkAutoStart);



            // 默认路径
            if (string.IsNullOrEmpty(pathSetting))
            {
                string installPath = FindInstallPathFromRegistry(GameNameCN);
                if (installPath != null)
                {
                    string path = null;
                    switch (GameNameEN)
                    {
                        case "Genshin":
                            path = Path.Combine(installPath, "Genshin Impact Game", "YuanShen.exe"); // 只支持国服
                            break;
                        case "GenshinOversea":
                            path = Path.Combine(installPath, "Genshin Impact Game", "GenshinImpact.exe");
                            break;
                        case "GenshinCloud":
                            path = Path.Combine(installPath, "Genshin Impact Cloud Game.exe");
                            break;
                        case "StarRail":
                            path = Path.Combine(installPath, "Game", "StarRail.exe");
                            break;
                        case "ZZZ":
                            path = Path.Combine(installPath, "Game", "ZenlessZoneZero.exe");
                            break;
                        case "ZZZOversea":
                            path = Path.Combine(installPath, "Game", "ZenlessZoneZero.exe");
                            break;
                        case "StarRailOversea":
                            path = Path.Combine(installPath, "Game", "StarRail.exe");
                            break;
                        case "HonkaiImpact3":
                            path = Path.Combine(installPath, "Game", "BH3.exe");
                            break;
                    }

                    if (path != null && File.Exists(path))
                    {
                        txtPath.Text = path;
                    }
                }
            }
            else
            {
                txtPath.Text = pathSetting;
            }

            // 绑定事件
            btnChoosePath.Click += btnChoosePathClick;
            btnAdd.Click += btnAddClick;
            btnSwitch.Click += btnSwitchClick;
            btnDelete.Click += btnDeleteClick;
            if (btnStart != null)
            {
                btnStart.Click += btnhStartClick;
            }

            lvwAcct.MouseDoubleClick += lvwAcct_MouseDoubleClick;

            RefreshList();
        }

        private void FindControl<T>(TabPage tabPage, string name, ref T result) where T : Control
        {
            var controls = tabPage.Controls.Find(name, true);
            if (controls.Length > 0)
            {
                result = (T)controls[0];
            }
        }

        private void btnChoosePathClick(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Multiselect = false; //是否可以选择多个文件
            dialog.Title = "请选择游戏启动程序（注意不是游戏启动器 launcher.exe！）";
            //选择某种类型的文件
            switch (GameNameEN)
            {
                case "Genshin":
                    dialog.Filter = "原神|YuanShen.exe|可执行文件(*.exe)|*.exe";
                    break;
                case "GenshinOversea":
                    dialog.Filter = "原神（国际服）|GenshinImpact.exe|可执行文件(*.exe)|*.exe";
                    break;
                case "GenshinCloud":
                    dialog.Filter = "云原神|Genshin Impact Cloud Game.exe|可执行文件(*.exe)|*.exe";
                    break;
                case "StarRail":
                    dialog.Filter = "崩坏：星穹铁道|StarRail.exe|可执行文件(*.exe)|*.exe";
                    break;
                case "ZZZ":
                    dialog.Filter = "绝区零|ZenlessZoneZero.exe|可执行文件(*.exe)|*.exe";
                    break;
                case "ZZZOversea":
                    dialog.Filter = "绝区零（国际服）|ZenlessZoneZero.exe|可执行文件(*.exe)|*.exe";
                    break;
                case "StarRailOversea":
                    dialog.Filter = "崩坏：星穹铁道（国际服）|StarRail.exe|可执行文件(*.exe)|*.exe";
                    break;
                case "HonkaiImpact3":
                    dialog.Filter = "崩坏3|BH3.exe|可执行文件(*.exe)|*.exe";
                    break;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileName.EndsWith("launcher.exe"))
                {
                    string msg = $@"请选择【{GameNameCN}】的游戏本体执行文件（注意不是启动器！！！）
以下是游戏本体执行文件的路径：
原神：\Genshin Impact\Genshin Impact Game\YuanShen.exe
原神（国际服）：\Genshin Impact\Genshin Impact Game\GenshinImpact.exe
云·原神：\Genshin Impact Cloud Game\Genshin Impact Cloud Game.exe
崩坏：星穹铁道：\Star Rail\Game\StarRail.exe
绝区零：\ZenlessZoneZero.exe
崩坏3：\Honkai Impact 3rd\Game\BH3.exe";
                    MessageBox.Show(msg, "提示");
                    return;
                }

                txtPath.Text = dialog.FileName;
            }
        }

        private void btnAddClick(object sender, EventArgs e)
        {
            FormInput form = new FormInput(GameNameEN);
            form.ShowDialog();
            RefreshList();
        }

        private void btnSwitchClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPath.Text))
            {
                MessageBox.Show($"请先选择【{GameNameCN}】游戏启动程序路径，才能进行账户切换", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (!txtPath.Text.ToLower().EndsWith("exe"))
            {
                MessageBox.Show($"请先选择正确游戏启动程序路径（注意不是目录，是exe可执行文件）", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (lvwAcct.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要切换的账号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string name = lvwAcct.SelectedItems[0]?.Text;
            Switch(name);
        }

        private void btnDeleteClick(object sender, EventArgs e)
        {
            if (lvwAcct.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要切换的账号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string name = lvwAcct.SelectedItems[0]?.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("请选择要切换的账号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MiHoYoAccount.DeleteFromDisk(userDataPath, name);
            RefreshList();
        }

        private void btnhStartClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPath.Text))
            {
                MessageBox.Show($"请先选择【{GameNameCN}】游戏启动程序路径，才能启动游戏", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (!txtPath.Text.ToLower().EndsWith("exe"))
            {
                MessageBox.Show($"请先选择正确游戏启动程序路径（注意不是目录，是exe可执行文件）", "提示", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WorkingDirectory = Environment.CurrentDirectory,
                    FileName = txtPath.Text,
                    Verb = "runas"
                };
                if (txtStartParam != null && !string.IsNullOrEmpty(txtStartParam.Text))
                {
                    startInfo.Arguments = txtStartParam.Text;
                }

                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("游戏启动失败！\n" + ex.Message + "\n" + ex.StackTrace, "错误", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void lvwAcct_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo info = lvwAcct.HitTest(e.X, e.Y);
            if (info.Item != null)
            {
                Switch(info.Item.Text);
            }
        }

        private void RefreshList()
        {
            lvwAcct.Items.Clear();
            AcctMenuItemList.Clear();
            DirectoryInfo root = new DirectoryInfo(userDataPath);
            FileInfo[] files = root.GetFiles();
            foreach (FileInfo file in files)
            {
                lvwAcct.Items.Add(new ListViewItem()
                {
                    Text = file.Name
                });
                var m = new ToolStripMenuItem()
                {
                    Name = file.Name,
                    Text = $"【{GameNameShortCN}】-【{file.Name}】",
                    Tag = GameNameEN, // 用tag来标识
                };
                m.Click += ToolStripMenuClick;
                AcctMenuItemList.Add(m);
            }

            if (lvwAcct.Items.Count > 0)
            {
                btnDelete.Enabled = true;
                btnSwitch.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
                btnSwitch.Enabled = false;
            }

            formMain.RefreshNotifyIconContextMenu(); // 调用主界面刷新菜单
        }

        private void ToolStripMenuClick(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem toolStripMenuItem)
            {
                Switch(toolStripMenuItem.Name);
                foreach (var menuItem in AcctMenuItemList)
                {
                    menuItem.Checked = false;
                }

                toolStripMenuItem.Checked = true;
            }
        }

        private void Switch(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("请选择要切换的账号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(txtPath.Text))
            {
                MessageBox.Show("请选择游戏路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var pros = Process.GetProcessesByName(ProcessName);


            MiHoYoAccount acct = MiHoYoAccount.CreateFromDisk(userDataPath, name);
            if (string.IsNullOrWhiteSpace(acct.AccountRegDataValue))
            {
                MessageBox.Show("账户内容为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            acct.WriteToRegistry();
            formMain.UpdateBottomLabel($"账户切换至【{name}】成功！");
            if (chkAutoStart.Checked)
            {
                if (pros.Any() && ProcessName != "StarRail")
                {
                    pros[0].Kill();
                    Thread.Sleep(200);
                }

                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.WorkingDirectory = Path.GetDirectoryName(txtPath.Text);
                    startInfo.FileName = txtPath.Text;
                    startInfo.Verb = "runas";
                    if (txtStartParam != null && !string.IsNullOrEmpty(txtStartParam.Text))
                    {
                        startInfo.Arguments = txtStartParam.Text;
                    }

                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("游戏启动失败！\n" + ex.Message + "\n" + ex.StackTrace, "错误", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 从注册表中寻找安装路径
        /// </summary>
        /// <param name="uninstallKeyName">
        /// 安装信息的注册表键名 "原神", "云·原神", "崩坏：星穹铁道","崩坏3"
        /// </param>
        /// <returns>安装路径</returns>
        public static string FindInstallPathFromRegistry(string uninstallKeyName)
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\" +
                                                 uninstallKeyName))
                {
                    if (key == null)
                    {
                        return null;
                    }

                    var installLocation = key.GetValue("InstallPath");
                    if (installLocation != null && !string.IsNullOrEmpty(installLocation.ToString()))
                    {
                        return installLocation.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}