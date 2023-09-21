using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WofHCalc.Models;
using WofHCalc.ExtendedModel;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace WofHCalc.FileManagers
{
    internal class FileManager : IFileManager
    {        
        public static bool CheckAccFile(string path)
        {
            if (!File.Exists(Path.GetFullPath(path))) throw new FileNotFoundException();
            string data = File.ReadAllText(path);
            try
            {
                JsonConvert.DeserializeObject<Account>(data);
                //System.Text.Json.JsonSerializer.Deserialize<Account>(data);
                return true;
            }
            catch { return false;}
        }

        public static bool CreateNewAccFile(string path, string? newdata = null)
        {            
            try
            {
                using (StreamWriter writer = new(File.Open(path, FileMode.CreateNew)))
                {
                    if (newdata is not null)                
                        writer.Write(newdata);
                    return true;
                }
            }
            catch { return false; }
        }

        public static bool DeleteAccFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    return true;
                }
                catch{ return false; }
            }            
            else return false;
        }

        public static string ReadAccFile(string path)
        {
            if (File.Exists(path))             
                return File.ReadAllText(path);                                    
            else 
                throw new FileNotFoundException(); 
        }

        public static bool UpdateAccFile(string path, string newdata)
        {
            try
            {
                using StreamWriter writer = new(File.Open(path, FileMode.Create));
                writer.Write(newdata);
                return true;
            }
            catch { return false; }
        }
    }
}
