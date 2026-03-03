# Result 类型

`Result<TValue, TError>` 是一个通用的结果类型，用于实现 Railway-Oriented Programming 风格的错误处理，替代传统的异常处理机制。

## 命名空间

```csharp
namespace Qomicex.Core.Common
```

## 类型定义

```csharp
public readonly record struct Result<TValue, TError>
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public TValue Value { get; }
    public TError Error { get; }

    public static Result<TValue, TError> Success(TValue value);
    public static Result<TValue, TError> Failure(TError error);
    
    public Result<TOut, TError> Map<TOut>(Func<TValue, TOut> mapper);
    public Result<TOut, TError> Bind<TOut>(Func<TValue, Result<TOut, TError>> binder);
    public TValue GetValueOr(TValue defaultValue);
    public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<TError, TResult> onFailure);
    public Result<TValue, TError> Tap(Action<TValue> onSuccess);
    public Result<TValue, TError> TapError(Action<TError> onFailure);
}
```

## 属性

| 属性 | 类型 | 描述 |
|------|------|------|
| `IsSuccess` | bool | 操作是否成功 |
| `IsFailure` | bool | 操作是否失败 |
| `Value` | TValue | 成功时的值（失败时访问会抛出异常） |
| `Error` | TError | 失败时的错误信息（成功时访问会抛出异常） |

## 静态方法

### Success

```csharp
public static Result<TValue, TError> Success(TValue value)
```

创建一个表示成功的结果。

**示例：**

```csharp
var result = Result<bool, PluginError>.Success(true);
```

### Failure

```csharp
public static Result<TValue, TError> Failure(TError error)
```

创建一个表示失败的结果。

**示例：**

```csharp
var result = Result<bool, PluginError>.Failure(new PluginError
{
    Code = "ERROR_CODE",
    Message = "操作失败"
});
```

## 实例方法

### Map

```csharp
public Result<TOut, TError> Map<TOut>(Func<TValue, TOut> mapper)
```

转换成功值到另一种类型。如果操作失败，则直接传播失败结果。

**示例：**

```csharp
Result<int, PluginError> result = Result<int, PluginError>.Success(42);
Result<string, PluginError> mapped = result.Map(x => x.ToString());
```

### Bind

```csharp
public Result<TOut, TError> Bind<TOut>(Func<TValue, Result<TOut, TError>> binder)
```

链式处理结果。如果当前操作成功，则应用转换函数；否则直接传播失败结果。

**示例：**

```csharp
Result<int, PluginError> result = Result<int, PluginError>.Success(42);

var bound = result.Bind(x => 
    x > 0 
        ? Result<string, PluginError>.Success($"值: {x}")
        : Result<string, PluginError>.Failure(new PluginError { Code = "NEGATIVE", Message = "值不能为负" })
);
```

### GetValueOr

```csharp
public TValue GetValueOr(TValue defaultValue)
```

获取成功值，如果失败则返回默认值。

**示例：**

```csharp
Result<int, PluginError> result = Result<int, PluginError>.Success(42);
int value = result.GetValueOr(0);  // 返回 42

Result<int, PluginError> failed = Result<int, PluginError>.Failure(new PluginError { Code = "ERROR" });
int defaultValue = failed.GetValueOr(0);  // 返回 0
```

### Match

```csharp
public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<TError, TResult> onFailure)
```

模式匹配解构，根据结果状态执行不同的函数。

**示例：**

```csharp
Result<int, PluginError> result = Result<int, PluginError>.Success(42);

string message = result.Match(
    onSuccess: value => $"成功: {value}",
    onFailure: error => $"失败: {error.Code}"
);
```

### Tap

```csharp
public Result<TValue, TError> TapTap(Action<TValue> onSuccess)
```

执行副作用操作（仅当成功时），返回原始结果用于链式调用。

**示例：**

```csharp
Result<int, PluginError> result = Result<int, PluginError>.Success(42)
    .Tap(x => Console.WriteLine($"处理值: {x}"));
```

### TapError

```csharp
public Result<TValue, TError> TapError(Action<TError> onFailure)
```

执行副作用操作（仅当失败时），返回原始结果用于链式调用。

**示例：**

```csharp
Result<int, PluginError> result = Result<int, PluginError>.Failure(new PluginError { Code = "ERROR" })
    .TapError(error => Console.WriteLine($"错误: {error.Code}"));
```

## 在插件中的使用

```csharp
public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    // 检查服务
    var toastService = services.GetService(typeof(IToastService));
    if (toastService == null)
    {
        return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
        {
            Code = "SERVICE_NOT_FOUND",
            Message = "IToastService 不可用"
        }));
    }

    // 执行初始化
    try
    {
        // 初始化逻辑
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

## 链式操作示例

```csharp
public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    return Result<bool, PluginError>.Success(true)
        .Bind(x => ValidateConfiguration())
        .Tap(x => Console.WriteLine("配置验证成功"))
        .Bind(x => LoadDependencies(services))
        .Tap(x => Console.WriteLine("依赖加载成功"));
}

private Result<bool, PluginError> ValidateConfiguration()
{
    // 验证配置
    return Result<bool, PluginError>.Success(true);
}

private Result<bool, PluginError> LoadDependencies(IServiceProvider services)
{
    // 加载依赖
    return Result<bool, PluginError>.Success(true);
}
```

## 相关文档

- [IPlugin](IPlugin.md)
- [PluginError](../models/PluginError.md)
- [错误处理最佳实践](../../best-practices/error-handling.md)
