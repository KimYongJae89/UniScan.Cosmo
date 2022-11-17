using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF.Base.Helpers;
using WPF.COSMO.Offline.Services;

namespace WPF.COSMO.Offline.Models
{
    public enum DefectViewMode
    {
        Total, Left, Right, Edge, Inner
    }

    public class DefectStorage : Observable
    {
        Section _section;
        public Section Section
        {
            get => _section ?? (_section = SectionService.Settings.Selected);
            set
            {
                Set(ref _section, value);
                if (FilterChanged != null)
                    FilterChanged();
            }
        }

        bool _sizeFilterEnable;
        public bool SizeFilterEnable
        {
            get => _sizeFilterEnable;
            set
            {
                Set(ref _sizeFilterEnable, value);
                if (FilterChanged != null)
                    FilterChanged();
            }
        }

        bool _sectionFilterEnable;
        public bool SectionFilterEnable
        {
            get => _sectionFilterEnable;
            set
            {
                Set(ref _sectionFilterEnable, value);
                if (FilterChanged != null)
                    FilterChanged();
            }
        }

        double _sizeFilter = -1;
        public double SizeFilter
        {
            get => _sizeFilter;
            set
            {
                Set(ref _sizeFilter, value);
                SizeFilterEnable = true;
            }
        }

        double _sectionFilter = -1;
        public double SectionFilter
        {
            get => _sectionFilter;
            set
            {
                Set(ref _sectionFilter, value);
                SectionFilterEnable = true;
            }
        }

        public EmptyDelegate FilterChanged;

        CosmoLotNoInfo _lotNoinfo;
        public CosmoLotNoInfo LotNoInfo
        {
            get => _lotNoinfo;
            set => Set(ref _lotNoinfo, value);
        }

        public EmptyDelegate DefectViewModeChanged;

        Defect _selected;
        public Defect Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }

        public List<Defect> Defects { get; } = new List<Defect>();

        DefectViewMode _defectViewMode;
        public DefectViewMode DefectViewMode
        {
            get => _defectViewMode;
            set
            {
                Set(ref _defectViewMode, value);

                if (DefectViewModeChanged != null)
                    DefectViewModeChanged();
            }
        }

        public DefectStorage()
        {
            AxisGrabService.Initialized += Initialized;
            ResultService.ReportInfoLoaded += Loaded;

            SectionService.SelectedChanged += SelectedChanged;
        }

        private void SelectedChanged()
        {
            Section = SectionService.Settings.Selected;

            if (SectionService.SectionChanged != null)
                SectionService.SectionChanged();
        }

        void Initialized()
        {
            Section = SectionService.Settings.Selected;
        }

        void Loaded(ReportInfo reportInfo)
        {
            Section = reportInfo.Section;
        }

        public IEnumerable<Defect> GetFilteredDefect()
        {
            switch (_defectViewMode)
            {
                case DefectViewMode.Left:
                    return Defects.Where(defect => defect is IDirectionDefect)
                                .Where(defect => (defect as IDirectionDefect).ScanDirection == ScanDirection.LeftToRight);
                case DefectViewMode.Right:
                    return Defects.Where(defect => defect is IDirectionDefect)
                                .Where(defect => (defect as IDirectionDefect).ScanDirection == ScanDirection.RightToLeft);
                case DefectViewMode.Edge:
                    return Defects.Where(defect => defect is IDirectionDefect);
                case DefectViewMode.Inner:
                    return Defects.Where(defect => defect is InnerDefect);
            }

            return Defects;
        }

        public IEnumerable<Defect> GetFilteredDefect(DefectViewMode defectViewMode)
        {
            switch (defectViewMode)
            {
                case DefectViewMode.Left:
                    return Defects.Where(defect => defect is IDirectionDefect)
                                .Where(defect => (defect as IDirectionDefect).ScanDirection == ScanDirection.LeftToRight);
                case DefectViewMode.Right:
                    return Defects.Where(defect => defect is IDirectionDefect)
                                .Where(defect => (defect as IDirectionDefect).ScanDirection == ScanDirection.RightToLeft);
                case DefectViewMode.Edge:
                    return Defects.Where(defect => defect is IDirectionDefect);
                case DefectViewMode.Inner:
                    return Defects.Where(defect => defect is InnerDefect);
            }

            return Defects;
        }
    }
}
