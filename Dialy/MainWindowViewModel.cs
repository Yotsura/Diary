﻿using System;
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
        public int IndicateSize
        {
            get => _indicateSize;
            set
            {
                _indicateSize = value;
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
                IndicatedDiary = AllDiaries.ContainsKey(date) ? AllDiaries[date] : "";
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
    }
}
