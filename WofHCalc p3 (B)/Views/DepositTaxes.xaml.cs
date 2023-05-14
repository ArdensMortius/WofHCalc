using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using WofHCalc.Models;

namespace WofHCalc.Views
{
    /// <summary>
    /// Interaction logic for DepositTaxes.xaml
    /// </summary>
    public partial class DepositTaxes : Window
    {
        public Account dc;
        public DepositTaxes(Account acc)
        {
            dc = acc;
            DataContext = dc;
            if (acc.Financial.DepositsTaxes is null) 
            {
                acc.Financial.DepositsTaxes = new ObservableCollection<int>();
                for (int i = 0; i < 53; i++) { acc.Financial.DepositsTaxes.Add(0); }
            }
            InitializeComponent();
        }

        private void DepositTaxEdit_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int v;
            string? new_value = (e.EditingElement as TextBox)!.Text;
            if (int.TryParse(new_value, out v))
            {
                dc.Financial.DepositsTaxes[e.Row.GetIndex()] = v;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = true;            
        }
    }
}
