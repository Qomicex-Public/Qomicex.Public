# ViewModels

Qomicex 启动器中的 ViewModel 类，可用于插件集成。

## 常用 ViewModel

### SettingsViewModel

设置页面的主 ViewModel，用于添加插件设置页面。

```csharp
// 添加插件设置页
public void AddPluginSettingsPage(string title, object viewModel, object view);
```

## 使用示例

### 获取 SettingsViewModel

```csharp
private SettingsViewModel? _settingsViewModel;

public async Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    _settingsViewModel = services.GetService<SettingsViewModel>();
    return Result<bool, PluginError>.Success(true);
}
```

### 添加设置页面

```csharp
public void AddSettingsPage()
{
    var vm = new MySettingsViewModel();
    var view = new MySettingsView();

    _settingsViewModel?.AddPluginSettingsPage("我的设置", vm, view);
}
```

## 注意事项

1. 使用 `ISettingsPageProvider` 接口是添加设置页面的推荐方式
2. ViewModel 和 View 必须兼容
3. 确保线程安全

## 相关文档

- [ISettingsPageProvider](../api/extension-interfaces/ISettingsPageProvider) - 设置页提供者
