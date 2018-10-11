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
        public int FontSize;
        private string _indicateSize;
        public string IndicateSize
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
            FontSize = Settings.Default.FontSize;
            IndicateSize = FontSize.ToString();
            FolderPath = String.IsNullOrEmpty(Settings.Default.FilePath) ?
                System.IO.Directory.GetCurrentDirectory()+"\\Logs" : Settings.Default.FilePath;
            AllDiaries = FileManager.GetAllDialy(FolderPath);
        }

        public DateTime NextRecord(DateTime indicated, string operation)
        {
            var next= operation == "<<"? AllDiaries.Where(x => x.Key < indicated): AllDiaries.Where(x => x.Key > indicated);
            if (!next.Any()) return indicated;
            return operation == "<<" ? next.Last().Key: next.First().Key;
        }

        public void Zoom(string btn)
        {
            FontSize = btn == "+" ? FontSize + 3 : FontSize - 3;
            IndicateSize = FontSize.ToString();
        }
    }
}
