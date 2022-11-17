﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynMvp.Base;
using System.Drawing;

//Rectangle targetRegion = new Rectangle();



//CadImporter cadImporter = CadImporterFactory.Create(CadType.STL);
//Cad3dModel model = cadImporter.Import("test.stl");

//CadConverter cadConverter = new CadConverter();
//Image3D image3d = cadConverter.Convert(model, targetRegion);

namespace DynMvp.Cad
{
    public abstract class CadImporter
    {
        public abstract Cad3dModel Import(string fileName);
    }
}
