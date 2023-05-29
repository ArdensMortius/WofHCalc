using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WofHCal.Supports;
using WofHCalc.FileManagers;
using WofHCalc.Models;
using WofHCalc.Views;

namespace WofHCalc.Controllers
{
    internal class AccMngrController
    {
        public Account? New_acc { get; set; }
        public string Input1 { get; set; } //name
        public string Input2 { get; set; } //world
        private readonly string saves_path;
        public ObservableCollection<Account> Accounts { get; set; }
        public Account? Selected_acc { get; set; }
        private RelayCommand? add_command;
        public RelayCommand Add
        {
            get
            {
                return add_command ??= new RelayCommand(obj =>
                {                    
                    New_acc = new(Input1, byte.Parse(Input2));
                    string newdata = New_acc.ToJSON();
                    string filepath = $"{saves_path}/{Input2}_{Input1}";
                    if (FileManager.CreateNewAccFile(filepath, newdata)) 
                    {
                        Accounts.Add(New_acc);
                        Selected_acc = Accounts.Last();
                        OnPropertyChanged(nameof(Accounts));
                    }
                    else throw new Exception();                    
                });
            }
        }
        private RelayCommand? delete_command;
        public RelayCommand Delete
        {
            get
            {
                return delete_command ??= new RelayCommand(
                    o1 =>
                    {
                        ConfirmAction c = new ConfirmAction($"Вы точно хотите удалить данные об аккаунте {Selected_acc!.Name} мира {Selected_acc!.World}?", 5);
                        if (c.ShowDialog() == true) 
                        {
                            string path = $"{saves_path}/{Selected_acc!.World}_{Selected_acc!.Name}";
                            if (FileManager.DeleteAccFile(path))
                            {
                                Accounts.Remove(Selected_acc);
                                OnPropertyChanged(nameof(Accounts));
                            }
                            else throw new FileNotFoundException();
                        }                        
                    },
                    o2 => { return (Selected_acc != null); });
            }
        }
        private RelayCommand? open_command;
        public RelayCommand Open
        {
            get
            {
                return open_command ??= new RelayCommand(
                    o1 =>
                    {
                        //ничего не делает и вроде не должна
                        //сама кнопка закрывает окно и возвращает ответ в основное окно
                    },
                    o2 => { return (Selected_acc != null); });
            }
        }
        internal AccMngrController()
        {
            Input1 = "";
            Input2 = "";
            saves_path = ConfigurationManager.AppSettings["savefilespath"]!;
            Accounts = new ObservableCollection<Account>();
            //проверяем наличие папки сохранений
            if (!Directory.Exists(saves_path))
                Directory.CreateDirectory(saves_path);
            else
            {
                //загружаем список не битых файлов
                //потом надо будет поменять это                       
                Directory
                    .GetFiles(saves_path)
                    .ToList()
                    .ForEach(path => {                        
                        //string path = p.Replace("\\", "/"); //просто так надо
                        if (FileManager.CheckAccFile(path))
                        {
                            string data = FileManager.ReadAccFile(path);
                            Account acc = System.Text.Json.JsonSerializer.Deserialize<Account>(data)!;
                            Accounts.Add(acc);
                        }
                    });
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //MessageBox.Show("here");
        }

    }
}
