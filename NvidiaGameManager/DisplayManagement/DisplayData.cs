using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsDisplayAPI;

namespace NvidiaGameManager.DisplayManagement
{
    internal struct DisplayData
    {
        public Display WindowsDisplay;
        public NvAPIWrapper.Display.Display NVDisplay;
    }
}
