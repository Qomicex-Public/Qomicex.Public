# 最小化插件示例

这是一个最简的插件实现示例，适合作为插件开发的基础。

## MyPlugin.cs

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
    public Version Version Version Version => new(1, 0, 0);
    public string Description => "我的第一个 Qomicex 插件";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        Console.WriteLine($"[{Name}] 插件已加载");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        Console.WriteLine($"[{Name}] 插件已卸载");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
```

## MyPlugin.csproj

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

## 使用步骤

1. 创建新的类库项目
2. 添加 Qomicex.PluginSDK NuGet 包
3. 复制上述代码到项目中
4. 构建项目
5. 将 DLL 复制到 Qomicex Launcher 的 Plugins 目录

## 运行效果

启动 Qomicex Launcher 后，控制台将输出：

```
[我的插件] 插件已加载
```

关闭启动器后，控制台将输出：

```
[我的插件] 插件已卸载
```
