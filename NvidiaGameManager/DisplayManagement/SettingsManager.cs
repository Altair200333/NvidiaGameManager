using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsDisplayAPI;

namespace NvidiaGameManager.DisplayManagement
{
    internal class SettingsManager
    {
        public static DisplaySettings DefaultSettings = new()
        {
            brightness = 0.5,
            contrast = 0.5,
            gamma = 1.0,
            hue = 0,
            vibrance = 50,
        };

        public static void ResetSettings(DisplayData display)
        {
            ApplySettings(display, DefaultSettings);
        }

        public static void ApplySettings(DisplayData display, DisplaySettings settings)
        {
            display.NVDisplay.DigitalVibranceControl.CurrentLevel = settings.vibrance;
            display.NVDisplay.HUEControl.CurrentAngle = settings.hue;


            display.WindowsDisplay.GammaRamp =
                new DisplayGammaRamp(settings.brightness, settings.contrast, settings.gamma);
        }

        public static void ApplySettingsSafe(DisplayData display, DisplaySettings settings)
        {
            try
            {
                ApplySettings(display, settings);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Invalid settings passed");
            }
        }


    }
}