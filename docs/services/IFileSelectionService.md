# IFileSelectionService

提供文件和文件夹选择对话框的服务。

## 获取服务

```csharp
private IFileSelectionService? _fileService;

public async Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    _fileService = services.GetService<IFileSelectionService>();
    return Result<bool, PluginError>.Success(true);
}
```

## 方法

### SelectFilesAsync

打开文件选择对话框。

```csharp
Task<IReadOnlyList<string>> SelectFilesAsync(string title, IReadOnlyList<string>? fileFilter = null);
```

**参数**：
- `title` - 对话框标题
- `fileFilter` - 文件扩展名过滤器，例如 `["*.exe", "*.cmd"]`

**返回值**：
- 选中的文件路径列表

### PickFolderAsync

打开文件夹选择对话框。

```csharp
Task<string?> PickFolderAsync(string title);
```

**参数**：
- `title` - 对话框标题

**返回值**：
- 选中的文件夹路径，取消时返回 null

## 使用示例

### 选择文件

```csharp
public async Task SelectGameExeAsync()
{
    var files = await _fileService?.SelectFilesAsync(
        "选择游戏可执行文件",
        new[] { "*.exe", "*.cmd", "*.sh" });

    if (files != null && files.Count > 0)
    {
        var selectedFile = files[0];
        _toast?.Success($"已选择: {selectedFile}");
    }
}
```

### 选择多个文件

```csharp
public async Task SelectModFilesAsync()
{
    var files = await _fileService?.SelectFilesAsync(
        "选择 Mod 文件",
        new[] { "*.jar", "*.zip" });

    if (files != null)
    {
        foreach (var file in files)
        {
            ProcessModFile(file);
        }
    }
}
```

### 选择文件夹

```csharp
public async Task SelectGameDirectoryAsync()
{
    var folder = await _fileService?.PickFolderAsync("选择游戏目录");

    if (!string.IsNullOrEmpty(folder))
    {
        _gameDirectory = folder;
        _toast?.Success($"游戏目录: {folder}");
    }
}
```

## 常用文件过滤器

| 过滤器 | 说明 |
|--------|------|
| `["*.exe"]` | Windows 可执行文件 |
| `["*.jar"]` | Java 档案文件 |
| `["*.zip"]` | ZIP 压缩文件 |
| `["*.json"]` | JSON 配置文件 |
| `["*.png", "*.jpg"]` | 图片文件 |

## 最佳实践

1. 检查返回值是否为 null
2. 验证选中的文件路径
3. 提供有意义的标题

```csharp
var files = await _fileService?.SelectFilesAsync("选择文件");
if (files != null && files.Count > 0)
{
    // 处理文件
}
```

## 相关文档

- [AccountService](AccountService) - 账户服务
