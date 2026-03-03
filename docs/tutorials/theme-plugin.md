# 主题插件教程

本教程将指导你创建一个自定义主题插件。

## 目标

创建一个自定义深色主题插件，修改 Qomicex Launcher 的配色方案。

## 步骤 1：创建项目

```bash
# 创建项目
dotnet new classlib -n CustomThemePlugin
cd CustomThemePlugin

# 添加依赖
dotnet add package Qomicex.PluginSDK
dotnet add package Avalonia
```

## 步骤 2：创建主题插件类

创建 `CustomThemePlugin.cs` 文件：

```csharp
using Avalonia.Controls;
using Avalonia.Media;
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.theme.dark", "自定义深色主题", "1.0.0",
    Description = "一个自定义的深色主题")]
public class CustomThemePlugin : IThemeProvider
{
    // IPlugin 属性
    public string Id => "qomicex.example.theme.dark";
    public string Name => "自定义深色主题";
    public Version Version Version => new(1, 0, 0);
    public string Description => "一个自定义的深色主题";

    // IThemeProvider 属性
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

    // 返回主题资源
    public ResourceDictionary GetResourceDictionary()
    {
        return new ResourceDictionary
        {
            // 主色调
            { "PrimaryColor", Color.Parse("#6366f1") },
            { "SecondaryColor", Color.Parse("#8b5cf6") },
            
            // 背景色
            { "BackgroundColor", Color.Parse("#1f2937") },
            { "SurfaceColor", Color.Parse("#374151") },
            
            // 文本色
            { "TextColor", Color.Parse("#f3f4f6") },
            { "SecondaryTextColor", Color.Parse("#9ca3af") },
            
            // 边框色
            { "BorderColor", Color.Parse("#4b5563") },
            
            // 其他颜色
            { "ErrorColor", Color.Parse("#ef4444") },
            { "WarningColor", Color.Parse("#f59e0b") },
            { "SuccessColor", Color.Parse("#10b981") },
            
            // 笔刷
            { "AccentBrush", new SolidColorBrush(Color.Parse("#6366f1")) },
            { "BackgroundBrush", new SolidColorBrush(Color.Parse("#1f2937")) },
        };
    }
}
```

## 步骤 3：构建并部署

```bash
# 构建
dotnet build

# 将 DLL 复制到 Plugins 目录
```

## 主题资源说明

主题插件通过 `ResourceDictionary` 提供颜色、样式等资源。

### 常用资源键

| 键名 | 类型 | 用途 |
|------|------|------|
| `PrimaryColor` | Color | 主色调 |
| `BackgroundColor` | Color | 背景色 |
| `TextColor` | Color | 文本色 |
| `AccentBrush` | Brush | 强调色笔刷 |

### 颜色格式

支持多种颜色格式：

```csharp
// 十六进制
Color.Parse("#ff0000")
Color.Parse("#f00")

// 命名颜色
Color.Parse("Red")
Color.Parse("CornflowerBlue")

// RGBA
Color.Parse("rgba(255, 0, 0, 0.5)")
```

## 使用 AXAML 资源文件

对于复杂的主题，可以使用 AXAML 文件：

### 创建 ThemeResources.axaml

```xml
<ResourceDictionaryDictionary
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

    <!-- 颜色 -->
    <Color x:Key="PrimaryColor">#6366f1</Color>
    <Color x:Key="BackgroundColor">#1f2937</Color>

    <!-- 笔刷 -->
    <SolidColorBrush x:Key="PrimaryBrush" Color="{DynamicResource PrimaryColor}" />
    <SolidColorBrush x:Key="BackgroundBrush" Color="{DynamicResource BackgroundColor}" />

</ResourceDictionary>
```

### 在插件中加载

```csharp
using Avalonia.Resources;

public ResourceDictionary GetResourceDictionary()
{
    // 使用资源文件
    return new ResourceDictionary
    {
        Source = new Uri("avares://CustomThemePlugin/ThemeResources.axaml")
    };
}
```

## 下一步

- [设置页插件教程](settings-plugin.md)
- [服务注入教程](service-injection.md)
