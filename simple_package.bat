@echo off
chcp 65001 >nul

echo ============================================
echo   VMCloneApp Simple Packaging Script
echo ============================================
echo.

:: Check if build exists
if not exist "bin\Release\publish\VMCloneApp.exe" (
    echo Error: Build not found. Please run build_simple.bat first.
    pause
    exit /b 1
)

echo [1/4] Creating package directory...
if exist "Package" rmdir /s /q "Package"
mkdir "Package"
mkdir "Package\VMCloneApp_v1.0.0"

echo [2/4] Copying application files...
xcopy "bin\Release\publish\*.*" "Package\VMCloneApp_v1.0.0\" /Y /E /I

echo [3/4] Creating startup script...
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
) > "Package\VMCloneApp_v1.0.0\Start_VMCloneApp.bat"

echo [4/4] Creating README file...
(
echo VMCloneApp - macOS Virtual Machine Clone Tool
echo ============================================
echo.
echo Version: 1.0.0
echo Build Date: 2026-02-02
echo.
echo Quick Start:
echo 1. Double-click 'Start_VMCloneApp.bat' to launch the application
echo 2. Configure your settings in Preferences
echo 3. Start cloning macOS virtual machines
echo.
echo System Requirements:
echo - Windows 10/11 (64-bit)
echo - .NET 6.0 Runtime
echo - VMware Workstation or VirtualBox
echo - Minimum 8GB RAM (16GB recommended)
echo.
echo For detailed configuration instructions, see the full README.md file.
) > "Package\VMCloneApp_v1.0.0\QUICK_START.txt"

echo.
echo ============================================
echo   Packaging Completed Successfully!
echo ============================================
echo.
echo Package Contents:
echo   - Application: Package\VMCloneApp_v1.0.0\VMCloneApp.exe
echo   - Startup Script: Package\VMCloneApp_v1.0.0\Start_VMCloneApp.bat
echo   - Quick Start Guide: Package\VMCloneApp_v1.0.0\QUICK_START.txt
echo.
echo Total files packaged: 
dir /s /b "Package\VMCloneApp_v1.0.0" | find /c /v ""
echo.
echo Package ready for distribution!
pause