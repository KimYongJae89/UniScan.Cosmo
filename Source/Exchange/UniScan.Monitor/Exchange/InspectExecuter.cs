using DynMvp.Base;
using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using DynMvp.InspData;
using DynMvp.Inspection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScan.Common;
using UniScan.Common.Exchange;

namespace UniScan.Monitor.Exchange
{
    public class InspectExcuter : MachineIfExecuter
    {
        ImageDevice imageDevice = new CameraVirtual();
        
        protected override bool Execute(string command)
        {
            string[] splitCommand = command.Split(',');

            ExchangeCommand inspectCommand;
            bool result = Enum.TryParse(splitCommand[0], out inspectCommand);

            if (result == false)
                return false;

            switch (inspectCommand)
            {
                case ExchangeCommand.I_DONE:
                    if (splitCommand.Count() != 5)
                        return false;
                    InspectionOption inspectionOption = new InspectionOption();
                    InspectionResult inspectionResult = new InspectionResult();
                    inspectionResult.AddExtraResult("Cam", splitCommand[1]);
                    inspectionResult.AddExtraResult("Client", splitCommand[2]);
                    inspectionResult.AddExtraResult("No", splitCommand[3]);
                    inspectionResult.AddExtraResult("Time", splitCommand[4]);
                    //LogHelper.Info(LoggerType.Inspection, string.Format("Cam : {0}, Client : {1}, Sheet : {2}", splitCommand[1], splitCommand[2], splitCommand[3]));
                    SystemManager.Instance().InspectRunner?.Inspect(null, IntPtr.Zero, inspectionResult);
                    result = true;
                    break;
                case ExchangeCommand.F_FOUNDED:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}
