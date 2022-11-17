using Standard.DynMvp.Base.Helpers;
using Standard.DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UWP.Base.Helpers;
using Windows.Storage;

namespace UWP.Base.Settings
{
    public class OperationSettings : Setting<OperationSettings>
    {
        [SettingData(SettingDataType.Numeric)]
        public int ResultStoringDays { get; set; } = 30;

        [SettingData(SettingDataType.Enum)]
        public ImagingLibrary ImagingLibrary { get; set; } = Standard.DynMvp.Vision.ImagingLibrary.OpenCv;

        public OperationSettings() : base("Operation")
        {

        }
    }
}
