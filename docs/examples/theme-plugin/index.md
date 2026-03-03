# 主题插件示例

这是一个完整的主题插件实现示例。

## MyThemePlugin.cs

```csharp
using Avalonia.Controls;
using Avalonia.Media;
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.theme.custom", "自定义深色主题", "1.0.0",
    Description = "一个自定义的深色主题")]
public class CustomThemePlugin : IThemeProvider
{
    public string Id => "qomicex.example.theme.custom";
    public string Name => "自定义深色主题";
    public Version Version => new(1, 0, 0);
    public string Description => "一个自定义的深色主题";
    public string ThemeName => "Custom Dark";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        Console.WriteLine($"[{Name}] 主题加载成功");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        Console.WriteLine($"[{Name}] 主题已卸载");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public ResourceDictionary GetResourceDictionary()
    {
        return new ResourceDictionary
        {
            { "PrimaryColor", Color.Parse("#6366f1") },
            { "SecondaryColor", Color.Parse("#8b5cf6") },
            { "BackgroundColor", Color.Parse("#1f2937") },
            { "SurfaceColor", Color.Parse("#374151") },
            { "TextColor", Color.Parse("#f3f4f6") },
            { "SecondaryTextColor", Color.Parse("#9ca3af") },
            { "BorderColor", Color.Parse("#4b5563") },
            { "ErrorColor", Color.Parse("#ef4444") },
            { "WarningColor", Color.Parse("#f59e0b") },
            { "SuccessColor", Color.Parse("#10b981") },
            { "AccentBrush", new SolidColorBrush(Color.Parse("#6366f1")) },
            { "BackgroundBrush", new SolidColorBrush(Color.Parse("#1f2937")) },
        };
    }
}
```

## MyThemePlugin.csproj

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
    <PackageReference Include="Avalonia" Version="11.0.0" />
  </ItemGroup>
</Project>
```
