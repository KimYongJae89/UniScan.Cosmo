using DynMvp.Devices.MotionController;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using WPF.Base.Helpers;

namespace WPF.SEMCNS.Offline.Models
{
    public static class TargetAxis
    {
        private const string _keyStart = "StartAxis";
        private const string _keyEnd = "EndAxis";

        public static AxisPosition Start { get; set; } = new AxisPosition(new float[] { 0 });
        public static AxisPosition End { get; set; } = new AxisPosition(new float[] { 400000 });

        public static async Task LoadFromSettingsAsync()
        {

            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Model);
            Start = await directoryInfo.ReadAsync<AxisPosition>(_keyStart, null) ?? Start;
            Start = await directoryInfo.ReadAsync<AxisPosition>(_keyStart, null) ?? End;
        }

        public static async Task SaveSettingsAsync()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(PathSettings.Instance().Model);
            await directoryInfo.SaveAsync<AxisPosition>(_keyStart, Start);
            await directoryInfo.SaveAsync<AxisPosition>(_keyEnd, End);
        }
    }
}
