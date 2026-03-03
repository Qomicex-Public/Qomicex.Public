# 创建第一个插件

让我们创建一个简单的 Hello World 插件来了解插件的基本结构。

## 基本插件结构

所有插件必须实现 `IPlugin` 接口并使用 `PluginAttribute` 特性标记。

## Hello World 示例

创建一个新的类文件 `HelloWorldPlugin.cs`：

```csharp
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.helloworld", "Hello World 插件", "1.0.0",
    Description = "我的第一个 Qomicex 插件")]
public class HelloWorldPlugin : IPlugin
{
    // 插件属性
    public string Id => "qomicex.example.helloworld";
    public string Name => "Hello World 插件";
    public Version Version => new(1, 0, 0);
    public string Description => "我的第一个 Qomicex 插件";

    // 初始化方法 - 插件加载时调用
    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        try
        {
            Console.WriteLine($"[{Name}] 正在初始化...");
            Console.WriteLine($"[{Name}] 版本: {Version}");
            Console.WriteLine($"[{Name}] ID: {Id}");
            Console.WriteLine($"[{Name}] Hello World!");

            // 获取 Toast 服务（如果需要）
            var toastService = services.GetService(typeof(IToastService)) as IToastService;
            toastService?.Show("Hello World 插件已加载！", NotificationType.Success);

            return Task.FromResult(Result<bool, PluginError>.Success(true));
        }
        catch (Exception ex)
        {
            var error = new PluginError
            {
                Code = "INIT_FAILED",
                Message = $"初始化失败: {ex.Message}"
            };
            return Task.FromResult(Result<bool, PluginError>.Failure(error));
        }
    }

    // 关闭方法 - 插件卸载时调用
    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        try
        {
            Console.WriteLine($"[{Name}] 正在关闭...");
            return Task.FromResult(Result<bool, PluginError>.Success(true));
        }
        catch (Exception ex)
        {
            var error = new PluginError
            {
                Code = "SHUTDOWN_FAILED",
                Message = $"关闭失败: {ex.Message}"
            };
            return Task.FromResult(Result<bool, PluginError>.Failure(error));
        }
    }
}
```

## PluginAttribute 说明

`PluginAttribute` 用于标记插件类并提供元数据：

```csharp
[Plugin(
    string id,      // 插件唯一标识符 (必需)
    string name,    // 插件名称 (必需)
    string version  // 版本号 (必需)
)]
```

可选属性：

| 属性 | 类型 | 描述 |
|------|------|------|
| `Description` | string | 插件描述 |

插件 ID 应使用反向域名格式（如 `qomicex.company.pluginname`），确保全局唯一性。

## IPlugin 接口说明

`IPlugin` 接口定义了插件的基本属性和方法：

| 属性/方法 | 类型 | 描述 |
|-----------|------|------|
| `Id` | string | 插件唯一标识符 |
| `Name` | string | 插件显示名称 |
| `Version` | Version | 插件版本号 |
| `Description` | string | 插件描述 |
| `InitializeAsync` | Task<Result<...>> | 插件初始化方法 |
| `ShutdownAsync` | Task<Result<...>> | 插件关闭方法 |

## Result 类型说明

插件方法使用 `Result<TValue, TError>` 类型返回结果：

```csharp
// 成功返回
Result<bool, PluginError>.Success(true)

// 失败返回
Result<bool, PluginError>.Failure(new PluginError
{
    Code = "ERROR_CODE",
    Message = "错误消息"
})
```

## 项目文件配置

确保你的 `.csproj` 文件配置正确：

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <Version>1.0.0</Version>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Qomicex.PluginSDK" Version="1.0.0" />
  </ItemGroup>
</Project>
```

## 测试插件

1. 构建项目：
   ```bash
   dotnet build
   ```

2. 将生成的 DLL 文件复制到 Qomicex Launcher 的插件目录：
   ```
   Qomicex.Plugins/HelloWorldPlugin.dll
   ```

3. 启动 Qomicex Launcher，插件将自动加载

## 调试插件

要调试插件：

1. 在 Qomicex Launcher 中启用开发者模式
2. 在 Visual Studio 中附加到启动器进程
3. 在插件代码中设置断点
4. 重新加载插件以触发断点

## 下一步

现在你已经创建了基础插件，可以学习：
- [构建与部署](build-deploy.md)
- [主题插件教程](../tutorials/theme-plugin.md)
- [设置页插件教程](../tutorials/settings-plugin.md)
