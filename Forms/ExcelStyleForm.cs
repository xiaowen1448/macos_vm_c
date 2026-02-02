using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using VMCloneApp.Models;

namespace VMCloneApp.Forms
{
    public partial class ExcelStyleForm : Form
    {
        private List<VirtualMachine> virtualMachines;
        private List<CloneTask> cloneTasks;
        private System.Windows.Forms.Timer refreshTimer;
        private TextBox txtResult;
        private Label lblTotalVMs;
        private Label lblRunningVMs;
        private Label lblStoppedVMs;
        private Label lblSuspendedVMs;
        private Label lblStatus;  // 状态显示标签
        private NotifyIcon trayIcon;  // 系统托盘图标
        private ContextMenuStrip trayMenu;  // 托盘右键菜单

        public ExcelStyleForm()
        {
            // 设置窗体基本属性
            this.Text = "虚拟机管理系统";
            this.Size = new Size(900, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(900, 750);
            this.BackColor = Color.White;
            
            // 设置窗体图标
            SetFormIcon();
            
            // 初始化菜单
            InitializeMenu();
            
            // 初始化数据
            InitializeData();
            
            // 初始化界面控件
            InitializeControls();
            
            // 初始化定时器
            InitializeTimer();
            
            // 初始化托盘图标
            InitializeTrayIcon();
            
            // 绑定窗体关闭事件
            this.FormClosing += ExcelStyleForm_FormClosing;
            
            // 窗体显示完成后立即调整布局，确保日志区域大小正确
            this.Shown += (s, e) => 
            {
                // 延迟一小段时间确保窗体完全稳定
                System.Threading.Thread.Sleep(100);
                MainForm_Resize(this, EventArgs.Empty);
            };
        }

        private void InitializeMenu()
        {
            // 创建主菜单栏
            var mainMenu = new MenuStrip();
            mainMenu.BackColor = Color.FromArgb(240, 240, 240);
            mainMenu.Renderer = new ToolStripProfessionalRenderer(new CustomColorTable());
            
            // 文件菜单
            var fileMenu = new ToolStripMenuItem("文件(&F)");
            var newProjectItem = new ToolStripMenuItem("新建项目", null, FileNew_Click);
            var openProjectItem = new ToolStripMenuItem("打开项目", null, FileOpen_Click);
            var saveProjectItem = new ToolStripMenuItem("保存项目", null, FileSave_Click);
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { newProjectItem, openProjectItem, saveProjectItem, new ToolStripSeparator() });
            
            var exportItem = new ToolStripMenuItem("导出配置", null, FileExport_Click);
            var importItem = new ToolStripMenuItem("导入配置", null, FileImport_Click);
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { exportItem, importItem, new ToolStripSeparator() });
            
            var exitItem = new ToolStripMenuItem("退出", null, FileExit_Click);
            fileMenu.DropDownItems.Add(exitItem);
            
            // 编辑菜单
            var editMenu = new ToolStripMenuItem("编辑(&E)");
            var undoItem = new ToolStripMenuItem("撤销", null, EditUndo_Click);
            var redoItem = new ToolStripMenuItem("重做", null, EditRedo_Click);
            editMenu.DropDownItems.AddRange(new ToolStripItem[] { undoItem, redoItem, new ToolStripSeparator() });
            
            var copyItem = new ToolStripMenuItem("复制", null, EditCopy_Click);
            var pasteItem = new ToolStripMenuItem("粘贴", null, EditPaste_Click);
            var selectAllItem = new ToolStripMenuItem("全选", null, EditSelectAll_Click);
            editMenu.DropDownItems.AddRange(new ToolStripItem[] { copyItem, pasteItem, selectAllItem });
            
            // 显示菜单
            var viewMenu = new ToolStripMenuItem("显示(&V)");
            var refreshItem = new ToolStripMenuItem("刷新显示", null, ViewRefresh_Click);
            var showDetailsItem = new ToolStripMenuItem("显示详细信息", null, ViewShowDetails_Click);
            var hideDetailsItem = new ToolStripMenuItem("隐藏详细信息", null, ViewHideDetails_Click);
            viewMenu.DropDownItems.AddRange(new ToolStripItem[] { refreshItem, new ToolStripSeparator(), showDetailsItem, hideDetailsItem });
            
