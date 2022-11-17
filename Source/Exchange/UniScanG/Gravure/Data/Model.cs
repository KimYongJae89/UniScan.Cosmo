//using DynMvp.Base;
//using DynMvp.Data;
//using System;
//using System.Collections.Generic;
//using System.Xml;
//using UniScanG.Data.Vision;

//namespace UniScanG.Gravure.Data
//{
//    internal class Model : UniScanG.Data.Model.Model
//    {
//        //public int BasePositionX { get => basePositionX; set => basePositionX = value; }
//        //public List<RegionInfoG> RegionInfoList { get => regionInfoList; set => regionInfoList = value; }

//        //int basePositionX = 0;
//        //List<RegionInfoG> regionInfoList=new List<RegionInfoG>();

//        public override void SaveModel(XmlElement xmlElement)
//        {
//            base.SaveModel(xmlElement);

//            //XmlHelper.SetValue(xmlElement, "BasePositionX", basePositionX.ToString());
//            //foreach (RegionInfo regionInfo in regionInfoList)
//            //{
//            //    XmlElement subElement = xmlElement.OwnerDocument.CreateElement("RegionInfo");
//            //    xmlElement.AppendChild(subElement);

//            //    regionInfo.SaveParam(subElement);
//            //}
//        }

//        public override bool LoadModel(XmlElement xmlElement)
//        {
//            base.LoadModel(xmlElement);

//            //foreach(RegionInfo regionInfo in regionInfoList)
//            //    regionInfo.Dispose();
//            //regionInfoList.Clear();

//            //basePositionX = Convert.ToInt32(XmlHelper.GetValue(xmlElement, "BasePositionX", basePositionX.ToString()));

//            //XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName("RegionInfo");
//            //foreach (XmlElement subElement in  xmlNodeList)
//            //{
//            //    RegionInfoG regionInfoG = RegionInfoG.Load(subElement);
//            //    regionInfoList.Add(regionInfoG);
//            //}

//            return true;
//        }
//    }
//}
