@echo off
echo ========================================
echo    VMCloneApp 构建脚本
echo ========================================
echo.

echo [1/4] 清理之前的构建文件...
if exist "bin\Release" rmdir /s /q "bin\Release"
if exist "Package" rmdir /s /q "Package"

echo [2/4] 恢复NuGet包...
dotnet restore
if %errorlevel% neq 0 (
    echo 错误: NuGet包恢复失败
    pause
    exit /b 1
)

echo [3/4] 构建项目...
dotnet build --configuration Release
if %errorlevel% neq 0 (
    echo 错误: 项目构建失败
    pause
    exit /b 1
)

echo [4/4] 发布项目...
dotnet publish --configuration Release --output "bin\Release\publish" --runtime win-x64 --self-contained true
if %errorlevel% neq 0 (
    echo 错误: 项目发布失败
    pause
    exit /b 1
)

echo.
echo ========================================
echo   构建完成！
echo ========================================
echo.
echo 输出文件:
echo   - 可执行文件: bin\Release\publish\VMCloneApp.exe
echo.
echo 构建成功！
pause