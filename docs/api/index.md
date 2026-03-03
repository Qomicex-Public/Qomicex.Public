# API 概览

Qomicex Plugin SDK 提供了一组接口和类型，用于开发功能丰富的插件。

## 核心接口

核心接口是所有插件必须实现的基础接口。

- [IPlugin](core-interfaces/IPlugin.md) - 所有插件的基础接口
- [Result 类型](core-interfaces/Result.md) - Railway-Oriented Programming 错误处理类型

## 扩展接口

扩展接口用于添加特定功能的插件。

- [IThemeProvider](extension-interfaces/IThemeProvider.md) - 主题提供者接口
- [ISettingsPageProvider](extension-interfaces/ISettingsPageProvider.md) - 设置页面提供者接口
- [IToastService](extension-interfaces/IToastService.md) - 通知服务接口

## 特性

特性用于标记插件类并提供元数据。

- [PluginAttribute](attributes/PluginAttribute.md) - 插件特性

## 模型

模型定义了插件系统中使用的数据结构。

- [PluginError](models/PluginError.md) - 插件错误类型

## 核心

核心类提供插件加载和运行时的基础功能。

- [PluginLoadContext](core/PluginLoadContext.md) - 插件加载上下文

## 命名空间

| 命名空间 | 描述 |
|----------|------|
| `Qomicex.PluginSDK.Interfaces` | 插件接口定义 |
| `Qomicex.PluginSDK.Attributes` | 插件特性 |
| `Qomicex.PluginSDK.Models` | 数据模型 |
| `Qomicex.PluginSDK.Core` | 核心类 |
| `Qomicex.Core.Common` | 通用类型（包括 Result） |

## 相关文档

- [可用服务](../services/index.md) - 主程序提供的各种服务
- [教程](../) - 插件开发教程
- [最佳实践](../best-practices/index.md) - 开发规范和注意事项
