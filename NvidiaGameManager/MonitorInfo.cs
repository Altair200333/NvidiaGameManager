using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NvidiaGameManager
{
    internal class MonitorInfo
    {
        public IntPtr Handle;
        public LRect Bounds;

        public MonitorInfo(IntPtr handle, LRect bounds)
        {
            this.Handle = handle;
            this.Bounds = bounds;
        }
    }
}