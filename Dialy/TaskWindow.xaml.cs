﻿using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Dialy
{
    /// <summary>
    /// TaskWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TaskWindow : MetroWindow
    {
        public TaskWindow(string folderPath,int fontSize)
        {
            InitializeComponent();
            _twvm = new TaskWindowViewModel(folderPath, fontSize);
            this.DataContext = _twvm;

            if (Settings.Default.TaskWindowStat == null) return;
            this.Top = Settings.Default.TaskWindowStat.Top;
            this.Left = Settings.Default.TaskWindowStat.Left;
            this.Width = Settings.Default.TaskWindowStat.Width;
            this.Height = Settings.Default.TaskWindowStat.Height;
        }

        TaskWindowViewModel _twvm;
        private void ForcusTxt(object sender, EventArgs e)
        {
            TaskTxt.Focus();
        }

        private void FontZoom(object sender, RoutedEventArgs e)
        {
            var btn = ((Button)sender).Content.ToString();
            _twvm.IndicateSize = btn == "+" ? _twvm.IndicateSize + 3 : _twvm.IndicateSize - 3;
        }

        public void TaskTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            _twvm.Task.SaveTaskFile();
            //var test = ((TextBox)sender).Text;
            //FileManager.SaveTaskFile(_twvm.TaskTxt, test);
        }
    }
}
