using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Supports;

namespace WofHCalc.Models
{
    public class FinancialPolicy
    {
        //выплаты
        public ObservableCollection<int> Prices { get; set; }
        public float ForKnowledgeInvestment { get; set; }
        public float ForStrategicBuildings { get; set; }
        public float ForMillitaryBuildings { get; set; }
        public float ForScientificBuildings { get; set; }
        public float ForFortificationBuildings { get; set; }
        //налоги
        public ObservableCollection<int> Taxes { get; set; }
        public float HeadTax { get; set; } //per 100
        public ObservableCollection<int> DepositsTaxes { get; set; }
        //public byte TradeTax { get; set; } //%

        public FinancialPolicy() 
        {
            List<int> list = new();
            for (int i = 0; i<23; i++) { list.Add(0); }
            Prices = new ObservableCollection<int>(list);
            Taxes= new ObservableCollection<int>(list);
            DepositsTaxes = new ObservableCollection<int>();
            for (int i = 0; i<53; i++) { DepositsTaxes.Add(i); }
        }
    
    }
}
