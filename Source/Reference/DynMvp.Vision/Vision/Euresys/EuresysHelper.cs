﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Euresys.Open_eVision_1_2;

namespace DynMvp.Vision.Euresys
{
    public class EuresysHelper
    {
        public static bool LicenseExist()
        {
            try
            {
                Easy.CheckLicenses();
                return true;
            }
            catch (EException)
            {
            }
            catch(TypeInitializationException)
            {

            }
            return false;
        }
    }
}
