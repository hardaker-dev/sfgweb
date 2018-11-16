using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaasFeeGuides.Extensions
{
    public static class ExceptionExtensions
    {
        public static void Trace(this Exception _this)
        {
            System.Diagnostics.Trace.TraceError("{0:HH:mm:ss.fff} Exception {1}", DateTime.Now, _this);
        }
    }
}
