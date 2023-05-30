using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WofHCalc.DataSourses.DataLoader
{
    internal static class GetData
    {
        public static async Task<string> GetConstjs(int world)
        {
            string url = world > 0 ? $"https://ru{world}.waysofhistory.com/gen/js/ru/const.js" : $"https://int{-world}.waysofhistory.com/gen/js/ru/const.js";
            return await GetHtmlPageAsync(url);
        }
        private static async Task<string> GetHtmlPageAsync(string url) //получаел колбасу const.js
        {
            try
            {
                HttpClient client = new HttpClient();
                using Stream stream = await client.GetStreamAsync(url);
                StreamReader reader = new StreamReader(stream);
                string content = await reader.ReadToEndAsync();     // считываем поток в строку
                return content;
            }
            catch { return null; }
        }
    }
}
