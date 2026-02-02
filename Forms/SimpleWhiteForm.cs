using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VMCloneApp.Models;

namespace VMCloneApp.Forms
{
    public partial class SimpleWhiteForm : Form
    {
        private List<VirtualMachine> virtualMachines;
        private List<CloneTask> cloneTasks;
        private System.Windows.Forms.Timer refreshTimer;
        private TextBox taskOutputTextBox;
        private Label totalVMsLabel;
        private Label runningVMsLabel;
        private Label stoppedVMsLabel;
        private Label suspendedVMsLabel;

        public SimpleWhiteForm()
        {
            // 设置窗体基本属性
            this.Text = "虚拟机管理系统";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            
            // 初始化数据
            InitializeData();
            
            // 初始化界面控件
            InitializeControls();
            
            // 初始化定时器
            InitializeTimer();
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
            // 创建顶部统计区域
            CreateStatsPanel();
            
            // 创建中部按钮区域
            CreateButtonsPanel();
            
            // 创建底部输出区域
            CreateOutputPanel();
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

        private void CreateStatsPanel()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Top;
            panel.Height = 100;
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;

            // 总虚拟机数
            var totalLabel = new Label();
            totalLabel.Text = "总虚拟机数";
            totalLabel.AutoSize = true;
            totalLabel.Location = new Point(50, 20);
            totalLabel.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            totalLabel.ForeColor = Color.Black;

            totalVMsLabel = new Label();
            totalVMsLabel.Text = "0";
            totalVMsLabel.AutoSize = true;
            totalVMsLabel.Location = new Point(70, 50);
            totalVMsLabel.Font = new Font("微软雅黑", 16, FontStyle.Bold);
            totalVMsLabel.ForeColor = Color.Blue;

            // 运行中
            var runningLabel = new Label();
            runningLabel.Text = "运行中";
            runningLabel.AutoSize = true;
            runningLabel.Location = new Point(180, 20);
            runningLabel.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            runningLabel.ForeColor = Color.Black;

            runningVMsLabel = new Label();
            runningVMsLabel.Text = "8";
            runningVMsLabel.AutoSize = true;
            runningVMsLabel.Location = new Point(200, 50);
            runningVMsLabel.Font = new Font("微软雅黑", 16, FontStyle.Bold);
            runningVMsLabel.ForeColor = Color.Green;

            // 已停止
            var stoppedLabel = new Label();
            stoppedLabel.Text = "已停止";
            stoppedLabel.AutoSize = true;
            stoppedLabel.Location = new Point(310, 20);
            stoppedLabel.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            stoppedLabel.ForeColor = Color.Black;

            stoppedVMsLabel = new Label();
            stoppedVMsLabel.Text = "10";
            stoppedVMsLabel.AutoSize = true;
            stoppedVMsLabel.Location = new Point(330, 50);
            stoppedVMsLabel.Font = new Font("微软雅黑", 16, FontStyle.Bold);
            stoppedVMsLabel.ForeColor = Color.Red;

            // 挂起
            var suspendedLabel = new Label();
            suspendedLabel.Text = "挂起";
            suspendedLabel.AutoSize = true;
            suspendedLabel.Location = new Point(440, 20);
            suspendedLabel.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            suspendedLabel.ForeColor = Color.Black;

            suspendedVMsLabel = new Label();
            suspendedVMsLabel.Text = "2";
            suspendedVMsLabel.AutoSize = true;
            suspendedVMsLabel.Location = new Point(460, 50);
            suspendedVMsLabel.Font = new Font("微软雅黑", 16, FontStyle.Bold);
            suspendedVMsLabel.ForeColor = Color.Orange;

            panel.Controls.Add(totalLabel);
            panel.Controls.Add(totalVMsLabel);
            panel.Controls.Add(runningLabel);
            panel.Controls.Add(runningVMsLabel);
            panel.Controls.Add(stoppedLabel);
            panel.Controls.Add(stoppedVMsLabel);
            panel.Controls.Add(suspendedLabel);
            panel.Controls.Add(suspendedVMsLabel);

            this.Controls.Add(panel);
        }

        private void CreateButtonsPanel()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;

            // 使用表格布局来居中显示按钮
            var tableLayout = new TableLayoutPanel();
            tableLayout.Dock = DockStyle.Fill;
            tableLayout.RowCount = 1;
            tableLayout.ColumnCount = 4;
            tableLayout.BackColor = Color.White;
            
            // 设置列宽百分比
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            
            // 设置行高
            tableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // 启动克隆按钮
            var startCloneButton = new Button();
            startCloneButton.Text = "启动克隆";
            startCloneButton.Dock = DockStyle.Fill;
            startCloneButton.Margin = new Padding(20);
            startCloneButton.BackColor = Color.White;
            startCloneButton.ForeColor = Color.Black;
            startCloneButton.Font = new Font("微软雅黑", 12, FontStyle.Bold);
            startCloneButton.FlatStyle = FlatStyle.Flat;
            startCloneButton.FlatAppearance.BorderColor = Color.Black;
            startCloneButton.FlatAppearance.BorderSize = 2;
            startCloneButton.Click += StartCloneButton_Click;

