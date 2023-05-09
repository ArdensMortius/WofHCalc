using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Supports;

namespace WofHCalc.Models.jsonTemplates
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public class AreaImprovement
    {
        public level_data[] levels { get; set; }
        public byte main;

    }
    public class level_data
    {
        public string cost { get; set; }
        public float effect { get; set; }
        public int science { get; set; }
        public int workers { get; set; }

        public int[] GetCost()
        {
            var ns = cost.Split('^', StringSplitOptions.RemoveEmptyEntries);
            var ans = new int[23];
            foreach (string ss in ns)
            {
                int x = convert(ss);
                switch (ss[0])
                {
                    case 'c': ans[(int)ResName.food] = x; break;
                    case 'd': ans[(int)ResName.wood] = x; break;
                    case 'e': ans[(int)ResName.iron] = x; break;
                    case 'f': ans[(int)ResName.fuel] = x; break;
                    case 'g': ans[(int)ResName.stone] = x; break;
                    case 'r': ans[(int)ResName.wine] = x; break;
                    case 's': ans[(int)ResName.jewelry] = x; break;
                    case 'p': ans[(int)ResName.fish] = x; break;
                    default: break;
                }
            }
            return ans;
        }
        private int convert(string s)
        {
            string ts = s.Remove(0, 1);
            float tr = float.Parse(ts);
            return (int)tr;
        }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
