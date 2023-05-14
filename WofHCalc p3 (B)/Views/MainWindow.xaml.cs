using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WofHCalc.Controllers;
using WofHCalc.FileManagers;
using WofHCalc.Supports;
using WofHCalc.Supports.ForEnumBinds;
using WofHCalc.Views;

namespace WofHCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainWindowController dc;
        public MainWindow()
        {
            dc = new MainWindowController();
            DataContext = dc;
            if (dc.ActiveAccount == null)
            {
                AccManager AM = new();
                if (AM.ShowDialog() == true)
                {
                    dc.ActiveAccount = AM.open_acc;
                }
                else this.Close();
            }
            if (dc.ActiveAccount == null) { this.Close(); }
            InitializeComponent();
            dc.clones.Add(null);
            dc.clones.Add(null);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)//пока просто сохранение без диалогового окна
        {
            if (dc.ActiveAccount != null)
            {
                string path = $"{ConfigurationManager.AppSettings["savefilespath"]!}/{dc.ActiveAccount.World}_{dc.ActiveAccount.Name}";
                FileManager.UpdateAccFile(path, dc.ActiveAccount.ToJSON());                
            }
            else MessageBox.Show("Nothing to save");
        }
        private void Button_OpenSlot(object sender, RoutedEventArgs e)
        {
            
            byte slot_id = (byte)WrapBuilds.SelectedIndex;
            SlotBuildsList sbv = new(dc.VisibleTown!, slot_id, dc.ActiveAccount!.Race);
            if (sbv.ShowDialog() == true)
            {
                dc.VisibleTown!.TownBuilds[slot_id].Building = (BuildName)sbv.selected_build!;
                dc.VisibleTown.TownBuilds[slot_id].Level = 0;
            }
        }
        private void Button_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (WrapBuilds.SelectedIndex < 0) return; //костыль, но помогает избежать вылета. Потом надо починить
            byte slot_id = (byte)WrapBuilds.SelectedIndex;
            dc.VisibleTown!.TownBuilds[slot_id].Building = BuildName.none;
            dc.VisibleTown!.TownBuilds[slot_id].Level = null;
        }

        private void GSV_LoadingRow(object sender, DataGridRowEventArgs e) =>        
            e.Row.Header = ((GreatCitizensNames)e.Row.GetIndex()).Description();
        private void LBV_LoadingRow(object sender, DataGridRowEventArgs e) =>
            e.Row.Header = ((LuckBonusNames)e.Row.GetIndex()).Description();


        private void GSV_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            byte v;
            string? new_value = (e.EditingElement as TextBox)!.Text;
            if (byte.TryParse(new_value, out v))
            {
                dc.VisibleTown!.GreatCitizens[e.Row.GetIndex()] = v;                
            }
        }

        private void LBV_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            byte v;
            string? new_value = (e.EditingElement as TextBox)!.Text;
            if (byte.TryParse(new_value, out v))
            {
                dc.VisibleTown!.LuckyTown[e.Row.GetIndex()] = v;
            }
        }
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            //а всё потому что при binding-е к элементу коллекции она умудряется обновляться через get, а не через set.
            dc.OnPropertyChanged(nameof(dc.VisibleTown));
        }

        private void ProdDisplay_MouseDown(object sender, RoutedEventArgs e)
        {
            if (Prod.Visibility == Visibility.Visible) 
            {
                Prod.Visibility = Visibility.Collapsed;                
            }
            else 
            {
                Prod.Visibility = Visibility.Visible;
            }
        }

        private void PricesEdit_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int v;
            string? new_value = (e.EditingElement as TextBox)!.Text;
            if (int.TryParse(new_value, out v))
            {
                dc.ActiveAccount.Financial.Prices[e.Row.GetIndex()] = v;
            }
        }
        private void TaxesEdit_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int v;
            string? new_value = (e.EditingElement as TextBox)!.Text;
            if (int.TryParse(new_value, out v))
            {
                dc.ActiveAccount.Financial.Taxes[e.Row.GetIndex()] = v;
            }
        }
        private void OpenDTE(object sender, RoutedEventArgs e)
        {
            DepositTaxes DTE = new DepositTaxes(dc.ActiveAccount!);
            if (DTE.ShowDialog()==true)
            {
                dc.ActiveAccount!.Financial.DepositsTaxes = DTE.t;
            }
        }
    }
}
