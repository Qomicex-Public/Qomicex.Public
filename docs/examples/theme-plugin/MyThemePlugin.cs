using Avalonia.Controls;
using Avalonia.Media;
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;

namespace ThemePlugin;

[Plugin("qomicex.example.theme", "我的主题", "1.0.0",
    Description = "自定义深色主题")]
public class MyThemePlugin : IThemeProvider
{
    public string Id => "qomicex.example.theme";
    public string Name => "我的主题";
    public Version Version => new(1, 0, 0);
    public string Description => "自定义深色主题";

    public string ThemeName => "CustomDark";

    public ResourceDictionary GetResourceDictionary()
    {
        var dict = new ResourceDictionary();

        // 自定义颜色
        dict["MyPrimaryColor"] = new SolidColorBrush(Color.Parse("#FF6B6B"));
        dict["MySecondaryColor"] = new SolidColorBrush(Color.Parse("#4ECDC4"));
        dict["MyAccentColor"] = new SolidColorBrush(Color.Parse("#FFE66D"));

        // 自定义背景
        dict["MyBackground"] = new SolidColorBrush(Color.Parse("#1A1A2E"));
        dict["MySurface"] = new SolidColorBrush(Color.Parse("#16213E"));

        return dict;
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
