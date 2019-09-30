using System;

namespace DeteccionAnomaliasMovil.Models
{
    public class Anomaly
    {
        public DateTime Timestamp { get; set; }
        public long Value { get; set; }
        public bool IsPositive { get; set; }
    }
}
