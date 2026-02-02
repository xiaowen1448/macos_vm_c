using System;
using System.Windows.Forms;
using VMCloneApp.Forms;

namespace VMCloneApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                
                // 显示启动信息
                Console.WriteLine("正在启动虚拟机管理系统...");
                
                // 创建并显示Excel风格主窗体
                var mainForm = new ExcelStyleForm();
                Application.Run(mainForm);
            }
            catch (Exception ex)
            {
                // 显示详细的错误信息
                string errorMessage = $"程序启动失败:\n{ex.Message}\n\n堆栈跟踪:\n{ex.StackTrace}";
                MessageBox.Show(errorMessage, "启动错误", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}