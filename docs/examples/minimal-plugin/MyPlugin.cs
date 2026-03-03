using Qomicex.PluginSDK.Attributes;
using Qomicex.PluginSDK.Interfaces;
using Qomicex.PluginSDK.Models;

namespace MinimalPlugin;

[Plugin("qomicex.example.minimal", "最小插件", "1.0.0",
    Description = "最小的插件示例")]
public class MyPlugin : IPlugin
{
    public string Id => "qomicex.example.minimal";
    public string Name => "最小插件";
    public Version Version => new(1, 0, 0);
    public string Description => "最小的插件示例";

    public Task<Result<bool, PluginError>> InitializeAsync(IServiceProvider services)
    {
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }

    public Task<Result<bool, PluginError>> ShutdownAsync()
    {
        return Task.FromResult(Result<bool, PluginError>.Success(true));
    }
}
