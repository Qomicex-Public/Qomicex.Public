# 可用服务概览

Qomicex Launcher 通过 `IServiceProvider` 向插件提供各种服务。插件可以在 `InitializeAsync` 方法中获取这些服务。

## 服务列表

| 服务接口 | 描述 |
|-----------|------|
| [IToastService](IToastService.md) | 显示通知消息 |
| [IConfigurationService](IConfigurationService.md) | 配置持久化服务 |
| [IFileSelectionService](IFileSelectionService.md) | 文件选择对话框服务 |
| [RunningInstanceManager](RunningInstanceManager.md) | 运行中的实例管理器 |

## 获取服务

### 在初始化时获取服务

```csharp
public class MyPlugin : IPlugin
{
    private IToastService? _toastService;
    private IConfigurationService? _configService;
    private IFileSelectionService? _fileSelectionService;
    private RunningInstanceManager? _instanceManager;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        // 获取各种服务
        _toastService = services.GetService(typeof(IToastService)) as IToastService;
        _configService = services.GetService(typeof(IConfigurationService)) as IConfigurationService;
        _fileSelectionService = services.GetService(typeof(IFileSelectionService)) as IFileSelectionService;
        _instanceManager = services.GetService(typeof(RunningInstanceManager)) as RunningInstanceManager;

        // 验证服务
        if (_toastService == null)
        {
            return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
            {
                Code = "SERVICE_NOT_FOUND",
                Message = "IToastService 不可用"
            }));
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
```

### 服务可用性检查

始终检查服务是否可用：

```csharp
public void ShowMessage(string message)
{
    if (_toastService != null)
    {
        _toastService.Info(message);
    }
    else
    {
        Console.WriteLine(message);
    }
}
```

## 服务生命周期

- 服务在插件初始化时提供
- 服务引用应该在插件关闭前释放
- 某些服务可能有生命周期限制

## 相关文档

- [服务注入教程](../tutorials/service-injection.md)
- [IPlugin](../api/core-interfaces/IPlugin.md)
- [生命周期](../lifecycle/index.md)
