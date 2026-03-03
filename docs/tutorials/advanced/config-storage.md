# 配置存储教程

本教程将指导你使用 IConfigurationService 持久化插件配置。

## 目标

创建一个插件，将用户配置保存到文件并在下次启动时加载。

## 步骤 1：创建配置类

```csharp
public class PluginSettings
{
    public bool EnableFeature { get; set; } = true;
    public string ApiKey { get; set; } = string.Empty;
    public int RefreshInterval { get; set; } = 60;
    public List<string> RecentItems { get; set; } = new();
}
```

## 步骤 2：创建插件类

```csharp
using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;
using Qomicex.Launcher.Services;
using System;
using System.Threading.Tasks;

[Plugin("qomicex.example.config", "配置存储示例", "1.0.0")]
public class ConfigStoragePlugin : IPlugin
{
    private IConfigurationService? _configService;
    private PluginSettings _settings = new();

    public string Id => "qomicex.example.config";
    public string Name => "配置存储示例";
    public Version Version => new(1, 0, 0);
    public string Description => "演示配置持久化";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        _configService = services.GetService(typeof(IConfigurationService)) as IConfigurationService;

        if (_configService != null)
        {
            // 加载配置
            _settings = _configService.Load<PluginSettings>(Id) ?? new PluginSettings();
            Console.WriteLine($"已加载配置: EnableFeature={_settings.EnableFeature}");
        }

        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        // 关闭前保存配置
        SaveConfig();
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    private void SaveConfig()
    {
        if (_configService != null)
        {
            _configService.Save(_settings, Id);
            Console.WriteLine("配置已保存");
        }
    }

    public void UpdateSettings(bool enableFeature, string apiKey)
    {
        _settings.EnableFeature = enableFeature;
        _settings.ApiKey = apiKey;
        SaveConfig();
    }
}
```

## 配置服务说明

### 加载配置

```csharp
// 加载配置，不存在则使用默认值
var settings = _configService?.Load<PluginSettings>(Id) ?? new PluginSettings();
```

### 保存配置

```csharp
// 保存配置
_configService?.Save(_settings, Id);
```

### 删除配置

```csharp
// 删除配置
_configService?.Delete(Id);
```

### 检查配置是否存在

```csharp
if (_configService?.Exists(Id) == true)
{
    // 配置存在
}
```

## 加密配置

对于敏感信息，可以使用加密存储：

```csharp
// 保存加密配置
_configService?.Save(_settings, Id, encrypt: true, password: "mySecretKey");

// 加载加密配置
var settings = _configService?.Load<PluginSettings>(Id, password: "mySecretKey");
```

## 压缩配置

对于大型配置，可以使用压缩：

```csharp
// 保存压缩配置
_configService?.Save(_settings, Id, compress: true);
```

## 异步操作

配置服务也支持异步操作：

```csharp
// 异步保存
await _configService?.SaveAsync(_settings, Id);

// 异步加载
var settings = await _configService?.LoadAsync<PluginSettings>(Id);

// 异步删除
await _configService?.DeleteAsync(Id);
```

## 最佳实践

### 1. 使用默认值

```csharp
var settings = _configService?.Load<PluginSettings>(Id) ?? new PluginSettings();
```

### 2. 关闭时保存

```csharp
public Task<Result<bool, PluginError>> ShutdownAsync()
{
    _configService?.Save(_settings, Id);
    return Task.FromResult(Result<bool, PluginError>.Success(true));
}
```

### 3. 处理复杂类型

```csharp
public class PluginSettings
{
    public List<string> Strings { get; set; } = new();
    public Dictionary<string, string> Dictionary { get; set; } = new();
    public NestedConfig Nested { get; set; } = new();
}

public class NestedConfig
{
    public string Value { get; set; }
}
```

## 下一步

- [事件处理教程](event-handling.md)
- [最佳实践](../../best-practices/index.md)
