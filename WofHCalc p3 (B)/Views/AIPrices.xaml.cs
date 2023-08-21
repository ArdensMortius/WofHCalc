using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
using WofHCalc.ExtendedModel;
using WofHCalc.MathFuncs;
using WofHCalc.Models;
using WofHCalc.Supports;
using WofHCalc.Supports.ForEnumBinds;

namespace WofHCalc.Views
{
    /// <summary>
    /// Interaction logic for AIPrices.xaml
    /// </summary>
    public partial class AIPrices : Window
    {
        public List<object> Info { get; set; }
        public AIPrices(ExtendedAccount acc)
        {
            WofHMath m = acc.WofHFuncs;
            var t = Enum.GetValues(typeof(AreaImprovementName)).Cast<AreaImprovementName>().Select(e => new KeyValuePair<AreaImprovementName, string>(e, e.Description())).SkipLast(1);
            Info = new();
            foreach (var el in t)
            {
                Info.Add(
                    new {
                        Name = el.Value,
                        Cost = new double[] {
                            m.AreaImprovementPrice(el.Key,1,acc),
                            m.AreaImprovementPrice(el.Key,2,acc),
                            m.AreaImprovementPrice(el.Key,3,acc)}
                        }
                    );
            }
            this.DataContext = Info;
            InitializeComponent();
        }
    }
}
