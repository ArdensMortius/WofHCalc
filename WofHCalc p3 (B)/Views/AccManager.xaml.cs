using System;
using System.Collections.Generic;
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
using WofHCalc.Controllers;
using WofHCalc.Models;

namespace WofHCalc.Views
{
    /// <summary>
    /// Interaction logic for AccManager.xaml
    /// </summary>
    public partial class AccManager : Window
    {
        public Account? open_acc { get; private set; }
        AccMngrController ds;
        public AccManager()
        {
            InitializeComponent();
            ds = new AccMngrController();
            DataContext = ds;
        }
        private void Open_Click(object sender, RoutedEventArgs e)
        {
            this.open_acc = ds.Selected_acc;
            this.DialogResult = true;
        }
    }
}