            // 控制菜单
            var controlMenu = new ToolStripMenuItem("控制(&C)");
            var startAllItem = new ToolStripMenuItem("启动所有虚拟机", null, ControlStartAll_Click);
            var stopAllItem = new ToolStripMenuItem("停止所有虚拟机", null, ControlStopAll_Click);
            var suspendAllItem = new ToolStripMenuItem("挂起所有虚拟机", null, ControlSuspendAll_Click);
            controlMenu.DropDownItems.AddRange(new ToolStripItem[] { startAllItem, stopAllItem, suspendAllItem, new ToolStripSeparator() });
            
            var cloneWizardItem = new ToolStripMenuItem("克隆向导", null, ControlCloneWizard_Click);
            var emailWizardItem = new ToolStripMenuItem("邮件发送向导", null, ControlEmailWizard_Click);
            controlMenu.DropDownItems.AddRange(new ToolStripItem[] { cloneWizardItem, emailWizardItem });
            
            // 设置菜单
            var settingsMenu = new ToolStripMenuItem("设置(&S)");
            var vmConfigItem = new ToolStripMenuItem("虚拟机配置", null, SettingsVMConfig_Click);
            var preferencesItem = new ToolStripMenuItem("偏好设置", null, SettingsPreferences_Click);
            settingsMenu.DropDownItems.AddRange(new ToolStripItem[] { vmConfigItem, preferencesItem });
            
            // 帮助菜单
            var helpMenu = new ToolStripMenuItem("帮助(&H)");
            var helpItem = new ToolStripMenuItem("帮助文档", null, HelpDocument_Click);
            var authorItem = new ToolStripMenuItem("作者", null, HelpAuthor_Click);
            var aboutItem = new ToolStripMenuItem("关于", null, HelpAbout_Click);
            helpMenu.DropDownItems.AddRange(new ToolStripItem[] { helpItem, authorItem, aboutItem });
            
            // 添加所有菜单到主菜单栏
            mainMenu.Items.AddRange(new ToolStripItem[] { fileMenu, editMenu, viewMenu, controlMenu, settingsMenu, helpMenu });
            
            // 设置菜单栏
            this.MainMenuStrip = mainMenu;
            this.Controls.Add(mainMenu);
        }

        private void InitializeData()
        {
            virtualMachines = new List<VirtualMachine>
            {
                new VirtualMachine { 
                    Id = "VM001", 
                    Name = "macOS-Dev", 
                    Status = "Running", 
                    OS = "macOS 14", 
                    MemoryMB = 8192, 
                    CPUCores = 4 
                },
                new VirtualMachine { 
                    Id = "VM002", 
                    Name = "macOS-Test", 
                    Status = "Stopped", 
                    OS = "macOS 13", 
                    MemoryMB = 4096, 
                    CPUCores = 2 
                },
                new VirtualMachine { 
                    Id = "VM003", 
                    Name = "macOS-Prod", 
                    Status = "Running", 
                    OS = "macOS 14", 
                    MemoryMB = 16384, 
                    CPUCores = 8 
                },
                new VirtualMachine { 
                    Id = "VM004", 
                    Name = "macOS-Backup", 
                    Status = "Suspended", 
                    OS = "macOS 12", 
                    MemoryMB = 2048, 
                    CPUCores = 2 
                },
                new VirtualMachine { 
                    Id = "VM005", 
                    Name = "Windows-Server", 
                    Status = "Running", 
                    OS = "Windows Server", 
                    MemoryMB = 4096, 
                    CPUCores = 4 
                },
                new VirtualMachine { 
                    Id = "VM006", 
                    Name = "Linux-Dev", 
                    Status = "Stopped", 
                    OS = "Ubuntu", 
                    MemoryMB = 2048, 
                    CPUCores = 2 
                },
                new VirtualMachine { 
                    Id = "VM007", 
                    Name = "macOS-Staging", 
                    Status = "Running", 
                    OS = "macOS 13", 
                    MemoryMB = 4096, 
                    CPUCores = 4 
                },
                new VirtualMachine { 
                    Id = "VM008", 
                    Name = "Windows-Client", 
                    Status = "Stopped", 
                    OS = "Windows 10", 
                    MemoryMB = 2048, 
                    CPUCores = 2 
                },
                new VirtualMachine { 
                    Id = "VM009", 
                    Name = "Linux-Prod", 
                    Status = "Running", 
                    OS = "CentOS", 
                    MemoryMB = 8192, 
                    CPUCores = 4 
                },
                new VirtualMachine { 
                    Id = "VM010", 
                    Name = "macOS-Archive", 
                    Status = "Suspended", 
                    OS = "macOS 11", 
                    MemoryMB = 1024, 
                    CPUCores = 1 
                }
            };

            cloneTasks = new List<CloneTask>();
        }

