using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using WofHCalc.MathFuncs;
using WofHCalc.Models;
using WofHCalc.Supports;
using WofHCalc.Supports.ForEnumBinds;

namespace WofHCalc.Views
{
    /// <summary>
    /// Interaction logic for UnitsInfoList.xaml
    /// </summary>
    public partial class UnitsInfoList : Window, INotifyPropertyChanged
    {
        //public byte scale = 1;
        public ObservableCollection<int> UnitsList { get; set; }
        public ObservableCollection<UnitInfo> InfoList { get; set; }
        public UnitsInfoList(ObservableCollection<int> units, FinancialPolicy f, WofHMath m)
        {
            this.UnitsList = units;

            //для проверки
            var t1 = Enum.GetValues(typeof(UnitsNames)).Cast<int>().ToArray();
            if (units is null) UnitsList = new ObservableCollection<int>(t1);

            this.InfoList = new();
            foreach (var unit in UnitsList) InfoList.Add(new UnitInfo((UnitsNames)unit,f,m));
            this.DataContext = this;
            InitializeComponent();            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class UnitInfo
    {
        public int id { get; }
        public string name { get; }
        //для отображения картинки
        public Thickness t { get => new Thickness((-1 - id) * 23, 0, 0, 0); } //сдвиг всего image
        public Rect rect { get => new Rect((id + 1) * 23, 0, 23, 23); } //сдвиг отображаемой области
        public float ResPrice { get; }
        public float PopPrice { get; }
        public float FullPrice { get; }
        public double Upkeep { get; }
        
        public UnitInfo(UnitsNames unit, FinancialPolicy f, WofHMath m)
        {
            this.id = (int)unit;
            this.name = unit.Description();
            this.ResPrice = m.UnitResPrice(unit,f);
            this.PopPrice = m.UnitPopPriceBase(unit,f);
            this.FullPrice = m.BaseUnitPrice(id, f);
            this.Upkeep = m.UnitUpkeep(unit,f);
        }
    }
}
