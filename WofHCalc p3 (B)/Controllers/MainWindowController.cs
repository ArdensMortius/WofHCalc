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
using WofHCalc.ExtendedModel;
using WofHCalc.Supports;

namespace WofHCalc.Controllers
{
    internal class MainWindowController : INotifyPropertyChanged
    {
        //поля и свойста
        //загруженный аккаунт с датой и остальным
        private ExtendedAccount? active_acc;
        public ExtendedAccount? ActiveAccount
        {
            get => active_acc;
            set
            {
                active_acc = value;
                OnPropertyChanged(nameof(ActiveAccount));
                VisibleTown ??= ActiveAccount!.ExtendedTowns.FirstOrDefault();
            }
        }
        //отображаемый город (оригинальный город или его клон)
        private ExtendedTown? visible_town; 
        public ExtendedTown? VisibleTown
        {
            get => visible_town;
            set
            {
                visible_town = value;
                OnPropertyChanged(nameof(VisibleTown));
                OnPropertyChanged(nameof(VisibleTown));
            }
        }
        //выбранный город, возможно стоит убрать
        private ExtendedTown? selected;
        public ExtendedTown? SelectedTown
        {
            get => selected;
            set
            {
                selected = value; 
                OnPropertyChanged(nameof(SelectedTown));
                VisibleTown = value;
            }
        }        
        //этот функционал уходить в extended model

        //private RelayCommand? reset_clone;        
        //public RelayCommand ResetClone
        //{
        //    get
        //    {
        //        return reset_clone ??= new RelayCommand(
        //            o1 => { clones[clones.IndexOf(VisibleTown)] = (Town)SelectedTown!.Clone(); },
        //            o2 => { return (SelectedTown is not null && clones.Any(t=> t==VisibleTown)); }
        //        );
        //    }
        //}        
        //private RelayCommand? apply_clone;
        //public RelayCommand ApplyClone
        //{
        //    get
        //    {
        //        return apply_clone ??= new RelayCommand(
        //            o1 => 
        //            {
        //                int ind = ActiveAccount!.Towns.IndexOf(SelectedTown!);
        //                ActiveAccount!.Towns[ind] = (Town)VisibleTown!.Clone();
        //                SelectedTown = ActiveAccount.Towns[ind];
        //            },
        //            o2 => { return (SelectedTown is not null && clones.Any(t => t == VisibleTown)); }
        //        );
        //    }
        //}

        private RelayCommand? add_town_command; //+
        public RelayCommand AddTown
        {
            get
            {
                return add_town_command ??= new RelayCommand(o1 =>
                {
                    try
                    {                        
                        ActiveAccount!.ExtendedTowns.Add(new ExtendedTown(ActiveAccount, new Town()));
                        SelectedTown = ActiveAccount.ExtendedTowns.Last();
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
        private RelayCommand? del_town_command; //+
        public RelayCommand DelTown
        {
            get
            {
                return del_town_command ??= new RelayCommand(o1 =>
                {
                    try
                    {
                        ExtendedTown d = SelectedTown!;
                        SelectedTown = ActiveAccount!.ExtendedTowns.FirstOrDefault();
                        ActiveAccount!.ExtendedTowns.Remove(d);
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
                { OnPropertyChanged(nameof(VisibleTown)); });
            }
        }

        //private ObservableCollection<PTDA>? ptda; //устарело
        //public ObservableCollection<PTDA> PriceTaxDataAdapter
        //{            
        //    set 
        //    { 
        //        ptda = value;
        //        OnPropertyChanged(nameof(PriceTaxDataAdapter)); 
        //    }
        //    get
        //    {
        //        if (ptda != null) return ptda;
        //        else
        //        {
        //            ptda = new ObservableCollection<PTDA>();
        //            if (ActiveAccount is null) { return ptda; }
        //            for (int i = 0; i < 23; i++)
        //            {
        //                ptda.Add(new PTDA
        //                {
        //                    ImgPath = $"/DataSourses/Img/icons/res/res{i}.png",
        //                    Resource = ((ResName)i).ToString(),
        //                    Price = ActiveAccount!.Financial.Prices[i],
        //                    Tax = ActiveAccount!.Financial.Taxes[i]
        //                }); 
        //            }
        //        }
        //        return ptda;
        //    }
        //}


        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //public class PTDA
    //{
    //    public string? ImgPath { get; set; }
    //    public string? Resource { get; set; }
    //    public int Price { get; set; }
    //    public int Tax { get; set; }
    //}
}
