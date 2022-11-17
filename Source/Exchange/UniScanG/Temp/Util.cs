//using DynMvp.Base;
//using DynMvp.UI;
//using UniScanG.Operation.Data;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Windows.Forms;
//using System.Xml;
//using UniEye.Base;

//namespace UniScanG.Temp
//{
//    enum DefectFilterType
//    {
//        Total, Black, White, PinHole, Shape
//    }

//    public class UniScanGUtil
//    {
//        static UniScanGUtil _instance;

//        private Color disConnected = Color.FromArgb(255, 117, 90);
//        public Color DisConnected
//        {
//            get { return disConnected; }
//        }

//        private Color wait = Color.FromArgb(140, 203, 255);
//        public Color Wait
//        {
//            get { return wait; }
//        }

//        private Color connected = Color.FromArgb(191, 191, 191);
//        public Color Connected
//        {
//            get { return connected; }
//        }

//        private Color inspect = Color.FromArgb(182, 255, 150);
//        public Color Inspect
//        {
//            get { return inspect; }
//        }

//        private Color pause = Color.FromArgb(252, 255, 145);
//        public Color Pause
//        {
//            get { return pause; }
//        }

//        private Color run = Color.LightGreen;
//        public Color Run
//        {
//            get { return run; }
//        }

//        private Color good = Color.Yellow;
//        public Color Good
//        {
//            get { return good; }
//        }

//        private Color ng = Color.Red;
//        public Color NG
//        {
//            get { return ng; }
//        }
        
//        public static UniScanGUtil Instance()
//        {
//            if (_instance == null)
//            {
//                _instance = new UniScanGUtil();
//            }

//            return _instance;
//        }
        
//        public Figure GetDefectFigure(SheetDefectType defectType, Rectangle rectangle)
//        {
//            Figure figure = null;
//            switch (defectType)
//            {
//                case SheetDefectType.BlackDefect:
//                    figure = new RectangleFigure(rectangle, new Pen(Color.Red, 3));
//                    break;
//                case SheetDefectType.WhiteDefect:
//                    figure = new RectangleFigure(rectangle, new Pen(Color.Yellow, 3));
//                    break;
//            }
//            return figure;
//        }

//        public string GetDefectString(SheetDefectType defectType)
//        {
//            string defectString = null;
//            switch (defectType)
//            {
//                case SheetDefectType.BlackDefect:
//                    defectString = "Dark";
//                    break;
//                case SheetDefectType.WhiteDefect:
//                    defectString = "Bright";
//                    break;
//            }
//            return defectString;
//        }
//    }
//}
