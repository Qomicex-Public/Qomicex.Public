# IToastService 服务

`IToastService` 提供显示通知消息的功能，插件可以通过此服务向用户显示各种类型的通知。

## 命名空间

```csharp
namespace Qomicex.PluginSDK.Interfaces
```

## 服务接口

```csharp
public interface IToastService
{
    void Show(string message, NotificationType type = NotificationType.Information);
    void Success(string message);
    void Error(string message);
    void Info(string message);
    void Warning(string message);
}
```

## 方法

| 方法 | 描述 |
|------|------|
| `Show(message, type)` | 显示指定类型的通知 |
| `Success(message)` | 显示成功消息（绿色） |
| `Error(message)` | 显示错误消息（红色） |
| `Info(message)` | 显示信息消息（蓝色） |
| `Warning(message)` | 显示警告消息（黄色） |

## 使用示例

```csharp
public class MyPlugin : IPlugin
{
    private IToastService? _toastService;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _toastService = services.GetService(typeof(IToastService)) as IToastService;
        
        if (_toastService != null)
        {
            _toastService.Success("插件初始化成功！");
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public void PerformOperation()
    {
        _toastService?.Info("正在处理...");
        
        // 执行操作...
        
        _toastService?.Success("操作完成！");
    }
}
```

## 相关文档

- [IToastService API](../api/extension-interfaces/IToastService.md)
- [服务注入教程](../tutorials/service-injection.md)
