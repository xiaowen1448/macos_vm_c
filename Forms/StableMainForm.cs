using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VMCloneApp.Models;

namespace VMCloneApp.Forms
{
    public partial class StableMainForm : Form
    {
        private List<VirtualMachine> virtualMachines;
        private List<CloneTask> cloneTasks;
        private System.Windows.Forms.Timer refreshTimer;
        private TextBox taskOutputTextBox;
        private TabControl mainTabControl;

        public StableMainForm()
        {
            // 先初始化数据
            InitializeData();
            
            // 然后初始化界面
            InitializeComponent();
            
            // 最后初始化定时器
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
            // 窗体基本设置
            this.Text = "虚拟机克隆管理系统";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = SystemColors.Control;

            // 创建顶部按钮区域
            CreateTopPanel();
            
            // 创建主内容区域
            CreateContentPanel();
            
            // 创建底部输出区域
            CreateBottomPanel();
        }

        private void CreateTopPanel()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Top;
            panel.Height = 60;
            panel.BackColor = Color.LightBlue;
            panel.BorderStyle = BorderStyle.FixedSingle;

            var cloneButton = new Button();
            cloneButton.Text = "虚拟机克隆";
            cloneButton.Size = new Size(120, 35);
            cloneButton.Location = new Point(20, 15);
            cloneButton.BackColor = Color.LightGreen;
            cloneButton.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            cloneButton.Click += CloneButton_Click;

            var vmManageButton = new Button();
            vmManageButton.Text = "虚拟机管理";
            vmManageButton.Size = new Size(120, 35);
            vmManageButton.Location = new Point(150, 15);
            vmManageButton.BackColor = Color.LightYellow;
            vmManageButton.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            vmManageButton.Click += VMManageButton_Click;

            var configButton = new Button();
            configButton.Text = "克隆配置";
            configButton.Size = new Size(120, 35);
            configButton.Location = new Point(280, 15);
            configButton.BackColor = Color.LightCoral;
            configButton.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            configButton.Click += ConfigButton_Click;

            var statusLabel = new Label();
            statusLabel.Text = "状态: 就绪";
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(420, 22);
            statusLabel.Font = new Font("微软雅黑", 10);

            panel.Controls.Add(cloneButton);
            panel.Controls.Add(vmManageButton);
            panel.Controls.Add(configButton);
            panel.Controls.Add(statusLabel);

            this.Controls.Add(panel);
        }

        private void CreateContentPanel()
        {
            mainTabControl = new TabControl();
            mainTabControl.Dock = DockStyle.Fill;
            mainTabControl.Appearance = TabAppearance.Normal;

            // 虚拟机管理选项卡
            var vmTab = new TabPage("虚拟机管理");
            CreateVMManagementTab(vmTab);
            mainTabControl.TabPages.Add(vmTab);

            // 克隆配置选项卡
            var configTab = new TabPage("克隆配置");
            CreateCloneConfigTab(configTab);
            mainTabControl.TabPages.Add(configTab);

            this.Controls.Add(mainTabControl);
        }

        private void CreateVMManagementTab(TabPage tabPage)
        {
            var vmLabel = new Label();
            vmLabel.Text = "虚拟机列表";
            vmLabel.AutoSize = true;
            vmLabel.Location = new Point(10, 10);
            vmLabel.Font = new Font("微软雅黑", 12, FontStyle.Bold);

            var vmListView = new ListView();
            vmListView.View = View.Details;
            vmListView.FullRowSelect = true;
            vmListView.GridLines = true;
            vmListView.Location = new Point(10, 40);
            vmListView.Size = new Size(750, 200);

            vmListView.Columns.Add("ID", 80);
            vmListView.Columns.Add("名称", 150);
            vmListView.Columns.Add("状态", 80);
            vmListView.Columns.Add("操作系统", 120);
            vmListView.Columns.Add("内存", 80);
            vmListView.Columns.Add("CPU核心", 80);

            foreach (var vm in virtualMachines)
            {
                var item = new ListViewItem(vm.Id);
                item.SubItems.Add(vm.Name);
                item.SubItems.Add(vm.Status);
                item.SubItems.Add(vm.OS);
                item.SubItems.Add($"{vm.MemoryMB / 1024} GB");
                item.SubItems.Add(vm.CPUCores.ToString());
                
                if (vm.IsRunning)
                {
                    item.BackColor = Color.LightGreen;
                }
                
                vmListView.Items.Add(item);
            }

            tabPage.Controls.Add(vmLabel);
            tabPage.Controls.Add(vmListView);
        }

