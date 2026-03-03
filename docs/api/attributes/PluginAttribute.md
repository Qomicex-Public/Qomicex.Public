# PluginAttribute 特性

`PluginAttribute` 用于标记插件类，并提供插件的元数据信息。

## 命名空间

```csharp
namespace Qomicex.PluginSDK.Attributes
```

## 特性定义

```csharp
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PluginAttribute : Attribute
{
    public string Id { get; }
    public string Name { get; }
    public string Version { get; }
    public string Description { get; }

    public PluginAttribute(string id, string name, string version)
    {
        Id = id;
        Name = name;
        Version = version;
    }
}
```

## 属性

| 属性 | 类型 | 必需 | 描述 |
|------|------|------|------|
| `Id` | string | 是 | 插件的唯一标识符 |
| `Name` | string | 是 | 插件的显示名称 |
| `Version` | string | 是 | 插件的版本号 |
| `Description` | string | 否 | 插件的描述信息 |

## 使用示例

### 基本用法

```csharp
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;

[Plugin("qomicex.example.myplugin", "我的插件", "1.0.0")]
public class MyPlugin : IPlugin
{
    // ...
}
```

### 完整用法

```csharp
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using System;
using System.Threading.Tasks;

[Plugin(
    "qomicex.example.myplugin", 
    "我的插件", 
    "1.0.0",
    Description = "这是一个示例插件，展示了如何使用 PluginAttribute"
)]
public class MyPlugin : IPlugin
{
    public string Id => "qomicex.example.myplugin";
    public string Name => "我的插件";
    public Version Version => new(1, 0, 0);
    public string Description => "这是一个示例插件";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
```

## 插件 ID 规范

插件 ID 应使用反向域名格式，确保全局唯一性：

```
[反向域名].[插件名称]

示例：
- qomicex.example.myplugin
- qomicex.myorg.myplugin
- qomicex.username.myplugin
```

## 版本号规范

建议使用语义化版本 (SemVer)：

```
MAJOR.MINOR.PATCH[-PRERELEASE][+BUILD]

示例：
- 1.0.0
- 2.1.3
- 3.0.0-beta.1
- 1.2.4+build.123
```

| 版本部分 | 含义 | 何时递增 |
|-----------|------|-----------|
| MAJOR | 主版本号 | 不兼容的 API 修改 |
| MINOR | 次版本号 | 向下兼容的功能新增 |
| PATCH | 修订号 | 向下兼容的问题修正 |

## 元数据读取

插件系统会在运行时读取这些元数据：

```csharp
// 插件系统内部代码示例
var pluginType = pluginAssembly.GetTypes()
    .FirstOrDefault(t => t.GetCustomAttribute<PluginAttribute>() != null);

if (pluginType != null)
{
    var attribute = pluginType.GetCustomAttribute<PluginAttribute>();
    Console.WriteLine($"插件 ID: {attribute.Id}");
    Console.WriteLine($"插件名称: {attribute.Name}");
    Console.WriteLine($"插件版本: {attribute.Version}");
    Console.WriteLine($"插件描述: {attribute.Description}");
}
```

## 最佳实践

1. **ID 唯一性**：使用反向域名确保唯一性
2. **版本管理**：遵循语义化版本规范
3. **描述清晰**：提供简洁清晰的插件描述
4. **命名一致**：确保特性中的 ID 和接口中返回的 Id 一致

## 注意事项

1. **特性继承**：`Inherited = false`，该特性不会被子类继承
2. **应用目标**：只能应用于类 (`AttributeTargets.Class`)
3. **版本一致性**：特性中的版本是字符串格式，接口中的 Version 是 Version 类型

## 相关文档

- [IPlugin](../core-interfaces/IPlugin.md)
- [PluginError](../models/PluginError.md)
- [创建第一个插件](../../getting-started/first-plugin.md)
