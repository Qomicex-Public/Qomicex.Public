# 设置页插件教程

本教程将指导你创建一个带有自定义设置页面的插件。

## 目标

创建一个插件，在 Qomicex Launcher 的设置页面中添加自定义设置。

## 步骤 1：创建项目

```bash
# 创建项目
dotnet new classlib -n MySettingsPlugin
cd MySettingsPlugin

# 添加依赖
dotnet add package Qomicex.PluginSDK
dotnet add package Avalonia
dotnet add package CommunityToolkit.Mvvm
```

## 步骤 2：创建 ViewModel

创建 `MySettingsViewModel.cs` 文件：

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
        // 保存配置逻辑
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
        // 测试连接逻辑
    }
}
```

## 步骤 3：创建 View

创建 `MySettingsView.axaml` 文件：

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             x:Class="MyNamespace.MySettingsView">
    
    <ScrollViewer>
        <StackPanel Margin="16" Spacing="16">
            
            <TextBlock Text="我的插件设置"
                       FontSize="20"
                       FontWeight="SemiBold" />

            <Separator />

            <CheckBox IsChecked="{Binding EnableFeature}"
                      Content="启用功能" />

            <StackPanel Spacing="8">
                <TextBlock Text="API 密钥" />
                <TextBox Text="{Binding ApiKey}"
                         Watermark="请输入 API 密钥" />
            </StackPanel>

            <StackPanel Spacing="8">
                <TextBlock Text="刷新间隔（秒）" />
                <NumericUpDown Value="{Binding RefreshInterval}"
                               Minimum="10"
                               Maximum="3600" />
            </StackPanel>

            <Separator />

            <StackPanel Orientation="Horizontal" Spacing="8">
                <Button Content="保存"
                        Command="{Binding SaveSettingsCommand}" />
                <Button Content="重置"
                        Command="{Binding ResetSettingsCommand}" />
                <Button Content="测试"
                        Command="{Binding TestConnectionCommand}" />
            </StackPanel>

            <TextBlock Text="{Binding StatusMessage}"
                       Foreground="Gray" />

        </StackPanel>
    </ScrollViewer>

</UserControl>
```

## 步骤 4：创建插件类

创建 `MySettingsPlugin.cs` 文件：

```csharp
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.settings", "设置插件", "1.0.0")]
public class MySettingsPlugin : ISettingsPageProvider
{
    public string Id => "qomicex.example.settings";
    public string Name => "设置插件";
    public Version Version => new(1, 0, 0);
    public string Description => "自定义设置页面";

    public string PageTitle => "我的设置";
    public string PageIcon => "cog";

    private MySettingsViewModel? _viewModel;
    private MySettingsView? _view;

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        Console.WriteLine($"[{Name}] 初始化成功");
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

## 步骤 5：构建并部署

```bash
# 构建
dotnet build

# 将 DLL 复制到 Plugins 目录
```

## 代码说明

### MVVM 模式

设置页面使用 MVVM (Model-View-ViewModel) 模式：

- **Model**：数据模型（可选）
- **View**：UI 界面（AXAML）
- **ViewModel**：业务逻辑和数据绑定

### CommunityToolkit.Mvvm

使用 CommunityToolkit.Mvvm 简化 MVVM 开发：

```csharp
// 可观察属性
[ObservableProperty]
private bool _enableFeature;

// 命令
[RelayCommand]
private void SaveSettings()
{
    // 保存逻辑
}
```

## 下一步

- [服务注入教程](service-injection.md)
- [配置存储教程](advanced/config-storage.md)
