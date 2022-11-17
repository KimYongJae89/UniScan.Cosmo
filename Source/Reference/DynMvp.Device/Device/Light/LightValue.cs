using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using DynMvp.Base;

namespace DynMvp.Devices.Light
{
    public class LightValue
    {
        private int[] value;
        public int[] Value
        {
            get { return value; }
        }

        public LightValue(int numLight, int defaultValue = 0)
        {
            value = new int[numLight];
            for (int i = 0; i < numLight; i++)
            {
                value[i] = defaultValue;
            }
        }

        public LightValue(params int[] values)
        {
            value = new int[values.Length];
            Array.Copy(values, value, values.Length);
        }

        public LightValue Clone()
        {
            LightValue lightValue = new LightValue(value.Count());
            lightValue.Copy(this);

            return lightValue;
        }

        public void Copy(LightValue lightValue)
        {
            value = new int[lightValue.NumLight];
            for (int i = 0; i < lightValue.NumLight; i++)
            {
                value[i] = lightValue.Value[i];
            }
        }

        public void TurnOn()
        {
            for (int i = 0; i < value.Count(); i++)
            {
                value[i] = 128;
            }
        }

        public void TurnOff()
        {
            for (int i = 0; i < value.Count(); i++)
            {
                value[i] = 0;
            }
        }

        public void Offset(int offset)
        {
            for (int i = 0; i < value.Count(); i++)
            {
                value[i] += offset;
            }
        }

        public int NumLight
        {
            get { return value.Count(); }
        }

        public string KeyValue
        {
            get
            {
                string keyValue = "";
                                            
                for(int i=0; i< value.Count(); i++)
                {
                    keyValue += value[i].ToString("x2");
                }

                return keyValue;
            }
        }
    }

    public class LightParamList
    {
        List<LightParam> lightParamList = new List<LightParam>();

        public IEnumerator<LightParam> GetEnumerator()
        {
            return lightParamList.GetEnumerator();
        }

        public void AddLightValue(LightParam lightParam)
        {
            if (IsContained(lightParam) == false)
                lightParamList.Add(lightParam);
        }

        public LightParam this[int index]
        {
            get { return lightParamList[index]; }
        }

        public bool IsContained(LightParam lightParam)
        {
            foreach (LightParam lp in lightParamList)
            {
                if (lp.KeyValue == lightParam.KeyValue)
                    return true;
            }

            return false;
        }
    }
}
