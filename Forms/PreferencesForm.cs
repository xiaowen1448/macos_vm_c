using System;
using System.Drawing;
using System.Windows.Forms;
using VMCloneApp.Utils;

namespace VMCloneApp.Forms
{
    public partial class PreferencesForm : Form
    {
        // 控件引用
        private ComboBox cmbMotherDisk;
        private ComboBox cmbCloneCount;
        private ComboBox cmbWumaConfig;
        private TextBox txtNamingPattern;
        private TextBox txtMotherDiskDirectory;
        private TextBox txtCloneVMDirectory;
        private TextBox txtVMDirectory;
        private ComboBox cmbWumaFile;
        private ComboBox cmbDefaultWuma;
        private ComboBox cmbAppleIdFile;
        private ComboBox cmbDefaultAppleId;
        private TextBox txtApiUrl;
        private ComboBox cmbEmailTemplate;
        private ComboBox cmbEmailInterval;
        private ComboBox cmbNumberTemplateFile;
        private Label lblNumberCount;
        private TextBox txtWumaConfigDirectory;  // 五码配置目录文本框引用
        private TextBox txtAppleIdConfigDirectory;  // AppleID配置目录文本框引用
        private Label lblWumaAvailableCount;  // 五码可用数量标签
        private Label lblAppleIdAvailableCount;  // AppleID可用数量标签
        private ExcelStyleForm mainForm;  // 主窗体引用

        public PreferencesForm(ExcelStyleForm parentForm)
        {
            mainForm = parentForm;
            InitializeComponent();
            LoadConfig();
        }

        public PreferencesForm() : this(null)
        {
        }

