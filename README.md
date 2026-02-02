# VMCloneApp - macOS虚拟机克隆工具

## 📋 项目简介

VMCloneApp 是一个专业的 macOS 虚拟机克隆管理工具，支持批量克隆、配置管理和自动化操作。

### ✨ 主要功能

- **批量虚拟机克隆** - 支持快速批量创建 macOS 虚拟机
- **五码配置管理** - 集成五码配置文件和自动分配
- **AppleID管理** - AppleID配置文件和批量管理
- **发信管理** - 邮件模板和号码模板管理
- **配置管理** - 完整的偏好设置和配置持久化
- **日志记录** - 详细的操作日志和错误追踪

## 🚀 快速开始

### 系统要求

- **操作系统**: Windows 10/11 (64位)
- **.NET版本**: .NET 6.0 Runtime
- **虚拟机软件**: VMware Workstation 或 VirtualBox
- **内存**: 最低 8GB RAM (推荐 16GB+)
- **存储**: 至少 50GB 可用空间

### 安装步骤

1. **下载发布包**
   - 从 Releases 页面下载最新版本的 ZIP 包
   - 解压到任意目录（建议不要放在系统盘）

2. **安装依赖**
   - 确保已安装 .NET 6.0 Runtime
   - 安装 VMware Workstation 或 VirtualBox

3. **首次运行**
   - 双击 `启动VMCloneApp.bat` 或直接运行 `VMCloneApp.exe`
   - 首次运行会自动创建默认配置文件

## ⚙️ 详细配置说明

### 1. 克隆配置选项卡

#### 母盘选择
- **功能**: 选择要克隆的 macOS 虚拟机母盘
- **配置项**: `MotherDisk`
- **默认值**: `macOS-14.1`
- **文件位置**: `C:\VM\MotherDisks\`

#### 克隆数量
- **功能**: 设置每次克隆的虚拟机数量
- **配置项**: `CloneCount`
- **默认值**: `5`
- **范围**: 1-50

#### 五码配置
- **功能**: 选择五码配置文件
- **配置项**: `WumaConfig`
- **可用数量**: 实时显示当前配置文件的可用数量
- **文件位置**: `C:\WumaConfigs\`

#### VM命名模式
- **功能**: 设置虚拟机命名规则
- **配置项**: `NamingPattern`
- **默认值**: `macos{版本}_timestamp_snapshot_index`
- **变量支持**: 
  - `{版本}`: macOS版本号
  - `{timestamp}`: 时间戳
  - `{snapshot}`: 快照编号
  - `{index}`: 虚拟机索引

#### 目录配置
- **母盘目录**: `MotherDiskDirectory` (默认: `C:\VM\MotherDisks`)
- **克隆VM目录**: `CloneVMDirectory` (默认: `C:\VM\Clones`)

### 2. 五码配置选项卡

#### 五码文件管理
- **功能**: 管理五码配置文件
- **配置项**: `WumaFile`
- **可用数量**: 实时显示当前文件的可用五码数量
- **文件格式**: `.txt` 文件，每行一个五码配置

#### 默认配置
- **功能**: 设置默认五码配置
- **配置项**: `DefaultWuma`
- **可用数量**: 实时显示默认配置的可用数量

#### 目录配置
- **功能**: 设置五码配置文件目录
- **配置项**: `WumaConfigDirectory`
- **默认值**: `C:\WumaConfigs`

### 3. AppleID配置选项卡

#### AppleID文件管理
- **功能**: 管理AppleID配置文件
- **配置项**: `AppleIdFile`
- **可用数量**: 实时显示当前文件的可用AppleID数量
- **文件格式**: `.txt` 文件，每行一个AppleID配置

#### 默认配置
- **功能**: 设置默认AppleID配置
- **配置项**: `DefaultAppleId`
- **可用数量**: 实时显示默认配置的可用数量

#### 目录配置
- **功能**: 设置AppleID配置文件目录
- **配置项**: `AppleIdConfigDirectory`
- **默认值**: `C:\AppleIdConfigs`

### 4. 发信管理选项卡

#### 邮件模板
- **功能**: 选择邮件模板
- **配置项**: `EmailTemplate`
- **默认值**: `macOS克隆完成通知`

#### 发送间隔
- **功能**: 设置邮件发送间隔（分钟）
- **配置项**: `EmailInterval`
- **默认值**: `3`

#### 号码模板文件
- **功能**: 选择号码模板文件
- **配置项**: `NumberTemplateFile`
- **号码数量**: 实时显示当前文件的号码数量
- **文件格式**: `.txt` 文件，每行一个号码

#### 模板目录配置
- **功能**: 设置号码模板文件目录
- **配置项**: `NumberTemplateDirectory`
- **默认值**: `C:\NumberTemplates`

### 5. 虚拟机软件配置

#### 虚拟机目录
- **功能**: 设置虚拟机软件安装目录
- **配置项**: `VMDirectory`
- **默认值**: `C:\Program Files (x86)\VMware\VMware Workstation`

### 6. 系统配置

#### 自动启动
- **功能**: 克隆完成后是否自动启动虚拟机
- **配置项**: `AutoStartVM`
- **默认值**: `true`

#### 日志记录
- **功能**: 启用/禁用日志记录
- **配置项**: `EnableLogging`
- **默认值**: `true`

#### 最大并发克隆数
- **功能**: 设置同时进行的最大克隆数量
- **配置项**: `MaxConcurrentClones`
- **默认值**: `3`

#### 日志级别
- **功能**: 设置日志记录级别
- **配置项**: `LogLevel`
- **可选值**: `DEBUG`, `INFO`, `WARN`, `ERROR`
- **默认值**: `INFO`

## 📁 文件结构说明

### 配置文件
```
应用程序目录/
├── VMCloneApp.exe          # 主程序
├── vmclone_config.xml      # 用户配置文件（自动生成）
└── ConfigTemplates/        # 配置模板目录
    └── vmclone_config_template.xml  # 配置模板
