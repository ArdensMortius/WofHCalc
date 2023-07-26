using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.DataSourses;
using WofHCalc.MathFuncs;
using WofHCalc.Models;

namespace WofHCalc.ExtendedModel
{
    internal class ExtendedAccount : Account, INotifyPropertyChanged
    {
        public ObservableCollection<ExtendedTown> ExtendedTowns { get; set; }
        public ObservableCollection<ExtendedTown> VariantsET1 { get; set; }
        public ObservableCollection<ExtendedTown> VariantsET2 { get; set; }
        //public ObservableCollection<ExtendedTown>? TargetETowns { get; set; }
        private DataWorldConst data;
        public WofHMath WofHFuncs;
        public ExtendedAccount(Account acc) : base()
        {
            data = DataWorldConst.GetInstance(acc.World);
            ExtendedTowns = new ObservableCollection<ExtendedTown>();
            VariantsET1 = new ObservableCollection<ExtendedTown>();
            VariantsET2 = new ObservableCollection<ExtendedTown>();
            foreach (Town town in acc.Towns) 
            {
                var et = new ExtendedTown (this, town);
                ExtendedTowns.Add(et);
                VariantsET1.Add((ExtendedTown)et.Clone());
                VariantsET2.Add((ExtendedTown)et.Clone());
            }
            WofHFuncs = new WofHMath(data);
        }
        public double RebuildCost(int n, byte v)
        {
            switch (v)
            {
                case 1: return WofHFuncs.TownRebuildCost((Town)ExtendedTowns[n], (Town)VariantsET1[n], (Account)this);
                case 2: return WofHFuncs.TownRebuildCost((Town)ExtendedTowns[n], (Town)VariantsET2[n], (Account)this);
                default: return 0;
            }            
        }

    }
}
