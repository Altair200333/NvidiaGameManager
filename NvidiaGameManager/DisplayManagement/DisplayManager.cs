using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsDisplayAPI;

namespace NvidiaGameManager.DisplayManagement
{
    internal class DisplayManager
    {
        public static DisplayData[] GetAllDisplays()
        {
            Display[] windowsDisplays = Display.GetDisplays().ToArray();
            NvAPIWrapper.Display.Display[] nvDisplays = NvAPIWrapper.Display.Display.GetDisplays();

            DisplayData[] results = new DisplayData[windowsDisplays.Length];
            for (int i = 0; i < windowsDisplays.Length; i++)
            {
                results[i] = new DisplayData() { WindowsDisplay = windowsDisplays[i], NVDisplay = nvDisplays[i] };
            }

            return results;
        }

        public static DisplayData GetDisplayData(int idx = 0)
        {
            Display[] windowsDisplays = Display.GetDisplays().ToArray();
            NvAPIWrapper.Display.Display[] nvDisplays = NvAPIWrapper.Display.Display.GetDisplays();

            return new DisplayData() { WindowsDisplay = windowsDisplays[idx], NVDisplay = nvDisplays[idx] };
        }
    }
}