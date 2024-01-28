using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NvidiaGameManager.DisplayManagement;

namespace NvidiaGameManager.DisplayManagement.Native
{
    internal class MonitorInfo
    {
        public IntPtr Handle;
        public LRect Bounds;

        public MonitorInfo(IntPtr handle, LRect bounds)
        {
            Handle = handle;
            Bounds = bounds;
        }
    }
}