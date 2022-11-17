using DynMvp.Devices;
using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScan.Common.Settings;

namespace UniScanG.Inspect
{
    public delegate void ProcessBufferReturnedDelegate();

    public class ProcessBufferManager : IDisposable
    {
        ImageDevice imageDevice;
        public ImageDevice ImageDevice
        {
            get { return imageDevice; }
            set { imageDevice = value; }
        }
        
        List<ProcessBufferSet> imageBufferSetList = new List<ProcessBufferSet>();

        public int Count { get { return imageBufferSetList.Count; } }
        public int UsingCount { get { return imageBufferSetList.FindAll(f=>f.IsUsing).Count; } }

        public ProcessBufferReturnedDelegate ProcessBufferReturnedDelegate;

        public void AddProcessBufferSet(ProcessBufferSet imageBufferSet, int count)
        {
            for (int i = 0; i < count; i++)
                imageBufferSetList.Add(imageBufferSet);
        }
        
        public void Dispose()
        {
            foreach (ProcessBufferSet imageBufferSet in imageBufferSetList)
                imageBufferSet.Dispose();

            imageBufferSetList.Clear();
        }

        public ProcessBufferSet Request(ImageDevice imageDevice)
        {
            ProcessBufferSet imageBufferSet = null;

            lock (imageBufferSetList)
            {
                imageBufferSet = imageBufferSetList.Find(f => f.IsUsing ==  false);

                if (imageBufferSet != null)
                {
                    imageBufferSet.Clear();
                    imageBufferSet.IsUsing = true;
                }
            }

            return imageBufferSet;
        }

        public void Return(ProcessBufferSet imageBufferSet)
        {
            lock (imageBufferSetList)
            {
                if (imageBufferSet == null)
                    imageBufferSetList.ForEach(b => b.IsUsing = false);
                else
                    imageBufferSet.IsUsing = false;
            }
        }

    }


    public abstract class ProcessBufferSet : IDisposable
    {
        private bool isUsing;
        public bool IsUsing
        {
            get { return isUsing; }
            set { isUsing = value; }
        }

        protected string algorithmTypeName = "";
        protected int width = 0;
        protected int height = 0;
        public virtual bool IsDone { get => true; }

        protected List<AlgoImage> bufferList = new List<AlgoImage>();
        
        public ProcessBufferSet()
        {

        }

        public ProcessBufferSet(string algorithmTypeName, int width, int height)
        {
            this.algorithmTypeName = algorithmTypeName;
            this.width = width;
            this.height = height;

            bufferList = new List<AlgoImage>();
            //BuildBuffers(algorithmTypeName, width, height);
        }

        public virtual void Dispose()
        {
            foreach (AlgoImage buffer in bufferList)
                buffer.Dispose();
            bufferList.Clear();
        }

        public void Clear()
        {
            foreach (AlgoImage buffer in bufferList)
                buffer.Clear();
        }

        public virtual void WaitDone()
        {

        }

        public abstract void BuildBuffers();
    }
}