            // 启动发信按钮
            var startEmailButton = new Button();
            startEmailButton.Text = "启动发信";
            startEmailButton.Dock = DockStyle.Fill;
            startEmailButton.Margin = new Padding(20);
            startEmailButton.BackColor = Color.White;
            startEmailButton.ForeColor = Color.Black;
            startEmailButton.Font = new Font("微软雅黑", 12, FontStyle.Bold);
            startEmailButton.FlatStyle = FlatStyle.Flat;
            startEmailButton.FlatAppearance.BorderColor = Color.Black;
            startEmailButton.FlatAppearance.BorderSize = 2;
            startEmailButton.Click += StartEmailButton_Click;

            // 发信模板管理按钮
            var templateManageButton = new Button();
            templateManageButton.Text = "发信模板管理";
            templateManageButton.Dock = DockStyle.Fill;
            templateManageButton.Margin = new Padding(20);
            templateManageButton.BackColor = Color.White;
            templateManageButton.ForeColor = Color.Black;
            templateManageButton.Font = new Font("微软雅黑", 12, FontStyle.Bold);
            templateManageButton.FlatStyle = FlatStyle.Flat;
            templateManageButton.FlatAppearance.BorderColor = Color.Black;
            templateManageButton.FlatAppearance.BorderSize = 2;
            templateManageButton.Click += TemplateManageButton_Click;

            // 发信记录按钮
            var emailRecordButton = new Button();
            emailRecordButton.Text = "发信记录";
            emailRecordButton.Dock = DockStyle.Fill;
            emailRecordButton.Margin = new Padding(20);
            emailRecordButton.BackColor = Color.White;
            emailRecordButton.ForeColor = Color.Black;
            emailRecordButton.Font = new Font("微软雅黑", 12, FontStyle.Bold);
            emailRecordButton.FlatStyle = FlatStyle.Flat;
            emailRecordButton.FlatAppearance.BorderColor = Color.Black;
            emailRecordButton.FlatAppearance.BorderSize = 2;
            emailRecordButton.Click += EmailRecordButton_Click;

            // 添加按钮到表格布局
            tableLayout.Controls.Add(startCloneButton, 0, 0);
            tableLayout.Controls.Add(startEmailButton, 1, 0);
            tableLayout.Controls.Add(templateManageButton, 2, 0);
            tableLayout.Controls.Add(emailRecordButton, 3, 0);

            panel.Controls.Add(tableLayout);
            this.Controls.Add(panel);
        }

        private void CreateOutputPanel()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 200;
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;

            var taskLabel = new Label();
            taskLabel.Text = "日志输出";
            taskLabel.AutoSize = true;
            taskLabel.Location = new Point(10, 10);
            taskLabel.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            taskLabel.ForeColor = Color.Black;

            taskOutputTextBox = new TextBox();
            taskOutputTextBox.Multiline = true;
            taskOutputTextBox.ScrollBars = ScrollBars.Vertical;
            taskOutputTextBox.Location = new Point(10, 40);
            taskOutputTextBox.Size = new Size(760, 140);
            taskOutputTextBox.ReadOnly = true;
            taskOutputTextBox.BackColor = Color.White;
            taskOutputTextBox.ForeColor = Color.Black;
            taskOutputTextBox.Font = new Font("Consolas", 9);
            taskOutputTextBox.BorderStyle = BorderStyle.FixedSingle;

            panel.Controls.Add(taskLabel);
            panel.Controls.Add(taskOutputTextBox);
            this.Controls.Add(panel);
        }

        private void UpdateStats()
        {
            var total = virtualMachines.Count;
            var running = virtualMachines.Count(vm => vm.Status == "Running");
            var stopped = virtualMachines.Count(vm => vm.Status == "Stopped");
            var suspended = virtualMachines.Count(vm => vm.Status == "Suspended");

            totalVMsLabel.Text = total.ToString();
            runningVMsLabel.Text = running.ToString();
            stoppedVMsLabel.Text = stopped.ToString();
            suspendedVMsLabel.Text = suspended.ToString();
        }

        // 事件处理方法
        private void StartCloneButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 启动虚拟机克隆任务...");
            
            var task = new CloneTask();
            cloneTasks.Add(task);
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 创建克隆任务: {task.TaskId}");
        }

        private void StartEmailButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 启动邮件发送任务...");
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 邮件发送任务已创建");
        }

        private void TemplateManageButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 打开发信模板管理");
        }

        private void EmailRecordButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 打开发信记录");
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
                    AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 克隆任务完成: {task.TaskId}");
                }
                else
                {
                    AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 克隆任务进度: {task.Progress}%");
                }
            }
        }

        private void AddTaskOutput(string message)
        {
            if (taskOutputTextBox != null)
            {
                if (taskOutputTextBox.InvokeRequired)
                {
                    taskOutputTextBox.Invoke(new Action<string>(AddTaskOutput), message);
                }
                else
                {
                    taskOutputTextBox.AppendText(message + "\r\n");
                    taskOutputTextBox.SelectionStart = taskOutputTextBox.Text.Length;
                    taskOutputTextBox.ScrollToCaret();
                }
            }
        }
    }
}