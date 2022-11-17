using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniScanG.Gravure.Settings;
using UniScanG.Gravure.Vision;

namespace UniScanG.Data
{
    public class RepeatedDefectItemList : IEnumerable, IEnumerator
    {
        List<RepeatedDefectItem> repeatedDefectItemList = new List<RepeatedDefectItem>();

        public int Count
        {
            get { return this.repeatedDefectItemList.Count; }
        }

        public RepeatedDefectItem this[int i]
        {
           get { return repeatedDefectItemList[i]; }
        }

        public object Current { get { return this.repeatedDefectItemList; } }

        public void Clear()
        {
            repeatedDefectItemList.Clear();
        }

        public void AddResult(SheetResult sheetResult, bool autoRemove)
        {
            lock (repeatedDefectItemList)
            {
                // 기존 반복불량 영역들에 null을 추가한다.
                this.repeatedDefectItemList.ForEach(f => f.Add(autoRemove));

                foreach (SheetSubResult sheetSubResult in sheetResult.SheetSubResultList)
                {
                    bool contained = false;
                    foreach (RepeatedDefectItem repeatedDefectItem in this.repeatedDefectItemList)
                    {
                        // 마지막 검사에서 검출된 불량이 기존 영역에 속하면 해당 영역 마지막에 덮어쓴다.
                        contained = repeatedDefectItem.IsContained(sheetSubResult);
                        if (contained)
                        {
                            repeatedDefectItem.Update(sheetSubResult);
                            break;
                        }
                    }

                    // 마지막 검사에서 검출된 불량이 기존 영역에 속하지 않으면 신규 영역으로 추가한다.
                    if (contained == false)
                    {
                        repeatedDefectItemList.Add(new RepeatedDefectItem(sheetSubResult));
                    }
                }

                // 모두 null인 영역은 지운다.
                RemovePureData();
            }
        }

        public void RemovePureData()
        {
            lock (repeatedDefectItemList)
                repeatedDefectItemList.RemoveAll(f => f.IsPureData);

        }

        public List<RepeatedDefectItem> GetAlarmData()
        {
            return repeatedDefectItemList.FindAll(f => f.IsAlarmState);
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return this.repeatedDefectItemList.GetEnumerator();
        }

        internal void Sort()
        {
            repeatedDefectItemList.Sort((f, g) => g.ValidItemCount.CompareTo(f.ValidItemCount));
        }
    }

    public class RepeatedDefectItem
    {
        AdditionalSettings settings = (AdditionalSettings)AdditionalSettings.Instance();
        // 최근 N장 중 불량률 R% 이상 알람
        // 최근 M장 중 불량률 100% 알람

        public RectangleF BoundingRect
        {
            get { return boundingRect; }
            set { boundingRect = value; }
        }


        public List<SheetSubResult> SheetSubResultList
        {
            get { return sheetSubResultList; }
        }

        public Image SheetImage
        {
            get { return this.sheetImage; }
            set { this.sheetImage = value; }
        }

        public int ValidItemCount
        {
            get {
                lock (sheetSubResultList)
                    return sheetSubResultList.Count(f => f != null);
            }
        }

        public float RepeatRatio
        {
            get
            {
                return ValidItemCount * 100.0f / settings.RepeatedDefectAlarm.Count;
            }
        }

        public float ContinueRatio
        {

            get
            {
                lock (sheetSubResultList)
                {
                    int lastIdx = sheetSubResultList.FindLastIndex(f => f == null);
                    int count = Math.Min(sheetSubResultList.Count - lastIdx - 1, settings.ContinuousDefectAlarm.Count);
                    return count * 100.0f / settings.ContinuousDefectAlarm.Count;
                }
            }
        }

        // 알람: 한번이라도 알람 기준점을 넘긴 경우
        public bool IsAlarmState
        {
            get { return isAlarmState; }
        }

        // 클리어: 알람장 CLOSE하여 닫은 경우
        public bool IsAlarmCleared
        {
            get { return isAlarmCleared; }
            set { isAlarmCleared = value; }
        }

        // 통지: 신규 발생한 알람
        public bool IsNotified
        {
            get { return isNotified; }
            set { isNotified = value; }
        }

