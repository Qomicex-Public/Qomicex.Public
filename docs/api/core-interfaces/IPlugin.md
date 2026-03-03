# IPlugin 接口

`IPlugin` 是所有插件必须实现的基础接口，定义了插件的基本属性和生命周期方法。

## 命名空间

```csharp
namespace Qomicex.PluginSDK.Interfaces
```

## 接口定义

```csharp
public interface IPlugin
{
    string Id { get; }
    string Name { get; }
    Version Version { get; }
    string Description { get; }

    Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services);
    Task<Result<bool, PluginError>> ShutdownAsync();
}
```

## 属性

| 属性 | 类型 | 描述 |
|------|------|------|
| `Id` | string | 插件的唯一标识符，使用反向域名格式，如 `qomicex.example.myplugin` |
| `Name` | string | 插件的显示名称 |
| `Version` | Version | 插件的版本号 |
| `Description` | string | 插件的描述信息 |

## 方法

### InitializeAsync

```csharp
Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
```

插件加载时调用，用于初始化插件。

**参数：**

| 参数 | 类型 | 描述 |
|------|------|------|
| `services` | IServiceProvider | 服务提供者，用于获取主程序提供的各种服务 |

**返回值：**

| 类型 | 描述 |
|------|------|
| `Result<bool, PluginError>` | 初始化结果，成功返回 `Success(true)`，失败返回 `Failure(PluginError)` |

**示例：**

```csharp
public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    var toastService = services.GetService(typeof(IToastService)) as IToastService;
    
    if (toastService == null)
    {
        return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
        {
            Code = "SERVICE_NOT_FOUND",
            Message = "IToastService 不可用"
        }));
    }

    toastService.Show("插件初始化成功！");
    
    return Task.FromResult(Result<bool, PluginError>.Success(true));
}
```

### ShutdownAsync

```csharp
Task<Result<bool, PluginError>> ShutdownAsync()
```

插件卸载或应用关闭时调用，用于清理资源。

**返回值：**

| 类型 | 描述 |
|------|------|
| `Result<bool, PluginError>` | 关闭结果，成功返回 `Success(true)`，失败返回 `Failure(PluginError)` |

**示例：**

```csharp
public Task<Result<bool, PluginError>> ShutdownAsync()
{
    // 清理资源
    return Task.FromResult(Result<bool, PluginError>.Success(true));
}
```

## 完整示例

```csharp
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.myplugin", "我的插件", "1.0.0")]
public class MyPlugin : IPlugin
{
    public string Id => "qomicex.example.myplugin";
    public string Name => "我的插件";
    public Version Version => new(1, 0, 0);
    public string Description => "这是一个示例插件";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        try
        {
            Console.WriteLine($"{Name} 初始化成功！");
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

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        try
        {
            Console.WriteLine($"{Name} 关闭成功！");
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

## 注意事项

1. **插件 ID**：必须使用反向域名格式，确保全局唯一性
2. **版本号**：建议使用语义化版本 (SemVer)
3. **错误处理**：始终使用 `Result` 类型返回结果，避免抛出异常
4. **资源管理**：在 `ShutdownAsync` 中清理所有资源
5. **服务注入**：缓存服务引用以提高性能

## 相关文档

- [Result 类型](Result.md)
- [PluginAttribute](../attributes/PluginAttribute.md)
- [PluginError](../models/PluginError.md)
- [生命周期](../../lifecycle/index.md)
