using System;
using System.Runtime.CompilerServices;

namespace Common
{
    public class BenchmarkMethodLogger
    {
        public string Method { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public BenchmarkMethodLogger([CallerMemberName] string callingMethod = null)
        {
            this.StartTime = DateTime.UtcNow;
            this.Method = callingMethod;
        }

        public void EndExecution()
        {
            this.EndTime = DateTime.UtcNow;
        }
    }
}
