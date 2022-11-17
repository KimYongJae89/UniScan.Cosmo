using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Inspect;

namespace UniScanG.Gravure.Vision.Calculator
{
    public abstract class CalculatorBase: Algorithm
    {
        public enum Version { V1,V2};

        public abstract ProcessBufferSetG CreateProcessingBuffer(float scaleFactor, bool isMultiLayer, int width, int height);

        public static string TypeName { get { return "Calculator"; } }

    }
}
