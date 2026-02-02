using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VMCloneApp.Models;

namespace VMCloneApp.Forms
{
    public partial class EnhancedMainForm : Form
    {
        private List<VirtualMachine> virtualMachines;
        private List<CloneTask> cloneTasks;
        private List<EmailTemplate> emailTemplates;
        private List<EmailRecord> emailRecords;
        private System.Windows.Forms.Timer refreshTimer;
        private TextBox taskOutputTextBox;
        private TabControl mainTabControl;

        public EnhancedMainForm()
        {
            InitializeData();
            InitializeComponent();
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
            
            emailTemplates = new List<EmailTemplate>
            {
                new EmailTemplate { 
                    Name = "克隆完成通知", 
                    Subject = "虚拟机克隆任务完成", 
                    Body = "尊敬的客户，您的虚拟机克隆任务已完成。" 
                },
                new EmailTemplate { 
                    Name = "系统状态报告", 
                    Subject = "系统运行状态报告", 
                    Body = "系统运行状态报告：所有服务正常运行。" 
                }
            };

            emailRecords = new List<EmailRecord>();
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
            UpdateEmailProgress();
        }

        private void InitializeComponent()
        {
            this.Text = "虚拟机克隆与邮件管理系统";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = SystemColors.Control;

            CreateTopPanel();
            CreateContentPanel();
            CreateBottomPanel();
        }

