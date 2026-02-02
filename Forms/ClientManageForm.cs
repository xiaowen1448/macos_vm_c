using System;
using System.Drawing;
using System.Windows.Forms;

namespace VMCloneApp.Forms
{
    public partial class ClientManageForm : Form
    {
        public ClientManageForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "ScptRunner客户端管理";
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
            // API调用地址
            var lblApiUrl = new Label()
            {
                Text = "API调用地址:",
                Location = new Point(20, 30),
                Size = new Size(100, 20),
                Font = new Font("微软雅黑", 9)
            };

            var txtApiUrl = new TextBox()
            {
                Text = "http://localhost:8080/api",
                Location = new Point(130, 30),
                Size = new Size(300, 25),
                Font = new Font("微软雅黑", 9)
            };

            // 客户端版本
            var lblClientVersion = new Label()
            {
                Text = "客户端版本:",
                Location = new Point(20, 80),
                Size = new Size(100, 20),
                Font = new Font("微软雅黑", 9)
            };

            var lblVersionValue = new Label()
            {
                Text = "v1.2.0",
                Location = new Point(130, 80),
                Size = new Size(100, 20),
                Font = new Font("微软雅黑", 9, FontStyle.Bold),
                ForeColor = Color.Blue
            };

            // 客户端状态
            var lblClientStatus = new Label()
            {
                Text = "客户端状态:",
                Location = new Point(20, 130),
                Size = new Size(100, 20),
                Font = new Font("微软雅黑", 9)
            };

            var lblStatusValue = new Label()
            {
                Text = "已连接",
                Location = new Point(130, 130),
                Size = new Size(100, 20),
                Font = new Font("微软雅黑", 9, FontStyle.Bold),
                ForeColor = Color.Green
            };

            // 连接/断开按钮
            var btnConnect = new Button()
            {
                Text = "连接",
                Location = new Point(250, 125),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnConnect.Click += BtnConnect_Click;

            var btnDisconnect = new Button()
            {
                Text = "断开",
                Location = new Point(340, 125),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnDisconnect.Click += BtnDisconnect_Click;

            // 测试连接按钮
            var btnTestConnection = new Button()
            {
                Text = "测试连接",
                Location = new Point(130, 180),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnTestConnection.Click += BtnTestConnection_Click;

            // 状态信息区域
            var lblStatusInfo = new Label()
            {
                Text = "最后连接时间: 2024-01-01 10:30:00\n连接时长: 2小时15分钟",
                Location = new Point(20, 230),
                Size = new Size(400, 40),
                Font = new Font("微软雅黑", 8),
                ForeColor = Color.Gray
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
                lblApiUrl, txtApiUrl,
                lblClientVersion, lblVersionValue,
                lblClientStatus, lblStatusValue,
                btnConnect, btnDisconnect,
                btnTestConnection,
                lblStatusInfo,
                btnOK, btnCancel
            });
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            MessageBox.Show("客户端连接成功！", "连接", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDisconnect_Click(object sender, EventArgs e)
        {
            MessageBox.Show("客户端已断开连接！", "断开", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnTestConnection_Click(object sender, EventArgs e)
        {
            MessageBox.Show("连接测试成功！", "测试", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            MessageBox.Show("客户端配置已保存！", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
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