        private void InitializeControls()
        {
            this.SuspendLayout();
            
            // 虚拟机统计区域
            var lblStats = new Label()
            {
                Text = "虚拟机状态统计:",
                Location = new Point(20, 50),
                Size = new Size(120, 20),
                Font = new Font("微软雅黑", 9, FontStyle.Bold)
            };

            // 统计信息分组框
            var grpStats = new GroupBox()
            {
                Text = "统计信息",
                Location = new Point(20, 80),
                Size = new Size(860, 100)
            };

            // 总虚拟机数
            var lblTotal = new Label()
            {
                Text = "总虚拟机数:",
                Location = new Point(20, 25),
                Size = new Size(80, 20)
            };

            lblTotalVMs = new Label()
            {
                Text = "0",
                Location = new Point(100, 25),
                Size = new Size(40, 20),
                Font = new Font("微软雅黑", 10, FontStyle.Bold),
                ForeColor = Color.Blue
            };

            // 运行中
            var lblRunning = new Label()
            {
                Text = "运行中:",
                Location = new Point(180, 25),
                Size = new Size(60, 20)
            };

            lblRunningVMs = new Label()
            {
                Text = "8",
                Location = new Point(240, 25),
                Size = new Size(40, 20),
                Font = new Font("微软雅黑", 10, FontStyle.Bold),
                ForeColor = Color.Green
            };

            // 已停止
            var lblStopped = new Label()
            {
                Text = "已停止:",
                Location = new Point(320, 25),
                Size = new Size(60, 20)
            };

            lblStoppedVMs = new Label()
            {
                Text = "10",
                Location = new Point(380, 25),
                Size = new Size(40, 20),
                Font = new Font("微软雅黑", 10, FontStyle.Bold),
                ForeColor = Color.Red
            };

            // 挂起
            var lblSuspended = new Label()
            {
                Text = "挂起:",
                Location = new Point(460, 25),
                Size = new Size(40, 20)
            };

            lblSuspendedVMs = new Label()
            {
                Text = "2",
                Location = new Point(500, 25),
                Size = new Size(40, 20),
                Font = new Font("微软雅黑", 10, FontStyle.Bold),
                ForeColor = Color.Orange
            };

            // 状态显示
            var lblStatusTitle = new Label()
            {
                Text = "当前状态:",
                Location = new Point(20, 60),
                Size = new Size(60, 20),
                Font = new Font("微软雅黑", 9, FontStyle.Bold)
            };

            lblStatus = new Label()
            {
                Text = "准备中",
                Location = new Point(80, 60),
                Size = new Size(200, 20),
                Font = new Font("微软雅黑", 10),
                ForeColor = Color.Blue
            };

            grpStats.Controls.AddRange(new Control[] { 
                lblTotal, lblTotalVMs, 
                lblRunning, lblRunningVMs, 
                lblStopped, lblStoppedVMs, 
                lblSuspended, lblSuspendedVMs,
                lblStatusTitle, lblStatus
            });

            // 操作按钮区域
            var grpActions = new GroupBox()
            {
                Text = "操作面板",
                Location = new Point(20, 200),
                Size = new Size(860, 90)
            };

            // 启动克隆按钮 - 标准Windows样式（加大版）
            var btnStartClone = new Button()
            {
                Text = "启动克隆",
                Location = new Point(20, 30),
                Size = new Size(120, 35),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 10)
            };
            btnStartClone.Click += BtnStartClone_Click;

