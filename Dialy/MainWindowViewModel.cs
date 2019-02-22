using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace Dialy
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private int _indicateSize;
        public int IndicateSize
        {
            get => _indicateSize;
            set
            {
                _indicateSize = value;
                OnPropertyChanged(nameof(IndicateSize));
            }
        }

        public string FolderPath;

        public SortedDictionary<DateTime, string> AllDiaries;

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            if (!SettingExsistCheck()) Settings.Default.Upgrade();
            IndicateSize = Settings.Default.FontSize;
            FolderPath = String.IsNullOrEmpty(Settings.Default.FilePath) ?
                System.IO.Directory.GetCurrentDirectory() + "\\Logs" : Settings.Default.FilePath;
            Settings.Default.FilePath = FolderPath;
            Settings.Default.Save();
            AllDiaries = FileManager.GetAllDiaries(FolderPath);
        }

        private bool SettingExsistCheck()
        {
            return File.Exists(ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath);
        }

        public DateTime NextRecord(DateTime indicated, string operation)
        {
            var next = operation == "<<" ? AllDiaries.Where(x => x.Key < indicated) : AllDiaries.Where(x => x.Key > indicated);
            if (!next.Any()) return indicated;
            return operation == "<<" ? next.Last().Key : next.First().Key;
        }
    }
}
