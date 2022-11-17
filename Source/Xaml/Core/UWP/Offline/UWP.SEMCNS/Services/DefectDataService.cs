using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.SEMCNS.Models;

namespace UWP.SEMCNS.Services
{
    public static class DefectDataService
    {
        static ObservableCollection<Defect> _defectList = new ObservableCollection<Defect>();
        public static ObservableCollection<Defect> DefectList { get => _defectList; }


        public static void Clear()
        {
            _defectList.Clear();
        }

        public static void Add(IEnumerable<Defect> defectList)
        {
            _defectList.Concat(defectList);
        }
    }
}
