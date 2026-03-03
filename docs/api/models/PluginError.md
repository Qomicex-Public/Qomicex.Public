# PluginError 模型

`PluginError` 表示插件操作过程中发生的错误信息。

## 命名空间

```csharp
namespace Qomicex.PluginSDK.Models
```

## 类型定义

```csharp
public readonly record struct PluginError
{
    public required string Code { get; init; }
    public string? Message { get; init; }
}
```

## 属性

| 属性 | 类型 | 必需 | 描述 |
|------|------|------|------|
| `Code` | string | 是 | 错误代码，用于识别错误类型 |
| `Message` | string? | 否 | 错误消息，提供详细的错误描述 |

## 使用示例

### 1. 创建错误

```csharp
var error = new PluginError
{
    Code = "SERVICE_NOT_FOUND",
    Message = "IToastService 服务不可用"
};
```

### 2. 在插件初始化中使用

```csharp
public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    var toastService = services.GetService(typeof(IToastService));
    
    if (toastService == null)
    {
        return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
        {
            Code = "SERVICE_NOT_FOUND",
            Message = "IToastService 服务不可用"
        }));
    }

    return Task.FromResult(Result<bool, PluginError>.Success(true));
}
```

### 3. 只使用错误代码

```csharp
var error = new PluginError
{
    Code = "INIT_FAILED"
};
```

## 标准错误代码

以下是一些常用的标准错误代码：

### 服务相关

| 代码 | 描述 |
|------|------|
| `SERVICE_NOT_FOUND` | 服务不可用 |
|`MISSING_SERVICE` | 缺少必需的服务 |

### 配置相关

| 代码 | 描述 |
|------|------|
| `INVALID_CONFIG` | 配置无效 |
| `CONFIG_NOT_FOUND` | 配置不存在 |
| `MISSING_CONFIG_SERVICE` | 配置服务不可用 |

### 初始化相关

| 代码 | 描述 |
|------|------|
| `INIT_ERROR` | 初始化错误 |
| `INIT_TIMEOUT` | 初始化超时 |
| `INIT_FAILED` | 初始化失败 |

### 关闭相关

| 代码 | 描述 |
|------|------|
| `SHUTDOWN_ERROR` | 关闭错误 |
| `SHUTDOWN_FAILED` | 关闭失败 |

### 依赖相关

| 代码 | 描述 |
|------|------|
| `DEPENDENCY_MISSING` | 缺少依赖 |
| `DEPENDENCY_VERSION_MISMATCH` | 依赖版本不匹配 |

### 权限相关

| 代码 | 描述 |
|------|------|
| `PERMISSION_DENIED` | 权限被拒绝 |
| `INSUFFICIENT_PRIVILEGES` | 权限不足 |

### 网络相关

| 代码 | 描述 |
|------|------|
| `NETWORK_ERROR` | 网络错误 |
| `CONNECTION_FAILED` | 连接失败 |
| `TIMEOUT` | 操作超时 |

## 错误处理模式

### 1. 基本错误处理

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
        return Task.FromResult(Result)bool, PluginError>.Failure(new PluginError
        {
            Code = "INIT_ERROR",
            Message = ex.Message
        }));
    }
}
```

### 2. 条件错误

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

    if (IsApiKeyExpired())
    {
        return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
        {
            Code = "API_KEY_EXPIRED",
            Message = "API 密钥已过期"
        }));
    }

    return Task.FromResult(Result<bool, PluginError>.Success(true));
}
```

### 3. 错误匹配

```csharp
var result = await InitializeAsync(services);

result.Match(
    onSuccess: value => Console.WriteLine("初始化成功"),
    onFailure: error => 
    {
        Console.WriteLine($"错误代码: {error.Code}");
        Console.WriteLine($"错误消息: {error.Message}");
        
        // 根据错误代码处理
        HandleError(error.Code);
    }
);

void HandleError(string errorCode)
{
    switch (errorCode)
    {
        case "SERVICE_NOT_FOUND":
            // 处理服务不可用
            break;
        case "INVALID_CONFIG":
            // 处理配置无效
            break;
        default:
            // 处理其他错误
            break;
    }
}
```

## 最佳实践

1. **使用标准代码**：优先使用标准错误代码
2. **提供详细消息**：错误消息应该清晰描述问题
3. **错误代码唯一性**：确保每个错误代码有唯一含义
4. **日志记录**：记录错误信息以便调试

## 注意事项

1. **只读结构**：`PluginError` 是 `readonly record struct`，创建后不可修改
2. **Code 是必需的**：必须提供错误代码
3. **Message 是可选的**：错误消息可以省略

## 相关文档

- [IPlugin](../core-interfaces/IPlugin.md)
- [Result 类型](../core-interfaces/Result.md)
- [错误处理最佳实践](../../best-practices/error-handling.md)
