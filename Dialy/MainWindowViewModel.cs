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
            var result = new DateTime();

            //var test = new SortedDictionary<DateTime, string>();
            //foreach (var diary in AllDiaries)
            //{//>>押したとして。
            //    if (diary.Key <= indicated) continue;
            //    test.Add(diary.Key, diary.Value);
            //}
            //var test2 = test.First().Key;

            if (operation == "<<")
            {
                var next = AllDiaries.Where(x => x.Key < indicated);
                result = next.Last().Key;
            }
            else
            {
                var last = AllDiaries.Where(x => x.Key > indicated);
                result = last.First().Key; ;
            }

            return result;
        }

        public void Zoom(string btn)
        {
            FontSize = btn == "+" ? FontSize + 3 : FontSize - 3;
            IndicateSize = FontSize.ToString();
        }
    }
}
