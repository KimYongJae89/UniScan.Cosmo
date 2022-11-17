﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynMvp.Devices
{
    public interface MilObject
    {
        void Free();
        void AddTrace();
        System.Diagnostics.StackTrace GetTrace();
    }

    public class MilObjectManager
    {
        object lockObj = null;

        public int MilObjCnt { get => milObjectList.Count; }
        List<MilObject> milObjectList;

        private static MilObjectManager instance = new MilObjectManager();
        public static MilObjectManager Instance
        {
            get { return instance; }
        }

        private MilObjectManager()
        {
            this.lockObj = new object();
            this.milObjectList = new List<MilObject>();
        }
      
        public void AddObject(MilObject milObject)
        {
            milObject.AddTrace();
            //lock (milObjectList)
            //    milObjectList?.Add(milObject);
        }

        public void ReleaseObject(MilObject milObject)
        {
            //lock (milObjectList)
            //    milObjectList.Remove(milObject);
            milObject?.Free();
        }

        public void RemoveAllObject()
        {
            System.Diagnostics.Debug.Assert(milObjectList.Count == 0);

            lock (milObjectList)
            {
                while (milObjectList.Count > 0)
                {
                    MilObject milObject = milObjectList.Last();
                    ReleaseObject(milObject);
                }
            }
            
            milObjectList.Clear();
        }
    }
}
