using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPF.SEMCNS.Offline.Models
{
    public class Result
    {
        public DateTime InspectTime { get; set; }
        public TargetParam TargetParam { get; set; }

        [JsonIgnore]
        public ImageSource ImageSource { get; set; }
        public IEnumerable<Defect> Defects { get; set; }
    }
}
