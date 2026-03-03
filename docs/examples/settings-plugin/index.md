# 设置页插件示例

这是一个完整的设置页插件实现示例。

## MySettingsPlugin.cs

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

## MySettingsViewModel.cs

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        Task.Run(async () =>
        {
            await Task.Delay(1000);
            StatusMessage = "连接成功！";
        });
    }
}
```

## MySettingsView.axaml

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
                         Watermark="请输入 API 密钥"
                         PasswordChar="•" />
            </StackPanel>

            <StackPanel Spacing="8">
                <TextBlock Text="刷新间隔（秒）" />
                <NumericUpDown Value="{Binding RefreshInterval}"
                               Minimum="10"
                               Maximum="3600"
                               Increment="10" />
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

## MySettingsPlugin.csproj

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
    <PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.0" />
  </ItemGroup>
</Project>
```
