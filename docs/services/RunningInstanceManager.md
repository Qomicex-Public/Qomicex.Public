# RunningInstanceManager 服务

`RunningInstanceManager` 负责管理同时运行的多个 Minecraft 实例。

## 命名空间

```csharp
namespace Qomicex.Launcher.Services
```

## 服务类

```csharp
public class RunningInstanceManager : ObservableObject
{
    public ObservableCollection<RunningInstanceInfo> Instances { get; }
    public int RunningCount { get; }
    public bool HasRunningInstances { get; }

    public event EventHandler<RunningInstanceInfo>? InstanceStateChanged;
    public event EventHandler<RunningInstanceInfo>? InstanceStarted;
    public event EventHandler<RunningInstanceInfo>? InstanceStopped;

    public RunningInstanceInfo RegisterInstance(string instanceId, string instanceName, string gameDir, string versionDir);
    public RunningInstanceInfo? GetRunningInstance(string sessionId);
    public RunningInstanceInfo[] GetRunningSessions(string instanceId);
    public bool IsInstanceRunning(string instanceId);
    public int GetRunningCount(string instanceId);
    public void UpdateInstanceState(string sessionId, InstanceRunningState state, string? description = null);
    public void SetInstanceProcess(string sessionId, Process? process);
    public void SetLaunchHelper(string sessionId, LaunchHelper launchHelper);
    public void LogOutput(string sessionId, string output);
    public void ReportCrash(string sessionId, string crashInfo);
    public void MarkInstanceExited(string sessionId, int exitCode);
    public Task<bool> StopInstanceAsync(string sessionId);
    public Task<int> StopAllSessionsAsync(string instanceId);
    public Task StopAllInstancesAsync();
    public void CleanupCompletedInstances(TimeSpan? maxAge = null);
    public void Dispose();
}
```

## 主要属性

| 属性 | 类型 | 描述 |
|------|------|------|
| `Instances` | ObservableCollection | 运行中的实例列表 |
| `RunningCount` | int | 运行中的实例数量 |
| `HasRunningInstances` | bool | 是否有实例在运行 |

## 主要事件

| 事件 | 描述 |
|------|------|
| `InstanceStateChanged` | 实例状态变化时触发 |
| `InstanceStarted` | 实例启动时触发 |
| `InstanceStopped` | 实例停止时触发 |

## 使用示例

```csharp
public class MyPlugin : IPlugin
{
    private RunningInstanceManager? _instanceManager;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _instanceManager = services.GetService(typeof(RunningInstanceManager)) as RunningInstanceManager;

        if (_instanceManager != null)
        {
            // 订阅事件
            _instanceManager.InstanceStarted += OnInstanceStarted;
            _instanceManager.InstanceStopped += OnInstanceStopped;
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    private void OnInstanceStarted(object? sender, RunningInstanceInfo e)
    {
    }

    private void OnInstanceStopped(object? sender, RunningInstanceInfo e)
    {
        // 处理实例停止事件
        Console.WriteLine($"实例 {e.InstanceName} 已停止");
    }

    public void CheckRunningInstances()
    {
        if (_instanceManager != null)
        {
            Console.WriteLine($"当前运行实例数: {_instanceManager.RunningCount}");
            
            foreach (var instance in _instanceManager.Instances)
            {
                Console.WriteLine($"- {instance.InstanceName} ({instance.State})");
            }
        }
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        if (_instanceManager != null)
        {
            // 取消订阅
            _instanceManager.InstanceStarted -= OnInstanceStarted;
            _instanceManager.InstanceStopped -= OnInstanceStopped;
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
```

## 相关文档

- [服务注入教程](../tutorials/service-injection.md)
