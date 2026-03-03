# 插件间通信教程

学习插件之间如何互相调用。

## 通过 PluginService 发现其他插件

### 获取特定插件

```csharp
private PluginService? _pluginService;

public async Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    _pluginService = services.GetService<PluginService>();

    // 获取特定插件
    var themePlugin = _pluginService?.GetPlugin("qomicex.example.mytheme");

    if (themePlugin != null)
    {
        Console.WriteLine($"找到主题插件: {themePlugin.Name}");
    }

    return Result<bool, PluginError>.Success(true);
}
```

### 获取所有主题插件

```csharp
public void ListThemePlugins()
{
    var themes = _pluginService?.GetExtensions<IThemeProvider>();

    foreach (var theme in themes ?? [])
    {
        Console.WriteLine($"主题: {theme.ThemeName}");
        var dict = theme.GetResourceDictionary();
        // 使用主题资源
    }
}
```

### 获取所有设置插件

```csharp
public void ListSettingsPlugins()
{
    var settings = _pluginService?.GetExtensions<ISettingsPageProvider>();

    foreach (var provider in settings ?? [])
    {
        Console.WriteLine($"设置页: {provider.PageTitle}");
    }
}
```

## 直接调用其他插件

### 调用主题插件

```csharp
public void ApplyTheme(string pluginId)
{
    var plugin = _pluginService?.GetPlugin(pluginId);

    if (plugin is IThemeProvider themeProvider)
    {
        var dict = themeProvider.GetResourceDictionary();

        // 应用主题资源
        Application.Current?.Resources.MergedDictionaries.Add(dict);

        _toast?.Success($"已应用主题: {themeProvider.ThemeName}");
    }
}
```

### 调用设置插件

```csharp
public void ShowSettings(string pluginId)
{
    var plugin = _pluginService?.GetPlugin(pluginId);

    if (plugin is ISettingsPageProvider settingsProvider)
    {
        var title = settingsProvider.PageTitle;
        var viewModel = settingsProvider.GetViewModel();
        var view = settingsProvider.GetView();

        // 显示设置页面
        // ...

        _toast?.Info($"打开设置: {title}");
    }
}
```

## 插件协作模式

### 1. 主题链

```csharp
public void ApplyThemeChain()
{
    var themes = _pluginService?.GetExtensions<IThemeProvider>()
        .OrderBy(t => t.Name)
        .ToList();

    // 按顺序应用多个主题
    foreach (var theme in themes ?? [])
    {
        var dict = theme.GetResourceDictionary();
        Application.Current?.Resources.MergedDictionaries.Add(dict);
    }
}
```

### 2. 共享数据

```csharp
// 提供数据的插件
public class DataProviderPlugin : IPlugin
{
    public static DataProviderPlugin? Instance { get; private set; }

    public string SharedData { get; set; } = "Hello";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        Instance = this;
        return Result<bool, PluginError>.Success(true);
    }
}

// 消费数据的插件
public class DataConsumerPlugin : IPlugin
{
    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        var data = DataProviderPlugin.Instance?.SharedData;
        Console.WriteLine($"收到数据: {data}");
        return Result<bool, PluginError>.Success(true);
    }
}
```

### 3. 通过配置共享

```csharp
// 插件 A 写入配置
public void WriteSharedData(string key, string value)
{
    _config?.Save(new SharedData { Value = value }, key);
}

// 插件 B 读取配置
public string ReadSharedData(string key)
{
    var data = _config?.Load<SharedData>(key);
    return data?.Value ?? string.Empty;
}
```

## 插件服务接口

### 定义公共接口

```csharp
// 在共享库中定义
public interface IMyPluginService
{
    string GetPluginData();
    void SetPluginData(string data);
    Task<string> ProcessAsync(string input);
}
```

### 实现服务

```csharp
public class MyPlugin : IPlugin, IMyPluginService
{
    private string _data = string.Empty;

    public string GetPluginData() => _data;
    public void SetPluginData(string data) => _data = data;

    public async Task<string> ProcessAsync(string input)
    {
        await Task.Delay(100);
        return input.ToUpper();
    }

    // ... IPlugin 实现
}
```

### 调用服务

```csharp
public async Task CallOtherPluginAsync()
{
    var plugin = _pluginService?.GetPlugin("qomicex.example.myplugin");

    if (plugin is IMyPluginService service)
    {
        var data = service.GetPluginData();
        var result = await service.ProcessAsync("hello");
        Console.WriteLine($"结果: {result}");
    }
}
```

## 完整示例

```csharp
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;

namespace MyPlugin;

[Plugin("qomicex.example.communicator", "插件通信示例", "1.0.0",
    Description = "演示插件间通信")]
public class CommunicatorPlugin : IPlugin
{
    private IToastService? _toast;
    private PluginService? _pluginService;

    public string Id => "qomicex.example.communicator";
    public string Name => "插件通信示例";
    public Version Version => new(1, 0, 0);
    public string Description => "演示插件间通信";

    // 公共服务接口
    public string GetStatus() => $"运行中: {Version}";

    public async Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _toast = services.GetService<IToastService>();
        _pluginService = services.GetService<PluginService>();

        ListOtherPlugins();
        return Result<bool, PluginError>.Success(true);
    }

    private void ListOtherPlugins()
    {
        // 列出所有主题插件
        var themes = _pluginService?.GetExtensions<IThemeProvider>();
        foreach (var theme in themes ?? [])
        {
            _toast?.Info($"发现主题: {theme.ThemeName}");
        }

        // 列出所有设置插件
        var settings = _pluginService?.GetExtensions<ISettingsPageProvider>();
        foreach (var provider in settings ?? [])
        {
            _toast?.Info($"发现设置页: {provider.PageTitle}");
        }
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
```

## 最佳实践

1. **检查类型安全**：使用 `is` 检查类型
2. **处理空值**：始终检查返回值是否为 null
3. **避免循环依赖**：插件之间不要形成循环
4. **使用接口**：通过接口进行通信，而不是直接引用

## 下一步

- [最佳实践](../../best-practices/) - 了解更多开发规范
