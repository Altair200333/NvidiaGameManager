using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using ReactiveUI;
using System.Windows.Input;
using DisplayController;
using System;
using System.Xml.Linq;
using System.ComponentModel;
using System.Xml.Xsl;

namespace NvidiaManager.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly DisplayController.DisplayController _controller;

    public DisplayInfo[] Displays => _controller.GetDisplays();

    private int? _selectedDisplayIndex;

    public int? SelectedDisplayIndex
    {
        get => _selectedDisplayIndex;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedDisplayIndex, value);
            if (_selectedDisplayIndex != null && _selectedDisplayIndex != -1)
            {
                _controller.SelectedDisplay = Displays[(int)_selectedDisplayIndex];
            }
        }
    }

    private DisplayConfig[] _configs;

    public DisplayConfig[] Configs
    {
        get => _configs;
        set => this.RaiseAndSetIfChanged(ref _configs, value);
    }

    private int? _selectedConfigIndex;

    public int? SelectedConfigIndex
    {
        get => _selectedConfigIndex;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedConfigIndex, value);
            OnConfigChanged();
        }
    }


    public ICommand AddConfigCommand { get; }
    public ICommand RemoveConfigCommand { get; }
    public ICommand ResetCommand { get; }
    public ICommand SaveCommand { get; }

    private double _brightness;

    public double Brightness
    {
        get => _brightness;
        set
        {
            this.RaiseAndSetIfChanged(ref _brightness, value);
            _controller.SetBrightness(value);
        }
    }

    private double _contrast;

    public double Contrast
    {
        get => _contrast;
        set
        {
            this.RaiseAndSetIfChanged(ref _contrast, value);
            _controller.SetContrast(value);
        }
    }

    private double _gamma;

    public double Gamma
    {
        get => _gamma;
        set
        {
            this.RaiseAndSetIfChanged(ref _gamma, value);
            _controller.SetGamma(value);
        }
    }

    private int _hue = 0;

    public int Hue
    {
        get => _hue;
        set
        {
            this.RaiseAndSetIfChanged(ref _hue, value);
            _controller.SetHue(value);
        }
    }


    private int _vibrance = 1;

    public int Vibrance
    {
        get => _vibrance;
        set
        {
            this.RaiseAndSetIfChanged(ref _vibrance, value);
            _controller.SetVibrance(value);
        }
    }

    private string _newConfigName = "";

    public string NewConfigName
    {
        get => _newConfigName;
        set => this.RaiseAndSetIfChanged(ref _newConfigName, value);
    }

    public MainViewModel()
    {
        _controller = new DisplayController.DisplayController();

        AddConfigCommand = ReactiveCommand.Create(() =>
        {
            _controller.AddConfig(NewConfigName);
            Configs = _controller.GetConfigs();
            SelectedConfigIndex = Configs.Length - 1;
            NewConfigName = "";
        });
        RemoveConfigCommand = ReactiveCommand.Create(() =>
        {
            if (SelectedConfigIndex == null)
            {
                return;
            }
            _controller.RemoveConfig((int)SelectedConfigIndex);
            Configs = _controller.GetConfigs();

            if (Configs.Length > 0)
            {
                SelectedConfigIndex = Configs.Length - 1;
            }
        });
        ResetCommand = ReactiveCommand.Create(() =>
        {
            _controller.ResetSettings();
            SetSettings(_controller.Settings);
        });
        SaveCommand = ReactiveCommand.Create(() =>
        {
            _controller.Save();
        });

        Configs = _controller.GetConfigs();
        SelectedDisplayIndex = 0;
        SelectedConfigIndex = 0;
        SetSettings(_controller.Settings);
        _controller.UpdateSettings();
    }

    private void OnConfigChanged()
    {
        if (SelectedConfigIndex != null)
        {
            _controller.SelectConfig((int)SelectedConfigIndex);
        }

        SetSettings(_controller.Settings);
    }

    private void SetSettings(DisplaySettings settings)
    {
        Brightness = _controller.Settings.brightness;
        Contrast = _controller.Settings.contrast;
        Gamma = _controller.Settings.gamma;
        Vibrance = _controller.Settings.vibrance;
        Hue = _controller.Settings.hue;
    }
}