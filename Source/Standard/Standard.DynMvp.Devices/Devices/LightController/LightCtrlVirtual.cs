//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;

//namespace Standard.DynMvp.Devices.Light
//{
//    public class LightCtrlVirtual : LightCtrl
//    {
//        int numChannel;

//        public LightCtrlVirtual(LightCtrlType lightCtrlType, string name, int numChannel) : base(lightCtrlType, name)
//        {
//            this.numChannel = numChannel;
//        }

//        public override int NumChannel
//        {
//            get { return numChannel; }
//        }

//        public override int GetMaxLightLevel()
//        {
//            return 255;
//        }

//        public override bool Initialize(LightCtrlInfo lightCtrlInfo)
//        {
//            return true;
//        }

//        //public override void TurnOff()
//        //{
            
//        //}

//        //public override void TurnOn()
//        //{
            
//        //}

//        public override void TurnOn(LightValue lightValue)
//        {
//            //LogHelper.Debug(LoggerType.Grab, String.Format("Set light value: {0}", lightValue.KeyValue));
//            Thread.Sleep(lightStableTimeMs);
//        }
//    }
//}
