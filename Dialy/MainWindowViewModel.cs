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
        private DateTime _selectedDate;
        private string _indicatedDiary;
        private string _currentTxt;
        private bool _isChanged;

        public int IndicateSize
        {
            get => _indicateSize;
            set
            {
                _indicateSize = value < 1 ? 1 : value;
                OnPropertyChanged(nameof(IndicateSize));
            }
        }
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;

                var date = SelectedDate;
                var txt = AllDiaries.ContainsKey(date) ? AllDiaries[date] : "";
                IndicatedDiary = txt;
                _currentTxt = txt;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        public string IndicatedDiary
        {
            get => _indicatedDiary;
            set
            {
                _indicatedDiary = value;
                OnPropertyChanged(nameof(IndicatedDiary));
            }
        }
        public string CurrentTxt
        {
            get => _currentTxt;
            set
            {
                _currentTxt = value;
                IsChanged = CurrentTxt != IndicatedDiary;
            }
        }

        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                _isChanged = value;
                OnPropertyChanged(nameof(IsChanged));
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
            FolderPath = String.IsNullOrEmpty(Settings.Default.FolderPath) ?
                System.IO.Directory.GetCurrentDirectory() + "\\Logs" : Settings.Default.FolderPath;
            Settings.Default.FolderPath = FolderPath;
            if(Settings.Default.HeadSpaces==null)Settings.Default.HeadSpaces= new List<char> { ' ', '　', '\t', };
            if (Settings.Default.HeadMarks == null) Settings.Default.HeadMarks = new List<char> { '〇', '・', '#', '＞', '>' };
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

        public void SaveInvoke(DateTime date,string txt)
        {
            AllDiaries[date] = txt;
            FileManager.SaveFile(FolderPath, SelectedDate, txt);
            IndicatedDiary = txt;
            CurrentTxt = txt;
        }
    }
}
