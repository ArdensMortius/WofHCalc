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
        public ObservableCollection<int> t;//возврат значений
        public DepositTaxes(Account acc)
        {
            dc = acc;
            t = new ObservableCollection<int>();
            DataContext = t;
            if (acc.Financial.DepositsTaxes is null) 
                for (int i = 0; i < 53; i++) { t.Add(0); }
            else 
                for (int i = 0; i< 53;i++) { t.Add(dc.Financial.DepositsTaxes[i]); }
            InitializeComponent();            
        }

        private void DepositTaxEdit_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            int v;
            string? new_value = (e.EditingElement as TextBox)!.Text;
            if (int.TryParse(new_value, out v))
            {
                t[e.Row.GetIndex()] = v;
            }
            else (e.EditingElement as TextBox)!.Text = t[e.Row.GetIndex()].ToString();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = true;
            //DepositTaxEdit.CancelEdit();
        }
    }
}
