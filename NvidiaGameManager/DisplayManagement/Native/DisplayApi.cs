using System;
using System.Runtime.InteropServices;

namespace NvidiaGameManager.DisplayManagement.Native
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct PHYSICAL_MONITOR
    {
        public IntPtr hPhysicalMonitor;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string PhysicalMonitorDescription;
    }

    [Flags]
    public enum MC_COLOR_TEMPERATURE
    {
        MC_COLOR_TEMPERATURE_UNKNOWN,
        MC_COLOR_TEMPERATURE_4000K,
        MC_COLOR_TEMPERATURE_5000K,
        MC_COLOR_TEMPERATURE_6500K,
        MC_COLOR_TEMPERATURE_7500K,
        MC_COLOR_TEMPERATURE_8200K,
        MC_COLOR_TEMPERATURE_9300K,
        MC_COLOR_TEMPERATURE_10000K,
        MC_COLOR_TEMPERATURE_11500K
    };

    /**
     * Exposes some part of windows native API that controls display settings (brightness, contrast, temperature)
     */
    public class DisplayApi
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate int MonitorEnumProcedure(
            IntPtr monitorHandle,
            IntPtr dcHandle,
            ref LRect rect,
            IntPtr callbackObject
        );

        [DllImport("user32")]
        internal static extern bool EnumDisplayMonitors(
            [In] IntPtr dcHandle,
            [In] IntPtr clip,
            MonitorEnumProcedure callback,
            IntPtr callbackObject
        );

        [DllImport("user32.dll", EntryPoint = "MonitorFromWindow")]
        public static extern IntPtr MonitorFromWindow([In] IntPtr hwnd, uint dwFlags);

        [DllImport("dxva2.dll", EntryPoint = "DestroyPhysicalMonitors")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyPhysicalMonitors(uint dwPhysicalMonitorArraySize,
            ref PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", EntryPoint = "GetNumberOfPhysicalMonitorsFromHMONITOR")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor,
            ref uint pdwNumberOfPhysicalMonitors);

        [DllImport("dxva2.dll", EntryPoint = "GetPhysicalMonitorsFromHMONITOR")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint dwPhysicalMonitorArraySize,
            [Out] PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorBrightness")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorBrightness(IntPtr handle, ref uint minimumBrightness,
            ref uint currentBrightness, ref uint maxBrightness);

        [DllImport("dxva2.dll", EntryPoint = "SetMonitorBrightness")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetMonitorBrightness(IntPtr handle, uint newBrightness);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorContrast")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorContrast(IntPtr handle, ref uint minimumContrast,
            ref uint currentContrast, ref uint maxContrast);

        [DllImport("dxva2.dll", EntryPoint = "SetMonitorContrast")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetMonitorContrast(IntPtr handle, uint newContrast);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorColorTemperature")]
        public static extern void GetMonitorColorTemperature(IntPtr handle, ref MC_COLOR_TEMPERATURE currentTemperature);

        [DllImport("dxva2.dll", EntryPoint = "SetMonitorColorTemperature")]
        public static extern void SetMonitorColorTemperature(IntPtr handle, MC_COLOR_TEMPERATURE ctCurrentColorTemperature);
    }
}