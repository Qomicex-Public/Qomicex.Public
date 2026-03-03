# IThemeProvider 接口

`IThemeProvider` 接口用于创建自定义主题插件，允许插件向 Qomicex Launcher 提供自定义的主题资源。

## 命名空间

```csharp
namespace Qomicex.PluginSDK.Interfaces
```

## 接口定义

```csharp
public interface IThemeProvider : IPlugin
{
    ResourceDictionary GetResourceDictionary();
    string ThemeName { get; }
}
```

## 属性

| 属性 | 类型 | 描述 |
|------|------|------|
| `ThemeName` | string | 主题名称，如 "Dark"、"Light" 或自定义名称 |

## 方法

### GetResourceDictionary

```csharp
ResourceDictionary GetResourceDictionary()
```

返回包含主题资源定义的 `ResourceDictionary` 对象。

**返回值：**

| 类型 | 描述 |
|------|------|
| `ResourceDictionary` | Avalonia 的资源字典，包含颜色、样式等主题资源 |

## 完整示例

### 1. 创建主题插件类

```csharp
using Avalonia.Controls;
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.theme.custom", "自定义主题", "1.0.0",
    Description = "一个自定义的深色主题")]
public class CustomThemePlugin : IThemeProvider
{
    // 实现 IPlugin 接口
    public string Id => "qomicex.example.theme.custom";
    public string Name => "自定义主题";
    public Version Version => new(1, 0, 0);
    public string Description => "一个自定义的深色主题";

    // IThemeProvider 特有属性
    public string ThemeName => "Custom Dark";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        Console.WriteLine($"[{Name}] 主题初始化");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        Console.WriteLine($"[{Name}] 主题关闭");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    // 返回主题资源字典
    public ResourceDictionary GetResourceDictionary()
    {
        return new ResourceDictionary
        {
            // 主色调
            { "PrimaryColor", Avalonia.Media.Color.Parse("#6366f1") },
            { "SecondaryColor", Avalonia.Media.Color.Parse("#8b5cf6") },
            
            // 背景色
            { "BackgroundColor", Avalonia.Media.Color.Parse("#1f2937") },
            { "SurfaceColor", Avalonia.Media.Color.Parse("#374151") },
            
            // 文本色
            { "TextColor", Avalonia.Media.Color.Parse("#f3f4f6") },
            { "SecondaryTextColor", Avalonia.Media.Color.Parse("#9ca3af") },
            
            // 边框色
            { "BorderColor", Avalonia.Media.Color.Parse("#4b5563") },
            
            // 其他自定义资源
            { "AccentBrush", new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.Parse("#6366f1")) },
        };
    }
}
```

### 2. 使用 AXAML 资源文件

创建 `ThemeResources.axaml` 文件：

```xml
<ResourceDictionary xmlns="https://github.com/avaloniaui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                    xmlns:sys="clr-namespace:System;assembly=mscorlib">

    <!-- 颜色资源 -->
    <Color x:Key="PrimaryColor">#6366f1</Color>
    <Color x:Key="SecondaryColor">#8b5cf6</Color>
    
    <Color x:Key="BackgroundColor">#1f2937</Color>
    <Color x:Key="SurfaceColor">#374151</Color>
    
    <Color x:Key="TextColor">#f3f4f6</Color>
    <Color x:Key="SecondaryTextColor">#9ca3af</Color>

    <!-- 笔刷资源 -->
    <SolidColorBrush x:Key="PrimaryBrush" Color="{StaticResource PrimaryColor}" />
    <SolidColorBrush x:Key="BackgroundBrush" Color="{StaticResource BackgroundColor}" />

    <!-- 样式资源 -->
    <Style x:Key="ButtonStyle" Selector="Button">
        <Setter Property="Background" Value="{DynamicResource PrimaryBrush}" />
        <Setter Property="Foreground" Value="White" />
    </Style>

</ResourceDictionary>
```

然后在插件中加载：

```csharp
public ResourceDictionary GetResourceDictionary()
{
    // 从资源文件加载
    var resources = new ResourceDictionary();
    var themeResources = Avalonia.Application.Current?.Resources.MergedDictionaries
        .FirstOrDefault(x => x.Source?.ToString().Contains("ThemeResources.axaml") == true);
    
    if (themeResources != null)
    {
        return themeResources;
    }

    // 或者创建动态资源
    return new ResourceDictionary
    {
        // 资源定义...
    };
}
```

## 主题资源规范

建议的主题资源键名：

| 键名 | 类型 | 描述 |
|------|------|------|
| `PrimaryColor` | Color | 主色调 |
| `SecondaryColor` | Color | 次要色调 |
| `BackgroundColor` | Color | 背景色 |
| `SurfaceColor` | Color | 表面色 |
| `TextColor` | Color | 主文本色 |
| `SecondaryTextColor` | Color | 次要文本色 |
| `BorderColor` | Color | 边框色 |
| `ErrorColor` | Color | 错误色 |
| `WarningColor` | Color | 警告色 |
| `SuccessColor` | Color | 成功色 |

## 注意事项

1. **资源键冲突**：使用具有唯一性的键名前缀
2. **性能考虑**：资源字典在初始化时加载，避免重复创建
3. **主题名称**：使用描述性的名称，便于用户识别
4. **兼容性**：确保与 Avalonia 的主题系统兼容

## 相关文档

- [IPlugin](../core-interfaces/IPlugin.md)
- [ISettingsPageProvider](ISettingsPageProvider.md)
- [主题插件教程](../../tutorials/theme-plugin.md)
- [主题插件示例](../../examples/theme-plugin/index.md)
