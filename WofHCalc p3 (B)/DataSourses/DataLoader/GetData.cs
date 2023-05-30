using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WofHCalc.DataSourses.DataLoader
{
    public static class GetData
    {
        public static string GetConstjson(int world)
        {
            string url = world > 0 ? $"https://ru{world}.waysofhistory.com/gen/js/ru/const.js" : $"https://int{-world}.waysofhistory.com/gen/js/ru/const.js";            
            string constjson = Task.Run(()=> GetHtmlPageAsync(url)).GetAwaiter().GetResult();
            constjson = constjson.Substring(10, constjson.Length - 11); //надо откусить начало и конец, чтоб оно парсилось. Вряд ли формат файла данных изменится, так что сойдёт
            return constjson;
        }
        private static async Task<string> GetHtmlPageAsync(string url) //получаем колбасу const.js
        {
            try
            {
                HttpClient client = new HttpClient();
                using Stream stream = await client.GetStreamAsync(url);
                StreamReader reader = new StreamReader(stream);
                string content = await reader.ReadToEndAsync();     // считываем поток в строку
                return content;
            }
#pragma warning disable CS8603 // Possible null reference return.
            catch { return null; }
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
