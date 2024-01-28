using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls.Primitives;
using NvidiaGameManager.ConfigurationManager;
using NvidiaGameManager.DisplayManagement;
using NvidiaGameManager.ViewModels;
using System.Runtime;
using System.Collections.Generic;

namespace NvidiaGameManager.Views;

public partial class MainView : UserControl
{
    // TODO read current settings at startup
    private DisplaySettings _settings = SettingsManager.DefaultSettings;
    private DisplayData _display;
    private DisplayData[] _allDisplays;

    private DisplayConfig[] _allConfigs;
    private DisplayConfig? _currentConfig;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public MainView()
    {
        InitializeComponent();

        _allDisplays = DisplayManager.GetAllDisplays();
        _display = DisplayManager.GetDisplayData();
        _allConfigs = ConfigManager.LoadConfig(ConfigManager.DefaultPath);

        this.Loaded += ControlLoaded;
    }

    void ControlLoaded(object? sender, RoutedEventArgs e)
    {
        this.LoadDefaultData();
    }

    private void LoadDefaultData()
    {
        var dataContext = (this.DataContext as MainViewModel);
        var displaySelector = GetDisplaySelector();

        dataContext.AllDisplays = _allDisplays.Select(x => x.WindowsDisplay.DisplayName).ToArray();

        displaySelector.ItemsSource = null;
        displaySelector.ItemsSource = dataContext.AllDisplays;

        displaySelector.SelectedIndex = 0;

        UpdateConfigList();
    }

    private void UpdateConfigList(bool updateSelection = true)
    {
        var dataContext = (this.DataContext as MainViewModel);
        var configSelector = GetConfigSelector();

        dataContext.AllConfigs = _allConfigs.Select(x => x.Name).ToArray();

        configSelector.ItemsSource = null;
        configSelector.ItemsSource = dataContext.AllConfigs;
        if (updateSelection)
        {
            configSelector.SelectedIndex = _allConfigs.Length - 1;
        }
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

    private void UpdateSettings(DisplaySettings settings)
    {
        ApplySettings(settings);
        SetSliderValue("Brightness", settings.brightness);
        SetSliderValue("Contrast", settings.contrast);
        SetSliderValue("Gamma", settings.gamma);
        SetSliderValue("Vibrance", settings.vibrance);
        SetSliderValue("Hue", settings.hue);
    }

    // reset all
    public void ResetClicked(object source, RoutedEventArgs args)
    {
        UpdateSettings(SettingsManager.DefaultSettings);
    }

    // save config to file
    private void SaveClicked(object? sender, RoutedEventArgs e)
    {
        ConfigManager.SaveConfig(_allConfigs);
    }

    private void ApplySettings(DisplaySettings settings)
    {
        SettingsManager.ApplySettingsSafe(_display, settings);
        if (_currentConfig != null)
        {
            _currentConfig.Config = settings;
        }
    }

    private void Brightness_OnValueChanged(object? source, RangeBaseValueChangedEventArgs e)
    {
        _settings.brightness = e.NewValue;
        ApplySettings(_settings);
    }

    private void Contrast_OnValueChanged(object? source, RangeBaseValueChangedEventArgs e)
    {
        _settings.contrast = e.NewValue;
        ApplySettings(_settings);
    }

    public void Gamma_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        _settings.gamma = e.NewValue;
        ApplySettings(_settings);
    }

    private void Vibrance_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        _settings.vibrance = (int)e.NewValue;
        ApplySettings(_settings);
    }

    private void Hue_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        _settings.hue = (int)e.NewValue;
        ApplySettings(_settings);
    }

    private ComboBox GetDisplaySelector()
    {
        return this.FindControl<ComboBox>("DisplaySelector");
    }

    private ComboBox GetConfigSelector()
    {
        return this.FindControl<ComboBox>("ConfigSelector");
    }

    private TextBox GetNameInput()
    {
        return this.FindControl<TextBox>("ConfigName");
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

    private void ConfigSelector_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var comboBox = GetConfigSelector();
        if (comboBox.SelectedIndex == -1)
        {
            return;
        }

        _currentConfig = _allConfigs[comboBox.SelectedIndex];
        UpdateSettings(_currentConfig.Config);
    }

    private void AddClicked(object? sender, RoutedEventArgs e)
    {
        var nameInput = GetNameInput();
        if (String.IsNullOrEmpty(nameInput.Text))
        {
            Debug.WriteLine("Empty config");
            return;
        }

        var list = _allConfigs.ToList();
        list.Add(new DisplayConfig()
        {
            Name = nameInput.Text,
            Config = _currentConfig?.Config ?? SettingsManager.DefaultSettings
        });

        _allConfigs = list.ToArray();
        UpdateConfigList();
        var comboBox = GetConfigSelector();
        comboBox.SelectedIndex = _allConfigs.Length - 1;
    }

    private void RemoveClicked(object? sender, RoutedEventArgs e)
    {
        if (_currentConfig == null || _allConfigs.Length <= 1 || _currentConfig?.Name == "Default")
        {
            return;
        }

        var list = _allConfigs.ToList();
        list.Remove(_currentConfig);

        _allConfigs = list.ToArray();
        UpdateConfigList();
        var comboBox = GetConfigSelector();
        comboBox.SelectedIndex = _allConfigs.Length - 1;
    }
}