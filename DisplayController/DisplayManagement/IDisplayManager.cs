using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayController
{
    internal interface IDisplayManager
    {
        public DisplayInfo[] GetDisplays();

        public void ApplyDisplaySettings(DisplayInfo display, DisplaySettings settings);
    }
}