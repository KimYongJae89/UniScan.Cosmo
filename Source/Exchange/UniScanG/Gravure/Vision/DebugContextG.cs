using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Vision
{
    public class DebugContextG : DebugContext
    {
        public bool IsForceDebug { get => (regionId == 0 && lineSetId == 0); }

        public int RegionId { get => regionId; set => regionId = value; }
        int regionId = -1;

        public int LineSetId { get => lineSetId; set => lineSetId = value; }
        int lineSetId = -1;

        public int LineId { get => lineId; set => lineId = value; }
        int lineId = -1;

        public override string FullPath =>
            regionId < 0 ? base.path
            : lineSetId < 0 ? Path.Combine(base.path, regionId.ToString())
            : lineId < 0 ? Path.Combine(base.path, regionId.ToString(), lineSetId.ToString())
            : Path.Combine(base.path, regionId.ToString(), lineSetId.ToString(), lineId.ToString());

        public string LogName =>
            regionId < 0 ? ""
            : lineSetId < 0 ? string.Format("R{0}", regionId)
            : lineId < 0 ? string.Format("R{0} S{1}", regionId, lineSetId)
            : string.Format("R{0} S{1} L{2}", regionId, lineSetId, lineId);
        //public DebugContextG(bool saveDebugImage, string path, int regionId, int lineSetId, int lineId) : base(saveDebugImage, path)
        //{
        //    this.regionId = regionId;
        //    this.lineSetId = lineSetId;
        //    this.lineId = lineSetId;
        //}

        public DebugContextG(DebugContext debugContext, int regionId, int lineSetId, int lineId) : base(debugContext.SaveDebugImage, debugContext.FullPath)
        {
            this.regionId = regionId;
            this.lineSetId = lineSetId;
            this.lineId = lineId;
        }
    }
}
