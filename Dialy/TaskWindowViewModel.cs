﻿using System;
using System.ComponentModel;

namespace Dialy
{
    public class TaskWindowViewModel : INotifyPropertyChanged
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

        private TaskRecord _task;
        public TaskRecord Task
        {
            get => _task;
            set
            {
                _task = value;
                OnPropertyChanged(nameof(Task));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TaskWindowViewModel(int fontSize,TaskRecord taskdata)
        {
            IndicateSize = fontSize;
            Task = taskdata;
            //string pass = "ckscks3485";
            //Task = new TaskRecord(folderPath, pass);
            //try
            //{
            //    Task.OpenTaskFile();
            //}
            //catch
            //{
            //    var oldfile = Task.Filepath.Replace("taskTxt.log", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}taskTxt.log");
            //    System.IO.File.Copy(Task.Filepath, oldfile);
            //    Task.Txt = $"データファイルの展開に失敗。\r\n旧データを退避しました。\r\n＜ファイルパス＞\r\n{oldfile}";
            //}
        }
    }
}