        public bool IsPureData
        {
            get
            {
                lock (sheetSubResultList)
                    return sheetSubResultList.Count(f => f != null) == 0;
            }
        }

        public bool isNotified;
        RectangleF boundingRect;
        List<SheetSubResult> sheetSubResultList;
        bool isAlarmState;
        bool isAlarmCleared;
        Image sheetImage;

        public RepeatedDefectItem(SheetSubResult sheetSubResult)
        {
            sheetSubResultList = new List<SheetSubResult>();

            Add(false);
            Update(sheetSubResult);
        }

        public bool IsContained(SheetSubResult sheetSubResult)
        {
            if (boundingRect.IsEmpty)
                return false;

            SheetSubResult lastAdded = sheetSubResultList.FirstOrDefault(f=>f!=null);
            if (lastAdded != null)
            {
                if (lastAdded.GetDefectType() != sheetSubResult.GetDefectType())
                    return false;
            }

            bool intersect = RectangleF.Inflate(boundingRect, 100, 100).IntersectsWith(sheetSubResult.Region);
            return intersect;
        }

        public void Add(bool autoRemove)
        {
            lock (sheetSubResultList)
            {
                sheetSubResultList.Insert(0, null);

                if (autoRemove)
                {
                    int listMaxCnt = Math.Max(settings.RepeatedDefectAlarm.Count, settings.ContinuousDefectAlarm.Count);
                    if (sheetSubResultList.Count > listMaxCnt)
                    {
                        int removeRange = sheetSubResultList.Count - listMaxCnt;
                        sheetSubResultList.RemoveRange(listMaxCnt, removeRange);
                    }
                }
            }
        }

        public void Update(SheetSubResult sheetSubResult)
        {
            lock (sheetSubResultList)
            {
                sheetSubResultList[0] = sheetSubResult;
                UpdateRect();

                bool isReapDefected = false;
                if (settings.ContinuousDefectAlarm.Use)
                {
                    // 최근 N시트 중 R% 이상
                    double thresholdR = settings.RepeatedDefectAlarm.Count * settings.RepeatedDefectAlarm.Ratio / 100.0;
                    int rangeR = Math.Min(settings.RepeatedDefectAlarm.Count, sheetSubResultList.Count);
                    int defectCountR = sheetSubResultList.GetRange(0, rangeR).Count(f => f != null);

                    isReapDefected = defectCountR > thresholdR;
                }

                bool isContDefected = false;
                if (settings.RepeatedDefectAlarm.Use)
                {
                    // 연속 N시트 중 R% 이상
                    double thresholdC = settings.ContinuousDefectAlarm.Count * settings.ContinuousDefectAlarm.Ratio / 100.0f;
                    int rangeC = Math.Min(settings.ContinuousDefectAlarm.Count, sheetSubResultList.Count);
                    int defectCountC = sheetSubResultList.GetRange(0, rangeC).Count(f => f != null);

                    isContDefected = defectCountC > thresholdC;
                }

                bool alarmNeed = (isReapDefected || isContDefected);
                if (alarmNeed)
                {
                    if (this.isAlarmState == false)
                        this.isNotified = false; // 신규 알람인 경우 통지해줘야 한다.

                    this.isAlarmState = true;
                }
                else
                {
                    //if (this.isAlarmState && this.isAlarmCleared)   // 이전에 알람이었는데, Clear 한 경우
                    //    this.isAlarmState = false;
                }
            }
        }

        private void UpdateRect()
        {
            List<SheetSubResult> validSheetSubResultList = sheetSubResultList.FindAll(f => f != null);
            int validSheetSubResultCount = validSheetSubResultList.Count;
            if (validSheetSubResultList.Count == 0)
            {
                boundingRect = RectangleF.Empty;
            }
            else
            {
                float l = 0, t = 0, r = 0, b = 0;
                validSheetSubResultList.ForEach(f =>
                {
                    l += f.Region.Left; t += f.Region.Top;
                    r += f.Region.Right; b += f.Region.Bottom;
                });

                boundingRect = RectangleF.FromLTRB(l / validSheetSubResultCount, t / validSheetSubResultCount, r / validSheetSubResultCount, b / validSheetSubResultCount);
                //boundingRect.Inflate(140, 140);
            }
        }
    }
}