```

### 数据目录结构
```
C:\VM\                     # 虚拟机根目录
├── MotherDisks/           # 母盘目录
│   └── macOS-14.1.vmx     # macOS母盘文件
├── Clones/                # 克隆虚拟机目录
│   ├── macos14.1_001/     # 克隆虚拟机1
│   ├── macos14.1_002/     # 克隆虚拟机2
│   └── ...
├── WumaConfigs/           # 五码配置目录
│   ├── config1.txt        # 五码配置文件1
│   ├── config2.txt        # 五码配置文件2
│   └── ...
├── AppleIdConfigs/        # AppleID配置目录
│   ├── appleid1.txt       # AppleID配置文件1
│   ├── appleid2.txt       # AppleID配置文件2
│   └── ...
└── NumberTemplates/       # 号码模板目录
    ├── numbers.txt        # 号码模板文件
    └── ...
```

## 🔧 配置文件格式

### 五码配置文件格式
```txt
设备标识1,IMEI1,序列号1,型号1,版本1
设备标识2,IMEI2,序列号2,型号2,版本2
...
```

### AppleID配置文件格式
```txt
appleid1@email.com,密码1
appleid2@email.com,密码2
...
```

### 号码模板文件格式
```txt
+8613800138000
+8613800138001
+8613800138002
...
```

## 🛠️ 构建和打包

### 使用构建脚本
```bash
# 运行构建和打包脚本
build_and_package.bat
```

### 手动构建步骤
```bash
# 1. 恢复NuGet包
dotnet restore

# 2. 构建项目
dotnet build --configuration Release

# 3. 发布项目
dotnet publish --configuration Release --output bin/Release/publish --runtime win-x64 --self-contained true
```

## 📊 使用指南

### 基本操作流程

1. **准备阶段**
   - 配置母盘虚拟机
   - 准备五码配置文件
   - 准备AppleID配置文件
   - 设置相关目录路径

2. **配置设置**
   - 打开偏好设置
   - 配置各选项卡参数
   - 保存配置

3. **执行克隆**
   - 在主界面设置克隆参数
   - 开始批量克隆
   - 监控克隆进度

4. **后续管理**
   - 查看克隆结果
   - 管理克隆的虚拟机
   - 处理异常情况

### 最佳实践

1. **目录规划**
   - 为不同类型的文件创建专用目录
   - 避免使用系统盘存储大量虚拟机文件
   - 定期备份重要配置文件

2. **配置管理**
   - 使用有意义的配置文件名
   - 定期清理无效配置
   - 备份配置文件

3. **性能优化**
   - 根据硬件配置调整并发克隆数
   - 使用SSD存储提高克隆速度
   - 合理分配内存资源

## ❗ 常见问题

### Q: 克隆过程中出现错误怎么办？
A: 检查日志文件中的错误信息，确保：
- 母盘虚拟机状态正常
- 有足够的磁盘空间
- 虚拟机软件正常运行

### Q: 五码配置无法加载？
A: 检查：
- 配置文件路径是否正确
- 文件格式是否符合要求
- 文件编码是否为UTF-8

### Q: 配置文件保存失败？
A: 检查：
- 应用程序是否有写入权限
- 磁盘空间是否充足
- 配置文件是否被其他程序占用

## 📞 技术支持

如有问题或建议，请通过以下方式联系：
- 查看项目文档
- 提交Issue报告问题
- 查看日志文件获取详细信息

## 📄 许可证

本项目采用 MIT 许可证，详情请查看 LICENSE 文件。

---

**最后更新**: 2026-02-02  
**版本**: 1.0.0  
**开发者**: VMCloneApp Team