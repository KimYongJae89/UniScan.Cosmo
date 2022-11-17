using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

namespace WPF.Base.Extensions
{
    public static class ResourceExtensions
    {
        public static ResourceManager _resLoader = new ResourceManager("WPF.Base.Strings.Resources", Assembly.GetExecutingAssembly());

        public static string GetLocalized(this string resourceKey)
        {
            try
            {
                return _resLoader.GetString(resourceKey);
            }
            catch (Exception e)
            {
            }

            return resourceKey;
        }
    }
}
