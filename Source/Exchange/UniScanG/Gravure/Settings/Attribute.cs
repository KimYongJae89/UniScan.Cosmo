using DynMvp.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniScanG.Gravure.Settings
{
    internal class LocalizedCategoryAttributeUniScanG : LocalizedCategoryAttribute
    {
        public LocalizedCategoryAttributeUniScanG(string value) : base("LocalizedCategoryAttributeUniScanG", value)
        {
        }
    }

    internal class LocalizedDisplayNameAttributeUniScanG : LocalizedDisplayNameAttribute
    {
        public LocalizedDisplayNameAttributeUniScanG(string value) : base("LocalizedDisplayNameAttributeUniScanG", value)
        {
        }
    }

    internal class LocalizedDescriptionAttributeUniScanG : LocalizedDescriptionAttribute
    {
        public LocalizedDescriptionAttributeUniScanG(string value) : base("LocalizedDescriptionAttributeUniScanG", value)
        {
        }
    }
}
