using DynMvp.Devices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using WPF.Base.Converters;
using WPF.Base.Extensions;
using WPF.Base.Models;

namespace WPF.COSMO.Offline.Models
{
    public class Param_COSMO
    {
        public int RightEdgeBinarizeValue { get; set; } = 0;
        public int LeftEdgeBinarizeValue { get; set; } = 0;

        public byte InnerBinarizeValue { get; set; } = 32;

        public int EdgeOuterMinLengthUM { get; set; } = 42;
        public int EdgeInnerMinLengthUM { get; set; } = 42;
        public int InnerMinLengthUM { get; set; } = 64;
    }

    public class Model_COSMO : Model
    {
        static string paramKey = "Parameter";

        [JsonIgnore]
        public static Param_COSMO Param { get; set; }

        [JsonIgnore]
        public static byte CalibrationValue { get; set; } = 32;
        
        public Dictionary<string, float[]> CalibrationDataMap { get; set; } = new Dictionary<string, float[]>();
        
        public uint Width { get; set; } = 10;
        public uint Thickness { get; set; } = 25;

        public Model_COSMO() : base()
        {

        }
        
        public double GetWeightValue(AxisGrabInfo info)
        {
            return 1.0 / CalibrationDataMap[info.Name].Average();
        }

        public float[] GetCalibrationData(AxisGrabInfo info)
        {
            if (CalibrationDataMap.ContainsKey(info.Name))
                return CalibrationDataMap[info.Name];

            return null;
        }

        public void SetCalibrationData(AxisGrabInfo info, float[] calData)
        {
            if (CalibrationDataMap.ContainsKey(info.Name))
            {
                CalibrationDataMap[info.Name] = calData;
            }
            else
            {
                CalibrationDataMap.Add(info.Name, calData);
            }
        }

        public static async Task SaveParam()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);
            await directoryInfo.SaveAsync(paramKey, Param);
        }

        public static async Task LoadParam()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Config);

            Param = await directoryInfo.ReadAsync<Param_COSMO>(paramKey) ?? new Param_COSMO();
        }
    }
}
