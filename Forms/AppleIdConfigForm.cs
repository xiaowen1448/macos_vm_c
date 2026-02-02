using System;
using System.Drawing;
using System.Windows.Forms;

namespace VMCloneApp.Forms
{
    public partial class AppleIdConfigForm : Form
    {
        public AppleIdConfigForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "AppleID配置";
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
        }

        private void SetFormIcon()
        {
            try
            {
                string currentDirIcon = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "app.ico");
                if (System.IO.File.Exists(currentDirIcon))
                {
                    this.Icon = new Icon(currentDirIcon);
                    return;
                }
                
                string appDirIcon = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");
                if (System.IO.File.Exists(appDirIcon))
                {
                    this.Icon = new Icon(appDirIcon);
                    return;
                }
            }
            catch (Exception)
            {
                // 忽略图标加载错误
            }
        }

        private void InitializeControls()
        {
            // AppleID文件
            var lblAppleIdFile = new Label()
            {
                Text = "AppleID文件:",
                Location = new Point(20, 30),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            var cmbAppleIdFile = new ComboBox()
            {
                Location = new Point(110, 30),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbAppleIdFile.Items.AddRange(new object[] { "2026id", "2026id002" });
            cmbAppleIdFile.SelectedIndex = 0;

            // 可用数量说明
            var lblAvailableCount = new Label()
            {
                Text = "可用AppleID数量: 50",
                Location = new Point(280, 32),
                Size = new Size(150, 20),
                Font = new Font("微软雅黑", 8),
                ForeColor = Color.Gray
            };

            // 导入AppleID文件按钮
            var btnImportAppleId = new Button()
            {
                Text = "导入AppleID文件",
                Location = new Point(110, 70),
                Size = new Size(120, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnImportAppleId.Click += BtnImportAppleId_Click;

            // 默认AppleID配置文件
            var lblDefaultAppleId = new Label()
            {
                Text = "默认配置:",
                Location = new Point(20, 120),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            var cmbDefaultAppleId = new ComboBox()
            {
                Location = new Point(110, 120),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDefaultAppleId.Items.AddRange(new object[] { "2026id", "2026id002" });
            cmbDefaultAppleId.SelectedIndex = 0;

            // 配置说明区域
            var lblConfigInfo = new Label()
            {
                Text = "AppleID配置包含：账号、密码、双重认证、设备管理等信息",
                Location = new Point(20, 170),
                Size = new Size(450, 40),
                Font = new Font("微软雅黑", 9),
                ForeColor = Color.DarkBlue
            };

            // 按钮区域 - 右下角布局
            var btnOK = new Button()
            {
                Text = "确定",
                Location = new Point(300, 320),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnOK.Click += BtnOK_Click;

            var btnCancel = new Button()
            {
                Text = "取消",
                Location = new Point(390, 320),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnCancel.Click += BtnCancel_Click;

            // 添加到窗体
            this.Controls.AddRange(new Control[] {
                lblAppleIdFile, cmbAppleIdFile, lblAvailableCount,
                btnImportAppleId,
                lblDefaultAppleId, cmbDefaultAppleId,
                lblConfigInfo,
                btnOK, btnCancel
            });
        }

        private void BtnImportAppleId_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "选择AppleID配置文件",
                Filter = "AppleID文件 (*.appleid)|*.appleid|所有文件 (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show($"已选择文件: {openFileDialog.FileName}", "导入成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            MessageBox.Show("AppleID配置已保存！", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}