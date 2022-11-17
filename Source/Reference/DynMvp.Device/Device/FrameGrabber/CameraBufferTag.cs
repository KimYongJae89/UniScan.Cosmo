using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynMvp.Device.Device.FrameGrabber
{
    public class CameraBufferTag : ICloneable
    {
        public int BufferId
        {
            get { return bufferId; }
        }

        public UInt64 FrameId
        {
            get { return frameId; }
        }

        public UInt64 TimeStamp
        {
            get { return timeStamp; }
        }

        private int bufferId = -1;
        private UInt64 frameId = 0;
        private UInt64 timeStamp = 0;
        private DateTime dateTime = DateTime.MinValue;

        private CameraBufferTag()
        {
            this.bufferId = -1;
            this.frameId = 0;
            this.timeStamp = 0;
            this.dateTime = DateTime.MinValue;
        }

        public CameraBufferTag(int bufferId, UInt64 frameId)
        {
            this.bufferId = bufferId;
            this.frameId = frameId;
            this.timeStamp = 0;
            this.dateTime = DateTime.Now;
            //Debug.WriteLine(string.Format("CameraBufferTag : FrameId is {0}", frameId));
        }

        public override string ToString()
        {
            return string.Format("Buffer {0} / Frame {1} / DateTime {2}", this.bufferId, this.FrameId, this.dateTime);
        }

        public void UpdateFrameId(ulong frameId)
        {
            this.frameId = frameId;
        }

        public object Clone()
        {
            CameraBufferTag newCameraBufferTag = new CameraBufferTag();
            newCameraBufferTag.bufferId = this.bufferId;
            newCameraBufferTag.frameId = this.frameId;
            newCameraBufferTag.timeStamp = this.timeStamp;
            newCameraBufferTag.dateTime = this.dateTime;

            return newCameraBufferTag;
        }
    }
}
