using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynMvp.Base;
using DynMvp.Devices.Comm;
using DynMvp.InspData;
using UniEye.Base;
using UniEye.Base.MachineInterface;
using UniScanM.EDMS.Settings;
using UniScanM.Operation;

namespace UniScanM.EDMS.MachineIF
{
    public class MachineIfDataExporter : DynMvp.Data.DataExporter
    {
        public override void Export(DynMvp.InspData.InspectionResult inspectionResult, CancellationToken cancellationToken)
        {
            if (SystemManager.Instance().DeviceBox.MachineIf == null)
                return;

            if (SystemManager.Instance().DeviceBox.MachineIf.IsConnected == false)
                return;
            if (OperationOption.Instance().OnTune)
                return;

            InspectionResult inspectionResult2 = (InspectionResult)inspectionResult;
            UniScanM.EDMS.Data.InspectionResult myInspectionResult = (UniScanM.EDMS.Data.InspectionResult)inspectionResult;

            double[] edgeArray = myInspectionResult.TotalEdgePositionResult;

            short filmEdge = (short)((edgeArray[(int)Data.DataType.FilmEdge]) * 1000);
            //filmEdge = Get3Values(filmEdge);

            short coating_Film = (short)((edgeArray[(int)Data.DataType.Coating_Film]) * 1000);
            //coating_Film = Get3Values(coating_Film);

            short printing_Coating = (short)((edgeArray[(int)Data.DataType.Printing_Coating]) * 1000);
            //printing_Coating = Get3Values(printing_Coating);

            short filmEdge_0 = (short)((edgeArray[(int)Data.DataType.FilmEdge_0]) * 1000);
            //filmEdge_0 = Get4Values(filmEdge_0);

            short printingEdge_0 = (short)((edgeArray[(int)Data.DataType.PrintingEdge_0]) * 1000);
            //printingEdge_0 = Get4Values(printingEdge_0);

            short printing_FilmEdge_0 = (short)((edgeArray[(int)Data.DataType.Printing_FilmEdge_0]));
            //printing_FilmEdge_0 = Get4Values(printing_FilmEdge_0);
            string resultString = "0000";
            if(EDMSSettings.Instance().UseLineStop == true)
            {
                resultString = inspectionResult.Judgment == Judgment.Reject ? "0001" : "0000"; // 워닝도 내보내면 안됨
            }

            string mergeString =
                string.Format("{0}{1}{2}{3}{4}{5}{6}",
                resultString
                , string.Format("{0:X04}", filmEdge)
                , string.Format("{0:X04}", coating_Film)
                , string.Format("{0:X04}", printing_Coating)
                , string.Format("{0:X04}", filmEdge_0)
                , string.Format("{0:X04}", printingEdge_0)
                , string.Format("{0:X04}", printing_FilmEdge_0));
            
            SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfEDMSCommand.SET_EDMS, mergeString);


            //원래 소스 
            //string mergeString =
            //    string.Format("{0}{1}{2}{3}{4}{5}{6}", 
            //    inspectionResult.IsGood() ? "0000" : "0001"
            //    , string.Format("{0:X04}", (short)(MicrometerToMilimeter(edgeArray[(int)Data.DataType.FilmEdge]) * 1000))
            //    , string.Format("{0:X04}", (short)(MicrometerToMilimeter(edgeArray[(int)Data.DataType.Coating_Film]) * 1000))
            //    , string.Format("{0:X04}", (short)(MicrometerToMilimeter(edgeArray[(int)Data.DataType.Printing_Coating]) * 1000))
            //    , string.Format("{0:X04}", (short)(MicrometerToMilimeter(edgeArray[(int)Data.DataType.FilmEdge_0]) * 1000))
            //    , string.Format("{0:X04}", (short)(MicrometerToMilimeter(edgeArray[(int)Data.DataType.PrintingEdge_0]) * 1000))
            //    , string.Format("{0:X04}", (short)(MicrometerToMilimeter(edgeArray[(int)Data.DataType.Printing_FilmEdge_0]) * 1000)));
            //원래소스 




            //machineIfProtocolResponce = SystemManager.Instance().DeviceBox.MachineIf.SendCommand(UniScanMMachineIfEDMSCommand.SET_EDMS_RUN, 500, );

            //if (machineIfProtocolResponce.IsResponced==false)
            //    LogHelper.Warn(LoggerType.Network, "MachineIfDataExporter::Export - no responce");
            //else if(machineIfProtocolResponce.IsGood==false)
            //    LogHelper.Warn(LoggerType.Network, "MachineIfDataExporter::Export - result is not good");

        }

        private double MicrometerToMilimeter(double microMeter)
        {
            double milimeter = (microMeter != 0.0) ? milimeter = microMeter / 1000 : 0.0;
            return milimeter;
        }

        short Get3Values(short data)
        {
            if (data >= 9999)
            {
                data = 999;
            }
            if (data <= -999)
            {
                data = -9999;
            }
            return data;
        }

        short Get4Values(short data)
        {
            if (data >= 9999)
            {
                data = 9999;
            }
            if (data <= -9999)
            {
                data = -9999;
            }
            return data;
        }
    }
}