        private void CreateCloneConfigTab(TabPage tabPage)
        {
            var configLabel = new Label();
            configLabel.Text = "克隆配置";
            configLabel.AutoSize = true;
            configLabel.Location = new Point(10, 10);
            configLabel.Font = new Font("微软雅黑", 12, FontStyle.Bold);

            var sourceLabel = new Label();
            sourceLabel.Text = "源虚拟机:";
            sourceLabel.AutoSize = true;
            sourceLabel.Location = new Point(10, 50);

            var sourceComboBox = new ComboBox();
            sourceComboBox.Location = new Point(100, 45);
            sourceComboBox.Size = new Size(200, 25);
            sourceComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            foreach (var vm in virtualMachines)
            {
                sourceComboBox.Items.Add($"{vm.Name} ({vm.Id})");
            }
            if (sourceComboBox.Items.Count > 0)
                sourceComboBox.SelectedIndex = 0;

            var targetLabel = new Label();
            targetLabel.Text = "目标名称:";
            targetLabel.AutoSize = true;
            targetLabel.Location = new Point(10, 90);

            var targetTextBox = new TextBox();
            targetTextBox.Location = new Point(100, 85);
            targetTextBox.Size = new Size(200, 25);
            targetTextBox.Text = "VM-Clone-" + DateTime.Now.ToString("yyyyMMddHHmmss");

            var saveButton = new Button();
            saveButton.Text = "保存配置";
            saveButton.Location = new Point(10, 130);
            saveButton.Size = new Size(100, 30);
            saveButton.BackColor = Color.LightBlue;
            saveButton.Click += SaveConfigButton_Click;

            tabPage.Controls.Add(configLabel);
            tabPage.Controls.Add(sourceLabel);
            tabPage.Controls.Add(sourceComboBox);
            tabPage.Controls.Add(targetLabel);
            tabPage.Controls.Add(targetTextBox);
            tabPage.Controls.Add(saveButton);
        }

        private void CreateBottomPanel()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Bottom;
            panel.Height = 200;
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;

            var taskLabel = new Label();
            taskLabel.Text = "任务输出";
            taskLabel.AutoSize = true;
            taskLabel.Location = new Point(10, 10);
            taskLabel.Font = new Font("微软雅黑", 10, FontStyle.Bold);

            taskOutputTextBox = new TextBox();
            taskOutputTextBox.Multiline = true;
            taskOutputTextBox.ScrollBars = ScrollBars.Vertical;
            taskOutputTextBox.Location = new Point(10, 40);
            taskOutputTextBox.Size = new Size(960, 140);
            taskOutputTextBox.ReadOnly = true;
            taskOutputTextBox.BackColor = Color.Black;
            taskOutputTextBox.ForeColor = Color.Lime;
            taskOutputTextBox.Font = new Font("Consolas", 9);

            panel.Controls.Add(taskLabel);
            panel.Controls.Add(taskOutputTextBox);
            this.Controls.Add(panel);
        }

        // 事件处理方法
        private void CloneButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 开始虚拟机克隆操作...");
            mainTabControl.SelectedIndex = 1;
        }

        private void VMManageButton_Click(object sender, EventArgs e)
        {
            mainTabControl.SelectedIndex = 0;
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 打开虚拟机管理页面");
        }

        private void ConfigButton_Click(object sender, EventArgs e)
        {
            mainTabControl.SelectedIndex = 1;
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 打开克隆配置页面");
        }

        private void SaveConfigButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 保存克隆配置成功");
            
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