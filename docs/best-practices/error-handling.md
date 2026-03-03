# 错误处理

良好的错误处理是创建可靠插件的关键。本文档介绍插件开发中的错误处理最佳实践。

## 使用 Result 类型

插件方法使用 `Result<TValue, TError>` 类型返回结果，避免抛出异常。

### 成功返回

```csharp
return Task.FromResult(Result<bool, PluginError>.Success(true));
```

### 失败返回

```csharp
return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
{
    Code = "ERROR_CODE",
    Message = "错误描述"
}));
```

## 标准错误代码

| 代码类别 | 错误代码 | 描述 |
|----------|----------|------|
| 服务 | `SERVICE_NOT_FOUND` | 服务不可用 |
| 配置 | `INVALID_CONFIG` | 配置无效 |
| 初始化 | `INIT_FAILED` | 初始化失败 |
| 网络 | `NETWORK_ERROR` | 网络错误 |
| 权限 | `PERMISSION_DENIED` | 权限不足 |

## 错误处理模式

### 1. Try-Catch 模式

```csharp
public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    try
    {
        // 可能抛出异常的代码
        InitializeServices(services);
        
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
    catch (Exception ex)
    {
        return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
        {
            Code = "INIT_ERROR",
            Message = ex.Message
        }));
    }
}
```

### 2. 条件检查模式

```csharp
public Task<Result<bool, PluginError>> ValidateConfiguration()
{
    if (!HasValidApiKey())
    {
        return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
        {
            Code = "INVALID_API_KEY",
            Message = "API 密钥无效"
        }));
    }

    return Task.FromResult(Result<bool, PluginError>.Success(true));
}
```

### 3. Result 链式模式

```csharp
public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    return Result<bool, PluginError>.Success(true)
        .Bind(x => ValidateConfiguration())
        .Bind(x => LoadDependencies(services))
        .Bind(x => InitializeResources());
}
```

## 错误消息

### 提供清晰的错误消息

```csharp
// 好的错误消息
"IToastService 服务不可用，请联系插件开发者"
"API 密钥无效，请检查设置"

// 不好的错误消息
"Error"
"Failed操作失败
```

### 本地化错误消息

```csharp
public class LocalizedErrors
{
    public static string ServiceNotFound(string serviceName)
    {
        // 根据当前语言返回本地化消息
        return $"{serviceName} 服务不可用";
    }
}
```

## 错误日志

### 记录错误信息

```csharp
public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    try
    {
        // 初始化逻辑
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
    catch (Exception ex)
    {
        // 记录错误
        Console.WriteLine($"[{Name}] 初始化失败: {ex.Message}");
        Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");

        return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
        {
            Code = "INIT_ERROR",
            Message = ex.Message
        }));
    }
}
```

### 使用日志框架

```csharp
// 如果有日志服务
private ILogger? _logger;

public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    _logger = services.GetService(typeof(ILogger)) as ILogger;

    try
    {
        // 初始化逻辑
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, "插件初始化失败");

        return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
        {
            Code = "INIT_ERROR",
            Message = ex.Message
        }));
    }
}
```

## 用户反馈

### 使用 ToastService 显示错误

```csharp
public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    var toastService = services.GetService(typeof(IToastService)) as IToastService;

    try
    {
        // 初始化逻辑
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
    catch (Exception ex)
    {
        // 显示错误消息
        toastService?.Error($"初始化失败: {ex.Message}");

        return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
        {
            Code = "INIT_ERROR",
            Message = ex.Message
        }));
    }
}
```

## 最佳实践总结

1. **使用 Result 类型**：避免抛出异常
2. **提供清晰的错误消息**：帮助用户理解问题
3. **使用标准错误代码**：便于错误分类和处理
4. **记录错误信息**：便于调试
5. **提供用户反馈**：让用户知道发生了什么
6. **恢复策略**：考虑从错误中恢复

## 相关资源
- [PluginError API](../api/models/PluginError.md)
- [Result 类型 API](../api/core-interfaces/Result.md)
