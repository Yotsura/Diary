﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Dialy
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        MainWindowViewModel _mwvm;
        public MainWindow()
        {
            InitializeComponent();
            _mwvm = new MainWindowViewModel();
            this.DataContext = _mwvm;

            if (Settings.Default.MainWindowStat == null) return;
            this.Top = Settings.Default.MainWindowStat.Top;
            this.Left = Settings.Default.MainWindowStat.Left;
            this.Width = Settings.Default.MainWindowStat.Width;
            this.Height = Settings.Default.MainWindowStat.Height;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DatePick.Text = DateTime.Today.ToString();
            OpenTaskWindow(sender, e);
            DiaryTxt.Focus();
        }

        private void FontZoom(object sender, RoutedEventArgs e)
        {
            var btn = ((Button)sender).Content.ToString();
            _mwvm.IndicateSize = btn == "+" ? _mwvm.IndicateSize + 3 : _mwvm.IndicateSize - 3;
        }

        private void SaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            _mwvm.AllDiaries[DatePick.SelectedDate.Value] = DiaryTxt.Text;
            FileManager.SaveFile(_mwvm.FolderPath, DatePick.SelectedDate.Value, DiaryTxt.Text);
            MessageLabel.Visibility = Visibility.Collapsed;
        }

        private void DatePick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = DatePick.SelectedDate.Value;
            Today.IsEnabled = date.Date != DateTime.Today;
            DiaryTxt.Clear();
            if (!_mwvm.AllDiaries.ContainsKey(date)) return;
            DiaryTxt.Text = _mwvm.AllDiaries[date];
            DiaryTxt.IsUndoEnabled = false;
            DiaryTxt.IsUndoEnabled = true;
            DiaryTxt.Focus();
        }

        private async void DateChangeButton(object sender, RoutedEventArgs e)
        {
            if (MessageLabel.Visibility == Visibility.Visible)
            {
                var metroDialogSettings = new MetroDialogSettings() { AffirmativeButtonText = "Yes", NegativeButtonText = "No" };
                var select = await this.ShowMessageAsync("エラー", "未保存の変更があります。変更を破棄しますか？",
                    MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
                if (select == MessageDialogResult.Negative) return;
            }
            var date = DatePick.SelectedDate.Value;
            var day = new DateTime();
            switch (((Button)sender).Content)
            {
                case "<":
                    day = date.AddDays(-1);
                    break;
                case ">":
                    day = date.AddDays(1);
                    break;
                case "T":
                    day = DateTime.Today;
                    break;
                case "<<":
                    day = _mwvm.NextRecord(date, "<<");
                    break;
                case ">>":
                    day = _mwvm.NextRecord(date, ">>");
                    break;
            }
            DatePick.Text = day.ToString();
        }

        SearchWindow _searchWindow;
        private void OpenSearchWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (_searchWindow != null)
            {
                _searchWindow.WindowState = WindowState.Normal;
                _searchWindow.Activate();
                return;
            }
            _searchWindow = new SearchWindow(_mwvm.AllDiaries,Settings.Default.SearchFontSize);
            _searchWindow.HitListBox.MouseDoubleClick += CheckMouseButton;
            _searchWindow.HitListBox.KeyDown += CheckKey;
            _searchWindow.Closed += SearchWindow_Closed;
            _searchWindow.Show();
        }

        private void CheckMouseButton(object sender ,MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;
            ReflectSearch(sender, e);
        }

        private void CheckKey(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            ReflectSearch(sender, e);
        }

        private async void ReflectSearch<T>(object sender, T e)
        {
            var s = (ListBox)sender;
            if (s.ItemsSource == null || s.SelectedIndex == -1) return;
            var result = _searchWindow._swvm.IndicateList[s.SelectedIndex];
            if (DatePick.SelectedDate == result) return;
            if (MessageLabel.Visibility == Visibility.Visible)
            {
                var metroDialogSettings = new MetroDialogSettings() { AffirmativeButtonText = "Yes", NegativeButtonText = "No" };
                var select = await this.ShowMessageAsync("エラー", "未保存の変更があります。変更を破棄しますか？",
                    MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
                if (select == MessageDialogResult.Negative) return;
            }
            DatePick.SelectedDate = result;
        }

        private void SearchWindow_Closed(object sender, EventArgs e)
        {
            Settings.Default.SearchFontSize = _searchWindow._swvm.IndicateSize;
            Settings.Default.SearchWindowStat = new WindowStat { Height = _searchWindow.Height, Width = _searchWindow.Width, Left = _searchWindow.Left, Top = _searchWindow.Top };
            Settings.Default.Save();
            _searchWindow = null;
        }

        private void DialyTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageLabel.Visibility = _mwvm.AllDiaries.ContainsKey(DatePick.SelectedDate.Value) ?
                _mwvm.AllDiaries[DatePick.SelectedDate.Value] == DiaryTxt.Text ? Visibility.Collapsed : Visibility.Visible :
                String.IsNullOrEmpty(DiaryTxt.Text) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(MessageLabel.Visibility == Visibility.Visible 
                && MessageBox.Show("未保存の変更があります。変更を破棄しますか？"
                ,"警告",MessageBoxButton.OKCancel,MessageBoxImage.Warning)==MessageBoxResult.Cancel)
            {
                e.Cancel=true;
                return;
            }
            Settings.Default.MainWindowStat = new WindowStat { Height = this.Height, Width = this.Width, Left = this.Left, Top = this.Top };
            Settings.Default.FontSize = _mwvm.IndicateSize;
            Settings.Default.Save();
        }

        private async void DelRecord(object sender, RoutedEventArgs e)
        {
            var day = DatePick.SelectedDate.Value;
            if (!_mwvm.AllDiaries.ContainsKey(day)) return;
            var metroDialogSettings = new MetroDialogSettings() { AffirmativeButtonText = "Yes", NegativeButtonText = "No" };
            var select = await this.ShowMessageAsync("確認", "本当に削除しますか？",
                MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
            if (select == MessageDialogResult.Negative) return;
            _mwvm.AllDiaries.Remove(day);
            FileManager.DeleteFile(_mwvm.FolderPath, day);
            DiaryTxt.Text = string.Empty;
        }

        private void ReloadRecords(object sender, RoutedEventArgs e)
        {
            _mwvm.AllDiaries = FileManager.GetAllDiaries(_mwvm.FolderPath);
            var date = DatePick.SelectedDate.Value;
            DiaryTxt.Text = _mwvm.AllDiaries.ContainsKey(date) ? _mwvm.AllDiaries[date] : string.Empty;
        }

        private async void ShowMessageDialog(string title, string message)
        {
            await this.ShowMessageAsync(title, message);
        }

        private void CreateBackUp(object sender, RoutedEventArgs e)
        {
            if (FileManager.CreateBackUp(_mwvm.AllDiaries)) ShowMessageDialog("確認", "バックアップを作成しました。");
        }

        private void OpenBackUp(object sender, RoutedEventArgs e)
        {
            var filename = FileManager.FileDialog();
            if (filename == string.Empty) return;
            if (FileManager.OpenBackUp(filename)) ShowMessageDialog("確認", "バックアップを解凍しました。");
        }

        private void TopMostCheck_CheckChanged(object sender, RoutedEventArgs e)
        {
            this.Topmost = TopMostCheck.IsChecked == true;
        }

        TaskWindow _taskWindow;
        private void OpenTaskWindow(object sender, RoutedEventArgs e)
        {
            if (_taskWindow != null)
            {
                _taskWindow.WindowState = WindowState.Normal;
                _taskWindow.Activate();
                return;
            }
            _taskWindow = new TaskWindow(FileManager.OpenTaskFile(_mwvm.FolderPath), Settings.Default.TaskFontSize);
            _taskWindow.TaskTxt.TextChanged += TaskTxt_TextChanged;
            _taskWindow.Closed += TaskWindow_Closed;
            _taskWindow.Show();
        }

        public void TaskTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var test = ((TextBox)sender).Text;
            FileManager.SaveFile(_mwvm.FolderPath, test);
        }

        private void TaskWindow_Closed(object sender, EventArgs e)
        {
            Int32.TryParse(_taskWindow.FontSize.Content.ToString(), out var size);
            Settings.Default.TaskWindowStat = new WindowStat { Height = _taskWindow.Height, Width = _taskWindow.Width, Left = _taskWindow.Left, Top = _taskWindow.Top };
            Settings.Default.TaskFontSize = size;
            Settings.Default.Save();
            _taskWindow = null;
        }

        private async void ShowVerInfo(object sender, RoutedEventArgs e)
        {
            var verInfo = App.ResourceAssembly.GetName().Version;
            var ver = $"{verInfo.Major}.{verInfo.Minor}.{verInfo.Build}";
            await this.ShowMessageAsync("バージョン情報", $"ver{ver}");
        }

        private void InheritLineHead(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            var calet = ((TextBox)sender).SelectionStart;
            var txt = ((TextBox)sender).Text;
            var foreTxt = txt.Substring(0, calet);
            if (!foreTxt.EndsWith("\r\n")) return;

            var lines = foreTxt.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();
            var lastLine = lines[lines.Count - 2];
            var exTxt = "";

            foreach (var t in lastLine.ToCharArray())
            {
                if (Space.Contains(t)) { exTxt += t; continue; }
                if (!Head.Contains(t)) break;
                exTxt += t;
                break;
            }

            DiaryTxt.Text = foreTxt + exTxt + txt.Substring(calet);
            ((TextBox)sender).SelectionStart = calet + exTxt.Length;
        }
        private static List<char> Space = new List<char> { ' ', '　', '\t', };
        private static List<char> Head = new List<char> { '〇', '・', '#', '＞', '>' };
    }
}
