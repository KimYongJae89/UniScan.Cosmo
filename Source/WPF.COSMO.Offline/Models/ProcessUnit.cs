using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Base.Helpers;

namespace WPF.COSMO.Offline.Models
{
    public class ProcessUnit : Observable
    {
        bool _processing;
        public bool Processing
        {
            get => _processing;
            set
            {
                if (value)
                {
                    Success = false;
                    Fail = false;
                }

                Set(ref _processing, value);
            }
        }

        bool _success;
        public bool Success
        {
            get => _success;
            set
            {
                if (value)
                {
                    Processing = false;
                    Fail = false;
                }
                Set(ref _success, value);
            }
        }

        bool _fail;
        public bool Fail
        {
            get => _fail;
            set
            {
                if (value)
                {
                    Processing = false;
                    Success = false;
                }
                Set(ref _fail, value);
            }
        }
    }
}
