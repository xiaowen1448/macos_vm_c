using System;
using System.Drawing;
using System.Windows.Forms;

namespace VMCloneApp.Forms
{
    public partial class WumaConfigForm : Form
    {
        public WumaConfigForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "五码配置";
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
            // 五码配置文件
            var lblWumaFile = new Label()
            {
                Text = "五码文件:",
                Location = new Point(20, 30),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            var cmbWumaFile = new ComboBox()
            {
                Location = new Point(110, 30),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbWumaFile.Items.AddRange(new object[] { "14.1", "18.1" });
            cmbWumaFile.SelectedIndex = 0;

            // 可用数量说明
            var lblAvailableCount = new Label()
            {
                Text = "可用数量: 100",
                Location = new Point(280, 32),
                Size = new Size(150, 20),
                Font = new Font("微软雅黑", 8),
                ForeColor = Color.Gray
            };

            // 导入五码文件按钮
            var btnImportWuma = new Button()
            {
                Text = "导入五码文件",
                Location = new Point(110, 70),
                Size = new Size(120, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnImportWuma.Click += BtnImportWuma_Click;

            // 默认五码配置文件
            var lblDefaultWuma = new Label()
            {
                Text = "默认配置:",
                Location = new Point(20, 120),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            var cmbDefaultWuma = new ComboBox()
            {
                Location = new Point(110, 120),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDefaultWuma.Items.AddRange(new object[] { "14.1", "18.1" });
            cmbDefaultWuma.SelectedIndex = 0;

            // 配置说明区域
            var lblConfigInfo = new Label()
            {
                Text = "五码配置包含：序列号、主板UUID、系统UUID、硬件地址、产品型号",
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
                lblWumaFile, cmbWumaFile, lblAvailableCount,
                btnImportWuma,
                lblDefaultWuma, cmbDefaultWuma,
                lblConfigInfo,
                btnOK, btnCancel
            });
        }

        private void BtnImportWuma_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "选择五码配置文件",
                Filter = "五码文件 (*.wuma)|*.wuma|所有文件 (*.*)|*.*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show($"已选择文件: {openFileDialog.FileName}", "导入成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            MessageBox.Show("五码配置已保存！", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
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