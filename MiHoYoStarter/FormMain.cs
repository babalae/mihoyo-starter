using Microsoft.Win32;
using MiHoYoStarter.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace MiHoYoStarter
{
    public partial class FormMain : Form
    {
        private GameFormControl genshinFormControl = new GameFormControl("原神", "原神", "Genshin", "YuanShen");

        private GameFormControl genshinCloudFormControl =
            new GameFormControl("云·原神", "云原神", "GenshinCloud", "Genshin Impact Cloud Game");

        private GameFormControl starRailFormControl = new GameFormControl("崩坏：星穹铁道", "崩铁", "StarRail", "StarRail");

        private GameFormControl zzzFormControl =
            new GameFormControl("绝区零", "绝区零", "ZZZ", "ZenlessZoneZero");

        private GameFormControl zzzOverseaFormControl =
            new GameFormControl("绝区零（国际服）", "绝区零（国际服）", "ZZZOversea", "ZenlessZoneZero");


        private GameFormControl honkaiImpact3FormControl = new GameFormControl("崩坏3", "崩坏3", "HonkaiImpact3", "BH3");

        private GameFormControl genshinOverseaFormControl =
            new GameFormControl("原神（国际服）", "原神（国际服）", "GenshinOversea", "GenshinImpact");

        private GameFormControl starRailOverseaFormControl =
            new GameFormControl("崩坏：星穹铁道（国际服）", "崩铁（国际服）", "StarRailOversea", "StarRail");

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // 标题加上版本号
            var version = HuiUtils.GetMyVersion();
            this.Text += version;
            GAHelper.Instance.RequestPageView($"MiHoYoStarter_{version}", $"进入{version}版本MHY账户切换工具主界面");

            // 初始化界面控制
            genshinFormControl.InitControl(this, tabPageGenshin, Properties.Settings.Default.GenshinPath);
            genshinOverseaFormControl.InitControl(this, tabPageGenshinOversea,
                Properties.Settings.Default.GenshinOverseaPath);
            genshinCloudFormControl.InitControl(this, tabPageGenshinCloud,
                Properties.Settings.Default.GenshinCloudPath);
            starRailFormControl.InitControl(this, tabPageSatrRail, Properties.Settings.Default.StarRailPath);
            zzzFormControl.InitControl(this, tabPageZZZ, Properties.Settings.Default.ZZZPath);
            zzzOverseaFormControl.InitControl(this, tabPageZZZOversea, Properties.Settings.Default.ZZZOverseaPath);
            starRailOverseaFormControl.InitControl(this, tabPageSatrRailOversea,
                Properties.Settings.Default.StarRailOverseaPath);
            honkaiImpact3FormControl.InitControl(this, tabPageHonkaiImpact3,
                Properties.Settings.Default.HonkaiImpact3Path);

            // 默认配置初始化
            DisplayGenshinTabToolStripMenuItem.Checked = Properties.Settings.Default.DisplayGenshinEnabled;
            DisplayGenshinOverseaTabToolStripMenuItem.Checked = Properties.Settings.Default.DisplayGenshinOverseaEnabled;
            DisplayGenshinCloudTabToolStripMenuItem.Checked = Properties.Settings.Default.DisplayGenshinCloudEnabled;
            DisplayStarRailTabToolStripMenuItem.Checked = Properties.Settings.Default.DisplayStarRailEnabled;
            DisplayZZZTabToolStripMenuItem.Checked = Properties.Settings.Default.DisplayZZZEnabled;
            DisplayZZZOverseaTabToolStripMenuItem.Checked = Properties.Settings.Default.DisplayZZZOverseaEnabled;
            DisplayStarRailOverseaTabToolStripMenuItem.Checked = Properties.Settings.Default.DisplayStarRailOverseaEnabled;
            DisplayHonkaiImpact3TabToolStripMenuItem.Checked = Properties.Settings.Default.DisplayHonkaiImpact3Enabled;

            txtGenshinStartParam.Text = Properties.Settings.Default.GenshinStartParam;
            txtGenshinOverseaStartParam.Text = Properties.Settings.Default.GenshinStartParam;
            txtStarRailStartParam.Text = Properties.Settings.Default.StarRailStartParam;
            txtZZZStartParam.Text = Properties.Settings.Default.ZZZStartParam;
            txtZZZOverseaStartParam.Text = Properties.Settings.Default.ZZZOverseaStartParam;
            txtStarRailOverseaStartParam.Text = Properties.Settings.Default.StarRailStartParam;
            txtHonkaiImpact3StartParam.Text = Properties.Settings.Default.HonkaiImpact3StartParam;

            chkGenshinAutoStart.Checked = Properties.Settings.Default.GenshinAutoStartEnabled;
            chkGenshinOverseaAutoStart.Checked = Properties.Settings.Default.GenshinAutoStartEnabled;
            chkGenshinCloudAutoStart.Checked = Properties.Settings.Default.GenshinCloudAutoStartEnabled;
            chkStarRailAutoStart.Checked = Properties.Settings.Default.StarRailAutoStartEnabled;
            chkZZZAutoStart.Checked = Properties.Settings.Default.ZZZAutoStartEnabled;
            chkZZZOverseaAutoStart.Checked = Properties.Settings.Default.ZZZOverseaAutoStartEnabled;
            chkStarRailOverseaAutoStart.Checked = Properties.Settings.Default.StarRailAutoStartEnabled;
            chkHonkaiImpact3AutoStart.Checked = Properties.Settings.Default.HonkaiImpact3AutoStartEnabled;

            RefreshTab();
            RefreshNotifyIconContextMenu();
        }

        public void RefreshNotifyIconContextMenu()
        {
            this.contextMenuStrip1.Items.Clear();
            if (DisplayGenshinTabToolStripMenuItem.Checked && genshinFormControl.AcctMenuItemList.Count > 0)
            {
                this.contextMenuStrip1.Items.AddRange(genshinFormControl.AcctMenuItemList.ToArray());
                this.contextMenuStrip1.Items.Add(new ToolStripSeparator());
            }

            if (DisplayGenshinOverseaTabToolStripMenuItem.Checked &&
                genshinOverseaFormControl.AcctMenuItemList.Count > 0)
            {
                this.contextMenuStrip1.Items.AddRange(genshinOverseaFormControl.AcctMenuItemList.ToArray());
                this.contextMenuStrip1.Items.Add(new ToolStripSeparator());
            }

            if (DisplayGenshinCloudTabToolStripMenuItem.Checked && genshinCloudFormControl.AcctMenuItemList.Count > 0)
            {
                this.contextMenuStrip1.Items.AddRange(genshinCloudFormControl.AcctMenuItemList.ToArray());
                this.contextMenuStrip1.Items.Add(new ToolStripSeparator());
            }

            if (DisplayStarRailTabToolStripMenuItem.Checked && starRailFormControl.AcctMenuItemList.Count > 0)
            {
                this.contextMenuStrip1.Items.AddRange(starRailFormControl.AcctMenuItemList.ToArray());
                this.contextMenuStrip1.Items.Add(new ToolStripSeparator());
            }
            if (DisplayZZZTabToolStripMenuItem.Checked && zzzFormControl.AcctMenuItemList.Count > 0)
            {
                this.contextMenuStrip1.Items.AddRange(zzzFormControl.AcctMenuItemList.ToArray());
                this.contextMenuStrip1.Items.Add(new ToolStripSeparator());
            }
            if (DisplayZZZOverseaTabToolStripMenuItem.Checked && zzzOverseaFormControl.AcctMenuItemList.Count > 0)
            {
                this.contextMenuStrip1.Items.AddRange(zzzOverseaFormControl.AcctMenuItemList.ToArray());
                this.contextMenuStrip1.Items.Add(new ToolStripSeparator());
            }

            if (DisplayStarRailOverseaTabToolStripMenuItem.Checked &&
                starRailOverseaFormControl.AcctMenuItemList.Count > 0)
            {
                this.contextMenuStrip1.Items.AddRange(starRailOverseaFormControl.AcctMenuItemList.ToArray());
                this.contextMenuStrip1.Items.Add(new ToolStripSeparator());
            }

            if (DisplayHonkaiImpact3TabToolStripMenuItem.Checked && honkaiImpact3FormControl.AcctMenuItemList.Count > 0)
            {
                this.contextMenuStrip1.Items.AddRange(honkaiImpact3FormControl.AcctMenuItemList.ToArray());
                this.contextMenuStrip1.Items.Add(new ToolStripSeparator());
            }

            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.显示主界面ToolStripMenuItem,
                this.退出ToolStripMenuItem
            });
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && notifyIcon.Visible)
            {
                this.ShowInTaskbar = false;
                this.Visible = false;
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                this.Visible = true;
            }

            this.Activate();
        }

        private void btnStarRailFPSEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\miHoYo\崩坏：星穹铁道",
                    "GraphicsSettings_Model_h2986158309", null);
                if (value != null)
                {
                    var json = Encoding.UTF8.GetString((byte[])value);
                    // 一般长这样：{"FPS":60,"EnableVSync":true,"RenderScale":1.4,"ResolutionQuality":5,"ShadowQuality":5,"LightQuality":5,"CharacterQuality":5,"EnvDetailQuality":5,"ReflectionQuality":5,"BloomQuality":5,"AAMode":1}
                    // JavaScriptSerializer 没法反序列化成通用对象，我也很绝望呀
                    Regex r = new Regex("\"FPS\":\\d*,");
                    if (r.IsMatch(json))
                    {
                        string newJson = r.Replace(json, $"\"FPS\":{numericUpDownFPS.Value},");
                        Registry.SetValue(@"HKEY_CURRENT_USER\Software\miHoYo\崩坏：星穹铁道",
                            "GraphicsSettings_Model_h2986158309", Encoding.UTF8.GetBytes(newJson));
                        MessageBox.Show("应用成功！", "提示");
                    }
                    else
                    {
                        MessageBox.Show("没有找到FPS相关配置，大概率是程序有问题啦，联系作者解决~", "提示");
                    }
                }
                else
                {
                    MessageBox.Show("获取注册表内容失败，请在游戏内重新修改图形设置后重试", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("应用时发生异常\n" + ex.Message + "\n" + ex.StackTrace, "提示");
            }
        }

        private void btnStarRailOverseaFPSEdit_Click(object sender, EventArgs e)
        {
            try
            {
                var value = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Cognosphere\Star Rail",
                    "GraphicsSettings_Model_h2986158309", null);
                if (value != null)
                {
                    var json = Encoding.UTF8.GetString((byte[])value);
                    // 一般长这样：{"FPS":60,"EnableVSync":true,"RenderScale":1.4,"ResolutionQuality":5,"ShadowQuality":5,"LightQuality":5,"CharacterQuality":5,"EnvDetailQuality":5,"ReflectionQuality":5,"BloomQuality":5,"AAMode":1}
                    // JavaScriptSerializer 没法反序列化成通用对象，我也很绝望呀
                    Regex r = new Regex("\"FPS\":\\d*,");
                    if (r.IsMatch(json))
                    {
                        string newJson = r.Replace(json, $"\"FPS\":{numericUpDownFPS.Value},");
                        Registry.SetValue(@"HKEY_CURRENT_USER\Software\Cognosphere\Star Rail",
                            "GraphicsSettings_Model_h2986158309", Encoding.UTF8.GetBytes(newJson));
                        MessageBox.Show("应用成功！", "提示");
                    }
                    else
                    {
                        MessageBox.Show("没有找到FPS相关配置，大概率是程序有问题啦，联系作者解决~", "提示");
                    }
                }
                else
                {
                    MessageBox.Show("获取注册表内容失败，请在游戏内重新修改图形设置后重试", "提示");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("应用时发生异常\n" + ex.Message + "\n" + ex.StackTrace, "提示");
            }
        }

        private void DisplayGenshinTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayGenshinTabToolStripMenuItem.Checked = !DisplayGenshinTabToolStripMenuItem.Checked;
            RefreshTab();
            RefreshNotifyIconContextMenu();
        }

        private void DisplayGenshinCloudTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayGenshinCloudTabToolStripMenuItem.Checked = !DisplayGenshinCloudTabToolStripMenuItem.Checked;
            RefreshTab();
            RefreshNotifyIconContextMenu();
        }

        private void DisplayStarRailTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayStarRailTabToolStripMenuItem.Checked = !DisplayStarRailTabToolStripMenuItem.Checked;
            RefreshTab();
            RefreshNotifyIconContextMenu();
        }

        private void DisplayZZZTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayZZZTabToolStripMenuItem.Checked = !DisplayZZZTabToolStripMenuItem.Checked;
            RefreshTab();
            RefreshNotifyIconContextMenu();
        }

        private void DisplayZZZOverseaTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayZZZOverseaTabToolStripMenuItem.Checked = !DisplayZZZOverseaTabToolStripMenuItem.Checked;
            RefreshTab();
            RefreshNotifyIconContextMenu();
        }

        private void DisplayHonkaiImpact3TabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayHonkaiImpact3TabToolStripMenuItem.Checked = !DisplayHonkaiImpact3TabToolStripMenuItem.Checked;
            RefreshTab();
            RefreshNotifyIconContextMenu();
        }

        private void DisplayGenshinOverseaTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayGenshinOverseaTabToolStripMenuItem.Checked = !DisplayGenshinOverseaTabToolStripMenuItem.Checked;
            RefreshTab();
            RefreshNotifyIconContextMenu();
        }

        private void DisplayStarRailOverseaTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayStarRailOverseaTabToolStripMenuItem.Checked = !DisplayStarRailOverseaTabToolStripMenuItem.Checked;
            RefreshTab();
            RefreshNotifyIconContextMenu();
        }

        public void RefreshTab()
        {
            if (DisplayGenshinTabToolStripMenuItem.Checked)
            {
                if (!tab1.TabPages.Contains(tabPageGenshin))
                {
                    tab1.TabPages.Add(tabPageGenshin);
                }
            }
            else
            {
                if (tab1.TabPages.Contains(tabPageGenshin))
                {
                    tab1.TabPages.Remove(tabPageGenshin);
                }
            }

            if (DisplayGenshinOverseaTabToolStripMenuItem.Checked)
            {
                if (!tab1.TabPages.Contains(tabPageGenshinOversea))
                {
                    tab1.TabPages.Add(tabPageGenshinOversea);
                }
            }
            else
            {
                if (tab1.TabPages.Contains(tabPageGenshinOversea))
                {
                    tab1.TabPages.Remove(tabPageGenshinOversea);
                }
            }

            if (DisplayGenshinCloudTabToolStripMenuItem.Checked)
            {
                if (!tab1.TabPages.Contains(tabPageGenshinCloud))
                {
                    tab1.TabPages.Add(tabPageGenshinCloud);
                }
            }
            else
            {
                if (tab1.TabPages.Contains(tabPageGenshinCloud))
                {
                    tab1.TabPages.Remove(tabPageGenshinCloud);
                }
            }

            if (DisplayStarRailTabToolStripMenuItem.Checked)
            {
                if (!tab1.TabPages.Contains(tabPageSatrRail))
                {
                    tab1.TabPages.Add(tabPageSatrRail);
                }
            }
            else
            {
                if (tab1.TabPages.Contains(tabPageSatrRail))
                {
                    tab1.TabPages.Remove(tabPageSatrRail);
                }
            }

            if (DisplayZZZTabToolStripMenuItem.Checked)
            {
                if (!tab1.TabPages.Contains(tabPageZZZ))
                {
                    tab1.TabPages.Add(tabPageZZZ);
                }
            }
            else
            {
                if (tab1.TabPages.Contains(tabPageZZZ))
                {
                    tab1.TabPages.Remove(tabPageZZZ);
                }
            }
            
            if (DisplayZZZOverseaTabToolStripMenuItem.Checked)
            {
                if (!tab1.TabPages.Contains(tabPageZZZOversea))
                {
                    tab1.TabPages.Add(tabPageZZZOversea);
                }
            }
            else
            {
                if (tab1.TabPages.Contains(tabPageZZZOversea))
                {
                    tab1.TabPages.Remove(tabPageZZZOversea);
                }
            }

            if (DisplayStarRailOverseaTabToolStripMenuItem.Checked)
            {
                if (!tab1.TabPages.Contains(tabPageSatrRailOversea))
                {
                    tab1.TabPages.Add(tabPageSatrRailOversea);
                }
            }
            else
            {
                if (tab1.TabPages.Contains(tabPageSatrRailOversea))
                {
                    tab1.TabPages.Remove(tabPageSatrRailOversea);
                }
            }

            if (DisplayHonkaiImpact3TabToolStripMenuItem.Checked)
            {
                if (!tab1.TabPages.Contains(tabPageHonkaiImpact3))
                {
                    tab1.TabPages.Add(tabPageHonkaiImpact3);
                }
            }
            else
            {
                if (tab1.TabPages.Contains(tabPageHonkaiImpact3))
                {
                    tab1.TabPages.Remove(tabPageHonkaiImpact3);
                }
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void 显示主界面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon_DoubleClick(sender, e);
        }

        private void 主页ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/babalae/mihoyo-starter");
        }

        private void 请作者喝咖啡ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/huiyadanli/huiyadanli/blob/master/DONATE.md");
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.DisplayGenshinEnabled = DisplayGenshinTabToolStripMenuItem.Checked;
            Properties.Settings.Default.DisplayGenshinOverseaEnabled = DisplayGenshinOverseaTabToolStripMenuItem.Checked;
            Properties.Settings.Default.DisplayGenshinCloudEnabled = DisplayGenshinCloudTabToolStripMenuItem.Checked;
            Properties.Settings.Default.DisplayStarRailEnabled = DisplayStarRailTabToolStripMenuItem.Checked;
            Properties.Settings.Default.DisplayZZZEnabled = DisplayZZZTabToolStripMenuItem.Checked;
            Properties.Settings.Default.DisplayZZZOverseaEnabled = DisplayZZZOverseaTabToolStripMenuItem.Checked;
            Properties.Settings.Default.DisplayStarRailOverseaEnabled = DisplayStarRailOverseaTabToolStripMenuItem.Checked;
            Properties.Settings.Default.DisplayHonkaiImpact3Enabled = DisplayHonkaiImpact3TabToolStripMenuItem.Checked;

            Properties.Settings.Default.GenshinPath = txtGenshinPath.Text;
            Properties.Settings.Default.GenshinOverseaPath = txtGenshinOverseaPath.Text;
            Properties.Settings.Default.GenshinCloudPath = txtGenshinCloudPath.Text;
            Properties.Settings.Default.StarRailPath = txtStarRailPath.Text;
            Properties.Settings.Default.ZZZPath = txtZZZPath.Text;
            Properties.Settings.Default.ZZZOverseaPath = txtZZZOverseaPath.Text;
            Properties.Settings.Default.StarRailOverseaPath = txtStarRailOverseaPath.Text;
            Properties.Settings.Default.HonkaiImpact3Path = txtHonkaiImpact3Path.Text;

            Properties.Settings.Default.GenshinStartParam = txtGenshinStartParam.Text;
            Properties.Settings.Default.GenshinOverseaStartParam = txtGenshinOverseaStartParam.Text;
            Properties.Settings.Default.StarRailStartParam = txtStarRailStartParam.Text;
            Properties.Settings.Default.ZZZStartParam = txtZZZStartParam.Text;
            Properties.Settings.Default.ZZZOverseaStartParam = txtZZZOverseaStartParam.Text;
            Properties.Settings.Default.StarRailOverseaStartParam = txtStarRailOverseaStartParam.Text;
            Properties.Settings.Default.HonkaiImpact3StartParam = txtHonkaiImpact3StartParam.Text;


            Properties.Settings.Default.GenshinAutoStartEnabled = chkGenshinAutoStart.Checked;
            Properties.Settings.Default.GenshinOverseaAutoStartEnabled = chkGenshinOverseaAutoStart.Checked;
            Properties.Settings.Default.GenshinCloudAutoStartEnabled = chkGenshinCloudAutoStart.Checked;
            Properties.Settings.Default.StarRailAutoStartEnabled = chkStarRailAutoStart.Checked;
            Properties.Settings.Default.ZZZAutoStartEnabled = chkZZZAutoStart.Checked;
            Properties.Settings.Default.ZZZOverseaAutoStartEnabled = chkZZZOverseaAutoStart.Checked;
            Properties.Settings.Default.StarRailOverseaAutoStartEnabled = chkStarRailOverseaAutoStart.Checked;
            Properties.Settings.Default.HonkaiImpact3AutoStartEnabled = chkHonkaiImpact3AutoStart.Checked;

            Properties.Settings.Default.Save();
        }

        public void UpdateBottomLabel(string info)
        {
            this.toolStripStatusLabel1.Text = info;
        }

        private void btnStarRailSwitch_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

            Process.Start("https://github.com/qsuron0/mihoyo-starter");
        }
    }
}