        private void CreateTopPanel()
        {
            var panel = new Panel();
            panel.Dock = DockStyle.Top;
            panel.Height = 80;
            panel.BackColor = Color.LightBlue;
            panel.BorderStyle = BorderStyle.FixedSingle;

            // 第一行按钮
            var startCloneButton = new Button();
            startCloneButton.Text = "启动克隆";
            startCloneButton.Size = new Size(100, 35);
            startCloneButton.Location = new Point(20, 10);
            startCloneButton.BackColor = Color.LightGreen;
            startCloneButton.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            startCloneButton.Click += StartCloneButton_Click;

            var startEmailButton = new Button();
            startEmailButton.Text = "启动发信";
            startEmailButton.Size = new Size(100, 35);
            startEmailButton.Location = new Point(130, 10);
            startEmailButton.BackColor = Color.LightYellow;
            startEmailButton.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            startEmailButton.Click += StartEmailButton_Click;

            var templateManageButton = new Button();
            templateManageButton.Text = "发信模版管理";
            templateManageButton.Size = new Size(120, 35);
            templateManageButton.Location = new Point(240, 10);
            templateManageButton.BackColor = Color.LightCoral;
            templateManageButton.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            templateManageButton.Click += TemplateManageButton_Click;

            var emailRecordButton = new Button();
            emailRecordButton.Text = "发信记录";
            emailRecordButton.Size = new Size(100, 35);
            emailRecordButton.Location = new Point(370, 10);
            emailRecordButton.BackColor = Color.LightPink;
            emailRecordButton.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            emailRecordButton.Click += EmailRecordButton_Click;

            // 第二行按钮
            var vmManageButton = new Button();
            vmManageButton.Text = "虚拟机管理";
            vmManageButton.Size = new Size(100, 35);
            vmManageButton.Location = new Point(20, 45);
            vmManageButton.BackColor = Color.LightGray;
            vmManageButton.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            vmManageButton.Click += VMManageButton_Click;

            var cloneConfigButton = new Button();
            cloneConfigButton.Text = "克隆配置";
            cloneConfigButton.Size = new Size(100, 35);
            cloneConfigButton.Location = new Point(130, 45);
            cloneConfigButton.BackColor = Color.LightCyan;
            cloneConfigButton.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            cloneConfigButton.Click += CloneConfigButton_Click;

            var emailConfigButton = new Button();
            emailConfigButton.Text = "邮件配置";
            emailConfigButton.Size = new Size(100, 35);
            emailConfigButton.Location = new Point(240, 45);
            emailConfigButton.BackColor = Color.LightSalmon;
            emailConfigButton.Font = new Font("微软雅黑", 10, FontStyle.Bold);
            emailConfigButton.Click += EmailConfigButton_Click;

            var statusLabel = new Label();
            statusLabel.Text = "状态: 就绪";
            statusLabel.AutoSize = true;
            statusLabel.Location = new Point(500, 30);
            statusLabel.Font = new Font("微软雅黑", 12, FontStyle.Bold);

            panel.Controls.Add(startCloneButton);
            panel.Controls.Add(startEmailButton);
            panel.Controls.Add(templateManageButton);
            panel.Controls.Add(emailRecordButton);
            panel.Controls.Add(vmManageButton);
            panel.Controls.Add(cloneConfigButton);
            panel.Controls.Add(emailConfigButton);
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
            var cloneTab = new TabPage("克隆配置");
            CreateCloneConfigTab(cloneTab);
            mainTabControl.TabPages.Add(cloneTab);

            // 发信模版管理选项卡
            var templateTab = new TabPage("发信模版管理");
            CreateTemplateManagementTab(templateTab);
            mainTabControl.TabPages.Add(templateTab);

            // 发信记录选项卡
            var recordTab = new TabPage("发信记录");
            CreateEmailRecordTab(recordTab);
            mainTabControl.TabPages.Add(recordTab);

            // 邮件配置选项卡
            var emailConfigTab = new TabPage("邮件配置");
            CreateEmailConfigTab(emailConfigTab);
            mainTabControl.TabPages.Add(emailConfigTab);

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
            vmListView.Size = new Size(950, 200);

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

        private void CreateTemplateManagementTab(TabPage tabPage)
        {
            var templateLabel = new Label();
            templateLabel.Text = "发信模版管理";
            templateLabel.AutoSize = true;
            templateLabel.Location = new Point(10, 10);
            templateLabel.Font = new Font("微软雅黑", 12, FontStyle.Bold);

            var templateListView = new ListView();
            templateListView.View = View.Details;
            templateListView.FullRowSelect = true;
            templateListView.GridLines = true;
            templateListView.Location = new Point(10, 40);
            templateListView.Size = new Size(950, 200);

            templateListView.Columns.Add("ID", 80);
            templateListView.Columns.Add("模版名称", 150);
            templateListView.Columns.Add("邮件主题", 200);
            templateListView.Columns.Add("创建时间", 120);
            templateListView.Columns.Add("修改时间", 120);

            foreach (var template in emailTemplates)
            {
                var item = new ListViewItem(template.Id);
                item.SubItems.Add(template.Name);
                item.SubItems.Add(template.Subject);
                item.SubItems.Add(template.CreatedTime.ToString("yyyy-MM-dd"));
                item.SubItems.Add(template.ModifiedTime.ToString("yyyy-MM-dd"));
                templateListView.Items.Add(item);
            }

            var addButton = new Button();
            addButton.Text = "添加模版";
            addButton.Location = new Point(10, 250);
            addButton.Size = new Size(100, 30);
            addButton.Click += AddTemplateButton_Click;

            var editButton = new Button();
            editButton.Text = "编辑模版";
            editButton.Location = new Point(120, 250);
            editButton.Size = new Size(100, 30);
            editButton.Click += EditTemplateButton_Click;

            tabPage.Controls.Add(templateLabel);
            tabPage.Controls.Add(templateListView);
            tabPage.Controls.Add(addButton);
            tabPage.Controls.Add(editButton);
        }

        private void CreateEmailRecordTab(TabPage tabPage)
        {
            var recordLabel = new Label();
            recordLabel.Text = "发信记录";
            recordLabel.AutoSize = true;
            recordLabel.Location = new Point(10, 10);
            recordLabel.Font = new Font("微软雅黑", 12, FontStyle.Bold);

            var recordListView = new ListView();
            recordListView.View = View.Details;
            recordListView.FullRowSelect = true;
            recordListView.GridLines = true;
            recordListView.Location = new Point(10, 40);
            recordListView.Size = new Size(950, 200);

            recordListView.Columns.Add("ID", 80);
            recordListView.Columns.Add("收件人", 150);
            recordListView.Columns.Add("邮件主题", 200);
            recordListView.Columns.Add("发送状态", 100);
            recordListView.Columns.Add("发送时间", 120);

            foreach (var record in emailRecords)
            {
                var item = new ListViewItem(record.Id);
                item.SubItems.Add(record.Recipient);
                item.SubItems.Add(record.Subject);
                item.SubItems.Add(record.Status);
                item.SubItems.Add(record.SendTime.ToString("yyyy-MM-dd HH:mm"));
                
                if (record.Status == "Success")
                {
                    item.BackColor = Color.LightGreen;
                }
                else if (record.Status == "Failed")
                {
                    item.BackColor = Color.LightPink;
                }
                
                recordListView.Items.Add(item);
            }

            tabPage.Controls.Add(recordLabel);
            tabPage.Controls.Add(recordListView);
        }

        private void CreateEmailConfigTab(TabPage tabPage)
        {
            var configLabel = new Label();
            configLabel.Text = "邮件服务器配置";
            configLabel.AutoSize = true;
            configLabel.Location = new Point(10, 10);
            configLabel.Font = new Font("微软雅黑", 12, FontStyle.Bold);

            var hostLabel = new Label();
            hostLabel.Text = "SMTP服务器:";
            hostLabel.AutoSize = true;
            hostLabel.Location = new Point(10, 50);

            var hostTextBox = new TextBox();
            hostTextBox.Location = new Point(120, 45);
            hostTextBox.Size = new Size(200, 25);
            hostTextBox.Text = "smtp.example.com";

            var portLabel = new Label();
            portLabel.Text = "端口:";
            portLabel.AutoSize = true;
            portLabel.Location = new Point(10, 90);

            var portNumeric = new NumericUpDown();
            portNumeric.Location = new Point(120, 85);
            portNumeric.Size = new Size(100, 25);
            portNumeric.Value = 587;
            portNumeric.Minimum = 1;
            portNumeric.Maximum = 65535;

            var saveConfigButton = new Button();
            saveConfigButton.Text = "保存配置";
            saveConfigButton.Location = new Point(10, 130);
            saveConfigButton.Size = new Size(100, 30);
            saveConfigButton.BackColor = Color.LightBlue;
            saveConfigButton.Click += SaveEmailConfigButton_Click;

            tabPage.Controls.Add(configLabel);
            tabPage.Controls.Add(hostLabel);
            tabPage.Controls.Add(hostTextBox);
            tabPage.Controls.Add(portLabel);
            tabPage.Controls.Add(portNumeric);
            tabPage.Controls.Add(saveConfigButton);
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
            taskOutputTextBox.Size = new Size(1160, 140);
            taskOutputTextBox.ReadOnly = true;
            taskOutputTextBox.BackColor = Color.Black;
            taskOutputTextBox.ForeColor = Color.Lime;
            taskOutputTextBox.Font = new Font("Consolas", 9);

            panel.Controls.Add(taskLabel);
            panel.Controls.Add(taskOutputTextBox);
            this.Controls.Add(panel);
        }

        // 事件处理方法
        private void StartCloneButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 启动虚拟机克隆任务...");
            mainTabControl.SelectedIndex = 1; // 切换到克隆配置页面
        }

