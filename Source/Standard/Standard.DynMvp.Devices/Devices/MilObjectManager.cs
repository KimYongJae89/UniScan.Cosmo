using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Standard.DynMvp.Devices
{
    public interface MilObject
    {
        void Free();
        void AddTrace();
        System.Diagnostics.StackTrace GetTrace();
    }

    public class MilObjectManager
    {
        private static MilObjectManager instance = new MilObjectManager();
        public static MilObjectManager Instance
        {
            get { return instance; }
        }

        private MilObjectManager()
        {

        }

        List<MilObject> milObjectList = new List<MilObject>();
        public int MilObjCnt { get => milObjectList.Count; }

        public void AddObject(MilObject milObject)
        {
            milObject.AddTrace();
            lock(milObjectList)
                milObjectList.Add(milObject);
            //System.Diagnostics.Debug.WriteLine(string.Format("MilObjectManager::AddObject - {0}", milObjectList.Count));
        }

        public void ReleaseObject(MilObject milObject)
        {
            lock(milObjectList)
                milObjectList.Remove(milObject);
            //System.Diagnostics.Debug.WriteLine(string.Format("MilObjectManager::ReleaseObject - {0}", milObjectList.Count));

            milObject?.Free();
        }

        public void RemoveAllObject()
        {
            System.Diagnostics.Debug.Assert(milObjectList.Count == 0);

            while (milObjectList.Count > 0)
            {
                MilObject milObject = milObjectList.Last();
                ReleaseObject(milObject);
            }
            
            milObjectList.Clear();
        }
    }
}
