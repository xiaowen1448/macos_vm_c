using System;
using System.Drawing;
using System.Windows.Forms;
using VMCloneApp.Utils;

namespace VMCloneApp.Forms
{
    public partial class VMConfigForm : Form
    {
        private CheckBox chkAutoDestroyFailedVM;
        private ComboBox cmbMaxLoginAttempts;
        private ComboBox cmbVMProxyMode;
        private CheckBox chkEnableSnapshotCleanup;
        private CheckBox chkCheckLoginFailure;
        private CheckBox chkCheckStartupFailure;
        private CheckBox chkCheckProxyFailure;
        private CheckBox chkCheckIPFailure;
        private CheckBox chkCheckWumaFailure;

        public VMConfigForm()
        {
            InitializeComponent();
            LoadConfig();
        }

        private void InitializeComponent()
        {
            this.Text = "虚拟机配置";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(500, 400);
            this.MaximumSize = new Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;  // 去掉最小化按钮
            this.ControlBox = true;     // 保留关闭按钮
            this.BackColor = Color.White;
            this.ShowInTaskbar = false; // 不在任务栏显示
            
            // 设置窗体图标
            SetFormIcon();
            
            // 初始化控件
            InitializeControls();
            
            // 按钮区域
            InitializeButtons();
        }

        private void SetFormIcon()
        {
            try
            {
                // 尝试从当前目录加载图标
                string iconPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");
                if (System.IO.File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
                else
                {
                    this.Icon = SystemIcons.Application;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"设置窗体图标时出错: {ex.Message}");
                this.Icon = SystemIcons.Application;
            }
        }

        private void InitializeControls()
        {
            // 虚拟机失效自动销毁
            chkAutoDestroyFailedVM = new CheckBox()
            {
                Text = "虚拟机失效自动销毁",
                Location = new Point(20, 30),
                Size = new Size(150, 20),
                Font = new Font("微软雅黑", 9)
            };

            // 登录最大尝试次数
            var lblMaxLoginAttempts = new Label()
            {
                Text = "登录最大尝试次数:",
                Location = new Point(20, 70),
                Size = new Size(120, 20),
                Font = new Font("微软雅黑", 9)
            };

            cmbMaxLoginAttempts = new ComboBox()
            {
                Location = new Point(150, 70),
                Size = new Size(80, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMaxLoginAttempts.Items.AddRange(new object[] { "1", "2", "3" });

            // 虚拟机代理设置
            var lblVMProxyMode = new Label()
            {
                Text = "虚拟机代理:",
                Location = new Point(20, 110),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            cmbVMProxyMode = new ComboBox()
            {
                Location = new Point(110, 110),
                Size = new Size(120, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbVMProxyMode.Items.AddRange(new object[] { "自动分配", "手动分配" });

            // 快照定期清理
            chkEnableSnapshotCleanup = new CheckBox()
            {
                Text = "快照定期清理",
                Location = new Point(20, 150),
                Size = new Size(120, 20),
                Font = new Font("微软雅黑", 9)
            };

            // 快照清理说明文字
            var lblSnapshotCleanupHelp = new Label()
            {
                Text = "快照过多会占用空间，影响虚拟机性能可能导致卡顿",
                Location = new Point(150, 150),
                Size = new Size(300, 40),
                Font = new Font("微软雅黑", 8),
                ForeColor = Color.Gray
            };

            // 虚拟机失效判断
            var lblVMFailureCheck = new Label()
            {
                Text = "虚拟机失效判断:",
                Location = new Point(20, 200),
                Size = new Size(100, 20),
                Font = new Font("微软雅黑", 9, FontStyle.Bold)
            };

            chkCheckLoginFailure = new CheckBox()
            {
                Text = "登录三次失败",
                Location = new Point(20, 230),
                Size = new Size(100, 20),
                Font = new Font("微软雅黑", 9)
            };

            chkCheckStartupFailure = new CheckBox()
            {
                Text = "启动异常",
                Location = new Point(130, 230),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            chkCheckProxyFailure = new CheckBox()
            {
                Text = "代理异常",
                Location = new Point(220, 230),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            chkCheckIPFailure = new CheckBox()
            {
                Text = "IP异常",
                Location = new Point(310, 230),
                Size = new Size(70, 20),
                Font = new Font("微软雅黑", 9)
            };

            chkCheckWumaFailure = new CheckBox()
            {
                Text = "五码异常",
                Location = new Point(390, 230),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            this.Controls.AddRange(new Control[] {
                chkAutoDestroyFailedVM,
                lblMaxLoginAttempts, cmbMaxLoginAttempts,
                lblVMProxyMode, cmbVMProxyMode,
                chkEnableSnapshotCleanup, lblSnapshotCleanupHelp,
                lblVMFailureCheck,
                chkCheckLoginFailure, chkCheckStartupFailure, chkCheckProxyFailure, chkCheckIPFailure, chkCheckWumaFailure
            });
        }

        private void InitializeButtons()
        {
            // 确定按钮
            var btnOK = new Button()
            {
                Text = "确定",
                Location = new Point(300, 300),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnOK.Click += BtnOK_Click;

            // 取消按钮
            var btnCancel = new Button()
            {
                Text = "取消",
                Location = new Point(390, 300),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnCancel.Click += BtnCancel_Click;

            this.Controls.AddRange(new Control[] { btnOK, btnCancel });
        }

        private void LoadConfig()
        {
            try
            {
                var config = ConfigManager.LoadConfig();
                
                // 加载虚拟机配置
                chkAutoDestroyFailedVM.Checked = config.AutoDestroyFailedVM;
                cmbMaxLoginAttempts.SelectedItem = config.MaxLoginAttempts.ToString();
                cmbVMProxyMode.SelectedItem = config.VMProxyMode;
                chkEnableSnapshotCleanup.Checked = config.EnableSnapshotCleanup;
                chkCheckLoginFailure.Checked = config.CheckLoginFailure;
                chkCheckStartupFailure.Checked = config.CheckStartupFailure;
                chkCheckProxyFailure.Checked = config.CheckProxyFailure;
                chkCheckIPFailure.Checked = config.CheckIPFailure;
                chkCheckWumaFailure.Checked = config.CheckWumaFailure;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveConfig()
        {
            try
            {
                var currentConfig = GetCurrentConfig();
                ConfigManager.SaveConfig(currentConfig);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private AppConfig GetCurrentConfig()
        {
            var config = ConfigManager.LoadConfig();
            
            // 更新虚拟机配置
            config.AutoDestroyFailedVM = chkAutoDestroyFailedVM.Checked;
            config.MaxLoginAttempts = int.TryParse(cmbMaxLoginAttempts.SelectedItem?.ToString(), out int attempts) ? attempts : 3;
            config.VMProxyMode = cmbVMProxyMode.SelectedItem?.ToString() ?? "自动分配";
            config.EnableSnapshotCleanup = chkEnableSnapshotCleanup.Checked;
            config.CheckLoginFailure = chkCheckLoginFailure.Checked;
            config.CheckStartupFailure = chkCheckStartupFailure.Checked;
            config.CheckProxyFailure = chkCheckProxyFailure.Checked;
            config.CheckIPFailure = chkCheckIPFailure.Checked;
            config.CheckWumaFailure = chkCheckWumaFailure.Checked;
            
            return config;
        }

        // 事件处理方法
        private void BtnOK_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}