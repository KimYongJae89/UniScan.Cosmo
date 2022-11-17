using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniEye.Base.MachineInterface;
using UniScan.Common.Exchange;

namespace UniScan.Monitor.Exchange
{
    public class JobExecuter : MachineIfExecuter
    {
        protected override bool Execute(string command)
        {
            string[] splitCommand = command.Split(',');

            ExchangeCommand jobCommand;
            bool result = Enum.TryParse(splitCommand[0], out jobCommand);

            if (result == false)
                return false;

            switch (jobCommand)
            {
                case ExchangeCommand.J_DONE:
                    result = true;
                    break;
                case ExchangeCommand.J_ERROR:
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
