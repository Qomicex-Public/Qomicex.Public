# ISettingsPageProvider 接口

`ISettingsPageProvider` 接口用于创建具有自定义设置页面的插件，允许插件向 Qomicex Launcher 的设置界面添加自己的设置页面。

## 命名空间

```csharp
namespace Qomicex.PluginSDK.Interfaces
```

## 接口定义

```csharp
public interface ISettingsPageProvider : IPlugin
{
    string PageTitle { get; }
    string PageIcon { get; }
    object GetViewModel();
    object GetView();
}
```

## 属性

| 属性 | 类型 | 描述 |
|------|------|------|
| `PageTitle` | string | 设置页面的显示标题 |
| `PageIcon` | string | 设置页面的图标（使用图标字符串或路径） |

## 方法

### GetViewModel

```csharp
object GetViewModel()
```

返回设置页面使用的 ViewModel 对象。

**返回值：**

| 类型 | 描述 |
|------|------|
| `object` | ViewModel 实例，通常继承自 `PageViewModelBase` |

### GetView

```csharp
object GetView()
```

返回设置页面的视图对象（UserControl）。

**返回值：**

| 类型 | 描述 |
|------|------|
| `object` | 视图实例，通常是 `UserControl` |

## 完整示例

### 1. 创建设置页面 ViewModel

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.settings", "设置页面插件", "1.0.0")]
public partial class MySettingsPlugin : ISettingsPageProvider
{
    // IPlugin 属性
    public string Id => "qomicex.example.settings";
    public string Name => "设置页面插件";
    public Version Version => new(1, 0, 0);
    public string Description => "自定义设置页面示例";

    // ISettingsPageProvider 属性
    public string PageTitle => "我的设置";
    public string PageIcon => "cog";  // 或使用图标路径

    private MySettingsViewModel? _viewModel;
    private MySettingsView? _view;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        Console.WriteLine($"[{Name}] 初始化");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        Console.WriteLine($"[{Name}] 关闭");
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public object GetViewModel()
    {
        _viewModel ??= new MySettingsViewModel();
        return _viewModel;
    }

    public object GetView()
    {
        _view ??= new MySettingsView
        {
            DataContext = GetViewModel()
        };
        return _view;
    }
}
```

### 2. 创建 ViewModel 类

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

public partial class MySettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _enableFeature = true;

    [ObservableProperty]
    private string _apiKey = string.Empty;

    [ObservableProperty]
    private int _refreshInterval = 60;

    [ObservableProperty]
    private string _statusMessage = "准备就绪";

    [RelayCommand]
    private void SaveSettings()
    {
        StatusMessage = "设置已保存！";
        Console.WriteLine($"设置保存: EnableFeature={EnableFeature}, ApiKey={ApiKey}");
    }

    [RelayCommand]
    private void ResetSettings()
    {
        EnableFeature = true;
        ApiKey = string.Empty;
        RefreshInterval = 60;
        StatusMessage = "设置已重置";
    }

    [RelayCommand]
    private void TestConnection()
    {
        StatusMessage = "正在测试连接...";
        
        // 模拟连接测试
        Task.Run(async () =>
        {
            await Task.Delay(1000);
            StatusMessage = "连接成功！";
        });
    }
}
```

### 3. 创建 View (AXAML)

创建 `MySettingsView.axaml` 文件：

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             mc:Ignorable="d" d:DesignWidth="400" d:DesignHeight="600"
             x:Class="MyNamespace.MySettingsView">

    <ScrollViewer>
        <StackPanel Margin="16" Spacing="16">
            
            <!-- 标题 -->
            <TextBlock Text="我的插件设置"
                       FontSize="20"
                       FontWeight="SemiBold" />

            <Separator />

            <!-- 功能开关 -->
            <CheckBox IsChecked="{Binding EnableFeature}"
                      Content="启用功能" />

            <!-- API 寍钥输入 -->
            <StackPanel Spacing="8">
                <TextBlock Text="API 密钥" />
                <TextBox Text="{Binding ApiKey}"
                         Watermark="请输入 API 密钥"
                         PasswordChar="•" />
            </StackPanel>

            <!-- 刷新间隔 -->
            <StackPanel Spacing="8">
                <TextBlock Text="刷新间隔（秒）" />
                <NumericUpDown Value="{Binding RefreshInterval}"
                               Minimum="10"
                               Maximum="3600"
                               Increment="10" />
            </StackPanel>

            <Separator />

            <!-- 按钮组 -->
            <StackPanel Orientation="Horizontal" Spacing="8">
                <Button Content="保存设置"
                        Command="{Binding SaveSettingsCommand}" />
                
                <Button Content="重置"
                        Command="{Binding ResetSettingsCommand}" />
                
                <Button Content="测试连接"
                        Command="{Binding TestConnectionCommand}" />
            </StackPanel>

            <!-- 状态消息 -->
            <TextBlock Text="{Binding StatusMessage}"
                       Foreground="Gray"
                       FontSize="12" />

        </StackPanel>
    </ScrollViewer>

</UserControl>
```

## 图标约定

可以使用以下图标字符串：

| 图标字符串 | 显示 |
|------------|------|
| `cog` | 齿轮图标 |
| `home` | 主页图标 |
| `info` | 信息图标 |
| `palette` | 调色板图标 |
| `plug` | 插头图标 |
| `folder` | 文件夹图标 |

或者使用自定义图标资源路径。

## 注意事项

1. **ViewModel 缓存**：建议在 `GetViewModel()` 中缓存 ViewModel 实例
2. **DataContext 设置**：确保 View 的 DataContext 正确设置
3. **MVVM 模式**：遵循 MVVM 设计模式，保持代码清晰
4. **资源清理**：在 `ShutdownAsync` 中清理必要的资源

## 相关文档

- [IPlugin](../core-interfaces/IPlugin.md)
- [IThemeProvider](IThemeProvider.md)
- [设置页插件教程](../../tutorials/settings-plugin.md)
- [设置页插件示例](../../examples/settings-plugin/index.md)
