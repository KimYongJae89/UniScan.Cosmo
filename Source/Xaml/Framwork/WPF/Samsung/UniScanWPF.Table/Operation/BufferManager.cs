using DynMvp.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UniScanWPF.Table.Operation.Operators;
using UniScanWPF.Table.Settings;

namespace UniScanWPF.Table.Operation
{
    public class Buffer : IDisposable
    {
        const int bufferHeight = 55000;

        AlgoImage image;
        int needLine;

        public AlgoImage Image { get => image; }
        public bool IsFull { get => needLine == 0; }
        public bool IsEmpty { get => needLine == DeveloperSettings.Instance.BufferHeight; }

        public Buffer()
        {
            image = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(DeveloperSettings.Instance.BufferWidth, DeveloperSettings.Instance.BufferHeight));
        }

        public void Clear()
        {
            needLine = DeveloperSettings.Instance.BufferHeight;
            image.Clear();
        }

        public void AddImage(AlgoImage grabbedImage, ScanDirection scanDirection)
        {
            ImageProcessing imageProcessing = AlgorithmBuilder.GetImageProcessing(grabbedImage);

            System.Drawing.Point srcPt= System.Drawing.Point.Empty;
            System.Drawing.Point dstPt = System.Drawing.Point.Empty;
            System.Drawing.Size size = System.Drawing.Size.Empty;
            switch (scanDirection)
            {
                case ScanDirection.Forward:
                    srcPt = new System.Drawing.Point();
                    dstPt = new System.Drawing.Point(0, DeveloperSettings.Instance.BufferHeight - needLine);
                    size = new System.Drawing.Size(Math.Min(grabbedImage.Width, DeveloperSettings.Instance.BufferWidth), Math.Min(grabbedImage.Height, needLine));
                    break;
                case ScanDirection.Backward:
                    imageProcessing.Flip(grabbedImage, grabbedImage, DynMvp.Vision.Direction.Vertical);
                    srcPt = new System.Drawing.Point(0, Math.Max(0, grabbedImage.Height - needLine));
                    dstPt = new System.Drawing.Point(0, Math.Max(0, needLine - grabbedImage.Height));
                    size = new System.Drawing.Size(Math.Min(grabbedImage.Width, DeveloperSettings.Instance.BufferWidth), Math.Min(grabbedImage.Height, needLine));
                    break;
            }
            image.Copy(grabbedImage, srcPt, dstPt, size);
            //grabbedImage.Save(@"C:\temp\grabbedImage.bmp");

            needLine = Math.Max(0, needLine - size.Height);
        }

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    image.Dispose();
                }

                image = null;

                disposedValue = true;
            }
        }

        ~Buffer()
        {
            Dispose(false);
        }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            Dispose(true);
            // TODO: 위의 종료자가 재정의된 경우 다음 코드 줄의 주석 처리를 제거합니다.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    class ScanBuffer : IDisposable
    {
        Buffer backLightBuffer;
        Buffer topLightBuffer;

        public Buffer BackLightBuffer { get => backLightBuffer; }
        public Buffer TopLightBuffer { get => topLightBuffer; }

        public bool IsFull
        {
            get { return backLightBuffer.IsFull && topLightBuffer.IsFull; }
        }

        public ScanBuffer()
        {
            backLightBuffer = new Buffer();
            topLightBuffer = new Buffer();
        }

        public void Clear()
        {
            backLightBuffer.Clear();
            topLightBuffer.Clear();
        }

        public void AddImage(AlgoImage grabbedImage, ScanDirection scanDirection)
        {
            switch (scanDirection)
            {
                case ScanDirection.Forward:
                    backLightBuffer.AddImage(grabbedImage, scanDirection);
                    break;
                case ScanDirection.Backward:
                    topLightBuffer.AddImage(grabbedImage, scanDirection);
                    break;
            }
        }

        public void Dispose()
        {
            backLightBuffer.Dispose();
            topLightBuffer.Dispose();
        }
    }

    public class InspectBuffer : IDisposable
    {
        bool isUsing;
        AlgoImage algoImage;

        public bool IsUsing
        {
            get => isUsing;

            set { lock (this) isUsing = value; }
        }

        public AlgoImage AlgoImage { get => algoImage; }
            
        public InspectBuffer()
        {
            isUsing = false;
            algoImage = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(DeveloperSettings.Instance.BufferWidth, DeveloperSettings.Instance.BufferHeight));
        }
        
        public void Dispose()
        {
            algoImage.Dispose();
        }

        public void Clear()
        {
            isUsing = false;
        }
    }

    class BufferManager : IDisposable
    {
        const int bufferNum = 2;
        const int MaxBufferNum = 15;

        CancellationTokenSource cancellationTokenSource;
        
        ScanBuffer[] scanBufferArray;

        AlgoImage[] sheetBufferArray;
        AlgoImage[] maskBufferArray;

        List<InspectBuffer> inspectBufferList;

        List<IDisposable> bufferList;
        List<IDisposable> disposableList;

        ManualResetEvent manualResetEvent;

        static BufferManager instance;
        public static BufferManager Instance()
        {
            if (instance == null)
            {
                instance = new BufferManager();
            }

            return instance;
        }

        private BufferManager()
        {
            bufferList = new List<IDisposable>();

            disposableList = new List<IDisposable>();
            manualResetEvent = new ManualResetEvent(false);

            scanBufferArray = new ScanBuffer[DeveloperSettings.Instance.ScanNum];
            sheetBufferArray = new AlgoImage[DeveloperSettings.Instance.ScanNum];
            maskBufferArray = new AlgoImage[DeveloperSettings.Instance.ScanNum];

            inspectBufferList = new List<InspectBuffer>();

            cancellationTokenSource = new CancellationTokenSource();
            Thread disposeThread = new Thread(new ThreadStart(DisposeProc));
            disposeThread.IsBackground = true;
            disposeThread.Priority = ThreadPriority.Lowest;
            disposeThread.Start();
        }

        public void Init()
        {
            //for (int i = 0; i < DeveloperSettings.Instance.ScanNum; i++)
            Parallel.For(0, DeveloperSettings.Instance.ScanNum, new Action<int>(i =>
             {
                 scanBufferArray[i] = new ScanBuffer();

                 sheetBufferArray[i] = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(DeveloperSettings.Instance.BufferWidth, DeveloperSettings.Instance.BufferHeight));
                 maskBufferArray[i] = ImageBuilder.Build(ImagingLibrary.MatroxMIL, ImageType.Grey, new System.Drawing.Size(DeveloperSettings.Instance.BufferWidth, DeveloperSettings.Instance.BufferHeight));
             }
            ));

            bufferList.AddRange(scanBufferArray);
            bufferList.AddRange(sheetBufferArray);
            bufferList.AddRange(maskBufferArray);

            for (int i = 0; i < bufferNum; i++)
                inspectBufferList.Add(new InspectBuffer());

            bufferList.AddRange(inspectBufferList);
        }

        public void Dispose()
        {
            bufferList.ForEach(buffer => buffer.Dispose());
            disposableList.ForEach(obj => obj.Dispose());
        }

        public void AddDispoableObj(IDisposable disposableObj)
        {
            if (disposableObj == null)
                return;

            lock (this.disposableList)
                this.disposableList.Add(disposableObj);

            manualResetEvent.Set();
        }

        public void DisposeProc()
        {
            while(cancellationTokenSource.IsCancellationRequested == false)
            {
                lock (disposableList)
                {
                    if (disposableList.Count == 0)
                        manualResetEvent.Reset();
                }

                manualResetEvent.WaitOne();

                IDisposable disposableObj = disposableList[0];
                lock (disposableList)
                    disposableList.RemoveAt(0);

                disposableObj.Dispose();
            }
        }

        public void Clear()
        {
            inspectBufferList.ForEach(buffer => buffer.Clear());

            foreach (ScanBuffer scanBuffer in scanBufferArray)
                scanBuffer.Clear();
        }

        public ScanBuffer GetScanBuffer(int flowPosition)
        {
            return scanBufferArray[flowPosition];
        }

        public AlgoImage GetSheetBuffer(int flowPosition)
        {
            return sheetBufferArray[flowPosition];
        }

        public AlgoImage GetMaskBuffer(int flowPosition)
        {
            return maskBufferArray[flowPosition];
        }

        public AlgoImage GetInspectBuffer()
        {
            InspectBuffer inspectBuffer = null;

            while(true)
            {
                lock (inspectBufferList)
                {
                    inspectBuffer = inspectBufferList.Find(buffer => buffer.IsUsing == false);
                    if (inspectBuffer != null)
                    {
                        inspectBuffer.IsUsing = true;
                    }
                    else if(inspectBufferList.Count< MaxBufferNum)
                    {
                        InspectBuffer newInspectBuffer = new InspectBuffer();
                        inspectBufferList.Add(newInspectBuffer);
                        bufferList.Add(newInspectBuffer);
                    }
                }

                if (inspectBuffer != null)
                {
                    inspectBuffer.AlgoImage.Clear();
                    break;
                }

                Thread.Sleep(10);
            }            

            return inspectBuffer.AlgoImage;
        }

        public void PutInspectBuffer(AlgoImage algoImage)
        {
            lock (inspectBufferList)
            {
                InspectBuffer inspectBuffer = inspectBufferList.Find(buffer => buffer.AlgoImage == algoImage);
                if (inspectBuffer != null)
                    inspectBuffer.IsUsing = false;
            }
        }
    }
}
