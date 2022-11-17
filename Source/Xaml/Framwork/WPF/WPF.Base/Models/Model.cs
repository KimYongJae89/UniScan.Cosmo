using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Base.Models
{
    public abstract class Model
    {
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public DateTime RegisteredDate { get; set; } = DateTime.Now;
        public string Description { get; set; }

        public Model()
        {

        }
    }
}
