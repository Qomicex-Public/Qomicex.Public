# IConfigurationService 服务

`IConfigurationService` 提供配置持久化功能，支持将配置对象保存到文件并自动加载。

## 命名空间

```csharp
namespace Qomicex.Launcher.Services
```

## 服务接口

```csharp
public interface IConfigurationService
{
    void Save<T>(T config, string key, bool encrypt = false, bool compress = false, string? password = null) where T : class;
    Task SaveAsync<T>(T config, string key, bool encrypt = false, bool compress = false, string? password = null) where T : class;
    T? Load<T>(string key, string? password = null) where T : class;
    Task<T?> LoadAsync<T>(string key, string? password = null) where T : class;
    void Delete(string key);
    Task DeleteAsync(string key);
    bool Exists(string key);
    string GetConfigPath(string key);
}
```

## 方法

| 方法 | 描述 |
|------|------|
| `Save<T>(config, key, ...)` | 同步保存配置 |
| `SaveAsync<T>(config, key, ...)` | 异步保存配置 |
| `Load<T>(key, ...)` | 同步加载配置 |
| `LoadAsync<T>(key, ...)` | 异步加载配置 |
| `Delete(key)` | 删除配置 |
| `DeleteAsync(key)` | 异步删除配置 |
| `Exists(key)` | 检查配置是否存在 |
| `GetConfigPath(key)` | 获取配置文件路径 |

## 使用示例

```csharp
public class MyPlugin : IPlugin
{
    private IConfigurationService? _configService;
    private PluginSettings _settings = new();

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _configService = services.GetService(typeof(IConfigurationService)) as IConfigurationService;

        if (_configService != null)
        {
            // 加载配置
            _settings = _configService.Load<PluginSettings>(Id) ?? new PluginSettings();
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public void SaveSettings()
    {
        _configService?.Save(_settings, Id);
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        // 关闭前保存配置
        _configService?.Save(_settings, Id);
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}

public class PluginSettings
{
    public bool EnableFeature { get; set; } = true;
    public string ApiKey { get; set; } = string.Empty;
    public int RefreshInterval { get; set; } = 60;
}
```

## 相关文档

- [配置存储教程](../tutorials/advanced/config-storage.md)
- [服务注入教程](../tutorials/service-injection.md)
