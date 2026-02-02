using System;

namespace VMCloneApp.Models
{
    public class VirtualMachine
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string OS { get; set; }
        public int MemoryMB { get; set; }
        public int CPUCores { get; set; }
        public string StoragePath { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsRunning => Status == "Running";

        public VirtualMachine()
        {
            Id = Guid.NewGuid().ToString().Substring(0, 8);
            Name = "New VM";
            Status = "Stopped";
            OS = "macOS";
            MemoryMB = 4096;
            CPUCores = 2;
            StoragePath = "C:\\VMs\\";
            CreatedTime = DateTime.Now;
        }
    }

    public class CloneConfig
    {
        public string SourceVMId { get; set; }
        public string TargetVMName { get; set; }
        public int MemoryMB { get; set; }
        public int CPUCores { get; set; }
        public string StoragePath { get; set; }
        public bool StartAfterClone { get; set; }

        public CloneConfig()
        {
            SourceVMId = "";
            TargetVMName = "VM-Clone-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            MemoryMB = 4096;
            CPUCores = 2;
            StoragePath = "C:\\VMs\\";
            StartAfterClone = true;
        }
    }

    public class CloneTask
    {
        public string TaskId { get; set; }
        public CloneConfig Config { get; set; }
        public string Status { get; set; }
        public int Progress { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Message { get; set; }

        public CloneTask()
        {
            TaskId = Guid.NewGuid().ToString().Substring(0, 8);
            Config = new CloneConfig();
            Status = "Pending";
            Progress = 0;
            StartTime = DateTime.Now;
            Message = "任务已创建";
        }
    }
}