using System;
using System.Diagnostics;

namespace Common
{
    public class BenchmarkMethodLogger
    {
        public string Method { get; set; }
        public Guid ID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public BenchmarkMethodLogger()
        {
            this.StartTime = DateTime.UtcNow;
            this.ID = Guid.NewGuid();
            this.Method = new StackTrace().GetFrame(1).GetMethod().Name;
        }

        public void EndExecution()
        {
            this.EndTime = DateTime.UtcNow;
        }
    }
}
