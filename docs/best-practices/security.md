# 安全注意事项

插件安全性对于保护用户数据和系统安全非常重要。本文档介绍插件开发的安全注意事项。

## 敏感数据处理

### 不在日志中记录敏感信息

```csharp
// 不好的做法
Console.WriteLine($"用户密码: {password}");

// 好的做法
Console.WriteLine($"密码长度: {password.Length} 字符");
```

### 使用加密存储敏感配置

```csharp
// 加密保存配置
_configService?.Save(settings, "pluginId", 
    encrypt: true, 
    password: GetEncryptionKey());

// 加密加载配置
var settings = _configService?.Load<PluginSettings>("pluginId", 
    password: GetEncryptionKey());
```

### 使用安全的密码管理

```csharp
// 使用 SecureString 存储密码（如果支持）
using System.Security;

public SecureString ReadSecurePassword()
{
    var password = new SecureString();
    // 读取密码字符
    return password;
}

// 清除密码
public void ClearSecurePassword(SecureString password)
{
    password.Dispose();
}
```

## 输入验证

### 验证用户输入

```csharp
public void SetApiKey(string apiKey)
{
    // 验证 API 密钥格式
    if (string.IsNullOrWhiteSpace(apiKey))
    {
        throw new ArgumentException("API 密钥不能为空");
    }

    if (apiKey.Length < 16)
    {
        throw new ArgumentException("API 密钥长度不足");
    }

    // 限制长度
    if (apiKey.Length > 256)
    {
        throw new ArgumentException("API 密钥过长");
    }

    _apiKey = apiKey;
}
```

### 验证文件路径

```csharp
public bool IsValidFilePath(string path)
{
    try
    {
        // 获取完整路径，防止路径遍历攻击
        var fullPath = Path.GetFullPath(path);
        
        // 检查路径是否在允许的目录内
        var appDir = AppDomain.CurrentDomain.BaseDirectory;
        return fullPath.StartsWith(appDir, StringComparison.OrdinalIgnoreCase);
    }
    catch
    {
        return false;
    }
}
```

### 验证 URL

```csharp
public bool IsValidUrl(string url)
{
    if (string.IsNullOrWhiteSpace(url))
        return false;

    return Uri.TryCreate(url, UriKind.Absolute, out var uriResult) &&
           (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
}
```

## 网络安全

### 使用 HTTPS

```csharp
// 好的做法
var client = new HttpClient();
var response = await client.GetAsync("https://api.example.com/data");

// 不好的做法
var response = await client.GetAsync("http://api.example.com/data");
```

### 设置超时

```csharp
var client = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(30) // 防止长时间挂起
};
```

### 验证 SSL 证书

```csharp
// HttpClient 默认会验证 SSL 证书
// 不要禁用证书验证
var handler = new HttpClientHandler();
handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
{
    // 自定义验证逻辑，如果需要
    return errors == System.Net.Security.SslPolicyErrors.None;
};

var client = new HttpClient(handler);
```

### 限制数据大小

```csharp
public async Task<string> DownloadDataAsync(string url)
{
    using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
    
    var response = await client.GetAsync(url);
    
    // 检查响应大小
    var contentLength = response.Content.Headers.ContentLength;
    if (contentLength.HasValue && contentLength.Value > 10 * 1024 * 1024)
    {
        throw new InvalidOperationException("响应过大");
    }
    
    return await response.Content.ReadAsStringAsync();
}
```

## 权限管理

### 检查权限

```csharp
public bool HasRequiredPermissions()
{
    try
    {
        // 尝试执行需要权限的操作
        var testPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.txt");
        File.WriteAllText(testPath, "test");
        File.Delete(testPath);
        return true;
    }
    catch (UnauthorizedAccessException)
    {
        return false;
    }
    catch (Exception)
    {
        return false;
    }
}
```

### 使用最小权限原则

只请求必要的权限，避免请求过多权限。

## 沙盒隔离

插件在隔离的加载上下文中运行，但仍需注意：

- 不要尝试访问主程序的内部状态
- 不要修改主程序的配置文件
- 不要读取其他插件的数据

## 错误处理

### 不暴露敏感信息

```csharp
// 不好的做法 - 可能暴露敏感信息
catch (Exception ex)
{
    return Result<bool, PluginError>.Failure(new PluginError
    {
        Code = "ERROR",
        Message = ex.ToString() // 包含堆栈跟踪
    });
}

// 好的做法
catch (Exception ex)
{
    return Result<bool, PluginError>.Failure(new PluginError
    {
        Code = "ERROR",
        Message = "操作失败，请检查设置"
    });
}
```

### 记录错误时脱敏

```csharp
private string SanitizeForLog(string message)
{
    // 移除敏感信息
    return message
        .Replace(_apiKey, "***")
        .Replace(_password, "***");
}
```

## 第三方依赖

### 使用可信的来源

只从可信的 NuGet 源添加依赖包。

### 定期更新依赖

保持依赖包更新，以获取安全修复。

### 审查依赖

使用工具审查依赖的安全性：

```bash
# 使用 dotnet list package --vulnerable 检查漏洞
dotnet list package --vulnerable
```

## 最佳实践总结

1. **保护敏感数据**：使用加密和安全的存储方式
2. **验证所有输入**：包括用户输入和外部数据
3. **使用 HTTPS**：所有网络通信使用加密连接
4. **设置超时**：防止资源耗尽
5. **限制数据大小**：防止 DoS 攻击
6. **不暴露敏感信息**：错误消息中不包含敏感数据
7. **使用可信依赖**：从可信来源获取依赖包
8. **定期更新**：保持依赖包更新

## 相关资源

- [最佳实践](index.md)
- [错误处理](error-handling.md)
