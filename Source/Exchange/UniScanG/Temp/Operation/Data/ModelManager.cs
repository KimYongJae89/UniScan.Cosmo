using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using DynMvp.Base;
using DynMvp.Data;
using DynMvp.UI;
using UniEye.Base;

namespace UniScanG.Temp
{
    //public class ModelManager : UniEye.Base.Data.ModelManager
    //{
    //    public override DynMvp.Data.Model CreateModel()
    //    {
    //        Model model = new Model();
    //        return model;
    //    }

    //    public override DynMvp.Data.Model LoadModel(DynMvp.Data.ModelDescription modelDesc, IReportProgress reportProgress)
    //    {
    //        //Model model = (Model)base.LoadModel(modelDesc, reportProgress);

    //        DynMvp.Data.Model model = CreateModel();
    //        model.ModelDescription = modelDesc as DynMvp.Data.ModelDescription;
    //        LoadModel(model, reportProgress);

    //        //if (this.IsCompatible(model) == false)
    //        //    return null;

    //        model.LoadModelSchema();
    //        //model.CreateProductionList();
            
    //        return model;
    //    }

    //    public override bool SaveModel(DynMvp.Data.Model model)
    //    {
    //        bool ok = base.SaveModel(model);
    //        if(ok)
    //        {
    //            Model samsungModel = (Model)model;
    //            ok = this.SaveModelDescription(model.ModelDescription);
    //        }

    //        if (ok)
    //            model.Modified = false;

    //        return ok;
    //    }

    //    public override DynMvp.Data.ModelDescription CreateModelDescription()
    //    {
    //        return new ModelDescription();
    //    }

    //    public override bool IsCompatible(DynMvp.Data.Model model)
    //    {
    //        if(model is Model)
    //        {
    //            if (model.InspectionStepList.Count == 2)
    //                return true;
    //        }
    //        return false;
    //    }
    //}
}
