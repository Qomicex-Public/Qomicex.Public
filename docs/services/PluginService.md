# PluginService

管理插件加载、卸载和查询的服务。

## 获取服务

```csharp
private PluginService? _pluginService;

public async Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    _pluginService = services.GetService<PluginService>();
    return Result<bool, PluginError>.Success(true);
}
```

## 方法

### GetPlugin

根据 ID 获取插件。

```csharp
IPlugin? GetPlugin(string id);
```

### GetExtensions

获取指定类型的插件扩展。

```csharp
IEnumerable<T> GetExtensions<T>() where T : class;
```

### UnloadAllPluginsAsync

卸载所有插件。

```csharp
Task UnloadAllPluginsAsync();
```

## 使用示例

### 获取特定插件

```csharp
public void CheckPlugin()
{
    var plugin = _pluginService?.GetPlugin("qomicex.example.myplugin");

    if (plugin != null)
    {
        Console.WriteLine($"插件名称: {plugin.Name}");
        Console.WriteLine($"插件版本: {plugin.Version}");
    }
}
```

### 获取所有主题插件

```csharp
public void ListThemes()
{
    var themes = _pluginService?.GetExtensions<IThemeProvider>();

    foreach (var theme in themes ?? [])
    {
        Console.WriteLine($"主题: {theme.ThemeName}");
    }
}
```

### 获取所有设置插件

```csharp
public void ListSettings()
{
    var settingsProviders = _pluginService?.GetExtensions<ISettingsPageProvider>();

    foreach (var provider in settingsProviders ?? [])
    {
        Console.WriteLine($"设置页: {provider.PageTitle}");
    }
}
```

### `检测插件是否存在`

```csharp
public bool HasPlugin(string pluginId)
{
    return _pluginService?.GetPlugin(pluginId) != null;
}
```

## 插件目录

插件存放在以下位置：

| 平台 | 路径 |
|------|------|
| Windows | `%LocalAppData%\Qomicex\QML\Plugins\` |
| Linux | `~/.local/share/Qomicex/QML/Plugins/` |
| macOS | `~/Library/Application Support/Qomicex/QML/Plugins/` |

## 最佳实践

1. 检查服务可用性
2. 验证插件是否存在
3. 处理空结果

```csharp
var plugin = _pluginService?.GetPlugin("qomicex.example.myplugin");
if (plugin != null)
{
    // 使用插件
}
```

## 相关文档

- [IPlugin](../api/core-interfaces/IPlugin) - 插件接口
