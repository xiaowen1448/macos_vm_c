using System;
using System.Drawing;
using System.Windows.Forms;

namespace VMCloneApp.Forms
{
    public partial class ScriptManageForm : Form
    {
        public ScriptManageForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "脚本管理";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(500, 400);
            this.MaximumSize = new Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.White;
            
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
            // 脚本列表区域
            var lblScriptList = new Label()
            {
                Text = "脚本列表:",
                Location = new Point(20, 30),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9, FontStyle.Bold)
            };

            var lstScripts = new ListBox()
            {
                Location = new Point(20, 60),
                Size = new Size(200, 150),
                Font = new Font("微软雅黑", 9)
            };
            lstScripts.Items.AddRange(new object[] { "初始化脚本.sh", "配置脚本.py", "启动脚本.bat", "清理脚本.ps1" });

            // 脚本操作按钮
            var btnAddScript = new Button()
            {
                Text = "添加脚本",
                Location = new Point(240, 60),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnAddScript.Click += BtnAddScript_Click;

            var btnEditScript = new Button()
            {
                Text = "编辑脚本",
                Location = new Point(240, 100),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnEditScript.Click += BtnEditScript_Click;

            var btnDeleteScript = new Button()
            {
                Text = "删除脚本",
                Location = new Point(240, 140),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnDeleteScript.Click += BtnDeleteScript_Click;

            var btnRunScript = new Button()
            {
                Text = "运行脚本",
                Location = new Point(240, 180),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnRunScript.Click += BtnRunScript_Click;

            // 脚本详细信息
            var lblScriptDetails = new Label()
            {
                Text = "脚本详情:",
                Location = new Point(20, 230),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9, FontStyle.Bold)
            };

            var txtScriptDetails = new TextBox()
            {
                Location = new Point(20, 260),
                Size = new Size(450, 60),
                Multiline = true,
                ReadOnly = true,
                Text = "选择脚本查看详细信息...",
                BackColor = Color.FromArgb(240, 240, 240),
                Font = new Font("微软雅黑", 9)
            };

            // 按钮区域 - 右下角布局
            var btnOK = new Button()
            {
                Text = "确定",
                Location = new Point(300, 340),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnOK.Click += BtnOK_Click;

            var btnCancel = new Button()
            {
                Text = "取消",
                Location = new Point(390, 340),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnCancel.Click += BtnCancel_Click;

            // 添加到窗体
            this.Controls.AddRange(new Control[] {
                lblScriptList, lstScripts,
                btnAddScript, btnEditScript, btnDeleteScript, btnRunScript,
                lblScriptDetails, txtScriptDetails,
                btnOK, btnCancel
            });
        }

        private void BtnAddScript_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = "选择脚本文件",
                Filter = "脚本文件 (*.sh;*.py;*.bat;*.ps1)|*.sh;*.py;*.bat;*.ps1|所有文件 (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show($"已选择 {openFileDialog.FileNames.Length} 个脚本文件", "导入成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnEditScript_Click(object sender, EventArgs e)
        {
            MessageBox.Show("编辑脚本功能", "编辑", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteScript_Click(object sender, EventArgs e)
        {
            MessageBox.Show("删除脚本功能", "删除", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRunScript_Click(object sender, EventArgs e)
        {
            MessageBox.Show("运行脚本功能", "运行", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            MessageBox.Show("脚本配置已保存！", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
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