using Newtonsoft.Json;
using Standard.DynMvp.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.Base.Models
{
    public abstract class Target
    {
        public string Name { get; set; }
        public DateTime RegisteredDate { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Description { get; set; }

        public Target()
        {
            Name = string.Empty;
            RegisteredDate = DateTime.Now;
            LastModifiedDate = DateTime.Now;
            Description = string.Empty;
        }
    }
}
