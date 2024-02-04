using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsDisplayAPI;

namespace DisplayController
{
    public class DisplayController
    {
        private readonly IDisplayManager _controller;
        private DisplayConfig[] _configs;
        private DisplayConfig? _selectedConfig;

        public DisplayInfo? SelectedDisplay { get; set; }

        private DisplaySettings _settings;

        public DisplaySettings Settings
        {
            get => _settings;
        }

        public static DisplaySettings DefaultSettings = new()
        {
            brightness = 0.5,
            contrast = 0.5,
            gamma = 1.0,
            hue = 0,
            vibrance = 50,
        };

        public DisplayController()
        {
            _controller = new NvidiaDisplayManager();
            _configs = ConfigManager.LoadConfig(ConfigManager.DefaultPath);
            if (_configs.Length > 0)
            {
                SelectConfig(0);
            }

            _settings = DefaultSettings;
        }

        public DisplayInfo[] GetDisplays()
        {
            return _controller.GetDisplays();
        }

        public DisplayConfig[] GetConfigs()
        {
            return _configs;
        }

        public void AddConfig(string name)
        {
            var list = _configs.ToList();
            list.Add(new DisplayConfig()
            {
                Name = name,
                Config = Settings
            });
            _configs = list.ToArray();
        }

        public void RemoveConfig(int idx)
        {
            var list = _configs.ToList();
            list.RemoveAt(idx);
            _configs = list.ToArray();
        }

        public void SelectConfig(int index)
        {
            if (index >= 0 && index < _configs.Length)
            {
                _selectedConfig = _configs[index];
                _settings = _selectedConfig.Config;
            }
            else
            {
                _selectedConfig = null;
                _settings = DefaultSettings;
            }

            ApplyChanges();
        }

        public void Save()
        {
            ConfigManager.SaveConfig(_configs);
        }

        public void SetBrightness(double value)
        {
            _settings.brightness = value;
            ApplyChanges();
        }

        public void SetContrast(double value)
        {
            _settings.contrast = value;
            ApplyChanges();
        }

        public void SetGamma(double value)
        {
            _settings.gamma = value;
            ApplyChanges();
        }

        public void SetHue(int value)
        {
            _settings.hue = value;
            ApplyChanges();
        }

        public void SetVibrance(int value)
        {
            _settings.vibrance = value;
            ApplyChanges();
        }

        public void ResetSettings()
        {
            _settings = DefaultSettings;
            ApplyChanges();
        }

        public void UpdateSettings()
        {
            ApplyChanges();
        }

        private void ApplyChanges()
        {
            if (SelectedDisplay == null)
            {
                return;
            }

            _controller.ApplyDisplaySettings(SelectedDisplay, Settings);

            if (_selectedConfig != null)
            {
                _selectedConfig.Config = Settings;
            }
        }
    }
}