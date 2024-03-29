﻿using System;
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
using WofHCalc.Views;

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
        //отображаемый город (оригинальный город или его клон/вариант)
        private ExtendedTown? visible_town; 
        public ExtendedTown? VisibleTown
        {
            get => visible_town;
            set
            {
                visible_town = value;
                OnPropertyChanged(nameof(VisibleTown));
                if (active_acc!.nvariant>0) OnPropertyChanged(nameof(ActiveAccount));
            }
        }
        //выбранный город
        private ExtendedTown? selected;
        public ExtendedTown? SelectedTown
        {
            get => selected;
            set
            {
                selected = value; 
                OnPropertyChanged(nameof(SelectedTown));
                ActiveAccount!.nvariant = 0;
                VisibleTown = value;
            }
        }
        private RelayCommand? reset_clone;
        public RelayCommand ResetClone
        {
            get
            {
                return reset_clone ??= new RelayCommand(
                    o1 =>
                    {
                        switch (active_acc!.nvariant)
                        {
                            case 1: active_acc!.variantsET1[active_acc.ntown] = (ExtendedTown)active_acc.ExtendedTowns[active_acc.ntown].Clone(); break;
                            case 2: active_acc!.variantsET2[active_acc.ntown] = (ExtendedTown)active_acc.ExtendedTowns[active_acc.ntown].Clone(); break;
                            default: break;
                        }
                        UpdateAll();
                    },
                    o2 => { return (SelectedTown is not null && active_acc!.nvariant>0); }
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
                        int nt = ActiveAccount!.ntown;
                        ActiveAccount!.ExtendedTowns[nt] = (ExtendedTown)VisibleTown!.Clone();                        
                        ActiveAccount!.nvariant = 0;
                        SelectedTown = ActiveAccount.ExtendedTowns[nt];
                    },
                    o2 => { return (SelectedTown is not null && active_acc!.nvariant > 0); }
                );
            }
        }

        private RelayCommand? add_town_command; //+
        public RelayCommand AddTown
        {
            get
            {
                return add_town_command ??= new RelayCommand(o1 =>
                {
                    try
                    {   
                        ActiveAccount!.Towns.Add(new Town());
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
                        ConfirmAction c = new($"Точно хотите удалить город {SelectedTown!.Name}?");
                        if (c.ShowDialog()==true)
                        {
                            int n = ActiveAccount!.ntown;                            
                            SelectedTown = ActiveAccount!.ExtendedTowns.FirstOrDefault();
                            ActiveAccount!.ExtendedTowns.RemoveAt(n);
                            ActiveAccount.Towns.RemoveAt(n);
                        }
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
                return updatecalcs ??= new RelayCommand(o1 => UpdateAll());
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void UpdateAll()
        {
            OnPropertyChanged(nameof(VisibleTown));
            OnPropertyChanged(nameof(SelectedTown));
            OnPropertyChanged(nameof(ActiveAccount));
        }
    }
}
