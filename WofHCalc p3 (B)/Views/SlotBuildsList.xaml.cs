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
using WofHCalc.DataSourses;
using WofHCalc.MathFuncs;
using WofHCalc.Models;
using WofHCalc.Supports;

namespace WofHCalc.Views
{
    /// <summary>
    /// Логика взаимодействия для SlotBuildsList.xaml
    /// </summary>
    public partial class SlotBuildsList : Window
    {
        public Race race;
        public Town town;
        public byte slot_id;
        public DataWorldConst data;
        public WofHMath WofHFuncs;
        public BuildName? selected_build { get; set; } //для возврата значения
        public List<BuildName>? AllBuilds { get; set; }
        public List<BuildName>? FortBuilds
        {
            get
            {
                List<BuildName>? ans;
                if (AllBuilds == null) ans = null;
                else ans = (List<BuildName>?)AllBuilds.Where(x => data.BuildindsData[(int)x].Type == BuildType.defence 
                                                               || data.BuildindsData[(int)x].Type == BuildType.airdef).ToList();
                if (ans is null || ans.Count == 0)
                {
                    Fort.Visibility = Visibility.Hidden;
                    Fort.Height = 0;
                    return null;
                }
                else return ans;
            }
        }
        public List<BuildName>? ScienceBuilds
        {
            get
            {
                List<BuildName>? ans;
                if (AllBuilds == null) ans = null;
                else ans = (List<BuildName>?)AllBuilds.Where(x => data.BuildindsData[(int)x].Type == BuildType.production
                                                               && data.BuildindsData[(int)x].Productres[0].Res == ResName.science).ToList();
                if (ans is null || ans.Count == 0)
                {
                    Science.Visibility = Visibility.Hidden;
                    Science.Height = 0;
                    return null;
                }
                else return ans;
            }
        }
        public List<BuildName>? ProdBuilds
        {
            get
            {
                List<BuildName>? ans;
                if (AllBuilds == null) ans = null;
                else ans = (List<BuildName>?)AllBuilds.Where(x => (data.BuildindsData[(int)x].Type == BuildType.production
                                                                && data.BuildindsData[(int)x].Productres[0].Res != ResName.science)
                                                                || data.BuildindsData[(int)x].Type == BuildType.prodBoost).ToList();
                if (ans is null || ans.Count == 0)
                {
                    Prod.Visibility = Visibility.Hidden;
                    Prod.Height = 0;
                    return null;
                }
                else return ans;
            }
        }
        public List<BuildName>? STBuilds
        {
            get
            {
                List<BuildName>? ans;
                if (AllBuilds == null) ans = null;
                else
                {
                    var types = new List<BuildType> { BuildType.store, BuildType.tradespeed, BuildType.trade, BuildType.watertradespeed };
                    ans = (List<BuildName>?)AllBuilds.Where(x => types.Any(t => t == data.BuildindsData[(int)x].Type)).ToList();
                }
                if (ans is null || ans.Count == 0)
                {
                    StorageTrade.Visibility = Visibility.Hidden;
                    StorageTrade.Height = 0;
                    return null;
                }
                else return ans;
            }
        }
        public List<BuildName>? CultBuilds
        {
            get
            {
                List<BuildName>? ans;
                if (AllBuilds == null) ans = null;
                else ans = (List<BuildName>?)AllBuilds.Where(x => data.BuildindsData[(int)x].Type == BuildType.culture).ToList();
                if (ans is null || ans.Count == 0)
                {
                    Cult.Visibility = Visibility.Hidden;
                    Cult.Height = 0;
                    return null;
                }
                else return ans;
            }
        }
        public List<BuildName>? GrownBuilds
        {
            get
            {
                List<BuildName>? ans;
                if (AllBuilds == null) ans = null;
                else ans = (List<BuildName>?)AllBuilds.Where(x => data.BuildindsData[(int)x].Type == BuildType.grown).ToList();
                if (ans is null || ans.Count == 0)
                {
                    Grown.Visibility = Visibility.Hidden;
                    Grown.Height = 0;
                    return null;
                }
                else return ans;
            }
        }
        public List<BuildName>? WarBuilds
        {
            get
            {
                List<BuildName>? ans;
                if (AllBuilds == null) ans = null;
                else ans = (List<BuildName>?)AllBuilds.Where(x => data.BuildindsData[(int)x].Type == BuildType.train
                                                               || data.BuildindsData[(int)x].Type == BuildType.waterarmyspeed
                                                               || data.BuildindsData[(int)x].Type == BuildType.airarmyspeed).ToList();
                if (ans is null || ans.Count == 0)
                {
                    War.Visibility = Visibility.Hidden;
                    War.Height = 0;
                    return null;
                }
                else return ans;
            }
        }
        public List<BuildName>? SpecialBuilds
        {
            get
            {
                List<BuildName>? ans;
                if (AllBuilds == null) ans = null;
                else ans = (List<BuildName>?)AllBuilds.Where(x => data.BuildindsData[(int)x].Type == BuildType.fake
                                                               || data.BuildindsData[(int)x].Type == BuildType.embassy
                                                               || data.BuildindsData[(int)x].Type == BuildType.administration
                                                               || data.BuildindsData[(int)x].Type == BuildType.corruption
                                                               || data.BuildindsData[(int)x].Type == BuildType.ecology
                                                               || data.BuildindsData[(int)x].Type == BuildType.hide                                                               
                                                               ).ToList();
                if (ans is null || ans.Count == 0)
                {
                    Special.Visibility = Visibility.Hidden;
                    Special.Height = 0;
                    return null;
                }
                else return ans;
            }
        }


        public SlotBuildsList(Town t, byte s, Race race, DataWorldConst data, WofHMath WofHFuncs)
        {
            this.data = data;
            this.race = race;
            this.WofHFuncs = WofHFuncs;
            town = t; slot_id = s;
            this.DataContext = this;
            AllBuilds = GetAvailable();
            if (AllBuilds is null || AllBuilds.Count == 0) { MessageBox.Show("Нет доступных строений"); }
            InitializeComponent();
        }
        private List<BuildName> GetAvailable() //работает
        {
            List<BuildName> ans = Enum
                .GetValues(typeof(BuildName))
                .Cast<Enum>()
                .Select(x=> (BuildName)x)
                .ToList();
            ans.RemoveAt(ans.Count - 1); //удаление "none". Если не убрать будет ошибка в data.cs
            //теперь фильтрация
            var builds = town.TownBuilds.Select(x => x.Building).ToArray();
            var lvls = town.TownBuilds.Select(x=> x.Level).ToArray();
            var slot = town.TownBuilds[slot_id].Slot;
            for (int i = 0; i < ans.Count; i++)
            {
                if (!WofHFuncs.AvailableCheck(ans[i], race, town.OnHill, town.WaterPlaces, builds, slot))
                    ans.RemoveAt(i--);
            }
            return ans;
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (selected_build != null)
                this.DialogResult = true;
        }
    }
}
