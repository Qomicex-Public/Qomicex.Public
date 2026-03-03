# 事件处理教程

本教程将指导你响应应用程序事件。

## 目标

创建一个插件，监听并响应实例启动和停止事件。

## 步骤 1：创建插件类

```csharp
using Qomicex.PluginSDK.Attributes;
using Qomicexares.Interfaces;
using Qomicex.PluginSDK.Models;
using Qomicex.Launcher.Services;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.events", "事件处理示例", "1.0.0")]
public class EventHandlingPlugin : IPlugin
{
    private RunningInstanceManager? _instanceManager;

    public string Id => "qomicex.example.events";
    public string Name => "事件处理示例";
    public Version Version => new(1, 0, 0);
    public string Description => "演示事件处理";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _instanceManager = services.GetService(typeof(RunningInstanceManager)) as RunningInstanceManager;

        if (_instanceManager != null)
        {
            // 订阅事件
            _instanceManager.InstanceStarted += OnInstanceStarted;
            _instanceManager.InstanceStopped += OnInstanceStopped;
            _instanceManager.InstanceStateChanged += OnInstanceStateChanged;

            Console.WriteLine($"[{Name}] 事件监听器已注册");
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        if (_instanceManager != null)
        {
            // 取消订阅
            _instanceManager.InstanceStarted -= OnInstanceStarted;
            _instanceManager.InstanceStopped -= OnInstanceStopped;
            _instanceManager.InstanceStateChanged -= OnInstanceStateChanged;
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    private void OnInstanceStarted(object? sender, RunningInstanceInfo e)
    {
        Console.WriteLine($"[事件] 实例启动: {e.InstanceName}");
        Console.WriteLine($"  会话 ID: {e.SessionId}");
        Console.WriteLine($"  游戏目录: {e.GameDir}");
    }

    private void OnInstanceStopped(object? sender, RunningInstanceInfo e)
    {
        Console.WriteLine($"[事件] 实例停止: {e.InstanceName}");
        Console.WriteLine($"  运行时长: {e.Duration}");
    }

    private void OnInstanceStateChanged(object? sender, RunningInstanceInfo e)
    {
        Console.WriteLine($"[事件] 实例状态变更: {e.InstanceName} -> {e.State}");
    }
}
```

## 可用事件

| 事件 | 触发时机 | 参数 |
|------|----------|------|
| `InstanceStarted` | 实例启动 | `RunningInstanceInfo` |
| `InstanceStopped` | 实例停止 | `RunningInstanceInfo` |
| `InstanceStateChanged` | 状态变更 | `RunningInstanceInfo` |

## RunningInstanceInfo 属性

| 属性 | 类型 | 描述 |
|------|------|------|
| `SessionId` | string | 会话 ID |
| `InstanceId` | string | 实例 ID |
| `InstanceName` | string | 实例名称 |
| `GameDir` | string | 游戏目录 |
| `State` | InstanceRunningState | 当前状态 |
| `StartTime` | DateTime | 启动时间 |
| `Duration` | TimeSpan | 运行时长 |

## 最佳实践

### 1. 取消订阅事件

```csharp
public Task<Result<bool, PluginError>> ShutdownAsync()
{
    if (_instanceManager != null)
    {
        _instanceManager.InstanceStarted -= OnInstanceStarted;
        _instanceManager.InstanceStopped -= OnInstanceStopped;
    }

    return Task.FromResult(Result<bool, PluginError>.Success(true));
}
```

### 2. 检查发送者

```csharp
private void OnInstanceStarted(object? sender, RunningInstanceInfo e)
{
    var manager = sender as RunningInstanceManager;
    if (manager != null)
    {
        // 处理事件
    }
}
```

### 3. 使用弱事件模式

对于长时间运行的插件，考虑使用弱事件模式防止内存泄漏：

```csharp
// 使用弱事件管理器
private WeakEventManager<InstanceEventArgs>? _weakEventManager;
```

## 下一步

- [最佳实践](../../best-practices/index.md)
- [API 参考](../../api/index.md)
