using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WofHCal.Supports;
using WofHCalc.DataSourses;
using WofHCalc.MathFuncs;
using WofHCalc.Models;
using WofHCalc.Supports;

namespace WofHCalc.Controllers
{
    internal class MainWindowController : INotifyPropertyChanged
    {
        //поля и свойста
        private Account? active_acc;
        public Account? ActiveAccount
        {
            get => active_acc;
            set
            {
                active_acc = value;
                OnPropertyChanged(nameof(ActiveAccount));
                VisibleTown ??= ActiveAccount!.Towns.FirstOrDefault();
            }
        }
        private Town? visible_town;
        public Town? VisibleTown
        {
            get => visible_town;
            set
            {
                visible_town = value;
                OnPropertyChanged(nameof(VisibleTown));
                Updatecalcinfo();
            }
        }        
        public ObservableCollection<Town?> clones = new();        
        private Town? selected;
        public Town? SelectedTown
        {
            get => selected;
            set
            {
                selected = value; 
                OnPropertyChanged(nameof(SelectedTown));
                VisibleTown = value;
                for(int i=0; i < clones.Count; i++)
                    clones[i] = (Town)selected!.Clone(); 
            }
        }

        #region вычисляемая инфа по городу
        private void Updatecalcinfo()
        {
            OnPropertyChanged(nameof(Culture));
            OnPropertyChanged(nameof(Growth));
            OnPropertyChanged(nameof(Products));
            OnPropertyChanged(nameof(Eat));
            OnPropertyChanged(nameof(Upkeep));
        }
        public int Culture//+
        {
            get
            {
                return TownFuncs.TownCulture(
                    (int)active_acc!.Culture,
                    visible_town!.TownBuilds.Select(x=>x.Building).ToArray(),
                    visible_town.TownBuilds.Select(x=>(int?)x.Level).ToArray(),
                    active_acc.Race, 
                    visible_town.GreatCitizens[(int)GreatCitizensNames.Creator],
                    visible_town.ResConsumption.ToArray(),
                    visible_town.AreaImprovements.Select( a=> a.AIName).ToArray(),
                    visible_town.AreaImprovements.Select(a=>a.Level).ToArray(),
                    visible_town.AreaImprovements.Select(a=>a.Users).ToArray(),
                    visible_town.LuckyTown[(int)LuckBonusNames.culture]);
            }
        }
        public double Growth//+
        {
            get
            {
                return TownFuncs.TownGrowth(
                    active_acc!.PopulationGrowth,
                    visible_town!.TownBuilds.Select(x => x.Building).ToArray(),
                    visible_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                    active_acc.Race,
                    (visible_town.Deposit != DepositName.none),
                    visible_town.ResConsumption.ToArray(),
                    visible_town.GreatCitizens[(int)GreatCitizensNames.Doctor],
                    visible_town.AreaImprovements.Select(a => a.AIName).ToArray(),
                    visible_town.AreaImprovements.Select(a => a.Level).ToArray(),
                    visible_town.AreaImprovements.Select(a => a.Users).ToArray(),
                    visible_town.LuckyTown[(int)LuckBonusNames.grown]
                    );
            }
        }
        public double Upkeep
        {
            get
            {
                return TownFuncs.BuildsUpkeep(
                    visible_town!.TownBuilds.Select(x => x.Building).ToArray(),
                    visible_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                    active_acc!.Race);
            }
        }
        public double[] Eat
        {
            get
            {
                double swob = 0;
                if (VisibleTown!.ResConsumption[(int)ResName.books] && VisibleTown.Product[(int)ResName.science])                
                    swob = Products[(int)ResName.science] / (1 + Data.ResData[(int)ResName.books].effect);
                else swob = Products[(int)ResName.science];
                
                return TownFuncs.GetResConsumption(
                    visible_town!.ResConsumption.ToArray(),
                    TownFuncs.TownGrowthWOUngrownAndResourses(
                        active_acc!.PopulationGrowth,
                        visible_town.TownBuilds.Select(x => x.Building).ToArray(),
                        visible_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                        active_acc.Race,
                        (visible_town.Deposit != DepositName.none),                        
                        visible_town.GreatCitizens[(int)GreatCitizensNames.Doctor],
                        visible_town.AreaImprovements.Select(a => a.AIName).ToArray(),
                        visible_town.AreaImprovements.Select(a => a.Level).ToArray(),
                        visible_town.AreaImprovements.Select(a => a.Users).ToArray(),
                        visible_town.LuckyTown[(int)LuckBonusNames.grown]),
                    TownFuncs.TownCultureWOResourses(
                        (int)active_acc.Culture,
                        visible_town.TownBuilds.Select(x => x.Building).ToArray(),
                        visible_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                        active_acc.Race,
                        visible_town.GreatCitizens[(int)GreatCitizensNames.Creator],                        
                        visible_town.AreaImprovements.Select(a => a.AIName).ToArray(),
                        visible_town.AreaImprovements.Select(a => a.Level).ToArray(),
                        visible_town.AreaImprovements.Select(a => a.Users).ToArray(),
                        visible_town.LuckyTown[(int)LuckBonusNames.culture]),
                    swob    
                    );
            }
        }
        public double[] Products
        {
            get
            {
                return TownFuncs.Production(
                    visible_town!.TownBuilds.Select(x => x.Building).ToArray(),
                    visible_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                    visible_town.GreatCitizens.ToArray(),
                    TownFuncs.Corruption(
                        visible_town.TownBuilds.Select(x => x.Building).ToArray(),
                        visible_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                        active_acc!.Towns.Count),
                    visible_town.Product.ToArray(),
                    visible_town.Deposit,
                    visible_town.Climate,
                    visible_town.OnHill,
                    visible_town.WaterPlaces,
                    active_acc.Science_Bonuses.ToArray(),
                    visible_town.AreaImprovements.Select(a => a.AIName).ToArray(),
                    visible_town.AreaImprovements.Select(a => a.Level).ToArray(),
                    visible_town.AreaImprovements.Select(a => a.Users).ToArray(),
                    visible_town.LuckyTown[(int)LuckBonusNames.science],
                    visible_town.LuckyTown[(int)LuckBonusNames.production],
                    visible_town.ResConsumption[(int)ResName.books]
                    );
            }
        }
        #endregion
        private RelayCommand? reset_clone;        
        public RelayCommand ResetClone
        {
            get
            {
                return reset_clone ??= new RelayCommand(
                    o1 => { clones[clones.IndexOf(VisibleTown)] = (Town)SelectedTown!.Clone(); },
                    o2 => { return (SelectedTown is not null && clones.Any(t=> t==VisibleTown)); }
                );
            }
        }
        private RelayCommand? apply_clone;
        public RelayCommand ApplyClone
        {
            get
            {
                return apply_clone ??= new RelayCommand(
                    o1 => 
                    {
                        int ind = ActiveAccount!.Towns.IndexOf(SelectedTown!);
                        ActiveAccount!.Towns[ind] = (Town)VisibleTown!.Clone();
                        SelectedTown = ActiveAccount.Towns[ind];
                    },
                    o2 => { return (SelectedTown is not null && clones.Any(t => t == VisibleTown)); }
                );
            }

        }
        private RelayCommand? add_town_command;
        public RelayCommand AddTown
        {
            get
            {
                return add_town_command ??= new RelayCommand(o1 =>
                {
                    try
                    {
                        ActiveAccount!.Towns.Add(new Town());
                        VisibleTown = ActiveAccount.Towns.Last();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка добавления города");
                    }
                },
                o2 => { return (ActiveAccount is not null); }
                );
            }
        }
        private RelayCommand? del_town_command;
        public RelayCommand DelTown
        {
            get
            {
                return del_town_command ??= new RelayCommand(o1 =>
                {
                    try
                    {
                        ActiveAccount!.Towns.Remove(SelectedTown!);
                        SelectedTown = ActiveAccount.Towns.FirstOrDefault();
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка удаления города");
                    }
                },
                o2 => { return (SelectedTown is not null); }
                );
            }
        }
        private RelayCommand? updatecalcs;
        public RelayCommand UpdateCalcs
        {
            get
            {
                return updatecalcs ??= new RelayCommand(o1 =>
                { Updatecalcinfo();});
            }
        }

        private ObservableCollection<PTDA>? ptda; //устарело
        public ObservableCollection<PTDA> PriceTaxDataAdapter
        {            
            set 
            { 
                ptda = value;
                OnPropertyChanged(nameof(PriceTaxDataAdapter)); 
            }
            get
            {
                if (ptda != null) return ptda;
                else
                {
                    ptda = new ObservableCollection<PTDA>();
                    if (ActiveAccount is null) { return ptda; }
                    for (int i = 0; i < 23; i++)
                    {
                        ptda.Add(new PTDA
                        {
                            ImgPath = $"/DataSourses/Img/icons/res/res{i}.png",
                            Resource = ((ResName)i).ToString(),
                            Price = ActiveAccount!.Financial.Prices[i],
                            Tax = ActiveAccount!.Financial.Taxes[i]
                        }); 
                    }
                }
                return ptda;
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class PTDA
    {
        public string? ImgPath { get; set; }
        public string? Resource { get; set; }
        public int Price { get; set; }
        public int Tax { get; set; }
    }
}
