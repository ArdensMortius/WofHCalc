using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace WofHCalc.Supports.jsontemplates
{
    public class Resource
    {
        public float consumption { get; set; } //сколько жрут
        public float effect { get; set; } //сколько ээфект
        public ResProdType prodtype { get; set; } //как производится
        public int type { get; set; } //категория
    }
}
