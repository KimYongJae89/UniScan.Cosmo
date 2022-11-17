using DynMvp.Devices;
using DynMvp.Devices.FrameGrabber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.Settings;
using UniScan.Common;
using UniScan.Common.Exchange;
using UniScanG.Data.Model;

namespace UniScan.Monitor.Exchange
{
    class CommExecuter: UniEye.Base.MachineInterface.MachineIfExecuter
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
                case ExchangeCommand.C_CONNECTED:
                    if (SystemManager.Instance().CurrentModel != null)
                    {
                        string[] modelDiscArgs = ((ModelDescription)SystemManager.Instance().CurrentModel.ModelDescription).GetArgs();
                        int camId = int.Parse(splitCommand[1]);
                        int clientId = int.Parse(splitCommand[2]);

                        List<string> argList = new List<string>();
                        argList.Add(camId.ToString());
                        argList.Add(clientId.ToString());
                        argList.AddRange(modelDiscArgs);
                        SystemManager.Instance().ExchangeOperator.SendCommand(ExchangeCommand.M_SELECT, argList.ToArray());
                    }
                    result = true;
                    break;
                case ExchangeCommand.C_DISCONNECTED:
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
