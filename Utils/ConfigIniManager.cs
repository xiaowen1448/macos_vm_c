using System;
using System.IO;
using System.Collections.Generic;

namespace VMCloneApp.Utils
{
    public static class ConfigIniManager
    {
        private static readonly string ConfigIniFileName = "config.ini";
        
        public static string GetConfigIniFilePath()
        {
            // 将配置文件保存在应用程序所在目录
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            return Path.Combine(basePath, ConfigIniFileName);
        }
        
        public static string GetConfigFilePath()
        {
            try
            {
                string configIniPath = GetConfigIniFilePath();
                
                if (File.Exists(configIniPath))
                {
                    var lines = File.ReadAllLines(configIniPath);
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("ConfigPath=") && line.Length > 11)
                        {
                            string configPath = line.Substring(11).Trim();
                            if (!string.IsNullOrEmpty(configPath) && File.Exists(configPath))
                            {
                                return configPath;
                            }
                        }
                    }
                }
                
                // 如果config.ini不存在或配置无效，返回默认路径
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vmclone_config.xml");
            }
            catch (Exception ex)
            {
                // 如果读取失败，返回默认路径
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vmclone_config.xml");
            }
        }
        
        public static void SaveConfigFilePath(string configPath)
        {
            try
            {
                string configIniPath = GetConfigIniFilePath();
                
                // 创建或更新config.ini文件
                var lines = new List<string>
                {
                    "# VMCloneApp 配置文件路径设置",
                    "# 此文件保存主配置文件的路径",
                    $"ConfigPath={configPath}",
                    $"# 最后更新: {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
                };
                
                File.WriteAllLines(configIniPath, lines);
            }
            catch (Exception ex)
            {
                throw new Exception($"保存配置文件路径失败: {ex.Message}", ex);
            }
        }
        
        public static string GetConfigFileInfo()
        {
            try
            {
                string configPath = GetConfigFilePath();
                return $"配置文件路径: {configPath}";
            }
            catch (Exception ex)
            {
                return $"获取配置文件信息失败: {ex.Message}";
            }
        }
    }
}