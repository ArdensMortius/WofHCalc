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
                SelectedTown ??= ActiveAccount!.Towns.FirstOrDefault();
            }
        }
        private Town? selected_town;
        public Town? SelectedTown
        {
            get => selected_town;
            set
            {
                selected_town = value;
                OnPropertyChanged(nameof(SelectedTown));
                updatecalcinfo();
            }
        }
        #region вычисляемая инфа по городу
        private void updatecalcinfo()
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
                    (int)active_acc.Culture,
                    selected_town.TownBuilds.Select(x=>x.Building).ToArray(),
                    selected_town.TownBuilds.Select(x=>(int?)x.Level).ToArray(),
                    active_acc.Race, 
                    selected_town.GreatCitizens[(int)GreatCitizensNames.Creator],
                    selected_town.ResConsumption.ToArray(),
                    selected_town.AreaImprovements.Select( a=> a.AIName).ToArray(),
                    selected_town.AreaImprovements.Select(a=>a.Level).ToArray(),
                    selected_town.AreaImprovements.Select(a=>a.Users).ToArray(),
                    selected_town.LuckyTown[(int)LuckBonusNames.culture]);
            }
        }
        public double Growth//+
        {
            get
            {
                return TownFuncs.TownGrowth(
                    active_acc.PopulationGrowth,
                    selected_town.TownBuilds.Select(x => x.Building).ToArray(),
                    selected_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                    active_acc.Race,
                    (selected_town.Deposit != DepositName.none),
                    selected_town.ResConsumption.ToArray(),
                    selected_town.GreatCitizens[(int)GreatCitizensNames.Doctor],
                    selected_town.AreaImprovements.Select(a => a.AIName).ToArray(),
                    selected_town.AreaImprovements.Select(a => a.Level).ToArray(),
                    selected_town.AreaImprovements.Select(a => a.Users).ToArray(),
                    selected_town.LuckyTown[(int)LuckBonusNames.grown]
                    );
            }
        }
        public double Upkeep
        {
            get
            {
                return TownFuncs.BuildsUpkeep(
                    selected_town!.TownBuilds.Select(x => x.Building).ToArray(),
                    selected_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                    active_acc!.Race);
            }
        }
        public double[] Eat
        {
            get
            {
                double swob = 0;
                if (SelectedTown.ResConsumption[(int)ResName.books] && SelectedTown.Product[(int)ResName.science])                
                    swob = Products[(int)ResName.science] / (1 + Data.ResData[(int)ResName.books].effect);
                else swob = Products[(int)ResName.science];
                
                return TownFuncs.GetResConsumption(
                    selected_town!.ResConsumption.ToArray(),
                    TownFuncs.TownGrowthWOUngrownAndResourses(
                        active_acc!.PopulationGrowth,
                        selected_town.TownBuilds.Select(x => x.Building).ToArray(),
                        selected_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                        active_acc.Race,
                        (selected_town.Deposit != DepositName.none),                        
                        selected_town.GreatCitizens[(int)GreatCitizensNames.Doctor],
                        selected_town.AreaImprovements.Select(a => a.AIName).ToArray(),
                        selected_town.AreaImprovements.Select(a => a.Level).ToArray(),
                        selected_town.AreaImprovements.Select(a => a.Users).ToArray(),
                        selected_town.LuckyTown[(int)LuckBonusNames.grown]),
                    TownFuncs.TownCultureWOResourses(
                        (int)active_acc.Culture,
                        selected_town.TownBuilds.Select(x => x.Building).ToArray(),
                        selected_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                        active_acc.Race,
                        selected_town.GreatCitizens[(int)GreatCitizensNames.Creator],                        
                        selected_town.AreaImprovements.Select(a => a.AIName).ToArray(),
                        selected_town.AreaImprovements.Select(a => a.Level).ToArray(),
                        selected_town.AreaImprovements.Select(a => a.Users).ToArray(),
                        selected_town.LuckyTown[(int)LuckBonusNames.culture]),
                    swob    
                    );
            }
        }
        public double[] Products
        {
            get
            {
                return TownFuncs.Production(
                    selected_town!.TownBuilds.Select(x => x.Building).ToArray(),
                    selected_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                    selected_town.GreatCitizens.ToArray(),
                    TownFuncs.Corruption(
                        selected_town.TownBuilds.Select(x => x.Building).ToArray(),
                        selected_town.TownBuilds.Select(x => (int?)x.Level).ToArray(),
                        active_acc!.Towns.Count),
                    selected_town.Product.ToArray(),
                    selected_town.Deposit,
                    selected_town.Climate,
                    selected_town.OnHill,
                    selected_town.WaterPlaces,
                    active_acc.Science_Bonuses.ToArray(),
                    selected_town.AreaImprovements.Select(a => a.AIName).ToArray(),
                    selected_town.AreaImprovements.Select(a => a.Level).ToArray(),
                    selected_town.AreaImprovements.Select(a => a.Users).ToArray(),
                    selected_town.LuckyTown[(int)LuckBonusNames.science],
                    selected_town.LuckyTown[(int)LuckBonusNames.production],
                    selected_town.ResConsumption[(int)ResName.books]
                    );
            }
        }
        #endregion
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
                        SelectedTown = ActiveAccount.Towns.Last();
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
                { updatecalcinfo();});
            }
        }

        private ObservableCollection<PTDA>? ptda;
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
