@echo off
chcp 65001 >nul

echo ================================
echo   VMCloneApp Build Script
echo ================================
echo.

echo [1/4] Cleaning previous build...
if exist "bin\Release" rmdir /s /q "bin\Release"
if exist "Package" rmdir /s /q "Package"

echo [2/4] Restoring NuGet packages...
dotnet restore
if %errorlevel% neq 0 (
    echo Error: NuGet package restore failed
    pause
    exit /b 1
)

echo [3/4] Building project...
dotnet build --configuration Release
if %errorlevel% neq 0 (
    echo Error: Project build failed
    pause
    exit /b 1
)

echo [4/4] Publishing project...
dotnet publish --configuration Release --output "bin\Release\publish" --runtime win-x64 --self-contained true
if %errorlevel% neq 0 (
    echo Error: Project publish failed
    pause
    exit /b 1
)

echo.
echo ================================
echo   Build Completed Successfully!
echo ================================
echo.
echo Output files:
echo   - Executable: bin\Release\publish\VMCloneApp.exe
echo.
echo Build successful!
pause