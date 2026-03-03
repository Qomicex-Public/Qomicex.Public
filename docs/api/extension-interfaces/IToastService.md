# IToastService 接口

`IToastService` 接口提供显示通知消息的功能，插件可以通过此服务向用户显示各种类型的通知。

## 命名空间

```csharp
namespace Qomicex.PluginSDK.Interfaces
```

## 接口定义

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

### Show

```csharp
void Show(string message, NotificationType type = NotificationType.Information)
```

显示指定类型的通知消息。

**参数：**

| 参数 | 类型 | 描述 |
|------|------|------|
| `message` | string | 要显示的消息内容 |
| `type` | NotificationType | 通知类型（默认为 Information） |

**NotificationType 枚举值：**

| 值 | 描述 |
|-----|------|
| `Information` | 信息提示（蓝色） |
| `Success` | 成功消息（绿色） |
| `Warning` | 警告消息（黄色） |
| `Error` | 错误消息（红色） |

### Success

```csharp
void Success(string message)
```

显示成功类型的绿色通知。

**示例：**

```csharp
_toastService?.Success("操作成功完成！");
```

### Error

```csharp
void Error(string message)
```

显示错误类型的红色通知。

**示例：**

```csharp
_toastService?.Error("操作失败，请重试");
```

### Info

```csharp
void Info(string message)
```

显示信息类型的蓝色通知。

**示例：**

```csharp
_toastService?.Info("插件已加载");
```

### Warning

```csharp
void Warning(string message)
```

显示警告类型的黄色通知。

**示例：**

```csharp
_toastService?.Warning("配置即将过期，请更新");
```

## 使用示例

### 1. 获取服务

```csharp
public class MyPlugin : IPlugin
{
    private IToastService? _toastService;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        // 获取 Toast 服务
        _toastService = services.GetService(typeof(IToastService)) as IToastService;
        
        // 验证服务
        if (_toastService == null)
        {
            Console.WriteLine("IToastService 不可用");
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
```

### 2. 显示各种通知

```csharp
public void PerformOperation()
{
    try
    {
        // 显示加载信息
        _toastService?.Info("正在处理...");
        
        // 执行操作
        var result = DoWork();
        
        // 显示成功消息
        _toastService?.Success("操作成功！");
    }
    catch (Exception ex)
    {
        // 显示错误消息
        _toastService?.Error($"操作失败: {ex.Message}");
    }
}

public void CheckConfiguration()
{
    if (!HasValidConfiguration())
    {
        _toastService?.Warning("配置无效，使用默认值");
    }
}
```

### 3. 使用 Show 方法

```csharp
public void ShowNotification(NotificationType type, string message)
{
    _toastService?.Show(message, type);
}

// 使用示例
ShowNotification(NotificationType.Success, "操作成功");
ShowNotification(NotificationType.Error, "发生错误");
ShowNotification(NotificationType.Warning, "警告信息");
ShowNotification(NotificationType.Information, "提示信息");
```

## 最佳实践

### 1. 服务可用性检查

```csharp
public void SafeShowSuccess(string message)
{
    _toastService?.Success(message);
}

public void SafeShowError(string message)
{
    _toastService?.Error(message);
    
    // 同时输出到控制台
    Console.WriteLine($"[ERROR] {message}");
}
```

### 2. 格式化消息

```csharp
public void ShowProgress(int current, int total, string item)
{
    var percentage = (current * 100) / total;
    _toastService?.Info($"处理中: {item} ({percentage}%)");
}

public void ShowOperationResult(string operation, bool success, TimeSpan duration)
{
    var status = success ? "成功" : "失败";
    var type = success ? NotificationType.Success : NotificationType.Error;
    _toastService?.Show($"{operation} {status} (耗时: {duration.TotalSeconds:F2}s)", type);
}
```

### 3. 避免频繁通知

```csharp
private DateTime _lastNotificationTime = DateTime.MinValue;
private const int NotificationInterval = 1000; // 毫秒

public void ShowThrottledNotification(string message)
{
    var now = DateTime.Now;
    var elapsed = (now - _lastNotificationTime).TotalMilliseconds;

    if (elapsed >= NotificationInterval)
    {
        _toastService?.Info(message);
        _lastNotificationTime = now;
    }
}
```

## 注意事项

1. **服务可用性**：始终检查服务是否可用（使用 null 条件运算符）
2. **消息长度**：避免过长的消息，保持简洁明了
3. **频率控制**：避免频繁显示通知，可能影响用户体验
4. **线程安全**：通知 UI 线程安全，但建议在同一线程调用

## 相关文档

- [IPlugin](../core-interfaces/IPlugin.md)
- [服务注入教程](../../tutorials/service-injection.md)
- [可用服务](../../services/index.md)
