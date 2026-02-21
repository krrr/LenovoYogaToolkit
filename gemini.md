# LenovoYogaToolkit 项目指南


## 项目概述
一个用于联想YOGA系列笔记本电脑的辅助工具，从Legion Toolkit修改而来，提供各种硬件控制功能（如电源模式、电池养护、屏幕亮度、Fn 键锁定等）。

## 技术栈
- **框架**: .NET 6.0 (WPF)
- **UI 库**: [WPF-UI](https://github.com/lepoco/wpfui) (提供现代化的 Win11 风格界面)
- **依赖注入**: Autofac
- **Windows 集成**: 
  - **Windows App SDK**: 用于现代 Windows 特性（如 Mica 材质、AppWindow 控制）。
  - **CsWin32**: 用于高效生成 P/Invoke 代码。
- **系统/硬件交互**:
  - **WMI**: 访问联想私有接口（驱动程序/UEFI 设置）。
  - **Registry**: 读取/写入系统及驱动配置。
  - **NVAPI**: 通过 `NvAPIWrapper.Net` 控制 NVIDIA 显卡。
  - **TaskScheduler**: 管理计划任务。

## 项目结构
- **LenovoYogaToolkit.WPF**: 界面层。包含窗口 (`Windows/`)、页面 (`Pages/`)、自定义控件 (`Controls/`) 及应用程序设置。
- **LenovoYogaToolkit.Lib**: 核心逻辑层。
  - `Features/`: 具体硬件特性的实现（如 `BatteryFeature.cs`, `FnLockFeature.cs`）。
  - `Listeners/`: 系统事件监听器（WMI 事件、电源状态改变、显示器变更）。
  - `Controllers/`: 高级控制器（如 `PowerPlanController.cs`, `GPUController.cs`）。
  - `System/`: 系统级组件封装（Registry, WMI, NVAPI, Vantage 模拟）。
- **LenovoYogaToolkit.Lib.Automation**: 自动化层。支持游戏检测及基于触发器的自动化操作。

## 关键配置

### WMI 交互
联想的硬件设置多通过 `ROOT\WMI` 命名空间下的 `Lenovo_BiosSetting` 或 `Lenovo_SmartSetting` 等类进行交互。相关逻辑集中在 `LenovoYogaToolkit.Lib\System\WMI.cs`。

## 常用操作
- **清理**: 运行根目录下的 `clean.bat`。
- **构建**: `dotnet build` 或运行 `make.bat`。
- **打包**: 使用 Inno Setup 配合 `make_installer.iss`。
