using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        public string TaskTxt = "test";

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel()
        {
            IndicateSize = Settings.Default.FontSize;
            FolderPath = String.IsNullOrEmpty(Settings.Default.FilePath) ?
                System.IO.Directory.GetCurrentDirectory() + "\\Logs" : Settings.Default.FilePath;
            AllDiaries = FileManager.GetAllDiaries(FolderPath);
        }

        public DateTime NextRecord(DateTime indicated, string operation)
        {
            var next = operation == "<<" ? AllDiaries.Where(x => x.Key < indicated) : AllDiaries.Where(x => x.Key > indicated);
            if (!next.Any()) return indicated;
            return operation == "<<" ? next.Last().Key : next.First().Key;
        }
    }
}