        private void InitializeComponent()
        {
            this.Text = "偏好设置";
            this.Size = new Size(550, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(550, 550);
            this.MaximumSize = new Size(550, 550);
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
            // 创建选项卡控件
            var tabControl = new TabControl()
            {
                Location = new Point(10, 10),
                Size = new Size(530, 430),
                Font = new Font("微软雅黑", 9)
            };

            // 克隆配置选项卡
            var tabCloneConfig = new TabPage("克隆配置");
            InitializeCloneConfigTab(tabCloneConfig);

            // 五码配置选项卡
            var tabWumaConfig = new TabPage("五码配置");
            InitializeWumaConfigTab(tabWumaConfig);

            // AppleID配置选项卡
            var tabAppleIdConfig = new TabPage("AppleID配置");
            InitializeAppleIdConfigTab(tabAppleIdConfig);

            // 客户端管理选项卡
            var tabClientManage = new TabPage("客户端管理");
            InitializeClientManageTab(tabClientManage);

            // 发信管理选项卡
            var tabEmailManage = new TabPage("发信管理");
            InitializeEmailManageTab(tabEmailManage);

            // 配置管理选项卡
            var tabConfigManage = new TabPage("配置管理");
            InitializeConfigManageTab(tabConfigManage);

            // 添加选项卡
            tabControl.TabPages.AddRange(new TabPage[] { tabCloneConfig, tabWumaConfig, tabAppleIdConfig, tabClientManage, tabEmailManage, tabConfigManage });

            // 按钮区域 - 右下角布局
            var btnOK = new Button()
            {
                Text = "确定",
                Location = new Point(360, 450),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnOK.Click += BtnOK_Click;

            var btnCancel = new Button()
            {
                Text = "取消",
                Location = new Point(450, 450),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnCancel.Click += BtnCancel_Click;

            // 添加到窗体
            this.Controls.AddRange(new Control[] { tabControl, btnOK, btnCancel });
        }

        private void InitializeCloneConfigTab(TabPage tabPage)
        {
            tabPage.BackColor = Color.White;

            // 母盘配置
            var lblMotherDisk = new Label()
            {
                Text = "母盘配置:",
                Location = new Point(20, 30),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            cmbMotherDisk = new ComboBox()
            {
                Location = new Point(110, 30),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            
            // 动态加载母盘配置
            LoadMotherDiskConfigs();

            // 虚拟克隆数量
            var lblCloneCount = new Label()
            {
                Text = "克隆数量:",
                Location = new Point(20, 70),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            cmbCloneCount = new ComboBox()
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

            cmbWumaConfig = new ComboBox()
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

            txtNamingPattern = new TextBox()
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

            // 母盘目录
            var lblMotherDiskDirectory = new Label()
            {
                Text = "母盘目录:",
                Location = new Point(20, 230),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            txtMotherDiskDirectory = new TextBox()
            {
                Location = new Point(110, 230),
                Size = new Size(250, 25),
                Font = new Font("微软雅黑", 9)
            };

            var btnSelectMotherDiskDir = new Button()
            {
                Text = "选择",
                Location = new Point(370, 230),
                Size = new Size(50, 25),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 8)
            };
            btnSelectMotherDiskDir.Click += BtnSelectMotherDiskDir_Click;
            
            // 母盘目录变更时自动刷新母盘配置
            txtMotherDiskDirectory.TextChanged += (s, e) => LoadMotherDiskConfigs();

            // 克隆虚拟机目录
            var lblCloneVMDirectory = new Label()
            {
                Text = "克隆目录:",
                Location = new Point(20, 270),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            txtCloneVMDirectory = new TextBox()
            {
                Location = new Point(110, 270),
                Size = new Size(250, 25),
                Font = new Font("微软雅黑", 9)
            };

            var btnSelectCloneVMDir = new Button()
            {
                Text = "选择",
                Location = new Point(370, 270),
                Size = new Size(50, 25),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 8)
            };
            btnSelectCloneVMDir.Click += BtnSelectCloneVMDir_Click;

            tabPage.Controls.AddRange(new Control[] {
                lblMotherDisk, cmbMotherDisk,
                lblCloneCount, cmbCloneCount,
                lblWumaConfig, cmbWumaConfig,
                lblNamingPattern, txtNamingPattern, lblNamingHelp,
                lblMotherDiskDirectory, txtMotherDiskDirectory, btnSelectMotherDiskDir,
                lblCloneVMDirectory, txtCloneVMDirectory, btnSelectCloneVMDir
            });
        }

        private void InitializeWumaConfigTab(TabPage tabPage)
        {
            tabPage.BackColor = Color.White;

            // 五码配置文件
            var lblWumaFile = new Label()
            {
                Text = "五码文件:",
                Location = new Point(20, 30),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            cmbWumaFile = new ComboBox()
            {
                Location = new Point(110, 30),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            
            // 动态加载五码配置文件
            LoadWumaConfigFiles();
            
            // 五码文件选择事件
            cmbWumaFile.SelectedIndexChanged += (s, e) => UpdateWumaFileLineCount();

            // 可用数量说明
            lblWumaAvailableCount = new Label()
            {
                Text = "可用数量: 0",
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

            // 五码配置目录
            var lblWumaConfigDirectory = new Label()
            {
                Text = "配置目录:",
                Location = new Point(20, 120),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            txtWumaConfigDirectory = new TextBox()
            {
                Location = new Point(110, 120),
                Size = new Size(200, 25),
                Font = new Font("微软雅黑", 9)
            };
            txtWumaConfigDirectory.Text = "C:\\WumaConfigs";

            var btnSelectWumaConfigDir = new Button()
            {
                Text = "选择",
                Location = new Point(320, 120),
                Size = new Size(50, 25),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 8)
            };
            btnSelectWumaConfigDir.Click += BtnSelectWumaConfigDir_Click;

            // 五码配置目录变更时自动刷新五码文件列表
            txtWumaConfigDirectory.TextChanged += (s, e) => LoadWumaConfigFiles();

            // 默认五码配置文件
            var lblDefaultWuma = new Label()
            {
                Text = "默认配置:",
                Location = new Point(20, 160),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            cmbDefaultWuma = new ComboBox()
            {
                Location = new Point(110, 160),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            
            // 动态加载默认五码配置
            LoadDefaultWumaConfigs();

            // 配置说明区域
            var lblConfigInfo = new Label()
            {
                Text = "五码配置包含：序列号、主板UUID、系统UUID、硬件地址、产品型号",
                Location = new Point(20, 200),
                Size = new Size(450, 40),
                Font = new Font("微软雅黑", 9),
                ForeColor = Color.DarkBlue
            };

            tabPage.Controls.AddRange(new Control[] {
                lblWumaFile, cmbWumaFile, lblWumaAvailableCount,
                btnImportWuma,
                lblWumaConfigDirectory, txtWumaConfigDirectory, btnSelectWumaConfigDir,
                lblDefaultWuma, cmbDefaultWuma,
                lblConfigInfo
            });
        }

        private void InitializeAppleIdConfigTab(TabPage tabPage)
        {
            tabPage.BackColor = Color.White;

            // AppleID文件
            var lblAppleIdFile = new Label()
            {
                Text = "AppleID文件:",
                Location = new Point(20, 30),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            cmbAppleIdFile = new ComboBox()
            {
                Location = new Point(110, 30),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            
            // 动态加载AppleID配置文件
            LoadAppleIdConfigFiles();
            
            // AppleID文件选择事件
            cmbAppleIdFile.SelectedIndexChanged += (s, e) => UpdateAppleIdFileLineCount();

            // 可用数量说明
            lblAppleIdAvailableCount = new Label()
            {
                Text = "可用AppleID数量: 0",
                Location = new Point(320, 32),
                Size = new Size(180, 20),
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

            // AppleID配置目录
            var lblAppleIdConfigDirectory = new Label()
            {
                Text = "配置目录:",
                Location = new Point(20, 120),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            txtAppleIdConfigDirectory = new TextBox()
            {
                Location = new Point(110, 120),
                Size = new Size(250, 25),
                Font = new Font("微软雅黑", 9)
            };
            txtAppleIdConfigDirectory.Text = "C:\\AppleIdConfigs";

            var btnSelectAppleIdConfigDir = new Button()
            {
                Text = "选择",
                Location = new Point(370, 120),
                Size = new Size(50, 25),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 8)
            };
            btnSelectAppleIdConfigDir.Click += BtnSelectAppleIdConfigDir_Click;

            // AppleID配置目录变更时自动刷新文件列表
            txtAppleIdConfigDirectory.TextChanged += (s, e) => LoadAppleIdConfigFiles();

            // 默认AppleID配置文件
            var lblDefaultAppleId = new Label()
            {
                Text = "默认配置:",
                Location = new Point(20, 160),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            cmbDefaultAppleId = new ComboBox()
            {
                Location = new Point(110, 160),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            
            // 动态加载默认AppleID配置
            LoadDefaultAppleIdConfigs();

            // 配置说明区域
            var lblConfigInfo = new Label()
            {
                Text = "AppleID配置包含：账号、密码、安全问题答案",
                Location = new Point(20, 200),
                Size = new Size(450, 40),
                Font = new Font("微软雅黑", 9),
                ForeColor = Color.DarkBlue
            };

            tabPage.Controls.AddRange(new Control[] {
                lblAppleIdFile, cmbAppleIdFile, lblAppleIdAvailableCount,
                btnImportAppleId,
                lblAppleIdConfigDirectory, txtAppleIdConfigDirectory, btnSelectAppleIdConfigDir,
                lblDefaultAppleId, cmbDefaultAppleId,
                lblConfigInfo
            });
        }

        private void InitializeClientManageTab(TabPage tabPage)
        {
            tabPage.BackColor = Color.White;

            // API调用地址
            var lblApiUrl = new Label()
            {
                Text = "API调用地址:",
                Location = new Point(20, 30),
                Size = new Size(100, 20),
                Font = new Font("微软雅黑", 9)
            };

            txtApiUrl = new TextBox()
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

            tabPage.Controls.AddRange(new Control[] {
                lblApiUrl, txtApiUrl,
                lblClientVersion, lblVersionValue,
                lblClientStatus, lblStatusValue,
                btnConnect, btnDisconnect,
                btnTestConnection
            });
        }

        private void InitializeEmailManageTab(TabPage tabPage)
        {
            tabPage.BackColor = Color.White;

            // 发信模板配置
            var lblEmailTemplate = new Label()
            {
                Text = "发信模板:",
                Location = new Point(20, 30),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            cmbEmailTemplate = new ComboBox()
            {
                Location = new Point(110, 30),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbEmailTemplate.Items.AddRange(new object[] { "模版1", "模版2", "模版三" });
            cmbEmailTemplate.SelectedIndex = 0;

            // 发信间隔配置
            var lblEmailInterval = new Label()
            {
                Text = "发信间隔(s):",
                Location = new Point(20, 80),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            cmbEmailInterval = new ComboBox()
            {
                Location = new Point(110, 80),
                Size = new Size(100, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            for (int i = 1; i <= 10; i++)
            {
                cmbEmailInterval.Items.Add(i.ToString());
            }
            cmbEmailInterval.SelectedIndex = 0;

            // 号码模板文件配置
            var lblNumberTemplateFile = new Label()
            {
                Text = "号码模板文件:",
                Location = new Point(20, 130),
                Size = new Size(80, 20),
                Font = new Font("微软雅黑", 9)
            };

            cmbNumberTemplateFile = new ComboBox()
            {
                Location = new Point(110, 130),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbNumberTemplateFile.Items.AddRange(new object[] { "numbers.txt", "phone_numbers.csv", "contacts.json" });
            cmbNumberTemplateFile.SelectedIndex = 0;

            // 号码数量显示
            lblNumberCount = new Label()
            {
                Text = "号码数量: 0",
                Location = new Point(270, 130),
                Size = new Size(100, 20),
                Font = new Font("微软雅黑", 9),
                ForeColor = Color.Gray
            };

            // 配置说明区域
            var lblConfigInfo = new Label()
            {
                Text = "发信管理配置包含邮件模板选择、发送间隔设置和号码模板管理",
                Location = new Point(20, 180),
                Size = new Size(400, 40),
                Font = new Font("微软雅黑", 9),
                ForeColor = Color.DarkBlue
            };

            // 模板管理按钮
            var btnManageTemplates = new Button()
            {
                Text = "管理模板",
                Location = new Point(280, 30),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnManageTemplates.Click += BtnManageTemplates_Click;

            tabPage.Controls.AddRange(new Control[] {
                lblEmailTemplate, cmbEmailTemplate,
                lblEmailInterval, cmbEmailInterval,
                lblNumberTemplateFile, cmbNumberTemplateFile, lblNumberCount,
                lblConfigInfo,
                btnManageTemplates
            });
        }

        // 事件处理方法
        private void BtnImportWuma_Click(object sender, EventArgs e)
        {
            MessageBox.Show("导入五码文件功能", "导入", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnImportAppleId_Click(object sender, EventArgs e)
        {
            MessageBox.Show("导入AppleID文件功能", "导入", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void BtnManageTemplates_Click(object sender, EventArgs e)
        {
            MessageBox.Show("发信模板管理功能", "模板管理", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InitializeConfigManageTab(TabPage tabPage)
        {
            tabPage.BackColor = Color.White;

            // 配置文件信息显示
            var lblConfigInfo = new Label()
            {
                Text = "配置文件信息:",
                Location = new Point(20, 30),
                Size = new Size(100, 20),
                Font = new Font("微软雅黑", 9, FontStyle.Bold)
            };

            var txtConfigInfo = new TextBox()
            {
                Location = new Point(20, 60),
                Size = new Size(430, 80), // 调整高度为80，变小一些
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(240, 240, 240),
                Font = new Font("微软雅黑", 8)
            };

            // 刷新配置信息按钮
            var btnRefreshInfo = new Button()
            {
                Text = "刷新信息",
                Location = new Point(20, 150),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnRefreshInfo.Click += (s, e) => RefreshConfigInfo(txtConfigInfo);

            // 备份配置文件按钮
            var btnBackupConfig = new Button()
            {
                Text = "备份配置",
                Location = new Point(120, 150),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnBackupConfig.Click += BtnBackupConfig_Click;

            // 恢复默认配置按钮
            var btnRestoreDefault = new Button()
            {
                Text = "恢复默认",
                Location = new Point(220, 150),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnRestoreDefault.Click += BtnRestoreDefault_Click;

            // 打开配置文件目录按钮
            var btnOpenConfigDir = new Button()
            {
                Text = "打开目录",
                Location = new Point(320, 150),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 9)
            };
            btnOpenConfigDir.Click += BtnOpenConfigDir_Click;

            // 虚拟机软件目录配置
            var lblVMDirectory = new Label()
            {
                Text = "虚拟机软件目录:",
                Location = new Point(20, 200),
                Size = new Size(100, 20),
                Font = new Font("微软雅黑", 9, FontStyle.Bold)
            };

            txtVMDirectory = new TextBox()
            {
                Location = new Point(130, 200),
                Size = new Size(300, 25), // 增大宽度为300，显示更多内容
                Font = new Font("微软雅黑", 9)
            };

            var btnSelectVMDir = new Button()
            {
                Text = "选择",
                Location = new Point(440, 200),
                Size = new Size(50, 25),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 8)
            };
            btnSelectVMDir.Click += BtnSelectVMDir_Click;

            // 初始化显示配置信息
            RefreshConfigInfo(txtConfigInfo);

            tabPage.Controls.AddRange(new Control[] {
                lblConfigInfo, txtConfigInfo,
                btnRefreshInfo, btnBackupConfig, btnRestoreDefault, btnOpenConfigDir,
                lblVMDirectory, txtVMDirectory, btnSelectVMDir
            });
        }

        private void RefreshConfigInfo(TextBox txtConfigInfo)
        {
            try
            {
                txtConfigInfo.Text = ConfigManager.GetConfigFileInfo();
            }
            catch (Exception ex)
            {
                txtConfigInfo.Text = $"获取配置信息失败: {ex.Message}";
            }
        }

        private void LoadMotherDiskConfigs()
        {
            try
            {
                cmbMotherDisk.Items.Clear();
                
                string motherDiskDirectory = txtMotherDiskDirectory.Text;
                if (string.IsNullOrEmpty(motherDiskDirectory) || !Directory.Exists(motherDiskDirectory))
                {
                    cmbMotherDisk.Items.Add("请选择有效的母盘目录");
                    cmbMotherDisk.SelectedIndex = 0;
                    return;
                }
                
                // 获取母盘目录下所有包含.vmx文件的文件夹
                var motherDiskFolders = Directory.GetDirectories(motherDiskDirectory)
                    .Where(dir => Directory.GetFiles(dir, "*.vmx", SearchOption.TopDirectoryOnly).Any())
                    .Select(dir => Path.GetFileName(dir))
                    .ToList();
                
                if (motherDiskFolders.Any())
                {
                    cmbMotherDisk.Items.AddRange(motherDiskFolders.ToArray());
                    cmbMotherDisk.SelectedIndex = 0;
                }
                else
                {
                    cmbMotherDisk.Items.Add("未找到包含.vmx文件的母盘");
                    cmbMotherDisk.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                cmbMotherDisk.Items.Clear();
                cmbMotherDisk.Items.Add($"加载母盘配置失败: {ex.Message}");
                cmbMotherDisk.SelectedIndex = 0;
            }
        }

        private void LoadWumaConfigFiles()
        {
            try
            {
                cmbWumaFile.Items.Clear();
                
                // 从配置目录获取五码配置文件
                string wumaConfigDirectory = txtWumaConfigDirectory?.Text ?? "C:\\WumaConfigs";
                
                if (!string.IsNullOrEmpty(wumaConfigDirectory) && Directory.Exists(wumaConfigDirectory))
                {
                    // 获取所有.txt文件
                    var wumaFiles = Directory.GetFiles(wumaConfigDirectory, "*.txt")
                        .Select(file => Path.GetFileNameWithoutExtension(file))
                        .ToList();
                    
                    if (wumaFiles.Any())
                    {
                        cmbWumaFile.Items.AddRange(wumaFiles.ToArray());
                        cmbWumaFile.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbWumaFile.Items.Add("未找到五码配置文件");
                        cmbWumaFile.SelectedIndex = 0;
                    }
                }
                else
                {
                    cmbWumaFile.Items.Add("五码配置目录不存在或无效");
                    cmbWumaFile.SelectedIndex = 0;
                }
                
                // 加载完成后刷新默认配置下拉框
                LoadDefaultWumaConfigs();
            }
            catch (Exception ex)
            {
                cmbWumaFile.Items.Clear();
                cmbWumaFile.Items.Add($"加载五码配置失败: {ex.Message}");
                cmbWumaFile.SelectedIndex = 0;
            }
        }

        private void LoadDefaultWumaConfigs()
        {
            try
            {
                cmbDefaultWuma.Items.Clear();
                
                // 从五码文件下拉框中获取可用的配置
                if (cmbWumaFile.Items.Count > 0 && !cmbWumaFile.Items[0].ToString().Contains("未找到") && !cmbWumaFile.Items[0].ToString().Contains("失败"))
                {
                    // 将ComboBox.Items转换为object数组
                    var itemsArray = new object[cmbWumaFile.Items.Count];
                    cmbWumaFile.Items.CopyTo(itemsArray, 0);
                    cmbDefaultWuma.Items.AddRange(itemsArray);
                    cmbDefaultWuma.SelectedIndex = 0;
                }
                else
                {
                    cmbDefaultWuma.Items.Add("请先配置五码文件");
                    cmbDefaultWuma.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                cmbDefaultWuma.Items.Clear();
                cmbDefaultWuma.Items.Add($"加载默认配置失败: {ex.Message}");
                cmbDefaultWuma.SelectedIndex = 0;
            }
        }

        private void BtnSelectWumaConfigDir_Click(object sender, EventArgs e)
        {
            try
            {
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "选择五码配置目录";
                    folderDialog.SelectedPath = txtWumaConfigDirectory?.Text ?? "C:\\WumaConfigs";
                    
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 自动更新文本框并刷新文件列表
                        if (txtWumaConfigDirectory != null)
                        {
                            txtWumaConfigDirectory.Text = folderDialog.SelectedPath;
                            LoadWumaConfigFiles();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"选择目录失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSelectAppleIdConfigDir_Click(object sender, EventArgs e)
        {
            try
            {
                using (var folderDialog = new FolderBrowserDialog())
                {
                    folderDialog.Description = "选择AppleID配置目录";
                    folderDialog.SelectedPath = txtAppleIdConfigDirectory?.Text ?? "C:\\AppleIdConfigs";
                    
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 自动更新文本框并刷新文件列表
                        if (txtAppleIdConfigDirectory != null)
                        {
                            txtAppleIdConfigDirectory.Text = folderDialog.SelectedPath;
                            LoadAppleIdConfigFiles();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"选择目录失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAppleIdConfigFiles()
        {
            try
            {
                cmbAppleIdFile.Items.Clear();
                
                // 从配置目录获取AppleID配置文件
                string appleIdConfigDirectory = txtAppleIdConfigDirectory?.Text ?? "C:\\AppleIdConfigs";
                
                if (!string.IsNullOrEmpty(appleIdConfigDirectory) && Directory.Exists(appleIdConfigDirectory))
                {
                    // 获取所有.txt文件（AppleID配置使用txt格式）
                    var appleIdFiles = Directory.GetFiles(appleIdConfigDirectory, "*.txt")
                        .Select(file => Path.GetFileNameWithoutExtension(file))
                        .ToList();
                    
                    if (appleIdFiles.Any())
                    {
                        cmbAppleIdFile.Items.AddRange(appleIdFiles.ToArray());
                        cmbAppleIdFile.SelectedIndex = 0;
                    }
                    else
                    {
                        cmbAppleIdFile.Items.Add("未找到AppleID配置文件");
                        cmbAppleIdFile.SelectedIndex = 0;
                    }
                }
                else
                {
                    cmbAppleIdFile.Items.Add("AppleID配置目录不存在或无效");
                    cmbAppleIdFile.SelectedIndex = 0;
                }
                
                // 加载完成后刷新默认配置下拉框
                LoadDefaultAppleIdConfigs();
            }
            catch (Exception ex)
            {
                cmbAppleIdFile.Items.Clear();
                cmbAppleIdFile.Items.Add($"加载AppleID配置失败: {ex.Message}");
                cmbAppleIdFile.SelectedIndex = 0;
            }
        }

        private void LoadDefaultAppleIdConfigs()
        {
            try
            {
                cmbDefaultAppleId.Items.Clear();
                
                // 从AppleID文件下拉框中获取可用的配置
                if (cmbAppleIdFile.Items.Count > 0 && !cmbAppleIdFile.Items[0].ToString().Contains("未找到") && !cmbAppleIdFile.Items[0].ToString().Contains("失败"))
                {
                    // 将ComboBox.Items转换为object数组
                    var itemsArray = new object[cmbAppleIdFile.Items.Count];
                    cmbAppleIdFile.Items.CopyTo(itemsArray, 0);
                    cmbDefaultAppleId.Items.AddRange(itemsArray);
                    cmbDefaultAppleId.SelectedIndex = 0;
                }
                else
                {
                    cmbDefaultAppleId.Items.Add("请先配置AppleID文件");
                    cmbDefaultAppleId.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                cmbDefaultAppleId.Items.Clear();
                cmbDefaultAppleId.Items.Add($"加载默认配置失败: {ex.Message}");
                cmbDefaultAppleId.SelectedIndex = 0;
            }
        }

        private void UpdateWumaFileLineCount()
        {
            try
            {
                if (cmbWumaFile.SelectedItem == null || cmbWumaFile.SelectedItem.ToString().Contains("未找到") || cmbWumaFile.SelectedItem.ToString().Contains("失败"))
                {
                    lblWumaAvailableCount.Text = "可用数量: 0";
                    return;
                }
                
                string wumaConfigDirectory = txtWumaConfigDirectory?.Text ?? "C:\\WumaConfigs";
                string fileName = cmbWumaFile.SelectedItem.ToString() + ".txt";
                string filePath = Path.Combine(wumaConfigDirectory, fileName);
                
                if (File.Exists(filePath))
                {
                    int lineCount = File.ReadAllLines(filePath).Length;
                    lblWumaAvailableCount.Text = $"可用数量: {lineCount}";
                }
                else
                {
                    lblWumaAvailableCount.Text = "可用数量: 0";
                }
            }
            catch (Exception ex)
            {
                lblWumaAvailableCount.Text = "可用数量: 0";
            }
        }

        private void UpdateAppleIdFileLineCount()
        {
            try
            {
                if (cmbAppleIdFile.SelectedItem == null || cmbAppleIdFile.SelectedItem.ToString().Contains("未找到") || cmbAppleIdFile.SelectedItem.ToString().Contains("失败"))
                {
                    lblAppleIdAvailableCount.Text = "可用AppleID数量: 0";
                    return;
                }
                
                string appleIdConfigDirectory = txtAppleIdConfigDirectory?.Text ?? "C:\\AppleIdConfigs";
                string fileName = cmbAppleIdFile.SelectedItem.ToString() + ".txt";
                string filePath = Path.Combine(appleIdConfigDirectory, fileName);
                
                if (File.Exists(filePath))
                {
                    int lineCount = File.ReadAllLines(filePath).Length;
                    lblAppleIdAvailableCount.Text = $"可用AppleID数量: {lineCount}";
                }
                else
                {
                    lblAppleIdAvailableCount.Text = "可用AppleID数量: 0";
                }
            }
            catch (Exception ex)
            {
                lblAppleIdAvailableCount.Text = "可用AppleID数量: 0";
            }
        }

        private void BtnBackupConfig_Click(object sender, EventArgs e)
        {
            try
            {
                string configPath = ConfigManager.GetConfigFilePath();
                if (File.Exists(configPath))
                {
                    string backupPath = configPath + "." + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".backup";
                    File.Copy(configPath, backupPath, false);
                    MessageBox.Show($"配置文件已备份到:\n{backupPath}", "备份成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("配置文件不存在，无法备份", "备份失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"备份配置文件失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRestoreDefault_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("确定要恢复默认配置吗？当前配置将被覆盖。", "确认恢复", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                try
                {
                    var defaultConfig = new AppConfig();
                    ConfigManager.SaveConfig(defaultConfig);
                    
                    // 重新加载配置到界面
                    LoadConfig();
                    
                    MessageBox.Show("默认配置已恢复并保存", "恢复成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"恢复默认配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnOpenConfigDir_Click(object sender, EventArgs e)
        {
            try
            {
                string configPath = ConfigManager.GetConfigFilePath();
                string configDir = Path.GetDirectoryName(configPath);
                
                if (Directory.Exists(configDir))
                {
                    System.Diagnostics.Process.Start("explorer.exe", configDir);
                }
                else
                {
                    MessageBox.Show("配置文件目录不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开配置文件目录失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSelectMotherDiskDir_Click(object sender, EventArgs e)
        {
            SelectDirectory("选择母盘目录", txtMotherDiskDirectory);
        }

        private void BtnSelectCloneVMDir_Click(object sender, EventArgs e)
        {
            SelectDirectory("选择克隆虚拟机目录", txtCloneVMDirectory);
        }

        private void SelectDirectory(string description, TextBox targetTextBox)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = description;
                folderDialog.SelectedPath = targetTextBox.Text;
                
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    targetTextBox.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void BtnSelectVMDir_Click(object sender, EventArgs e)
        {
            SelectDirectory("选择虚拟机软件目录", txtVMDirectory);
        }

        private void AddResultToMainForm(string message)
        {
            // 如果主窗体引用存在，向主窗体输出日志
            if (mainForm != null)
            {
                // 使用反射调用主窗体的AddResult方法
                var method = mainForm.GetType().GetMethod("AddResult");
                if (method != null)
                {
                    method.Invoke(mainForm, new object[] { message });
                }
            }
            else
            {
                // 如果没有主窗体引用，直接输出到控制台
                Console.WriteLine(message);
            }
        }

        private void LoadConfig()
        {
            try
            {
                var config = ConfigManager.LoadConfig();
                
                // 克隆配置
                cmbMotherDisk.SelectedItem = config.MotherDisk;
                cmbCloneCount.SelectedItem = config.CloneCount.ToString();
                cmbWumaConfig.SelectedItem = config.WumaConfig;
                txtNamingPattern.Text = config.NamingPattern;
                txtMotherDiskDirectory.Text = config.MotherDiskDirectory;
                txtCloneVMDirectory.Text = config.CloneVMDirectory;
                
                // 五码配置
                cmbWumaFile.SelectedItem = config.WumaFile;
                cmbDefaultWuma.SelectedItem = config.DefaultWuma;
                txtWumaConfigDirectory.Text = config.WumaConfigDirectory;
                
                // AppleID配置
                cmbAppleIdFile.SelectedItem = config.AppleIdFile;
                cmbDefaultAppleId.SelectedItem = config.DefaultAppleId;
                txtAppleIdConfigDirectory.Text = config.AppleIdConfigDirectory;
                
                // 客户端管理
                txtApiUrl.Text = config.ApiUrl;
                
                // 发信管理
                cmbEmailTemplate.SelectedItem = config.EmailTemplate;
                cmbEmailInterval.SelectedItem = config.EmailInterval.ToString();
                cmbNumberTemplateFile.SelectedItem = config.NumberTemplateFile;
                lblNumberCount.Text = $"号码数量: {config.NumberCount}";
                
                // 虚拟机软件配置
                txtVMDirectory.Text = config.VMDirectory;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private AppConfig GetCurrentConfig()
        {
            return new AppConfig
            {
                // 克隆配置
                MotherDisk = cmbMotherDisk.SelectedItem?.ToString() ?? "mupan1",
                CloneCount = int.TryParse(cmbCloneCount.SelectedItem?.ToString(), out int cloneCount) ? cloneCount : 1,
                WumaConfig = cmbWumaConfig.SelectedItem?.ToString() ?? "14.1",
                NamingPattern = txtNamingPattern.Text,
                MotherDiskDirectory = txtMotherDiskDirectory.Text,
                CloneVMDirectory = txtCloneVMDirectory.Text,
                
                // 五码配置
                WumaFile = cmbWumaFile.SelectedItem?.ToString() ?? "14.1",
                DefaultWuma = cmbDefaultWuma.SelectedItem?.ToString() ?? "14.1",
                WumaConfigDirectory = txtWumaConfigDirectory.Text,
                
                // AppleID配置
                AppleIdFile = cmbAppleIdFile.SelectedItem?.ToString() ?? "2026id",
                DefaultAppleId = cmbDefaultAppleId.SelectedItem?.ToString() ?? "2026id",
                AppleIdConfigDirectory = txtAppleIdConfigDirectory.Text,
                
                // 客户端管理
                ApiUrl = txtApiUrl.Text,
                
                // 发信管理
                EmailTemplate = cmbEmailTemplate.SelectedItem?.ToString() ?? "macOS克隆完成通知",
                EmailInterval = int.TryParse(cmbEmailInterval.SelectedItem?.ToString(), out int interval) ? interval : 3,
                NumberTemplateFile = cmbNumberTemplateFile.SelectedItem?.ToString() ?? "numbers.txt",
                NumberCount = int.TryParse(lblNumberCount.Text.Replace("号码数量: ", ""), out int count) ? count : 0,
                
                // 虚拟机软件配置
                VMDirectory = txtVMDirectory.Text
            };
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                var config = GetCurrentConfig();
                string configPath = ConfigManager.GetConfigFilePath();
                ConfigManager.SaveConfig(config);
                
                // 在日志输出框中显示保存路径信息
                string logMessage = $"[{DateTime.Now:HH:mm:ss}] 偏好设置已保存到XML文件: {configPath}";
                AddResultToMainForm(logMessage);
                
                MessageBox.Show($"偏好设置已保存到XML文件！\n\n保存路径:\n{configPath}", "保存成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}