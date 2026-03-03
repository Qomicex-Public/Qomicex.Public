using Avalonia.Controls;
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;

namespace SettingsPlugin;

[Plugin("qomicex.example.settings", "我的设置", "1.0.0",
    Description = "自定义设置页面")]
public class MySettingsPlugin : ISettingsPageProvider
{
    public string Id => "qomicex.example.settings";
    public string Name => "我的设置";
    public Version Version => new(1, 0, 0);
    public string Description => "自定义设置页面";

    public string PageTitle => "我的设置";
    public string PageIcon => "\uf013";

    public object GetViewModel()
    {
        return new MySettingsViewModel();
    }

    public object GetView()
    {
        return new MySettingsView();
    }

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