        private void StartEmailButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 启动邮件发送任务...");
            
            // 模拟邮件发送
            var record = new EmailRecord
            {
                Recipient = "user@example.com",
                Subject = "测试邮件",
                Status = "Sending"
            };
            emailRecords.Add(record);
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 创建邮件发送任务: {record.Id}");
        }

        private void TemplateManageButton_Click(object sender, EventArgs e)
        {
            mainTabControl.SelectedIndex = 2; // 切换到发信模版管理页面
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 打开发信模版管理页面");
        }

        private void EmailRecordButton_Click(object sender, EventArgs e)
        {
            mainTabControl.SelectedIndex = 3; // 切换到发信记录页面
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 打开发信记录页面");
        }

        private void VMManageButton_Click(object sender, EventArgs e)
        {
            mainTabControl.SelectedIndex = 0; // 切换到虚拟机管理页面
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 打开虚拟机管理页面");
        }

        private void CloneConfigButton_Click(object sender, EventArgs e)
        {
            mainTabControl.SelectedIndex = 1; // 切换到克隆配置页面
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 打开克隆配置页面");
        }

        private void EmailConfigButton_Click(object sender, EventArgs e)
        {
            mainTabControl.SelectedIndex = 4; // 切换到邮件配置页面
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 打开邮件配置页面");
        }

        private void SaveConfigButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 保存克隆配置成功");
            
            var task = new CloneTask();
            cloneTasks.Add(task);
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 创建克隆任务: {task.TaskId}");
        }

        private void SaveEmailConfigButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 保存邮件配置成功");
        }

        private void AddTemplateButton_Click(object sender, EventArgs e)
        {
            var template = new EmailTemplate();
            emailTemplates.Add(template);
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 添加新的发信模版: {template.Name}");
        }

        private void EditTemplateButton_Click(object sender, EventArgs e)
        {
            AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 编辑发信模版");
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

        private void UpdateEmailProgress()
        {
            foreach (var record in emailRecords.Where(r => r.Status == "Sending"))
            {
                record.Status = "Success";
                AddTaskOutput($"[{DateTime.Now:HH:mm:ss}] 邮件发送成功: {record.Id}");
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