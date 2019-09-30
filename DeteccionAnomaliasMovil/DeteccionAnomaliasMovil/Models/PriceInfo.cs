using System.Collections.Generic;

namespace DeteccionAnomaliasMovil.Models
{
    public class PriceInfo
    {
        public string granularity { get; set; }
        public List<Series> series { get; set; }
        public double maxAnomalyRatio { get; set; }
        public int sensitivity { get; set; }
    }
}
