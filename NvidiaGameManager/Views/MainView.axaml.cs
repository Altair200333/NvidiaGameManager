using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NvAPIWrapper.GPU;
using System.Diagnostics;
using System.Linq;
using NvAPIWrapper.Display;
using System.Runtime.InteropServices;
using System;
using Avalonia.VisualTree;
using WindowsDisplayAPI;
using Display = WindowsDisplayAPI.Display;
using Avalonia.Platform;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Avalonia.Controls.Primitives;
using NvAPIWrapper.Native.GPU;
using NvAPIWrapper.Native.GPU.Structures;
using NvAPIWrapper.Native;
using NvidiaGameManager.DisplayManagement;
using static NvAPIWrapper.Native.Mosaic.Structures.DisplayTopologyStatus;
using NvidiaGameManager.DisplayManagement.Native;
using DisplayApi = NvidiaGameManager.DisplayManagement.Native.DisplayApi;
using System.Diagnostics.Contracts;
using NvidiaGameManager.ViewModels;

namespace NvidiaGameManager.Views;

public partial class MainView : UserControl
{
    // TODO read current settings at startup
    private DisplaySettings _settings = SettingsManager.DefaultSettings;
    private DisplayData _display;
    private DisplayData[] _allDisplays;

    public enum ShapeType
    {
        RedCircle,
        RedSquare,
        BlueCircle,
        BlueSquare
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public MainView()
    {
        InitializeComponent();

        _allDisplays = DisplayManager.GetAllDisplays();
        _display = DisplayManager.GetDisplayData();

        this.Loaded += ControlLoaded;
    }

    void ControlLoaded(object? sender, RoutedEventArgs e)
    {
        var dataContext = (this.DataContext as MainViewModel);
        var comboBox = GetDisplaySelector();

        dataContext.AllDisplays = _allDisplays.Select(x => x.WindowsDisplay.DisplayName).ToArray();

        comboBox.ItemsSource = null;
        comboBox.ItemsSource = dataContext.AllDisplays;

        comboBox.SelectedIndex = 0;
    }

    private void SetSliderValue(string name, double value)
    {
        var slider = this.FindControl<Slider>(name);
        if (slider != null)
        {
            slider.Value = value;
        }
        else
        {
            Debug.WriteLine($"Could not find slider {name}");
        }
    }

    // reset all
    public void ResetClicked(object source, RoutedEventArgs args)
    {
        SettingsManager.ResetSettings(_display);
        var defaultSettings = SettingsManager.DefaultSettings;
        SetSliderValue("Brightness", defaultSettings.brightness);
        SetSliderValue("Contrast", defaultSettings.contrast);
        SetSliderValue("Gamma", defaultSettings.gamma);
        SetSliderValue("Vibrance", defaultSettings.vibrance);
        SetSliderValue("Hue", defaultSettings.hue);
    }

    private void Brightness_OnValueChanged(object? source, RangeBaseValueChangedEventArgs e)
    {
        _settings.brightness = e.NewValue;
        SettingsManager.ApplySettingsSafe(_display, _settings);
    }

    private void Contrast_OnValueChanged(object? source, RangeBaseValueChangedEventArgs e)
    {
        _settings.contrast = e.NewValue;
        SettingsManager.ApplySettingsSafe(_display, _settings);
    }

    public void Gamma_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        _settings.gamma = e.NewValue;
        SettingsManager.ApplySettingsSafe(_display, _settings);
    }

    private void Vibrance_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        _settings.vibrance = (int)e.NewValue;
        SettingsManager.ApplySettingsSafe(_display, _settings);
    }

    private void Hue_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        _settings.hue = (int)e.NewValue;
        SettingsManager.ApplySettingsSafe(_display, _settings);
    }

    private ComboBox GetDisplaySelector()
    {
        return this.FindControl<ComboBox>("DisplaySelector");
    }

    private void SetSelectedDisplay(DisplayData display)
    {
        _display = display;
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var comboBox = GetDisplaySelector();
        SetSelectedDisplay(_allDisplays[comboBox.SelectedIndex]);
    }
}