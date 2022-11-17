using System;
using System.Runtime.InteropServices;

using Windows.ApplicationModel.Resources;

namespace UWP.Base.Helpers
{
    public static class ResourceExtensions
    {
        private static ResourceLoader _resLoader = new ResourceLoader();
        private static ResourceLoader _extendLoader = new ResourceLoader("/UWP.Base/Resources");

        public static string GetLocalized(this string resourceKey)
        {
            string temp = _resLoader.GetString(resourceKey);

            if (String.IsNullOrEmpty(temp))
            {
                temp = _extendLoader?.GetString(resourceKey);

                if (String.IsNullOrEmpty(temp))
                    temp = resourceKey;
            }

            return temp;
        }
    }
}
