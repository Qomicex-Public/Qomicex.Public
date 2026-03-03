# 构建与部署

本指南将帮助你构建插件并将其部署到 Qomicex Launcher。

## 构建插件

### 使用命令行

```bash
# 清理旧的构建
dotnet clean

# 发布插件（Release 模式）
dotnet publish -c Release -o ./publish
```

### 使用 Visual Studio

1. 在 Visual Studio 中打开项目
2. 选择 **发布** 菜单
3. 选择配置为 **Release**
4. 点击 **发布** 按钮

## 输出文件

构建成功后，你将在输出目录看到以下文件：

```
publish/
├── MyPlugin.dll                    # 插件主程序集
├── Qomicex.PluginSDK.dll           # SDK 依赖
├── System.*.dll                    # .NET 依赖
└── MyPlugin.deps.json              # 依赖清单
```

## 部署插件

### 方法一：手动复制

1. 找到 Qomicex Launcher 的安装目录
2. 定位到 `Plugins` 子目录
3. 将插件 DLL 和所有依赖文件复制到该目录

### 方法二：使用插件包

创建一个插件包目录结构：

```
MyPluginPackage/
├── plugin.json                     # 插件元数据
├── bin/
│   ├── MyPlugin.dll
│   ├── Qomicex.PluginSDK.dll
│   └── ...                         # 其他依赖
└── resources/                      # 可选：资源文件
```

**plugin.json 示例**：

```json
{
  "id": "qomicex.example.myplugin",
  "name": "我的插件",
  "version": "1.0.0",
  "description": "插件描述",
  "author": "作者",
  "entryPoint": "bin/MyPlugin.dll",
  "minLauncherVersion": "1.0.0",
  "dependencies": []
}
```

### 方法三：通过 Qomicex Launcher 安装

1. 打开 Qomicex Launcher
2. 进入 **设置** → **插件**
3. 点击 **安装插件**
4. 选择插件包或 ZIP 文件

## 验证部署

部署后，在 Qomicex Launcher 中验证插件是否正确加载：

1. 打开插件管理页面
2. 查看插件列表
3. 确认你的插件显示在列表中
4. 检查插件状态是否为"已加载"

## 更新插件

### 版本控制

遵循语义化版本规范 (SemVer)：

- `MAJOR.MINOR.PATCH`
- 主版本号：不兼容的 API 修改
- 次版本号：向下兼容的功能新增
- 修订号：向下兼容的问题修正

### 更新流程

1. 更新项目版本号
2. 修复或添加新功能
3. 重新构建插件
4. 替换旧的插件文件

## 卸载插件

### 手动卸载

1. 停止 Qomicex Launcher
2. 删除插件目录中的相关文件
3. 启动 Qomicex Launcher

### 通过 Launcher 卸载

1. 打开插件管理页面
2. 选择要卸载的插件
3. 点击 **卸载** 按钮

## 故障排除

### 插件加载失败

如果插件无法加载，检查：

1. 依赖文件是否完整
2. .NET 版本是否兼容
3. 插件 ID 是否唯一
4. 控制台日志中的错误信息

### 依赖问题

使用 `dotnet publish` 可以自动处理依赖：

```bash
# 发布并包含所有依赖
dotnet publish -c Release --self-contained -o ./publish
```

### 调试日志

启用详细日志：

```bash
# 通过命令行启动并启用日志
QomicexLauncher.exe --log-level debug --log-file plugin.log
```

## 最佳实践

1. **版本管理**：始终使用语义化版本号
2. **依赖管理**：明确声明所有依赖
3. **测试**：在部署前进行充分测试
4. **日志**：在关键位置添加日志输出
5. **错误处理**：妥善处理所有可能的错误

## 下一步

部署完成后，可以继续学习：
- [生命周期管理](../lifecycle/index.md)
- [API 参考](../api/index.md)
- [高级教程](../tutorials/advanced/index.md)
