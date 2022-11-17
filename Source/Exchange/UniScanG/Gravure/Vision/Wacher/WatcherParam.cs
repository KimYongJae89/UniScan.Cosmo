using DynMvp.Base;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UniScanG.Data.Vision;
using UniScanG.Gravure.Data;

namespace UniScanG.Gravure.Vision.Watcher
{
    public class WatcherParam : AlgorithmParam
    {
        List<WatchItem> watchItemList = new List<WatchItem>();
        public List<WatchItem> WatchItemList { get => watchItemList;  }

        public WatcherParam()
        {
        }

        #region override
        public override AlgorithmParam Clone()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            //base.Dispose();
        }
        #endregion

        public override void SaveParam(XmlElement algorithmElement)
        {
            base.SaveParam(algorithmElement);

            foreach(WatchItem watchItem in this.watchItemList)
            {
                XmlElement element = algorithmElement.OwnerDocument.CreateElement("WatchItem");
                algorithmElement.AppendChild(element);
                watchItem.Export(element);
            }
        }

        public override void LoadParam(XmlElement algorithmElement)
        {
            base.LoadParam(algorithmElement);

            XmlNodeList xmlNodeList= algorithmElement.GetElementsByTagName("WatchItem");
            foreach(XmlElement element in xmlNodeList)
            {
                WatchItem watchItem = new WatchItem();
                bool ok = watchItem.Import(element);
                if(ok)
                    this.watchItemList.Add(watchItem);

            }
        }
    }
}
