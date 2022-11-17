﻿using Standard.DynMvp.Devices;
using Matrox.MatroxImagingLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Standard.DynMvp.Vision.Matrox.Object
{
    public class MilImResult : IDisposable, MilObject
    {
        private MIL_ID id = MIL.M_NULL;
        public MIL_ID Id
        {
            get { return id; }
        }

        StackTrace stackTrace = null;

        public MilImResult(MIL_INT entries, long resultType)
        {
            this.id = MIL.MimAllocResult(MIL.M_DEFAULT_HOST, entries, resultType, MIL.M_NULL);
            MilObjectManager.Instance.AddObject(this);
        }

        ~MilImResult()
        {
            Dispose();
        }

        public void Dispose()
        {
            MilObjectManager.Instance.ReleaseObject(this);
        }

        public void Free()
        {
            if (id != MIL.M_NULL)
            {
                MIL.MbufFree(id);
                id = MIL.M_NULL;
            }
        }

        public void AddTrace()
        {
#if DEBUG
            this.stackTrace = new StackTrace();
#endif
        }

        public StackTrace GetTrace()
        {
            return stackTrace;
        }
    }
}
