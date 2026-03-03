# 兼容性指南

插件需要兼容不同版本的 Qomicex Launcher 和 SDK。本文档介绍版本兼容性处理。

## 语义化版本

遵循语义化版本 (SemVer) 规范：

```
MAJOR.MINOR.PATCH[-PRERELEASE][+BUILD]

示例：
- 1.0.0 - 第一个稳定版本
- 1.1.0 - 添加新功能，向下兼容
- 1.1.1 - 修复问题，向下兼容
- 2.0.0 - 重大变更，可能不兼容
```

## 版本号定义

在 `PluginAttribute` 和 `IPlugin` 属性中定义版本：

```csharp
[Plugin("qomicex.example.myplugin", "我的插件", "1.0.0")]
public class MyPlugin : IPlugin
{
    public Version Version => new(1, 0, 0);
}
```

## 最小启动器版本

在插件元数据中声明兼容的最小启动器版本：

```json
{
  "id": "qomicex.example.myplugin",
  "name": "我的插件",
  "version": "1.0.0",
  "minLauncherVersion": "1.0.0"
}
```

## 版本检查

### 检查启动器版本

```csharp
private const string MinLauncherVersion = "1.0.0";

public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    var launcherVersion = Environment.GetEnvironmentVariable("QOMICEX_APP_VERSION");
    
    if (!string.IsNullOrEmpty(launcherVersion))
    {
        var currentVersion = new Version(launcherVersion);
        var minVersion = new Version(MinLauncherVersion);
        
        if (currentVersion < minVersion)
        {
            return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
            {
                Code = "INCOMPATIBLE_VERSION",
                Message = $"需要 Qomicex Launcher {MinLauncherVersion} 或更高版本，当前版本: {launcherVersion}"
            }));
        }
    }

    return Task.FromResult(Result<bool, PluginError>.Success(true));
}
```

## 特性检测

### 使用特性检测而非版本检测

```csharp
// 好的做法 - 检查特性
if (services.GetService(typeof(IAdvancedFeature)) != null)
{
    // 使用高级特性
}

// 不好的做法 - 检查版本
if (launcherVersion >= new Version(2, 0, 0))
{
    // 使用高级特性
}
```

## API 变更处理

### 处理已弃用的 API

```csharp
public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    // 检查新 API 是否可用
    var newApi = services.GetService(typeof(INewApiService));
    
    if (newApi != null)
    {
        // 使用新 API
        UseNewApi((INewApiService)newApi);
    }
    else
    {
        // 回退到旧 API（如果存在）
        var oldApi = services.GetService(typeof(IOldApiService));
        if (oldApi != null)
        {
            UseOldApi((IOldApiService)oldApi);
        }
        else
        {
            return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
            {
                Code = "API_NOT_FOUND",
                Message = "所需 API 不可用"
            }));
        }
    }

    return Task.FromResult(Result<bool, PluginError>.Success(true));
}
```

## 配置迁移

### 处理配置格式变更

```csharp
private void MigrateConfiguration()
{
    var oldConfigPath = GetConfigPath("old_format");
    var newConfigPath = GetConfigPath("new_format");

    if (File.Exists(oldConfigPath) && !File.Exists(newConfigPath))
    {
        // 迁移旧配置到新格式
        var oldConfig = LoadOldConfig(oldConfigPath);
        var newConfig = ConvertToNewConfig(oldConfig);
        SaveNewConfig(newConfigPath, newConfig);
        
        // 备份旧配置
        File.Move(oldConfigPath, oldConfigPath + ".backup");
    }
}
```

## 运行时兼容性

### 使用可选依赖

```csharp
public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    // 核心服务 - 必需
    var toastService = services.GetService(typeof(IToastService));
    if (toastService == null)
    {
        return Task.FromResult(Result<bool, PluginError>.Failure(new PluginError
        {
            Code = "MISSING_REQUIRED_SERVICE",
            Message = "缺少必需的服务"
        }));
    }

    // 可选服务 - 不必需
    var optionalService = services.GetService(typeof(IOptionalService));
    if (optionalService != null)
    {
        _optionalServiceAvailable = true;
    }

    return Task.FromResult(Result<bool, PluginError>.Success(true));
}
```

## 测试兼容性

### 测试多个版本

在开发过程中测试插件与不同版本启动器的兼容性：

1. 测试最新的稳定版
2. 测试支持的最低版本
3. 测试中间版本

## 最佳实践总结

1. **使用语义化版本**：清晰地表示版本变更
2. **声明最小版本**：指定兼容的最小启动器版本
3. **特性检测**：检查特性而非版本
4. **处理 API 变更**：提供回退方案
5. **迁移配置**：处理配置格式变更
6. **使用可选依赖**：处理服务可用性
7. **测试兼容性**：测试多个版本

## 相关资源

- [PluginAttribute API](../api/attributes/PluginAttribute.md)
- [生命周期](../lifecycle/index.md)
