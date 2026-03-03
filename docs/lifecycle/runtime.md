# 运行时管理

插件初始化成功后进入运行状态。本文档介绍插件运行时的管理。

## 运行时状态

插件在运行时可以处于以下状态：

| 状态 | 描述 |
|------|------|
| **Active** | 插件正常运行 |
| **Paused** | 插件被暂停（可选功能） |
| **Error** | 插件遇到错误但可恢复 |
| **Disabled** | 插件被用户禁用 |

## 访问服务

### 缓存服务引用

推荐在初始化时缓存服务引用：

```csharp
public class MyPlugin : IPlugin
{
    private IToastService? _toastService;
    private IConfigurationService? _configService;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _toastService = services.GetService(typeof(IToastService)) as IToastService;
        _configService = services.GetService(typeof(IConfigurationService)) as IConfigurationService;

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    // 在其他方法中使用缓存的服务
    public void ShowNotification(string message)
    {
        _toastService?.Info(message);
    }
}
```

### 服务可用性检查

始终检查服务是否可用：

```csharp
public void PerformAction()
{
    if (_toastService == null)
    {
        Console.WriteLine("IToastService 不可用");
        return;
    }

    _toastService.Info("操作成功");
}
```

## 后台任务

### 使用 Timer

```csharp
public class MyPlugin : IPlugin
{
    private Timer? _timer;
    private int _tickCount = 0;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        // 创建定时器：延迟1秒启动，每5秒执行一次
        _timer = new Timer(OnTimerTick, null, 1000, 5000);

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        // 停止定时器
        _timer?.Dispose();
        _timer = null;

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    private void OnTimerTick(object? state)
    {
        _tickCount++;
        Console.WriteLine($"定时器触发: {_tickCount}");

        // 执行周期性任务
    }
}
```

### 使用 Task.Run

```csharp
public class MyPlugin : IPlugin
{
    private CancellationTokenSource? _cancellationSource;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _cancellationSource = new CancellationTokenSource();

        // 启动后台任务
        _ = Task.Run(() => BackgroundTask(_cancellationSource.Token));

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        // 取消后台任务
        _cancellationSource?.Cancel();
        _cancellationSource?.Dispose();

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    private async Task BackgroundTask(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // 执行任务
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    break;

                // 任务逻辑
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("后台任务已取消");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"后台任务错误: {ex.Message}");
        }
    }
}
```

## 事件处理

### 订阅应用事件

```csharp
public class MyPlugin : IPlugin
{
    private IEventService? _eventService;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _eventService = services.GetService(typeof(IEventService)) as IEventService;

        if (_eventService != null)
        {
            // 订阅事件
            _eventService.InstanceStarted += OnInstanceStarted;
            _eventService.InstanceStopped += OnInstanceStopped;
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        if (_eventService != null)
        {
            // 取消订阅
            _eventService.InstanceStarted -= OnInstanceStarted;
            _eventService.InstanceStopped -= OnInstanceStopped;
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    private void OnInstanceStarted(object? sender, InstanceEventArgs e)
    {
        Console.WriteLine($"实例启动: {e.InstanceName}");
    }

    private void OnInstanceStopped(object? sender, InstanceEventArgs e)
    {
        Console.WriteLine($"实例停止: {e.InstanceName}");
    }
}
```

## 状态保存

### 保存运行时状态

```csharp
public class MyPlugin : IPlugin
{
    private IConfigurationService? _configService;
    private PluginState _state = new();

PluginState
    {
        public DateTime LastRun { get; set; }
        public int RunCount { get; set; }
    }

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _configService = services.GetService(typeof(IConfigurationService)) as IConfigurationService;

        // 加载状态
        _state = _configService?.Load<PluginState>(Id) ?? new PluginState();

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public void PerformAction()
    {
        // 更新状态
        _state.LastRun = DateTime.Now;
        _state.RunCount++;

        // 保存状态
        _configService?.Save(_state, Id);
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        // 关闭前保存状态
        _configService?.Save(_state, Id);

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
```

## 错误处理

### 运行时错误恢复

```csharp
public class MyPlugin : IPlugin
{
    private bool _isInErrorState = false;
    private DateTime _errorStartTime = DateTime.MinValue;

    public void PerformOperation()
    {
        try
        {
            // 检查错误状态
            if (_isInErrorState)
            {
                var errorDuration = DateTime.Now - _errorStartTime;
                if (errorDuration < TimeSpan.FromMinutes(5))
                {
                    Console.WriteLine("插件处于错误状态，等待恢复");
                    return;
                }
                else
                {
                    // 尝试恢复
                    _isInErrorState = false;
                }
            }

            // 执行操作
            ExecuteOperation();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"操作失败: {ex.Message}");

            // 进入错误状态
            _isInErrorState = true;
            _errorStartTime = DateTime.Now;
        }
    }

    private void ExecuteOperation()
    {
        // 实际操作逻辑
    }
}
```

## 性能监控

### 简单性能计时

```csharp
public class MyPlugin : IPlugin
{
    private readonly Dictionary<string, (int Count, TimeSpan TotalTime)> _performanceMetrics = new();

    public T TrackPerformance<T>(string operationName, Func<T> operation)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            return operation();
        }
        finally
        {
            stopwatch.Stop();

            if (_performanceMetrics.TryGetValue(operationName, out var metric))
            {
                _performanceMetrics[operationName] = (
                    metric.Count + 1,
                    metric.TotalTime + stopwatch.Elapsed
                );
            }
            else
            {
                _performanceMetrics[operationName] = (1, stopwatch.Elapsed);
            }
        }
    }

    public void PrintPerformanceMetrics()
    {
        foreach (var (name, metric) in _performanceMetrics)
        {
            var avgTime = metric.TotalTime.TotalMilliseconds / metric.Count;
            Console.WriteLine($"{name}: {metric.Count} 次, 平均 {avgTime:F2}ms");
        }
    }
}
```

## 下一步

- [关闭流程详解](shutdown.md)
- [最佳实践](../best-practices/index.md)
