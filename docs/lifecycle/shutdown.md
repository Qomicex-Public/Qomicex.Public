# 关闭流程

关闭流程确保插件能够正确清理资源并保存状态。本文档详细介绍关闭流程。

## 关闭流程图

关闭过程包含以下步骤：

1. 系统调用 `ShutdownAsync()`
2. 插件清理所有资源
3. 插件保存运行时状态
4. 插件释放所有服务引用
5. 系统卸载插件加载上下文

## ShutdownAsync 方法签名

```csharp
public Task<Result<bool, PluginError>> ShutdownAsync()
```

### 返回值说明

| 类型 | 描述 |
|------|------|
| `Result<bool, PluginError>` | 成功返回 `Success(true)`，失败返回 `Failure(PluginError)` |

## 关闭最佳实践

### 1. 清理资源

```csharp
public class MyPlugin : IPlugin
{
    private HttpClient? _httpClient;
    private Timer? _timer;
    private CancellationTokenSource? _cancellationSource;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        _timer = new Timer(OnTimerTick, null, 1000, 5000);
        _cancellationSource = new CancellationTokenSource();

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        try
        {
            // 停止定时器
            _timer?.Dispose();
            _timer = null;

            // 取消后台任务
            _cancellationSource?.Cancel();
            _cancellationSource?.Dispose();
            _cancellationSource = null;

            // 释放 HttpClient
            _httpClient?.Dispose();
            _httpClient = null;

            return Task.FromResult(Result<bool, PluginError>.Success(true));
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
            {
                Code = "SHUTDOWN_ERROR",
                Message = ex.Message
            }));
        }
    }
}
```

### 2. 保存状态

```csharp
public class MyPlugin : IPlugin
{
    private IConfigurationService? _configService;
    private PluginState _state = new();

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _configService = services.GetService(typeof(IConfigurationService)) as IConfigurationService;
        _state = _configService?.Load<PluginState>(Id) ?? new PluginState();

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        try
        {
            // 保存当前状态
            _state.LastShutdownTime = DateTime.Now;

            if (_configService != null)
            {
                _configService.Save(_state, Id);
            }

            return Task.FromResult(Result<bool, PluginError>.Success(true));
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
            {
                Code = "SAVE_STATE_FAILED",
                Message = $"保存状态失败: {ex.Message}"
            }));
        }
    }
}

public class PluginState
{
    public DateTime LastShutdownTime { get; set; }
    public int RunCount { get; set; }
}
```

### 3. 取消事件订阅

```csharp
public class MyPlugin : IPlugin
{
    private IEventService? _eventService;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _eventService = services.GetService(typeof(IEventService)) as IEventService;

        if (_eventService != null)
        {
            _eventService.InstanceStarted += OnInstanceStarted;
            _eventService.InstanceStopped += OnInstanceStopped;
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        try
        {
            // 取消事件订阅，防止内存泄漏
            if (_eventService != null)
            {
                _eventService.InstanceStarted -= OnInstanceStarted;
                _eventService.InstanceStopped -= OnInstanceStopped;
            }

            return Task.FromResult(Result<bool, PluginError>.Success(true));
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
            {
                Code = "UNSUBSCRIBE_FAILED",
                Message = ex.Message
            }));
        }
    }

    private void OnInstanceStarted(object? sender, InstanceEventArgs e)
    {
        // 事件处理
    }

    private void OnInstanceStopped(object? sender, InstanceEventArgs e)
    {
        // 事件处理
    }
}
```

## 使用 IDisposable

将插件实现 `IDisposable` 可以确保资源正确释放：

```csharp
[Plugin("qomicex.example.myplugin", "我的插件", "1.0.0")]
public class MyPlugin : IPlugin, IDisposable
{
    private bool _disposed = false;
    private HttpClient? _httpClient;
    private Timer? _timer;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _httpClient = new HttpClient();
        _timer = new Timer(OnTimerTick, null, 1000, 5000);

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _timer?.Dispose();
            _httpClient?.Dispose();
        }

        _disposed = true;
    }

    ~MyPlugin()
    {
        Dispose(false);
    }
}
```

## 下一步

- [最佳实践](../best-practices/index.md)
- [API 参考](../api/index.md)
