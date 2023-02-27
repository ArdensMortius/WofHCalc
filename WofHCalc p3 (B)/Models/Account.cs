using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WofHCalc.Supports;

namespace WofHCalc.Models
{
    public class Account : INotifyPropertyChanged
    {
        public string Name { get;}        
        public byte World { get;}
        public Race Race { get; set; }
        public ObservableCollection<Town> Towns { get; set; }
        public float PopulationGrowth { get; set; }
        public float Culture { get; set; }
        public float Traiders { get; set; }
        public ObservableCollection<int> Science_Bonuses { get; set; } //бонус к производствам в процентах. Индекс по ResProdType.
        public FinancialPolicy Financial { get; set; }
        public Account(string name="unknown", byte world=0)
        {            
            Name = name;
            World = world;
            Race = Race.unknown;
            PopulationGrowth = 0;
            Culture = 0;
            Traiders = 0;
            Science_Bonuses = new ObservableCollection<int> { 100, 100, 100, 100};
            Towns = new() {new Town()};
            Financial = new();
        }
        public string ToJSON() => JsonSerializer.Serialize<Account>(this);
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
