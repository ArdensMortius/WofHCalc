using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
