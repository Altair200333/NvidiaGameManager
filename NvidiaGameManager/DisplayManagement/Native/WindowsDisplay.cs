using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NvidiaGameManager.DisplayManagement;

namespace NvidiaGameManager.DisplayManagement.Native
{
    internal class WindowsDisplay
    {
        private readonly List<MonitorInfo> _displays = new();

        public WindowsDisplay()
        {
            LoadDisplays();
        }

        public void Initialize()
        {
            LoadDisplays();
        }

        private void LoadDisplays()
        {
            _displays.Clear();
            var callback = new DisplayApi.MonitorEnumProcedure(
                (IntPtr handle, IntPtr dcHandle, ref LRect rect, IntPtr callbackObject) =>
                {
                    uint physicalMonitorsCount = 0;
                    if (!DisplayApi.GetNumberOfPhysicalMonitorsFromHMONITOR(handle, ref physicalMonitorsCount))
                    {
                        Debug.WriteLine($"no monitors for {handle}");
                        return 1;
                    }

                    var physicalMonitors = new PHYSICAL_MONITOR[physicalMonitorsCount];
                    if (!DisplayApi.GetPhysicalMonitorsFromHMONITOR(handle, physicalMonitorsCount, physicalMonitors))
                    {
                        Debug.WriteLine($"Could not find physical monitors for {handle}");
                        return 1;
                    }

                    foreach (var monitor in physicalMonitors)
                    {
                        _displays.Add(new MonitorInfo(monitor.hPhysicalMonitor, rect));
                    }


                    return 1;
                }
            );

            DisplayApi.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, callback, IntPtr.Zero);
        }

        public MonitorInfo[] GetDisplays()
        {
            return _displays.ToArray();
        }

        private uint NormalizeValue(uint current, uint min, uint max)
        {
            return (uint)(min + (max - min) * Math.Clamp(current, min, max) / 100.0f);
        }

        public void SetBrightness(MonitorInfo monitor, uint value)
        {
            uint minValue = 0, currentValue = 0, maxValue = 0;
            DisplayApi.GetMonitorBrightness(monitor.Handle, ref minValue, ref currentValue, ref maxValue);

            DisplayApi.SetMonitorBrightness(monitor.Handle, NormalizeValue(value, minValue, maxValue));
        }

        public void SetContrast(MonitorInfo monitor, uint value)
        {
            uint minValue = 0, currentValue = 0, maxValue = 0;
            DisplayApi.GetMonitorBrightness(monitor.Handle, ref minValue, ref currentValue, ref maxValue);

            DisplayApi.SetMonitorContrast(monitor.Handle, NormalizeValue(value, minValue, maxValue));
        }


        public MC_COLOR_TEMPERATURE GetTemperature(MonitorInfo monitor)
        {
            MC_COLOR_TEMPERATURE temp = MC_COLOR_TEMPERATURE.MC_COLOR_TEMPERATURE_UNKNOWN;
            DisplayApi.GetMonitorColorTemperature(monitor.Handle, ref temp);
            return temp;
        }

        public void SetTemperature(MonitorInfo monitor, MC_COLOR_TEMPERATURE temperature)
        {
            DisplayApi.SetMonitorColorTemperature(monitor.Handle, temperature);
        }
    }
}