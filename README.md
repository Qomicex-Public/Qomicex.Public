# Qomicex Plugin SDK

## 简介

Qomicex Plugin SDK 是为 Qomicex 启动器开发的插件系统，允许开发者创建功能丰富的插件来扩展启动器的功能。通过本 SDK，你可以：

- 创建自定义主题
- 添加设置页面
- 使用主程序提供的各种服务
- 响应应用生命周期事件

## 快速开始

### 最小化插件示例

```csharp
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.myplugin", "我的插件", "1.0.0")]
public class MyPlugin : IPlugin
{
    public string Id => "qomicex.example.myplugin";
    public string Name => "我的插件";
    public Version Version => new(1, 0, 0);
    public string Description => "我的第一个 Qomicex 插件";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        Console.WriteLine("插件初始化成功！");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        Console.WriteLine("插件已关闭");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
```

## 核心特性

| 功能 | 描述 |
|------|------|
| **主题扩展** | 通过 `IThemeProvider` 接口创建自定义主题 |
| **设置设置** | 通过 `ISettingsPageProvider` 接口添加插件设置页面 |
| **服务注入** | 通过 IServiceProvider 访问主程序提供的各种服务 |
| **生命周期管理** | 完整的插件初始化和关闭流程 |
| **错误处理** | 基于 Result 类型的 Railway-Oriented Programming 错误处理 |

## 可用服务

插件可以通过 `IServiceProvider` 获取以下服务：

- `IToastService` - 显示通知消息
- `IConfigurationService` - 配置持久化服务
- `IFileSelectionService` - 文件选择对话框

## 文档导航

- [快速开始](docs/getting-started/index.md) - 环境配置和创建第一个插件
- [生命周期](docs/lifecycle/index.md) - 了解插件的生命周期管理
- [API 参考](docs/api/index.md) - 详细的接口和类说明
- [教程](docs/tutorials/index.md) - 从简单到复杂的插件开发教程
- [最佳实践](docs/best-practices/index.md) - 开发规范和注意事项
- [示例代码](docs/examples/index.md) - 可直接运行的完整示例

## 系统要求

- .NET 10.0 或更高版本
- Qomicex Launcher 最新版本

## 安装 SDK

通过 NuGet 安装：

```bash
dotnet add package Qomicex.PluginSDK
```

## 获取帮助

- [GitHub Issues](https://github.com/qomicex/qomicex-issues)
- [Discord 社区](https://discord.gg/qomicex)

## 许可证

MIT License
