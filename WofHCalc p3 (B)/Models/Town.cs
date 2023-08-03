using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.ExtendedModel;
using WofHCalc.Supports;
using WofHCalc.Supports.ForEnumBinds;

namespace WofHCalc.Models
{

    public class Town : INotifyPropertyChanged, ICloneable
    {
        private double? science_ef; //процент влива колб, потом надо будет переделать на нормальное вычисление по табличке
        public double? ScienceEfficiency 
        {
            get => science_ef;
            set => science_ef = value;
        }
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
        private ObservableCollection<byte> great_citizens;
        public ObservableCollection<byte> GreatCitizens
        {
            get=> great_citizens;
            set { great_citizens= value; OnPropertyChanged(nameof(GreatCitizens));}
        }
        private ObservableCollection<byte?> lucky_town;
        public ObservableCollection<byte?> LuckyTown
        {
            get => lucky_town;
            set { lucky_town= value; OnPropertyChanged(nameof(LuckyTown)); }
        }
        private ObservableCollection<bool> resconsumption;
        public ObservableCollection<bool> ResConsumption
        {
            get=> resconsumption;
            set { resconsumption= value; OnPropertyChanged(nameof(ResConsumption));}
        }
        private ObservableCollection<bool> product;
        public ObservableCollection<bool> Product
        {
            get => product;                
            set { product = value; OnPropertyChanged(nameof(Product)); }
        }
        private ObservableCollection<AreaImprovementSlot> area_improvements;
        public ObservableCollection<AreaImprovementSlot> AreaImprovements
        {
            get=> area_improvements;
            set { area_improvements= value; OnPropertyChanged(nameof(AreaImprovements)); }
        }
        [JsonConstructor]
        public Town(object sup)
        {
            this.name = "New city";
            Climate = Climate.unknown;
            water_places = 0;
            on_hill = false;
            town_buldings = new ObservableCollection<BuildSlot>
            {
                new BuildSlot(Slot.wounder), //0 чудо //НЕ МЕНЯТЬ ТИП ЭТОГО СЛОТА
                new BuildSlot(Slot.fort), //1 защита //НЕ МЕНЯТЬ ТИП ЭТОГО СЛОТА
                new BuildSlot(Slot.center), //2 центр //НЕ МЕНЯТЬ ТИП ЭТОГО СЛОТА
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
            great_citizens = new ObservableCollection<byte>();
            lucky_town = new ObservableCollection<byte?>();
            area_improvements = new ObservableCollection<AreaImprovementSlot>();
            resconsumption = new ObservableCollection<bool>();
            product = new ObservableCollection<bool>();
        }
        public Town() 
        {
            this.name = "New city";
            Climate = Climate.unknown;
            water_places = 0;
            on_hill = false;
            town_buldings = new ObservableCollection<BuildSlot>
            {
                new BuildSlot(Slot.wounder), //0 чудо //НЕ МЕНЯТЬ ТИП ЭТОГО СЛОТА
                new BuildSlot(Slot.fort), //1 защита //НЕ МЕНЯТЬ ТИП ЭТОГО СЛОТА
                new BuildSlot(Slot.center), //2 центр //НЕ МЕНЯТЬ ТИП ЭТОГО СЛОТА
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
            great_citizens = new ObservableCollection<byte>() { 0, 0, 0, 0, 0, 0, 0 };
            lucky_town = new ObservableCollection<byte?>() { 0, 0, null, 0, 0, 0, null, null, null, 0};
            area_improvements = new ObservableCollection<AreaImprovementSlot>();
            resconsumption = new ObservableCollection<bool>();
            product = new ObservableCollection<bool>();
            for (int i = 0; i < 23; i++) { resconsumption.Add(false); product.Add(false); }

        }        
        //костыль, чтоб нормально сереализовалось в JSON
        public Town(ExtendedTown et)
        {
            science_ef = et.ScienceEfficiency;
            name = et.Name;
            clm = et.Climate;
            deposit= et.Deposit;
            water_places = et.WaterPlaces;
            on_hill = et.OnHill;
            town_buldings = et.TownBuilds;
            great_citizens = et.GreatCitizens;
            lucky_town= et.LuckyTown;
            resconsumption = et.ResConsumption;
            product = et.Product;
            area_improvements = et.AreaImprovements;
        }
        public event PropertyChangedEventHandler? PropertyChanged;        

        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual object Clone()
        {
            Town ans = new();
            ans.Name = this.Name;
            ans.Climate = this.Climate;
            ans.Deposit = this.Deposit;
            ans.WaterPlaces = this.WaterPlaces;
            ans.OnHill= this.OnHill;
            for (int i = 0; i < 19; i++)
                ans.TownBuilds[i] = (BuildSlot)this.TownBuilds[i].Clone();
            for (int i = 0; i < 7; i++)
                ans.GreatCitizens[i] = this.GreatCitizens[i];
            for (int i = 0; i < 6; i++)
                ans.LuckyTown[i] = this.LuckyTown[i];
            for (int i = 0; i < 23; i++)
            {
                ans.ResConsumption[i] = this.ResConsumption[i];
                ans.Product[i] = this.Product[i];
            }
            for (int i = 0; i<this.AreaImprovements.Count; i++)
                ans.AreaImprovements.Add((AreaImprovementSlot)this.AreaImprovements[i].Clone());
            return ans;
        }
    }
    public class BuildSlot : INotifyPropertyChanged, ICloneable
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
        private bool available;//куплен?
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

        public object Clone()
        {
            return new BuildSlot
            {
                Slot = this.Slot,
                Building = this.Building,
                Available = this.Available,
                Level = this.Level
            };            
        }
    }
    public class AreaImprovementSlot : INotifyPropertyChanged, ICloneable
    {
        private AreaImprovementName name;
        public AreaImprovementName AIName 
        {
            get => name;            
            set
            {
                name = value;
                OnPropertyChanged(nameof(AIName));
            }   
        }
        private byte level;
        public byte Level
        {
            get=> level;
            set
            {
                level = value;
                OnPropertyChanged(nameof(Level));
            }
        }
        private byte users;
        public byte Users
        {
            get=> users;
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }
        public AreaImprovementSlot()
        {
            AIName = AreaImprovementName.none; Level = 0; users = 1;
        }
        public AreaImprovementSlot(AreaImprovementName name, byte level, byte users)
        {
            AIName = name; Level = level; Users = users;
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public object Clone()
        {
            return new AreaImprovementSlot
            {
                AIName = this.AIName,
                Level = this.Level,
                Users = this.Users,
            };
        }
    }
}
