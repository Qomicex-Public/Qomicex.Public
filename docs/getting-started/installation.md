# 环境配置

本指南将帮助你配置插件开发环境。

## 安装 .NET SDK

确保你已安装 .NET 10.0 SDK 或更高版本：

```bash
dotnet --version
```

如果未安装或版本过低，请从 [官网](https://dotnet.microsoft.com/download/dotnet/10.0) 下载安装。

## 安装插件 SDK

创建新的插件项目后，通过 NuGet 安装 Qomicex.PluginSDK：

```bash
dotnet add package Qomicex.PluginSDK
```

## 创建插件项目

### 使用 Visual Studio

1. 打开 Visual Studio
2. 创建新的 **类库 (.NET)** 项目
3. 命名项目，例如 `MyQomicexPlugin`
4. 添加 Qomicex.PluginSDK NuGet 包

### 使用命令行

```bash
# 创建新的类库项目
dotnet new classlib -n MyQomicexPlugin

# 进入项目目录
cd MyQomicexPlugin

# 添加插件 SDK
dotnet add Qomicex.PluginSDK
```

## 项目配置

插件项目的 `.csproj` 文件应类似以下配置：

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Qomicex.PluginSDK" Version="1.0.0" />
  </ItemGroup>
</Project>
```

## 开发工具推荐

- **Visual Studio 2022** - 完整的 IDE，提供强大的调试和智能提示
- **Visual Studio Code** - 轻量级编辑器，适合快速开发
- **JetBrains Rider** - 跨平台 .NET IDE

## 环境变量

Qomicex Launcher 在运行插件时会设置以下环境变量：

| 变量名 | 描述 |
|--------|------|
| `QOMICEX_PLUGIN_PATH` | 插件所在的目录路径 |
| `QOMICEX_APP_VERSION` | 启动器版本号 |
| `QOMICEX_DATA_DIR` | 数据目录路径 |

## 验证环境

创建一个简单的测试文件来验证环境是否配置正确：

```csharp
// TestEnvironment.cs
using System;
using System.Diagnostics;

public static class TestEnvironment
{
    public static void PrintEnvironmentInfo()
    {
        Console.WriteLine("=".PadRight(40, '='));
        Console.WriteLine("环境信息");
        Console.WriteLine("=".PadRight(40, '='));
        Console.WriteLine($"操作系统: {Environment.OSVersion}");
        Console.WriteLine($".NET 版本: {Environment.Version}");
        Console.WriteLine($"机器名称: {Environment.MachineName}");
        Console.WriteLine("=".PadRight(40, '='));
    }
}
```

## 下一步

环境配置完成后，[创建你的第一个插件](first-plugin.md)。
