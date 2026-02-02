using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VMCloneApp.Models;

namespace VMCloneApp.Forms
{
    public partial class MainForm : Form
    {
        private List<VirtualMachine> virtualMachines;
        private List<CloneTask> cloneTasks;
        private System.Windows.Forms.Timer refreshTimer;
        private TextBox taskOutputTextBox;
        private TabControl mainTabControl;

        public MainForm()
        {
            InitializeComponent();
            InitializeData();
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
                }
            };

            cloneTasks = new List<CloneTask>();
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
            UpdateTaskProgress();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            
            this.Text = "虚拟机克隆管理系统";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            
            CreateMainLayout();
            
            this.ResumeLayout(false);
        }

        private void CreateMainLayout()
        {
            // 创建主布局面板
            var mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = SystemColors.Control
            };

            // 创建顶部按钮区域
            var topPanel = CreateTopPanel();
            
            // 创建主内容区域
            var contentPanel = CreateContentPanel();
            
            // 创建底部输出区域
            var bottomPanel = CreateBottomPanel();

            // 添加所有面板到主面板
            mainPanel.Controls.Add(topPanel);
            mainPanel.Controls.Add(contentPanel);
            mainPanel.Controls.Add(bottomPanel);

            this.Controls.Add(mainPanel);
        }

        private Panel CreateTopPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.LightBlue,
                BorderStyle = BorderStyle.FixedSingle
            };

            var buttonsPanel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            var cloneButton = new Button
            {
                Text = "虚拟机克隆",
                Size = new Size(120, 35),
                BackColor = Color.LightGreen,
                Font = new Font("微软雅黑", 10, FontStyle.Bold)
            };
            cloneButton.Click += CloneButton_Click;

            var vmManageButton = new Button
            {
                Text = "虚拟机管理",
                Size = new Size(120, 35),
                BackColor = Color.LightYellow,
                Font = new Font("微软雅黑", 10, FontStyle.Bold)
            };
            vmManageButton.Click += VMManageButton_Click;

            var configButton = new Button
            {
                Text = "克隆配置",
                Size = new Size(120, 35),
                BackColor = Color.LightCoral,
                Font = new Font("微软雅黑", 10, FontStyle.Bold)
            };
            configButton.Click += ConfigButton_Click;

            var statusLabel = new Label
            {
                Text = "状态: 就绪",
                AutoSize = true,
                Font = new Font("微软雅黑", 10),
                TextAlign = ContentAlignment.MiddleLeft
            };

            buttonsPanel.Controls.Add(cloneButton);
            buttonsPanel.Controls.Add(vmManageButton);
            buttonsPanel.Controls.Add(configButton);
            buttonsPanel.Controls.Add(statusLabel);

            panel.Controls.Add(buttonsPanel);
            return panel;
        }

        private Panel CreateContentPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            // 创建选项卡控件
            mainTabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Appearance = TabAppearance.Normal
            };

            // 虚拟机管理选项卡
            var vmTab = new TabPage("虚拟机管理");
            CreateVMManagementTab(vmTab);
            mainTabControl.TabPages.Add(vmTab);

            // 克隆配置选项卡
            var configTab = new TabPage("克隆配置");
            CreateCloneConfigTab(configTab);
            mainTabControl.TabPages.Add(configTab);

            panel.Controls.Add(mainTabControl);
            return panel;
        }

        private void CreateVMManagementTab(TabPage tabPage)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            var vmLabel = new Label
            {
                Text = "虚拟机列表",
                AutoSize = true,
                Location = new Point(10, 10),
                Font = new Font("微软雅黑", 12, FontStyle.Bold)
            };

            var vmListView = new ListView
            {
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                Location = new Point(10, 40),
                Size = new Size(750, 200)
            };

            vmListView.Columns.Add("ID", 80);
            vmListView.Columns.Add("名称", 150);
            vmListView.Columns.Add("状态", 80);
            vmListView.Columns.Add("操作系统", 120);
            vmListView.Columns.Add("内存", 80);
            vmListView.Columns.Add("CPU核心", 80);
            vmListView.Columns.Add("创建时间", 120);

            foreach (var vm in virtualMachines)
            {
                var item = new ListViewItem(vm.Id);
                item.SubItems.Add(vm.Name);
                item.SubItems.Add(vm.Status);
                item.SubItems.Add(vm.OS);
                item.SubItems.Add($"{vm.MemoryMB / 1024} GB");
                item.SubItems.Add(vm.CPUCores.ToString());
                item.SubItems.Add(vm.CreatedTime.ToString("yyyy-MM-dd HH:mm"));
                
                if (vm.IsRunning)
                {
                    item.BackColor = Color.LightGreen;
                }
                
                vmListView.Items.Add(item);
            }

            // 操作按钮
            var startButton = new Button
            {
                Text = "启动虚拟机",
                Location = new Point(10, 250),
                Size = new Size(100, 30)
            };
            startButton.Click += StartVMButton_Click;

            var stopButton = new Button
            {
                Text = "停止虚拟机",
                Location = new Point(120, 250),
                Size = new Size(100, 30)
            };
            stopButton.Click += StopVMButton_Click;

            var editButton = new Button
            {
                Text = "编辑配置",
                Location = new Point(230, 250),
                Size = new Size(100, 30)
            };
            editButton.Click += EditVMButton_Click;

            panel.Controls.Add(vmLabel);
            panel.Controls.Add(vmListView);
            panel.Controls.Add(startButton);
            panel.Controls.Add(stopButton);
            panel.Controls.Add(editButton);

            tabPage.Controls.Add(panel);
        }

        private void CreateCloneConfigTab(TabPage tabPage)
        {
            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            var configLabel = new Label
            {
                Text = "克隆配置",
                AutoSize = true,
                Location = new Point(10, 10),
                Font = new Font("微软雅黑", 12, FontStyle.Bold)
            };

            // 源虚拟机选择
            var sourceLabel = new Label
            {
                Text = "源虚拟机:",
                AutoSize = true,
                Location = new Point(10, 50)
            };

            var sourceComboBox = new ComboBox
            {
                Location = new Point(100, 45),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            foreach (var vm in virtualMachines)
            {
                sourceComboBox.Items.Add($"{vm.Name} ({vm.Id})");
            }
            if (sourceComboBox.Items.Count > 0)
                sourceComboBox.SelectedIndex = 0;

            // 目标虚拟机名称
            var targetLabel = new Label
            {
                Text = "目标名称:",
                AutoSize = true,
                Location = new Point(10, 90)
            };

            var targetTextBox = new TextBox
            {
                Location = new Point(100, 85),
                Size = new Size(200, 25),
                Text = "VM-Clone-" + DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            // 内存配置
            var memoryLabel = new Label
            {
                Text = "内存(MB):",
                AutoSize = true,
                Location = new Point(10, 130)
            };

            var memoryNumeric = new NumericUpDown
            {
                Location = new Point(100, 125),
                Size = new Size(100, 25),
                Minimum = 1024,
                Maximum = 32768,
                Value = 4096,
                Increment = 1024
            };

            // CPU配置
            var cpuLabel = new Label
            {
                Text = "CPU核心:",
                AutoSize = true,
                Location = new Point(10, 170)
            };

            var cpuNumeric = new NumericUpDown
            {
                Location = new Point(100, 165),
                Size = new Size(100, 25),
                Minimum = 1,
                Maximum = 16,
                Value = 2,
                Increment = 1
            };

            // 启动选项
            var startCheckBox = new CheckBox
            {
                Text = "克隆后自动启动",
                Location = new Point(10, 210),
                AutoSize = true,
                Checked = true
            };

            // 保存配置按钮
            var saveButton = new Button
            {
                Text = "保存配置",
                Location = new Point(10, 250),
                Size = new Size(100, 30),
                BackColor = Color.LightBlue
            };
            saveButton.Click += SaveConfigButton_Click;

            panel.Controls.Add(configLabel);
            panel.Controls.Add(sourceLabel);
            panel.Controls.Add(sourceComboBox);
            panel.Controls.Add(targetLabel);
            panel.Controls.Add(targetTextBox);
            panel.Controls.Add(memoryLabel);
            panel.Controls.Add(memoryNumeric);
            panel.Controls.Add(cpuLabel);
            panel.Controls.Add(cpuNumeric);
            panel.Controls.Add(startCheckBox);
            panel.Controls.Add(saveButton);

            tabPage.Controls.Add(panel);
        }

        private Panel CreateBottomPanel()
        {
            var panel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 200,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var taskLabel = new Label
            {
                Text = "任务输出",
                AutoSize = true,
                Location = new Point(10, 10),
                Font = new Font("微软雅黑", 10, FontStyle.Bold)
            };

            taskOutputTextBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Location = new Point(10, 40),
                Size = new Size(960, 140),
                ReadOnly = true,
                BackColor = Color.Black,
                ForeColor = Color.Lime,
                Font = new Font("Consolas", 9)
            };

            panel.Controls.Add(taskLabel);
            panel.Controls.Add(taskOutputTextBox);
            return panel;
        }

        // 事件处理方法
        private void CloneButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 开始虚拟机克隆操作...");
            mainTabControl.SelectedIndex = 1; // 切换到配置页面
        }

        private void VMManageButton_Click(object sender, EventArgs e)
        {
            mainTabControl.SelectedIndex = 0; // 切换到虚拟机管理页面
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 打开虚拟机管理页面");
        }

        private void ConfigButton_Click(object sender, EventArgs e)
        {
            mainTabControl.SelectedIndex = 1; // 切换到配置页面
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 打开克隆配置页面");
        }

        private void StartVMButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 启动虚拟机操作");
        }

        private void StopVMButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 停止虚拟机操作");
        }

        private void EditVMButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 编辑虚拟机配置");
        }

        private void SaveConfigButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 保存克隆配置成功");
            
            // 模拟克隆任务
            var task = new CloneTask();
            cloneTasks.Add(task);
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 创建克隆任务: {task.TaskId}");
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
                    AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 任务进度: {task.Progress}%");
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