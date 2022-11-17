using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.COSMO.Offline.Models
{
    public class DailyInfo
    {
        public DateTime DateTime { get; set; }
        public int ResultCount { get; set; }
        public List<Defect> defect { get; set; } = new List<Defect>();
    }
}
