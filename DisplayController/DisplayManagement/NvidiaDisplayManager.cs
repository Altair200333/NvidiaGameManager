using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsDisplayAPI;

namespace DisplayController
{
    internal struct DisplayData
    {
        public Display WindowsDisplay;
        public NvAPIWrapper.Display.Display NvDisplay;
        public string Uuid;
    }

    internal class NvidiaDisplayManager: IDisplayManager
    {
        private readonly DisplayData[] _systemDisplays;

        public NvidiaDisplayManager()
        {
            this._systemDisplays = LoadAllDisplays();
        }

        public DisplayInfo[] GetDisplays()
        {
            DisplayInfo[] result = new DisplayInfo[_systemDisplays.Length];
            for (int i = 0; i < _systemDisplays.Length; i++)
            {
                var item = _systemDisplays[i];
                result[i] = new DisplayInfo() { Name = item.WindowsDisplay.ScreenName, Uuid = item.Uuid };
            }

            return result;
        }

        public void ApplyDisplaySettings(DisplayInfo display, DisplaySettings settings)
        {
            var displayData = _systemDisplays.FirstOrDefault(x => x.Uuid == display.Uuid);
            ApplySettingsSafe(displayData, settings);
        }

        private DisplayData[] LoadAllDisplays()
        {
            Display[] windowsDisplays = Display.GetDisplays().ToArray();
            NvAPIWrapper.Display.Display[] nvDisplays = NvAPIWrapper.Display.Display.GetDisplays();

            DisplayData[] results = new DisplayData[windowsDisplays.Length];
            for (int i = 0; i < windowsDisplays.Length; i++)
            {
                // TODO look by name?
                results[i] = new DisplayData()
                {
                    WindowsDisplay = windowsDisplays[i], NvDisplay = nvDisplays[i], Uuid = Guid.NewGuid().ToString()
                };
            }

            return results;
        }

        private void ApplySettingsSafe(DisplayData display, DisplaySettings settings)
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

        private void ApplySettings(DisplayData display, DisplaySettings settings)
        {
            display.NvDisplay.DigitalVibranceControl.CurrentLevel = settings.vibrance;
            display.NvDisplay.HUEControl.CurrentAngle = settings.hue;


            display.WindowsDisplay.GammaRamp =
                new DisplayGammaRamp(settings.brightness, settings.contrast, settings.gamma);
        }
    }
}