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

        public Dictionary<DateTime, string> AllDiaries;

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

        //public string NextRecord(DateTime indicated,string operation)
        //{
        //    foreach(var key in AllDiaries.Keys)
        //    {
        //        if (DateTime.Parse(key) > indicated) continue;

        //    }


        //    var next = AllDiaries.Where(x => DateTime.Parse(x.Key) > indicated);
        //    var last = AllDiaries.Where(x => DateTime.Parse(x.Key) < indicated);

        //    return "";
        //}

        public void Zoom(string btn)
        {
            FontSize = btn == "+" ? FontSize + 3 : FontSize - 3;
            IndicateSize = FontSize.ToString();
        }
    }
}