            // 启动发信按钮 - 标准Windows样式（加大版）
            var btnStartEmail = new Button()
            {
                Text = "启动发信",
                Location = new Point(160, 30),
                Size = new Size(120, 35),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 10)
            };
            btnStartEmail.Click += BtnStartEmail_Click;



            // 发信记录按钮 - 标准Windows样式（加大版）
            var btnEmailRecord = new Button()
            {
                Text = "发信记录",
                Location = new Point(300, 30),
                Size = new Size(120, 35),
                FlatStyle = FlatStyle.Standard,
                UseVisualStyleBackColor = true,
                Font = new Font("微软雅黑", 10)
            };
            btnEmailRecord.Click += BtnEmailRecord_Click;

            grpActions.Controls.AddRange(new Control[] {
                btnStartClone, btnStartEmail, btnEmailRecord
            });

            // 结果显示区域 - iTunes风格
            var lblResult = new Label()
            {
                Text = "操作日志",
                Location = new Point(20, 310),
                Size = new Size(100, 25),
                Font = new Font("微软雅黑", 10),
                ForeColor = Color.FromArgb(64, 64, 64)
            };

            // 创建iTunes风格的日志输出框容器
            var logContainer = new Panel()
            {
                Location = new Point(20, 340),
                Size = new Size(860, 250), // 调整高度为250，变小一些
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };



            txtResult = new TextBox()
            {
                Location = new Point(5, 5),
                Size = new Size(850, 240), // 调整高度为240，变小一些
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(248, 248, 248), // iTunes浅灰色背景
                ForeColor = Color.FromArgb(64, 64, 64), // iTunes深灰色文字
                Font = new Font("Consolas", 9.5f)
            };

            // 将文本框添加到容器
            logContainer.Controls.Add(txtResult);
            
            // 添加到窗体
            this.Controls.AddRange(new Control[] {
                lblStats, grpStats, grpActions, lblResult, logContainer
            });

            this.ResumeLayout(false);
            
            // 注册窗体大小改变事件
            this.Resize += MainForm_Resize;
        }

        private void SetFormIcon()
        {
            try
            {
                // 尝试从当前目录加载图标
                string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.ico");
                if (File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                    return;
                }
                
                // 尝试从资源加载图标
                try
                {
                    // 使用AppContext.BaseDirectory替代Assembly.Location，避免单文件应用警告
                    string baseDirectory = AppContext.BaseDirectory;
                    string resourceIconPath = System.IO.Path.Combine(baseDirectory, "app.ico");
                    if (System.IO.File.Exists(resourceIconPath))
                    {
                        this.Icon = new Icon(resourceIconPath);
                    }
                    else
                    {
                        this.Icon = SystemIcons.Application;
                    }
                }
                catch
                {
                    // 如果都失败，使用默认图标
                    this.Icon = SystemIcons.Application;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"设置窗体图标时出错: {ex.Message}");
                this.Icon = SystemIcons.Application;
            }
        }

        private void InitializeTrayIcon()
        {
            // 创建托盘右键菜单
            trayMenu = new ContextMenuStrip();
            
            // 添加"打开"菜单项
            var openItem = new ToolStripMenuItem("打开");
            openItem.Click += TrayOpen_Click;
            trayMenu.Items.Add(openItem);
            
            // 添加分隔符
            trayMenu.Items.Add(new ToolStripSeparator());
            
            // 添加"退出"菜单项
            var exitItem = new ToolStripMenuItem("退出");
            exitItem.Click += TrayExit_Click;
            trayMenu.Items.Add(exitItem);
            
            // 创建托盘图标
            trayIcon = new NotifyIcon();
            trayIcon.Text = "虚拟机管理系统";
            trayIcon.Icon = this.Icon ?? SystemIcons.Application;
            trayIcon.ContextMenuStrip = trayMenu;
            trayIcon.Visible = true;
            
            // 绑定托盘图标单击事件
            trayIcon.MouseClick += TrayIcon_MouseClick;
            trayIcon.DoubleClick += TrayIcon_DoubleClick;
        }

        private void InitializeTimer()
        {
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 1000;
            refreshTimer.Tick += RefreshTimer_Tick;
            refreshTimer.Start();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            UpdateStats();
            UpdateTaskProgress();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // 调整分组框宽度与窗体一致
            var grpStats = this.Controls.OfType<GroupBox>().FirstOrDefault(g => g.Text == "统计信息");
            var grpActions = this.Controls.OfType<GroupBox>().FirstOrDefault(g => g.Text == "操作面板");
            var logContainer = this.Controls.OfType<Panel>().FirstOrDefault(p => p.BorderStyle == BorderStyle.FixedSingle);
            
            if (grpStats != null)
                grpStats.Width = this.ClientSize.Width - 40;
            
            if (grpActions != null)
                grpActions.Width = this.ClientSize.Width - 40;
            
            // 调整日志容器宽度和高度
            if (logContainer != null)
            {
                logContainer.Width = this.ClientSize.Width - 40;
                
                // 计算可用高度，精确匹配窗体底部
                int availableHeight = this.ClientSize.Height - logContainer.Top;
                int minHeight = 250; // 最小高度，与初始高度一致
                
                // 精确计算高度，确保底部没有多余留白
                // 使用更精确的计算，考虑窗体边框和标题栏
                int calculatedHeight = Math.Max(minHeight, availableHeight - 10);
                
                // 确保高度不会超过窗体可用空间
                if (calculatedHeight > this.ClientSize.Height - logContainer.Top - 10)
                {
                    calculatedHeight = this.ClientSize.Height - logContainer.Top - 10;
                }
                
                logContainer.Height = calculatedHeight;
                
                // 调整内部文本框大小
                txtResult.Width = logContainer.Width - 10;
                txtResult.Height = logContainer.Height - 10;
            }
        }

        private void UpdateStats()
        {
            var total = virtualMachines.Count;
            var running = virtualMachines.Count(vm => vm.Status == "Running");
            var stopped = virtualMachines.Count(vm => vm.Status == "Stopped");
            var suspended = virtualMachines.Count(vm => vm.Status == "Suspended");

            lblTotalVMs.Text = total.ToString();
            lblRunningVMs.Text = running.ToString();
            lblStoppedVMs.Text = stopped.ToString();
            lblSuspendedVMs.Text = suspended.ToString();
        }

        // 事件处理方法
        private void BtnStartClone_Click(object sender, EventArgs e)
        {
            UpdateStatus("克隆中");
            AddResult($"[{DateTime.Now:HH:mm:ss}] 启动虚拟机克隆任务...");
            
            var task = new CloneTask();
            cloneTasks.Add(task);
            AddResult($"[{DateTime.Now:HH:mm:ss}] 创建克隆任务: {task.TaskId}");
        }

        private void BtnStartEmail_Click(object sender, EventArgs e)
        {
            UpdateStatus("发信执行中");
            AddResult($"[{DateTime.Now:HH:mm:ss}] 启动邮件发送任务...");
            AddResult($"[{DateTime.Now:HH:mm:ss}] 邮件发送任务已创建");
        }



        private void BtnEmailRecord_Click(object sender, EventArgs e)
        {
            UpdateStatus("准备中");
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开发信记录");
        }

        private void UpdateTaskProgress()
        {
            foreach (var task in cloneTasks.Where(t => t.Status == "Running"))
            {
                task.Progress += 10;
                if (task.Progress >= 100)
                {
                    task.Progress = 100;
                    task.Status = "Completed";
                    task.EndTime = DateTime.Now;
                    AddResult($"[{DateTime.Now:HH:mm:ss}] 克隆任务完成: {task.TaskId}");
                    UpdateStatus("准备中");
                }
                else
                {
                    AddResult($"[{DateTime.Now:HH:mm:ss}] 克隆任务进度: {task.Progress}%");
                }
            }
        }

        private void UpdateStatus(string status)
        {
            if (lblStatus != null)
            {
                if (lblStatus.InvokeRequired)
                {
                    lblStatus.Invoke(new Action<string>(UpdateStatus), status);
                }
                else
                {
                    lblStatus.Text = status;
                    
                    // 根据状态设置颜色
                    switch (status)
                    {
                        case "准备中":
                            lblStatus.ForeColor = Color.Blue;
                            break;
                        case "克隆中":
                            lblStatus.ForeColor = Color.Green;
                            break;
                        case "发信执行中":
                            lblStatus.ForeColor = Color.Orange;
                            break;
                        default:
                            lblStatus.ForeColor = Color.Black;
                            break;
                    }
                }
            }
        }

        private void AddResult(string message)
        {
            if (txtResult != null)
            {
                if (txtResult.InvokeRequired)
                {
                    txtResult.Invoke(new Action<string>(AddResult), message);
                }
                else
                {
                    txtResult.AppendText(message + "\r\n");
                    txtResult.SelectionStart = txtResult.Text.Length;
                    txtResult.ScrollToCaret();
                }
            }
        }

        // 菜单事件处理方法
        private void FileNew_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 创建新项目");
        }

        private void FileOpen_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开项目");
        }

        private void FileSave_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 保存项目");
        }

        private void FileExport_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 导出配置");
        }

        private void FileImport_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 导入配置");
        }

        private void FileExit_Click(object sender, EventArgs e)
        {
            // 确认退出
            var result = MessageBox.Show("确定要退出程序吗？", "确认退出", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                ExitApplication();
            }
        }

        private void EditUndo_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 撤销操作");
        }

        private void EditRedo_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 重做操作");
        }

        private void EditCopy_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 复制内容");
        }

        private void EditPaste_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 粘贴内容");
        }

        private void EditSelectAll_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 全选内容");
        }

        private void ViewRefresh_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 刷新显示");
            UpdateStats();
        }

        private void ViewShowDetails_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 显示详细信息");
        }

        private void ViewHideDetails_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 隐藏详细信息");
        }

        private void ControlStartAll_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 启动所有虚拟机");
        }

        private void ControlStopAll_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 停止所有虚拟机");
        }

        private void ControlSuspendAll_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 挂起所有虚拟机");
        }

        private void ControlCloneWizard_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开克隆向导");
        }

        private void ControlEmailWizard_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开邮件发送向导");
        }

        private void HelpDocument_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开帮助文档");
        }

        private void HelpAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("虚拟机管理系统 v1.0\n\n功能：\n- 虚拟机状态监控\n- 批量克隆操作\n- 邮件发送管理\n- 模板配置管理\n- 克隆配置管理\n- 五码配置管理\n- AppleID配置管理\n- 脚本管理", "关于", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HelpAuthor_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "https://github.com/xiaowen1448",
                    UseShellExecute = true
                });
                AddResult($"[{DateTime.Now:HH:mm:ss}] 打开作者GitHub页面");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开链接: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 设置菜单事件处理方法
        private void SettingsCloneConfig_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开克隆配置");
            
            // 打开克隆配置子窗口
            var cloneConfigForm = new VMCloneApp.Forms.CloneConfigForm();
            var result = cloneConfigForm.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 克隆配置已保存");
            }
            else
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 克隆配置已取消");
            }
        }

        private void SettingsWumaConfig_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开五码配置");
            
            // 打开五码配置子窗口
            var wumaConfigForm = new VMCloneApp.Forms.WumaConfigForm();
            var result = wumaConfigForm.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 五码配置已保存");
            }
            else
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 五码配置已取消");
            }
        }

        private void SettingsAppleIdConfig_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开AppleID配置");
            
            // 打开AppleID配置子窗口
            var appleIdConfigForm = new VMCloneApp.Forms.AppleIdConfigForm();
            var result = appleIdConfigForm.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] AppleID配置已保存");
            }
            else
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] AppleID配置已取消");
            }
        }

        private void SettingsScriptManage_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开脚本管理");
            
            // 打开脚本管理子窗口
            var scriptManageForm = new VMCloneApp.Forms.ScriptManageForm();
            var result = scriptManageForm.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 脚本配置已保存");
            }
            else
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 脚本配置已取消");
            }
        }

        private void SettingsClientManage_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开客户端管理");
            
            // 打开客户端管理子窗口
            var clientManageForm = new VMCloneApp.Forms.ClientManageForm();
            var result = clientManageForm.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 客户端配置已保存");
            }
            else
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 客户端配置已取消");
            }
        }

        private void SettingsPreferences_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开偏好设置");
            
            // 打开偏好设置子窗口，传递主窗体引用
            var preferencesForm = new VMCloneApp.Forms.PreferencesForm(this);
            var result = preferencesForm.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 偏好设置已保存");
            }
            else
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 偏好设置已取消");
            }
        }

        private void SettingsVMConfig_Click(object sender, EventArgs e)
        {
            AddResult($"[{DateTime.Now:HH:mm:ss}] 打开虚拟机配置");
            
            // 打开虚拟机配置窗口
            var vmConfigForm = new VMCloneApp.Forms.VMConfigForm();
            var result = vmConfigForm.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 虚拟机配置已保存");
            }
            else
            {
                AddResult($"[{DateTime.Now:HH:mm:ss}] 虚拟机配置已取消");
            }
        }
        
        // 窗体关闭事件处理
        private void ExcelStyleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 如果用户点击关闭按钮，最小化到托盘而不是退出程序
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;  // 取消关闭操作
                MinimizeToTray();
            }
        }
        
        private void MinimizeToTray()
        {
            // 隐藏窗口
            this.Hide();
            
            // 显示托盘提示
            trayIcon?.ShowBalloonTip(1000, "虚拟机管理系统", "程序已最小化到系统托盘", ToolTipIcon.Info);
            
            AddResult($"[{DateTime.Now:HH:mm:ss}] 程序已最小化到系统托盘");
        }
        
        // 托盘图标事件处理方法
        private void TrayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            // 右键点击显示菜单，左键点击还原窗口
            if (e.Button == MouseButtons.Left)
            {
                RestoreFromTray();
            }
        }
        
        private void TrayIcon_DoubleClick(object sender, EventArgs e)
        {
            // 双击托盘图标还原窗口
            RestoreFromTray();
        }
        
        private void TrayOpen_Click(object sender, EventArgs e)
        {
            // 从托盘菜单打开程序
            RestoreFromTray();
        }
        
        private void TrayExit_Click(object sender, EventArgs e)
        {
            // 从托盘菜单退出程序
            ExitApplication();
        }
        
        private void RestoreFromTray()
        {
            // 显示窗口并激活
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            
            // 将窗口置于前台
            this.TopMost = true;
            this.TopMost = false;
            
            AddResult($"[{DateTime.Now:HH:mm:ss}] 从托盘恢复窗口");
        }
        
        private void ExitApplication()
        {
            // 清理托盘图标
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
            }
            
            // 退出应用程序
            Application.Exit();
        }
    }

    // 自定义菜单颜色表
    public class CustomColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(200, 230, 255); }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.FromArgb(200, 230, 255); }
        }

        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.FromArgb(150, 200, 255); }
        }

        public override Color MenuItemBorder
        {
            get { return Color.FromArgb(100, 150, 200); }
        }

        public override Color MenuBorder
        {
            get { return Color.FromArgb(200, 200, 200); }
        }

        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(180, 210, 240); }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(130, 180, 230); }
        }

        public override Color MenuStripGradientBegin
        {
            get { return Color.FromArgb(240, 240, 240); }
        }

        public override Color MenuStripGradientEnd
        {
            get { return Color.FromArgb(240, 240, 240); }
        }
    }
}