using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace VMCloneApp.Utils
{
    [Serializable]
    public class AppConfig
    {
        // 克隆配置
        public string MotherDisk { get; set; } = "macOS-14.1";
        public int CloneCount { get; set; } = 5;
        public string WumaConfig { get; set; } = "macOS-14.1";
        public string PlistConfig { get; set; } = "default.plist";
        public string NamingPattern { get; set; } = "macos{版本}_timestamp_snapshot_index";
        public string MotherDiskDirectory { get; set; } = "C:\\VM\\MotherDisks";
        public string CloneVMDirectory { get; set; } = "C:\\VM\\Clones";
        public string PlistConfigDirectory { get; set; } = "D:\\xiaowen_1448\\macos_vm_c#\\config\\plist";

        // 五码配置
        public string WumaFile { get; set; } = "macOS-14.1.wuma";
        public string DefaultWuma { get; set; } = "macOS-14.1";
        public string WumaConfigDirectory { get; set; } = "C:\\WumaConfigs";

        // AppleID配置
        public string AppleIdFile { get; set; } = "appleid-2026.json";
        public string DefaultAppleId { get; set; } = "default@apple.com";
        public string AppleIdConfigDirectory { get; set; } = "C:\\AppleIdConfigs";

        // 客户端管理
        public string ApiUrl { get; set; } = "http://localhost:8080/api/v1";

        // 发信管理
        public string EmailTemplate { get; set; } = "macOS克隆完成通知";
        public int EmailInterval { get; set; } = 3;
        public string NumberTemplateFile { get; set; } = "numbers.txt";
        public int NumberCount { get; set; } = 0;
        public string NumberTemplateDirectory { get; set; } = "C:\\NumberTemplates";

        // 虚拟机软件配置
        public string VMDirectory { get; set; } = "C:\\Program Files (x86)\\VMware\\VMware Workstation";
        
        // 系统配置
        public bool AutoStartVM { get; set; } = true;
        public bool EnableLogging { get; set; } = true;
        public int MaxConcurrentClones { get; set; } = 3;
        public string LogLevel { get; set; } = "INFO";

        // 虚拟机配置
        public bool AutoDestroyFailedVM { get; set; } = true;
        public int MaxLoginAttempts { get; set; } = 3;
        public string VMProxyMode { get; set; } = "自动分配";
        public bool EnableSnapshotCleanup { get; set; } = true;
        public bool CheckLoginFailure { get; set; } = true;
        public bool CheckStartupFailure { get; set; } = true;
        public bool CheckProxyFailure { get; set; } = true;
        public bool CheckIPFailure { get; set; } = true;
        public bool CheckWumaFailure { get; set; } = true;
    }

    public static class ConfigManager
    {
        private static readonly string ConfigFileName = "vmclone_config.xml";
        
        public static string GetConfigFilePath()
        {
            // 使用ConfigIniManager获取配置文件路径
            return ConfigIniManager.GetConfigFilePath();
        }

        public static AppConfig LoadConfig()
        {
            try
            {
                string configPath = GetConfigFilePath();
                
                if (File.Exists(configPath))
                {
                    var serializer = new XmlSerializer(typeof(AppConfig));
                    using (var reader = new StreamReader(configPath))
                    {
                        var config = (AppConfig)serializer.Deserialize(reader);
                        
                        // 验证配置完整性，确保所有字段都有有效值
                        return ValidateConfig(config);
                    }
                }
                else
                {
                    // 如果配置文件不存在，创建默认配置并保存
                    var defaultConfig = new AppConfig();
                    SaveConfig(defaultConfig);
                    Console.WriteLine($"创建默认配置文件: {configPath}");
                    return defaultConfig;
                }
            }
            catch (Exception ex)
            {
                // 如果加载失败，记录错误并返回默认配置
                Console.WriteLine($"加载配置文件失败: {ex.Message}");
                return new AppConfig();
            }
        }
        
        private static AppConfig ValidateConfig(AppConfig config)
        {
            // 确保所有配置项都有有效值
            if (string.IsNullOrEmpty(config.MotherDisk)) config.MotherDisk = "macOS-14.1";
            if (config.CloneCount <= 0) config.CloneCount = 5;
            if (string.IsNullOrEmpty(config.WumaConfig)) config.WumaConfig = "macOS-14.1";
            if (string.IsNullOrEmpty(config.PlistConfig)) config.PlistConfig = "default.plist";
            if (string.IsNullOrEmpty(config.NamingPattern)) config.NamingPattern = "macos{版本}_timestamp_snapshot_index";
            if (string.IsNullOrEmpty(config.MotherDiskDirectory)) config.MotherDiskDirectory = "C:\\VM\\MotherDisks";
            if (string.IsNullOrEmpty(config.CloneVMDirectory)) config.CloneVMDirectory = "C:\\VM\\Clones";
            if (string.IsNullOrEmpty(config.PlistConfigDirectory)) config.PlistConfigDirectory = "D:\\xiaowen_1448\\macos_vm_c#\\config\\plist";
            if (string.IsNullOrEmpty(config.WumaFile)) config.WumaFile = "macOS-14.1.wuma";
            if (string.IsNullOrEmpty(config.DefaultWuma)) config.DefaultWuma = "macOS-14.1";
            if (string.IsNullOrEmpty(config.AppleIdFile)) config.AppleIdFile = "appleid-2026.json";
            if (string.IsNullOrEmpty(config.DefaultAppleId)) config.DefaultAppleId = "default@apple.com";
            if (string.IsNullOrEmpty(config.ApiUrl)) config.ApiUrl = "http://localhost:8080/api/v1";
            if (string.IsNullOrEmpty(config.EmailTemplate)) config.EmailTemplate = "macOS克隆完成通知";
            if (config.EmailInterval <= 0) config.EmailInterval = 3;
            if (string.IsNullOrEmpty(config.NumberTemplateFile)) config.NumberTemplateFile = "numbers.txt";
            if (config.NumberCount < 0) config.NumberCount = 0;
            if (string.IsNullOrEmpty(config.NumberTemplateDirectory)) config.NumberTemplateDirectory = "C:\\NumberTemplates";
            if (string.IsNullOrEmpty(config.VMDirectory)) config.VMDirectory = "C:\\Program Files (x86)\\VMware\\VMware Workstation";
            if (config.MaxConcurrentClones <= 0) config.MaxConcurrentClones = 3;
            if (string.IsNullOrEmpty(config.LogLevel)) config.LogLevel = "INFO";
            if (config.MaxLoginAttempts <= 0) config.MaxLoginAttempts = 3;
            if (string.IsNullOrEmpty(config.VMProxyMode)) config.VMProxyMode = "自动分配";
            
            return config;
        }

        public static void SaveConfig(AppConfig config)
        {
            try
            {
                string configPath = GetConfigFilePath();
                
                // 确保目录存在
                var directory = Path.GetDirectoryName(configPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 验证配置数据
                config = ValidateConfig(config);
                
                // 创建备份文件
                if (File.Exists(configPath))
                {
                    string backupPath = configPath + ".backup";
                    File.Copy(configPath, backupPath, true);
                }

                var serializer = new XmlSerializer(typeof(AppConfig));
                using (var writer = new StreamWriter(configPath))
                {
                    serializer.Serialize(writer, config);
                }
                
                Console.WriteLine($"配置文件已保存: {configPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存配置文件失败: {ex.Message}");
                throw new Exception($"保存配置失败: {ex.Message}");
            }
        }
        
        public static bool ConfigFileExists()
        {
            return File.Exists(GetConfigFilePath());
        }
        
        public static string GetConfigFileInfo()
        {
            string configPath = GetConfigFilePath();
            if (File.Exists(configPath))
            {
                var fileInfo = new FileInfo(configPath);
                return $"{configPath}";
            }
            else
            {
                return $"配置文件不存在，将使用默认配置\n默认路径: {configPath}";
            }
        }
    }
}