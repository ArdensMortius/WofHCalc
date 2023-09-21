using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
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
        public ObservableCollection<bool> UnitsList { get; set; }
        public ObservableCollection<UnitInfo> InfoList { get; set; }
        private FinancialPolicy f;
        private WofHMath m;
        public UnitsInfoList(ObservableCollection<bool> UnitsVisibility, FinancialPolicy f, WofHMath m)
        {            

            //для проверки
            //var t1 = Enum.GetValues(typeof(UnitsNames)).Cast<int>().ToArray();
            this.f = f;
            this.m = m;
            this.UnitsList = UnitsVisibility;
            this.InfoList = new();
            for (int i=0; i<UnitsList.Count;i++)
                if (UnitsList[i])
                    InfoList.Add(new UnitInfo((UnitsNames)i,f,m));
            this.DataContext = this;
            InitializeComponent();            
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void ButtonHide(object sender, RoutedEventArgs e)
        {
            int unit_id = (int)(sender as Button).Tag;
            this.UnitsList[unit_id] = false; //отправка изменения обратно в источник данных (сейчас это объект типа ActiveAccount)
            InfoList.Remove(InfoList.Where(x => x.id == unit_id).FirstOrDefault()); //обновление существующей коллекции
        }

        private void ButtonShow(object sender, RoutedEventArgs e)
        {
            int unit_id = -1;
            if (int.TryParse(this.id_box.Text, out unit_id))            
                if (!this.UnitsList[unit_id])
                {
                    this.UnitsList[unit_id]=true;
                    int index = this.InfoList.Where(x=> x.id < unit_id).Count();
                    this.InfoList.Insert(index, new UnitInfo((UnitsNames)unit_id, f, m));
                }
            
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
