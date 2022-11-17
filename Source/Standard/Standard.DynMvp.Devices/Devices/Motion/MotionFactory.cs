//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using Standard.DynMvp.Base;

//namespace Standard.DynMvp.Devices.MotionController
//{
//    public class MotionFactory
//    {
//        public static Motion Create(MotionInfo motionInfo)
//        {
//            Motion motion = null;
//            if (isVirtual)
//            {
//                motion = new MotionVirtual(motionInfo.Name);
//            }
//            else
//            {
//                switch (motionInfo.Type)
//                {
//                    case MotionType.AlphaMotion302:
//                        motion = new MotionAlphaMotion302(motionInfo.Name);
//                        break;
//                    case MotionType.AlphaMotion304:
//                        motion = new MotionAlphaMotion304(motionInfo.Name);
//                        break;
//                    case MotionType.AlphaMotion314:
//                        motion = new MotionAlphaMotion314(motionInfo.Name);
//                        break;
//                    case MotionType.AlphaMotionBx:
//                        motion = new MotionAlphaMotionBx(motionInfo.Name);
//                        break;
//                    case MotionType.FastechEziMotionPlusR:
//                        motion = new MotionEziMotionPlusR(motionInfo.Name);
//                        break;
//                    case MotionType.PowerPmac:
//                        motion = new MotionPowerPmac(motionInfo.Name);
//                        break;
//                    case MotionType.Ajin:
//                        motion = new MotionAjin(motionInfo.Name);
//                        break;
//                    case MotionType.Virtual:
//                        motion = new MotionVirtual(motionInfo.Name);
//                        break;
//                }
//            }

//            if (motion == null)
//            {
//                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToCreate,
//                    ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToCreate.ToString(), String.Format("Can't create motion. {0}", motionInfo.Type.ToString()));
//                return null;
//            }

//            if (motion.Initialize(motionInfo) == false)
//            {
//                ErrorManager.Instance().Report((int)ErrorSection.Motion, (int)CommonError.FailToInitialize,
//                    ErrorLevel.Error, ErrorSection.Motion.ToString(), CommonError.FailToInitialize.ToString(), String.Format("Can't initialize motion. {0}", motionInfo.Type.ToString()));

//                motion = new MotionVirtual(motionInfo.Type, motionInfo.Name);
//                motion.Initialize(motionInfo);
//                motion.UpdateState(DeviceState.Error, "Motion is invalid.");
//            }
//            else
//            {
//                motion.UpdateState(DeviceState.Ready, "Motion initialization succeeded.");
//            }

//            //DeviceManager.Instance().AddDevice(motion);
//            return motion;
//        }
//    }
//}
