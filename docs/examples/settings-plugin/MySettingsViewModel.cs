using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SettingsPlugin;

public partial class MySettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private string _setting1 = "默认值";

    [ObservableProperty]
    private bool _enableFeature = true;

    [ObservableProperty]
    private int _sliderValue = 50;

    [RelayCommand]
    private void SaveSettings()
    {
        // 保存设置逻辑
    }

    [RelayCommand]
    private void ResetToDefaults()
    {
        Setting1 = "默认值";
        EnableFeature = true;
        SliderValue = 50;
    }
}
