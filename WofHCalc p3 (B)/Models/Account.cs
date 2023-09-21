using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WofHCalc.Supports;
using System.Windows.Documents;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace WofHCalc.Models
{
    public class Account : INotifyPropertyChanged
    {
        public string Name { get;}        
        public int World { get;}
        public Race Race { get; set; }
        public ObservableCollection<Town> Towns { get; set; }
        //public ObservableCollection<Town>? TargetTowns { get; set; }
        public float PopulationGrowth { get; set; }
        public float Culture { get; set; }
        public int Traiders { get; set; }
        public ObservableCollection<int> Science_Bonuses { get; set; } //бонус к производствам в процентах. Индекс по ResProdType.
        public ObservableCollection<bool>? UnitsVisibilyty { get; set; } //видимость юнитов в списках. Позволяет скрывать не актуальных.
        public FinancialPolicy Financial { get; set; }
        [JsonConstructor]
        public Account(string name, int world)
        {
            Name = name;
            World = world;
            Race = Race.unknown;
            PopulationGrowth = 0;
            Culture = 0;
            Traiders = 0;
            Science_Bonuses = new ObservableCollection<int> { 100, 100, 100, 100 };
            Towns = new() { new Town() };
            Financial = new();
        }
        public Account(Account a)
        {
            Name = a.Name;
            World = a.World;
            Race = a.Race;
            Towns = a.Towns;
            //TargetTowns = a.TargetTowns;
            PopulationGrowth = a.PopulationGrowth;
            Culture = a.Culture;
            Traiders = a.Traiders;
            Science_Bonuses = a.Science_Bonuses;
            Financial = a.Financial;
        }

        public virtual string ToJSON() => JsonConvert.SerializeObject(this);
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

    }
}
