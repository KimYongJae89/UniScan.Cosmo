using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Base.Helpers;
using WPF.SEMCNS.Offline.Models;

namespace WPF.SEMCNS.Offline.ViewModels
{
    public class ReportViewModel :Observable
    {
        DateTime _inspectTime;
        public DateTime InspectTime
        {
            get => _inspectTime;
            set
            {
                Set(ref _inspectTime, value);
            }
        }

        TargetParam _param;
        public TargetParam Param
        {
            get => _param;
            set
            {
                Set(ref _param, value);
            }
        }

        ImageViewModel _imageViewModel;
        DefectViewModel _defectViewModel;

        public void Initialize(ImageViewModel imageViewModel, DefectViewModel defectViewModel)
        {
            _imageViewModel = imageViewModel;
            _defectViewModel = defectViewModel;
        }

        public void Update(Result result)
        {
            _imageViewModel.GrabbedImage = result.ImageSource;
            _defectViewModel.ItemSource = result.Defects;
            Param = result.TargetParam;
            InspectTime = result.InspectTime;
        }
    }
}
