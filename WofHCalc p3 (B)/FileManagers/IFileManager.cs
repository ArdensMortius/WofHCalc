using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WofHCalc.FileManagers
{
    public interface IFileManager
    {
        //работает с файлами сохранений
        //принимает и возвращает данные в виде строки (содержимое json)
        public static abstract string ReadAccFile(string path);
        public static abstract bool UpdateAccFile(string path, string newdata);
        public static abstract bool CreateNewAccFile(string path, string newdata);
        public static abstract bool DeleteAccFile(string path);
        public static abstract bool CheckAccFile(string path);
        //что-то надо для бэкапов придумать
    }
}
