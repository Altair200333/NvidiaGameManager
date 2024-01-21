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
using System.Threading;
using Avalonia.Controls.Primitives;
using NvAPIWrapper.Native.GPU;
using NvAPIWrapper.Native.GPU.Structures;
using NvAPIWrapper.Native;

namespace NvidiaGameManager.Views;

public partial class MainView : UserControl
{
    Display display = new Display();

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public MainView()
    {
        InitializeComponent();
        display.Initialize();
    }


    public void ButtonClicked(object source, RoutedEventArgs args)
    {
        Debug.WriteLine("Click!");
        var monitor = display.GetDisplays().First();

        display.SetBrightness(monitor, 100);
        var temp = display.GetTemperature(monitor);

        Debug.WriteLine($"T {temp}");
        display.SetTemperature(monitor, MC_COLOR_TEMPERATURE.MC_COLOR_TEMPERATURE_6500K);
    }

    private void Brightness_OnValueChanged(object? source, RangeBaseValueChangedEventArgs e)
    {
        var value = e.NewValue;
        var monitor = display.GetDisplays().First();
        display.SetBrightness(monitor, (uint)value);
    }

    private void Contrast_OnValueChanged(object? source, RangeBaseValueChangedEventArgs e)
    {
        var value = e.NewValue;
        var monitor = display.GetDisplays().First();
        display.SetContrast(monitor, (uint)value);
    }
}