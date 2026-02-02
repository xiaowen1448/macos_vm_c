@echo off
chcp 65001 >nul

echo ============================================
echo   VMCloneApp Packaging Script
echo ============================================
echo.

set VERSION=1.0.0
set PACKAGE_DIR=Package
set APP_NAME=VMCloneApp

:: Check if build exists
if not exist "bin\Release\publish\VMCloneApp.exe" (
    echo Error: Build not found. Please run build_simple.bat first.
    pause
    exit /b 1
)

echo [1/6] Creating package directory...
if exist "%PACKAGE_DIR%" rmdir /s /q "%PACKAGE_DIR%"
mkdir "%PACKAGE_DIR%"
mkdir "%PACKAGE_DIR%\%APP_NAME%_v%VERSION%"

echo [2/6] Copying application files...
xcopy "bin\Release\publish\*.*" "%PACKAGE_DIR%\%APP_NAME%_v%VERSION%\" /Y /E /I

echo [3/6] Creating configuration template...
mkdir "%PACKAGE_DIR%\%APP_NAME%_v%VERSION%\ConfigTemplates"

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
) > "%PACKAGE_DIR%\%APP_NAME%_v%VERSION%\ConfigTemplates\vmclone_config_template.xml"

echo [4/6] Creating sample data files...
mkdir "%PACKAGE_DIR%\%APP_NAME%_v%VERSION%\SampleData"

:: Create sample wuma config file
(
echo device001,123456789012345,SN001,iPhone14,16.0
echo device002,123456789012346,SN002,iPhone14,16.0
echo device003,123456789012347,SN003,iPhone14,16.0
) > "%PACKAGE_DIR%\%APP_NAME%_v%VERSION%\SampleData\sample_wuma.txt"

:: Create sample appleid config file
(
echo test1@apple.com,password123
echo test2@apple.com,password123
echo test3@apple.com,password123
) > "%PACKAGE_DIR%\%APP_NAME%_v%VERSION%\SampleData\sample_appleid.txt"

:: Create sample number template file
(
echo +8613800138000
echo +8613800138001
echo +8613800138002
) > "%PACKAGE_DIR%\%APP_NAME%_v%VERSION%\SampleData\sample_numbers.txt"

echo [5/6] Creating startup script...
(
echo @echo off
echo chcp 65001 ^>nul
echo.
echo ============================================
echo   VMCloneApp Startup Script
echo ============================================
echo.
echo Starting VMCloneApp...
echo.
if exist "VMCloneApp.exe" (
    echo Launching VMCloneApp.exe...
    start "" "VMCloneApp.exe"
    echo Application started successfully!
) else (
    echo Error: VMCloneApp.exe not found!
    echo Please ensure the application files are in the same directory.
)
echo.
echo Press any key to close this window...
pause ^>nul
) > "%PACKAGE_DIR%\%APP_NAME%_v%VERSION%\Start_VMCloneApp.bat"

echo [6/6] Creating ZIP package...
cd "%PACKAGE_DIR%"

:: Try using 7-Zip first
if exist "C:\Program Files\7-Zip\7z.exe" (
    "C:\Program Files\7-Zip\7z.exe" a -tzip "%APP_NAME%_v%VERSION%.zip" "%APP_NAME%_v%VERSION%\*" >nul
    if %errorlevel% equ 0 (
        echo Using 7-Zip for compression...
    ) else (
        goto :use_powershell
    )
) else (
    goto :use_powershell
)

goto :zip_complete

:use_powershell
echo Using PowerShell for compression...
powershell -Command "Compress-Archive -Path '%APP_NAME%_v%VERSION%' -DestinationPath '%APP_NAME%_v%VERSION%.zip' -Force"

:zip_complete
cd ..

echo.
echo ============================================
echo   Packaging Completed Successfully!
echo ============================================
echo.
echo Package Contents:
echo   - Application: %APP_NAME%_v%VERSION%\VMCloneApp.exe
echo   - Configuration Template: %APP_NAME%_v%VERSION%\ConfigTemplates\
echo   - Sample Data: %APP_NAME%_v%VERSION%\SampleData\
echo   - Startup Script: %APP_NAME%_v%VERSION%\Start_VMCloneApp.bat
echo   - ZIP Package: %PACKAGE_DIR%\%APP_NAME%_v%VERSION%.zip
echo.
echo Total files packaged: 
dir /s /b "%PACKAGE_DIR%\%APP_NAME%_v%VERSION%" | find /c /v ""
echo.
echo Package ready for distribution!
pause