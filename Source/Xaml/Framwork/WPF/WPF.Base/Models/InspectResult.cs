using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Base.Models
{
    public abstract class InspectResult
    {
        Model _model;
        DateTime _inspectTime;

        public Model Model { get => _model; }
        public DateTime InspectTime { get => _inspectTime; }

        public InspectResult(Model model)
        {
            _model = model;
            _inspectTime = DateTime.Now;
        }
    }
}
