# AccountService

管理 Minecraft 账户的服务。

## 获取服务

```csharp
private AccountService? _accountService;

public async Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
{
    _accountService = services.GetService<AccountService>();
    return Result<bool, PluginError>.Success(true);
}
```

## 属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `Accounts` | `ObservableCollection<AccountModel>` | 账户列表 |
| `SelectedAccount` | `AccountModel?` | 当前选中的账户 |
| `AccountCount` | `int` | 账户数量 |
| `Instance` | `AccountService` | 单例实例 |

## 方法

### AddAccount

添加账户。

```csharp
bool AddAccount(AccountModel account);
```

### RemoveAccount

删除账户。

```csharp
bool RemoveAccount(AccountModel account);
```

### SelectAccount

选中账户。

```csharp
void SelectAccount(AccountModel? account);
```

### UpdateAccount

更新账户信息。

```csharp
void UpdateAccount(AccountModel account);
```

### GetAccountById

根据 ID 获取账户。

```csharp
AccountModel? GetAccountById(Guid id);
```

### GetAccountByUsername

根据用户名获取账户。

```csharp
AccountModel? GetAccountByUsername(string username);
```

### RefreshAccountAsync

刷新账户令牌。

```csharp
Task<bool> RefreshAccountAsync(AccountModel account);
```

### ExportAccounts

导出账户。

```csharp
void ExportAccounts(string filePath);
```

### ImportAccounts

导入账户。

```csharp
int ImportAccounts(string filePath);
```

### ClearAllAccounts

清除所有账户。

```csharp
void ClearAllAccounts();
```

## 使用示例

### 获取当前账户

```csharp
public void LogCurrentAccount()
{
    var currentAccount = _accountService?.SelectedAccount;

    if (currentAccount != null)
    {
        Console.WriteLine($"当前账户: {currentAccount.Username}");
        Console.WriteLine($"类型: {currentAccount.AccountType}");
    }
}
```

### 获取所有账户

```csharp
public void ListAllAccounts()
{
    var accounts = _accountService?.Accounts;

    if (accounts != null)
    {
        foreach (var account in accounts)
        {
            Console.WriteLine($"- {account.Username}");
        }
    }
}
```

### 添加离线账户

```csharp
public void AddOfflineAccount(string username)
{
    var account = new AccountModel
    {
        Id = Guid.NewGuid(),
        Username = username,
        AccountType = AccountType.Offline,
        Uuid = Guid.NewGuid().ToString("N")
    };

    _accountService?.AddAccount(account);
}
```

### 刷新账户

```csharp
public async Task RefreshAsync()
{
    var account = _accountService?.SelectedAccount;
    if (account != null)
    {
        var success = await _accountService?.RefreshAccountAsync(account);
        if (success)
        {
            _toast?.Success("账户刷新成功");
        }
    }
}
```

## AccountModel 属性

| 属性 | 类型 | 说明 |
|------|------|------|
| `Id` | `Guid` | 本地 ID |
| `Username` | `string` | 用户名 |
| `Uuid` | `string` | Minecraft UUID |
| `AccountType` | `AccountType` | 账户类型 |
| `Status` | `AccountStatus` | 账户状态 |
| `AccessToken` | `string` | 访问令牌 |
| `RefreshToken` | `string` | 刷新令牌 |

## AccountType 枚举

| 值 | 说明 |
|-----|------|
| `Microsoft` | 微软账户 |
| `External` | 外部认证服务器 |
| `Offline` | 离线模式 |

## 最佳实践

1. 检查服务可用性
2. 验证账户状态
3. 处理刷新失败

```csharp
var account = _accountService?.SelectedAccount;
if (account != null)
{
    var success = await _accountService?.RefreshAccountAsync(account);
    if (!success)
    {
        _toast?.Error("账户刷新失败");
    }
}
```

## 相关文档

- [RunningInstanceManager](RunningInstanceManager) - 实例管理器
