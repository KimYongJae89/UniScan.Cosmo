using DynMvp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Base.Helpers;

namespace WPF.SEMCNS.Offline.Models
{
    public class TargetParam :Observable
    {
        uint _lower = 30;
        uint _upper = 30;
        uint _lightValue = 255;

        uint _startY = 0;
        uint _endY = 0;

        public uint StartY
        {
            get => _startY * 26 / 1000;
            set
            {
                Set(ref _startY, value * 1000 / 26);
                OnPropertyChanged("StartYPixel");
            }
        }

        public uint EndY
        {
            get => _endY * 26 / 1000;
            set
            {
                Set(ref _endY, value * 1000 / 26);
                OnPropertyChanged("EndYPixel");
                OnPropertyChanged("EndHeightPixel");
            }
        }

        public uint EndHeightPixel { get => 15000 - _endY; }

        public uint StartYPixel { get => _startY; }
        public uint EndYPixel { get => _endY; }

        public uint Lower
        {
            get => _lower;
            set
            {
                if (_lower > 255)
                    return;

                Set(ref _lower ,value);
            }
        }

        public uint Upper
        {
            get => _upper;
            set
            {
                if (_upper > 255)
                    return;

                Set(ref _upper, value);
            }
        }

        uint _lowerMinLength = 50;
        public uint LowerMinLength
        {
            get => _lowerMinLength;
            set { Set(ref _lowerMinLength, value); }
        }

        uint _upperMinLength = 50;
        public uint UpperMinLength
        {
            get => _upperMinLength;
            set { Set(ref _upperMinLength, value); }
        }

        public uint LightValue
        {
            get => _lightValue;
            set
            {
                if (_lightValue > 255)
                    return;

                Set(ref _lightValue, value);
            }
        }
    }

    public class Target : Model
    {
        TargetParam _targetParam = new TargetParam();
        public TargetParam TargetParam { get => _targetParam; }
    }
}
