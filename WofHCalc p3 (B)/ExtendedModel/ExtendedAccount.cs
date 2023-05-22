using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Models;

namespace WofHCalc.ExtendedModel
{
    internal class ExtendedAccount : Account, INotifyPropertyChanged
    {
        public ObservableCollection<ExtendedTown> ExtendedTowns { get; set; }
        public ObservableCollection<ExtendedTown> VariantsET1 { get; set; }
        public ObservableCollection<ExtendedTown> VariantsET2 { get; set; }
        public ObservableCollection<ExtendedTown>? TargetETowns { get; set; }
        public ExtendedAccount(Account acc)
        {
            //ExtendedTowns = acc.Towns;

        }
    }
}
