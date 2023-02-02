using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Supports;

namespace WofHCalc.Models
{

    public class Town : INotifyPropertyChanged
    {
        private string name;
        public string Name 
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }
        private Climate clm;
        public Climate Climate
        {
            get => clm;
            set { clm = value; OnPropertyChanged(nameof(Climate)); }
        }
        private DepositName deposit;
        public DepositName Deposit 
        { 
            get => deposit;
            set { deposit = value; OnPropertyChanged(nameof(Deposit)); }
        }
        private byte water_places;
        public byte WaterPlaces 
        {
            get => water_places;
            set
            {
                water_places = value;
                for (int i = 10; i < 14; i++)
                    if (water_places + 10 > i) TownBuilds[i].Slot = Slot.water;
                    else TownBuilds[i].Slot = Slot.plain;
                OnPropertyChanged(nameof(WaterPlaces));
            }
        }
        private bool on_hill;
        public bool OnHill 
        {
            get => on_hill;          
            set
            {
                on_hill = value;
                if (OnHill)
                {
                    TownBuilds[16].Slot = Slot.hill;
                    TownBuilds[17].Slot = Slot.hill;
                }
                else
                {
                    TownBuilds[16].Slot = Slot.plain;
                    TownBuilds[17].Slot = Slot.plain;
                }
                OnPropertyChanged(nameof(OnHill));
            }
        }
        private ObservableCollection<BuildSlot> town_buldings;
        public ObservableCollection<BuildSlot> TownBuilds 
        { 
            get=>town_buldings;
            set
            {
                town_buldings= value;
                OnPropertyChanged(nameof(TownBuilds));
            }
        }
        public Town() 
        {
            name = "new town";
            Climate = Climate.unknown;
            water_places = 0;
            on_hill = false;
            town_buldings = new ObservableCollection<BuildSlot>
            {
                new BuildSlot(Slot.wounder), //0 чудо
                new BuildSlot(Slot.fort), //1 защита
                new BuildSlot(Slot.center), //2 центр
                new BuildSlot(), //3
                new BuildSlot(), //4
                new BuildSlot(), //5
                new BuildSlot(), //6
                new BuildSlot(), //7
                new BuildSlot(), //8
                new BuildSlot(), //9
                new BuildSlot(), //10 равнина или вода
                new BuildSlot(available: false), //11 равнина или вода
                new BuildSlot(), //12 равнина или вода
                new BuildSlot(), //13 равнина или вода
                new BuildSlot(available: false), //14
                new BuildSlot(available: false), //15
                new BuildSlot(available: false), //16 холм или равнина
                new BuildSlot(), //17 холм или равнина
                new BuildSlot(Slot.hill) //18 холм
            };
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class BuildSlot : INotifyPropertyChanged
    {
        private Slot slot;
        public Slot Slot 
        { 
            get=>slot;
            set { slot = value; OnPropertyChanged(nameof(Slot)); }
        }
        private BuildName bulding;
        public BuildName Building
        { 
            get=>bulding;
            set { bulding = value; OnPropertyChanged(nameof(Building)); }
        }
        private byte? level;
        public byte? Level 
        { 
            get=>level;
            set { level = value; OnPropertyChanged(nameof(Level)); }
        }
        private bool available;
        public bool Available 
        { 
            get=>available;
            set { available= value; OnPropertyChanged(nameof(Available));}
        }
        public BuildSlot()
        {
            Slot = Slot.plain; Building= BuildName.none; Level = null; Available = true;
        }
        public BuildSlot(Slot slot = Slot.plain, bool available = true)
        {
            Slot = slot;
            Building = BuildName.none;
            Level = null;
            this.Available = available;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
