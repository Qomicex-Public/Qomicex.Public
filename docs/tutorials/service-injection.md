# 服务注入教程

本教程将指导你使用主程序提供的各种服务。

## 目标

创建一个插件，使用 IToastService、IConfigurationService 等服务。

## 步骤 1：创建插件类

```csharp
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using Qomicex.Launcher.Services;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.services", "服务示例插件", "1.0.0")]
public class ServiceExamplePlugin : IPlugin
{
    private IToastService? _toastService;
    private IConfigurationService? _configService;
    private IFileSelectionService? _fileSelectionService;

    public string Id => "qomicex.example.services";
    public string Name => "服务示例插件";
    public Version Version => new(1, 0, 0);
    public string Description => "演示如何使用主程序服务";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        // 获取服务
        _toastService = services.GetService(typeof(IToastService)) as IToastService;
        _configService = services.GetService(typeof(IConfigurationService)) as IConfigurationService;
        _fileSelectionService = services.GetService(typeof(IFileSelectionService)) as IFileSelectionService;

        // 验证服务
        if (_toastService == null)
        {
            return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
            {
                Code = "SERVICE_NOT_FOUND",
                Message = "IToastService 不可用"
            }));
        }

        // 显示初始化消息
        _toastService.Success($"[{Name}] 初始化成功！");

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        _toastService?.Info($"[{Name}] 已关闭");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
```

## 可用服务

| 服务接口 | 描述 |
|-----------|------|
| `IToastService` | 显示通知消息 |
| `IConfigurationService` | 配置持久化 |
| `IFileSelectionService` | 文件选择对话框 |
| `RunningInstanceManager` | 运行中的实例管理 |

## 使用 IToastService

```csharp
public void ShowNotification()
{
    _toastService?.Success("操作成功！");
    _toastService?.Error("操作失败");
    _toastService?.Info("提示信息");
    _toastService?.Warning("警告信息");
}
```

## 使用 IConfigurationService

```csharp
public class PluginSettings
{
    public bool EnableFeature { get; set; }
    public string ApiKey { get; set; }
}

private PluginSettings _settings = new();

public void LoadSettings()
{
    if (_configService != null)
    {
        _settings = _configService.Load<PluginSettings>(Id) ?? new PluginSettings();
    }
}

public void SaveSettings()
{
    _configService?.Save(_settings, Id);
}
```

## 使用 IFileSelectionService

```csharp
public async Task SelectFileAsync()
{
    if (_fileSelectionService != null)
    {
        var files = await _fileSelectionService.SelectFilesAsync(
            "选择文件",
            new[] { "*.exe", "*.dll" });

        if (files != null && files.Count > 0)
        {
            _toastService?.Success($"已选择: {files[0]}");
        }
    }
}
```

## 最佳实践

### 1. 服务可用性检查

```csharp
public void SafeShowMessage(string message)
{
    if (_toastService != null)
    {
        _toastService.Info(message);
    }
}
```

### 2. 缓存服务引用

```csharp
private IToastService? _toastService;

public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    _toastService = services.GetService(typeof(IToastService)) as IToastService;
    return Task.FromResult(Result<bool, PluginError>.Success(true));
}
```

### 3. 使用扩展方法

```csharp
// 如果有扩展方法
public class ServiceExtensions
{
    public static T? GetRequiredService<T>(this IServiceProvider provider)
    {
        var service = provider.GetService(typeof(T)) as T;
        if (service == null)
        {
            throw new InvalidOperationException($"Service {typeof(T).Name} not found");
        }
        return service;
    }
}

// 使用
_toastService = services.GetRequiredService<IToastService>();
```

## 下一步

- [配置存储教程](advanced/config-storage.md)
- [事件处理教程](advanced/event-handling.md)
