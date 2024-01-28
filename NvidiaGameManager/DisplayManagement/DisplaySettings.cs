using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvidiaGameManager.DisplayManagement
{
    /**
     * Limit gamma in range [0.4-2.8]
       Contrast in range [0,1]
       Brightness in range [0,1]
       Vibrance in range [0,100]
       hue in range [0,359]
     */
    struct DisplaySettings
    {
        public double brightness;
        public double contrast;
        public double gamma;

        public int vibrance;
        public int hue;
    }
}