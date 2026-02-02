using System;
using System.Drawing;
using System.Windows.Forms;

namespace VMCloneApp.Forms
{
    public partial class CloneConfigForm : Form
    {
        public CloneConfigForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "克隆配置";
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
            // 母盘配置
            var lblMotherDisk = new Label()
            {
                Text = "母盘配置:",
                Location = new Point(20, 30),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            var cmbMotherDisk = new ComboBox()
            {
                Location = new Point(110, 30),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbMotherDisk.Items.AddRange(new object[] { "mupan1", "mupan2" });
            cmbMotherDisk.SelectedIndex = 0;

            // 虚拟克隆数量
            var lblCloneCount = new Label()
            {
                Text = "克隆数量:",
                Location = new Point(20, 70),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            var cmbCloneCount = new ComboBox()
            {
                Location = new Point(110, 70),
                Size = new Size(100, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            for (int i = 1; i <= 10; i++)
            {
                cmbCloneCount.Items.Add(i.ToString());
            }
            cmbCloneCount.SelectedIndex = 0;

            // 五码配置文件
            var lblWumaConfig = new Label()
            {
                Text = "五码配置:",
                Location = new Point(20, 110),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            var cmbWumaConfig = new ComboBox()
            {
                Location = new Point(110, 110),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbWumaConfig.Items.AddRange(new object[] { "14.1", "18.1" });
            cmbWumaConfig.SelectedIndex = 0;

            // VM命名模式
            var lblNamingPattern = new Label()
            {
                Text = "命名模式:",
                Location = new Point(20, 150),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            var txtNamingPattern = new TextBox()
            {
                Text = "macos{版本}_timestamp_snapshot_index",
                Location = new Point(110, 150),
                Size = new Size(300, 25),
                ReadOnly = true,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            // 命名模式说明
            var lblNamingHelp = new Label()
            {
                Text = "变量说明: {版本} - 系统版本, timestamp - 时间戳, snapshot - 快照编号, index - 序号",
                Location = new Point(110, 180),
                Size = new Size(350, 40),
                Font = new Font("微软雅黑", 8),
                ForeColor = Color.Gray
            };

            // 按钮区域 - 右下角布局
            var btnSave = new Button()
            {
                Text = "确定",
                Location = new Point(300, 320),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnSave.Click += BtnSave_Click;

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
                lblMotherDisk, cmbMotherDisk,
                lblCloneCount, cmbCloneCount,
                lblWumaConfig, cmbWumaConfig,
                lblNamingPattern, txtNamingPattern, lblNamingHelp,
                btnSave, btnCancel
            });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("克隆配置已保存！", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
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