@echo off
setlocal enabledelayedexpansion

echo ========================================
echo    VMCloneApp 构建和打包脚本
echo ========================================
echo.

:: 设置变量
set PROJECT_NAME=VMCloneApp
set BUILD_CONFIG=Release
set OUTPUT_DIR=bin\%BUILD_CONFIG%
set PACKAGE_DIR=Package
set VERSION=1.0.0

echo [1/6] 清理之前的构建文件...
if exist "%OUTPUT_DIR%" (
    rmdir /s /q "%OUTPUT_DIR%"
)
if exist "%PACKAGE_DIR%" (
    rmdir /s /q "%PACKAGE_DIR%"
)

echo [2/6] 恢复NuGet包...
dotnet restore
if %errorlevel% neq 0 (
    echo 错误: NuGet包恢复失败
    pause
    exit /b 1
)

echo [3/6] 构建项目...
dotnet build --configuration %BUILD_CONFIG% --no-restore
if %errorlevel% neq 0 (
    echo 错误: 项目构建失败
    pause
    exit /b 1
)

echo [4/6] 发布项目...
dotnet publish --configuration %BUILD_CONFIG% --output "%OUTPUT_DIR%\publish" --runtime win-x64 --self-contained true --p:DebugType=None --p:DebugSymbols=false
if %errorlevel% neq 0 (
    echo 错误: 项目发布失败
    pause
    exit /b 1
)

echo [5/6] 创建打包目录...
mkdir "%PACKAGE_DIR%"
mkdir "%PACKAGE_DIR%\%PROJECT_NAME%_v%VERSION%"

:: 复制可执行文件
xcopy "%OUTPUT_DIR%\publish\*.*" "%PACKAGE_DIR%\%PROJECT_NAME%_v%VERSION%\" /Y /E /I

:: 复制配置文件模板
if not exist "%PACKAGE_DIR%\%PROJECT_NAME%_v%VERSION%\ConfigTemplates" mkdir "%PACKAGE_DIR%\%PROJECT_NAME%_v%VERSION%\ConfigTemplates"

:: 创建示例配置文件
echo 创建示例配置文件...
(
echo ^<?xml version="1.0" encoding="utf-8"?^>
echo ^<AppConfig xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"^>
echo   ^<!-- 克隆配置 --^>
echo   ^<MotherDisk^>macOS-14.1^</MotherDisk^>
echo   ^<CloneCount^>5^</CloneCount^>
echo   ^<WumaConfig^>macOS-14.1^</WumaConfig^>
echo   ^<NamingPattern^>macos{版本}_timestamp_snapshot_index^</NamingPattern^>
echo   ^<MotherDiskDirectory^>C:\VM\MotherDisks^</MotherDiskDirectory^>
echo   ^<CloneVMDirectory^>C:\VM\Clones^</CloneVMDirectory^>
echo   ^<!-- 五码配置 --^>
echo   ^<WumaFile^>macOS-14.1.wuma^</WumaFile^>
echo   ^<DefaultWuma^>macOS-14.1^</DefaultWuma^>
echo   ^<WumaConfigDirectory^>C:\WumaConfigs^</WumaConfigDirectory^>
echo   ^<!-- AppleID配置 --^>
echo   ^<AppleIdFile^>appleid-2026.json^</AppleIdFile^>
echo   ^<DefaultAppleId^>default@apple.com^</DefaultAppleId^>
echo   ^<AppleIdConfigDirectory^>C:\AppleIdConfigs^</AppleIdConfigDirectory^>
echo   ^<!-- 发信管理 --^>
echo   ^<EmailTemplate^>macOS克隆完成通知^</EmailTemplate^>
echo   ^<EmailInterval^>3^</EmailInterval^>
echo   ^<NumberTemplateFile^>numbers.txt^</NumberTemplateFile^>
echo   ^<NumberCount^>0^</NumberCount^>
echo   ^<NumberTemplateDirectory^>C:\NumberTemplates^</NumberTemplateDirectory^>
echo   ^<!-- 系统配置 --^>
echo   ^<AutoStartVM^>true^</AutoStartVM^>
echo   ^<EnableLogging^>true^</EnableLogging^>
echo   ^<MaxConcurrentClones^>3^</MaxConcurrentClones^>
echo   ^<LogLevel^>INFO^</LogLevel^>
echo ^</AppConfig^>
) > "%PACKAGE_DIR%\%PROJECT_NAME%_v%VERSION%\ConfigTemplates\vmclone_config_template.xml"

:: 创建运行脚本
echo 创建运行脚本...
(
echo @echo off
echo title VMCloneApp - macOS虚拟机克隆工具
echo.
echo ========================================
echo    VMCloneApp 启动脚本
echo ========================================
echo.
echo 正在启动应用程序...
echo.
set APP_NAME=VMCloneApp.exe

if exist "%APP_NAME%" (
    echo 启动 %APP_NAME%...
    start "" "%APP_NAME%"
    echo 应用程序已启动！
) else (
    echo 错误: 找不到 %APP_NAME%
    echo 请确保可执行文件存在于当前目录
)

echo.
echo 按任意键退出...
pause >nul
) > "%PACKAGE_DIR%\%PROJECT_NAME%_v%VERSION%\启动VMCloneApp.bat"

echo [6/6] 创建ZIP压缩包...
cd "%PACKAGE_DIR%"
"C:\Program Files\7-Zip\7z.exe" a -tzip "%PROJECT_NAME%_v%VERSION%.zip" "%PROJECT_NAME%_v%VERSION%\*" >nul 2>&1

if %errorlevel% neq 0 (
    echo 警告: 7-Zip未找到，使用系统压缩工具...
    powershell -Command "Compress-Archive -Path '%PROJECT_NAME%_v%VERSION%' -DestinationPath '%PROJECT_NAME%_v%VERSION%.zip' -Force"
)

cd ..

echo.
echo ========================================
echo   构建和打包完成！
echo ========================================
echo.
echo 输出文件:
echo   - 可执行文件: %OUTPUT_DIR%\publish\VMCloneApp.exe
echo   - 打包目录: %PACKAGE_DIR%\%PROJECT_NAME%_v%VERSION%
echo   - 压缩包: %PACKAGE_DIR%\%PROJECT_NAME%_v%VERSION%.zip
echo.
echo 打包内容包含:
echo   - VMCloneApp.exe 主程序
echo   - 配置文件模板
echo   - 启动脚本
echo   - 所有依赖文件
echo.

pause