# Hello World 教程

本教程将指导你创建一个简单的 Hello World 插件，这是学习插件开发的最佳起点。

## 目标

创建一个简单的插件，在加载时显示欢迎消息。

## 步骤 1：创建项目

```bash
# 创建新的类库项目
dotnet new classlib -n HelloWorldPlugin
cd HelloWorldPlugin

# 添加插件 SDK
dotnet add package Qomicex.PluginSDK
```

## 步骤 2：创建插件类

创建 `HelloWorldPlugin.cs` 文件：

```csharp
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.helloworld", "Hello World 插件", "1.0.0",
    Description = "我的第一个 Qomicex 插件")]
public class HelloWorldPlugin : IPlugin
{
    public string Id => "qomicex.example.helloworld";
    public string Name => "Hello World 插件";
    public Version Version => new(1, 0, 0);
    public string Description => "我的第一个 Qomicex 插件";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        Console.WriteLine("=====================================");
        Console.WriteLine("Hello World 插件已加载！");
        Console.WriteLine($"插件名称: {Name}");
        Console.WriteLine($"插件版本: {Version}");
        Console.WriteLine($"插件 ID: {Id}");
        Console.WriteLine("=====================================");

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        Console.WriteLine($"[{Name}] 插件已卸载");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
```

## 步骤 3：构建项目

```bash
dotnet build
```

## 步骤 4：部署插件

将生成的 `HelloWorldPlugin.dll` 复制到 Qomicex Launcher 的 `Plugins` 目录。

## 步骤 5：运行测试

启动 Qomicex Launcher，查看控制台输出，应该能看到欢迎消息。

## 代码解释

### PluginAttribute

```csharp
[Plugin("qomicex.example.helloworld", "Hello World 插件", "1.0.0")]
```

这个特性标记了插件类，并提供了元数据：
- 插件 ID：`qomicex.example.helloworld`
- 插件名称：`Hello World 插件`
- 插件版本：`1.0.0`

### IPlugin 接口实现

插件必须实现以下属性：

| 属性 | 值 |
|------|-----|
| `Id` | 插件唯一标识符 |
| `Name` | 插件显示名称 |
| `Version` | 插件版本号 |
| `Description` | 插件描述 |

### InitializeAsync 方法

插件加载时调用，用于初始化插件。

### ShutdownAsync 方法

插件卸载或应用关闭时调用，用于清理资源。

## 下一步

- [主题插件教程](theme-plugin.md)
- [设置页插件教程](settings-plugin.md)
- [服务注入教程](service-injection.md)
