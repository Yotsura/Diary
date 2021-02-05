using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Dialy.Funcs;

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
            TaskAreaCol.Width = Settings.Default.TaskAreaWidth;
            TaskArea.Text= Settings.Default.TaskAreaValue;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //DatePick.Text = DateTime.Today.ToString();
            _mwvm.SelectedDate = DateTime.Today;
            //OpenTaskWindow(sender, e);
            DiaryTxt.Focus();
        }

        private void FontZoom(object sender, RoutedEventArgs e)
        {
            var btn = ((Button)sender).Content.ToString();
            _mwvm.IndicateSize = btn == "+" ? _mwvm.IndicateSize + 3 : _mwvm.IndicateSize - 3;
        }

        private void SaveInvoke(object sender, ExecutedRoutedEventArgs e)
        {
            var txt = DiaryTxt.Text;
            _mwvm.AllDiaries[DatePick.SelectedDate.Value] = txt;
            //_mwvm.AllDiaries[DatePick.SelectedDate.Value] = _mwvm.IndicatedDiary;
            FileManager.SaveFile(_mwvm.FolderPath, _mwvm.SelectedDate, txt);
            MessageLabel.Visibility = Visibility.Collapsed;
        }

        private void DatePick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var date = _mwvm.SelectedDate;
            Today.IsEnabled = date.Date != DateTime.Today;
            //DiaryTxt.Clear();
            //if (_mwvm.AllDiaries.ContainsKey(date)) DiaryTxt.Text = _mwvm.AllDiaries[date];
            //DiaryTxt.IsUndoEnabled = false;
            //DiaryTxt.IsUndoEnabled = true;
            DiaryTxt.Focus();
        }

        void MoveNext(object sender, RoutedEventArgs e)
        {
            DateChangeButton(NextRecord, null);
        }
        void MovePrev(object sender, RoutedEventArgs e)
        {
            DateChangeButton(LastRecord, null);
        }
        void MoveNextDay(object sender, RoutedEventArgs e)
        {
            DateChangeButton(NextDay, null);
        }
        void MovePrevDay(object sender, RoutedEventArgs e)
        {
            DateChangeButton(LastDay, null);
        }
        void IndicateToday(object sender, RoutedEventArgs e)
        {
            DateChangeButton(Today, null);
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
            var date = _mwvm.SelectedDate;
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
            _mwvm.SelectedDate = day;
        }

        private void DialyTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            //var test = ((TextBox)sender).Text == _mwvm.IndicatedDiary;
            MessageLabel.Visibility = ((TextBox)sender).Text == _mwvm.IndicatedDiary ? Visibility.Collapsed : Visibility.Visible;
            //MessageLabel.Visibility = _mwvm.AllDiaries.ContainsKey(_mwvm.SelectedDate) ?
            //    _mwvm.AllDiaries[_mwvm.SelectedDate] == _mwvm.IndicatedDiary ? Visibility.Collapsed : Visibility.Visible :
            //    String.IsNullOrEmpty(_mwvm.IndicatedDiary) ? Visibility.Collapsed : Visibility.Visible;
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
            Settings.Default.TaskAreaWidth = TaskAreaCol.Width;
            Settings.Default.TaskAreaValue = TaskArea.Text;
            Settings.Default.Save();
        }

        private async void DelRecord(object sender, RoutedEventArgs e)
        {
            var day = _mwvm.SelectedDate;
            if (!_mwvm.AllDiaries.ContainsKey(day)) return;
            var metroDialogSettings = new MetroDialogSettings() { AffirmativeButtonText = "Yes", NegativeButtonText = "No" };
            var select = await this.ShowMessageAsync("確認", "本当に削除しますか？",
                MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
            if (select == MessageDialogResult.Negative) return;
            _mwvm.AllDiaries.Remove(day);
            FileManager.DeleteFile(_mwvm.FolderPath, day);
            _mwvm.IndicatedDiary = string.Empty;
        }

        private void ReloadRecords(object sender, RoutedEventArgs e)
        {
            _mwvm.AllDiaries = FileManager.GetAllDiaries(_mwvm.FolderPath);
            var date = _mwvm.SelectedDate;
            _mwvm.IndicatedDiary = _mwvm.AllDiaries.ContainsKey(date) ? _mwvm.AllDiaries[date] : string.Empty;
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

        private async void ShowVerInfo(object sender, RoutedEventArgs e)
        {
            var verInfo = App.ResourceAssembly.GetName().Version;
            var ver = $"{verInfo.Major}.{verInfo.Minor}.{verInfo.Build}.{verInfo.Revision}";
            await this.ShowMessageAsync("バージョン情報", $"ver{ver}");
        }

        private void InheritLineHead(object sender, KeyEventArgs e)
        {
            TxtFuncs.InheritLineHead(sender, e, Settings.Default.HeadSpaces, Settings.Default.HeadMarks);
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
            _searchWindow = new SearchWindow(_mwvm.AllDiaries, Settings.Default.SearchFontSize);
            _searchWindow.HitListBox.MouseDoubleClick += CheckMouseButton;
            _searchWindow.HitListBox.KeyDown += CheckKey;
            _searchWindow.Closed += SearchWindow_Closed;
            _searchWindow.Show();
        }
        private void CheckMouseButton(object sender, MouseButtonEventArgs e)
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

        private CustomDialog _customDialog;
        private LoginControl _loginCtrl;

        private void ButtonLoginOnClick(object sender, RoutedEventArgs e)
        {
            var pass = _loginCtrl.PasswordBox1.Password;
            if (string.IsNullOrEmpty(pass)) return;
            this.HideMetroDialogAsync(_customDialog);
            OpenTaskWindow(pass);
        }
        private void ButtonCancelOnClick(object sender, RoutedEventArgs e)
        {
            this.HideMetroDialogAsync(_customDialog);
        }

        private void FocusPass(object sender, EventArgs e)
        {
            _loginCtrl.PasswordBox1.Focus();
        }

        TaskWindow _taskWindow;
        private async void CheckTaskPass(object sender, RoutedEventArgs e)
        {
            if (_taskWindow != null)
            {
                _taskWindow.WindowState = WindowState.Normal;
                _taskWindow.Activate();
                return;
            }

            MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;
            _loginCtrl = new LoginControl();
            _loginCtrl.ButtonCancel.Click += ButtonCancelOnClick;
            _loginCtrl.ButtonLogin.Click += ButtonLoginOnClick;
            _loginCtrl.Loaded += FocusPass;
            _customDialog = new CustomDialog
            {
                Title="PASS",
                Content = _loginCtrl
            };
            await this.ShowMetroDialogAsync(_customDialog);
        }

        private async void OpenTaskWindow(string pass)
        {
            //pass = "ckscks3485";
            var taskdata = new TaskRecord(_mwvm.FolderPath, pass);
            //暗号の鍵を復号できるか確認
            if (!taskdata.encrypt.CheckKey())
            {
                var metroDialogSettings = new MetroDialogSettings() { AffirmativeButtonText = "Yes", NegativeButtonText = "No" };
                var select = await this.ShowMessageAsync("エラー", "鍵の復号に失敗しました。\r\n鍵を更新する必要があります。" +
                    "\r\n旧データは開けなくなりますが構いませんか？",
                    MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
                if (select == MessageDialogResult.Negative) return;
                taskdata.encrypt.UpdateKey();
            }
            try
            {
                taskdata.OpenTaskFile();
            }
            catch
            {
                var oldfile = taskdata.Filepath.Replace("taskTxt.log", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}taskTxt.log");
                System.IO.File.Copy(taskdata.Filepath, oldfile);
                taskdata.Txt = $"データファイルの展開に失敗。\r\n旧データを退避しました。\r\n＜ファイルパス＞\r\n{oldfile}";
            }

            _taskWindow = new TaskWindow(Settings.Default.TaskFontSize, taskdata);
            _taskWindow.Closed += TaskWindow_Closed;
            _taskWindow.Show();
        }

        private void TaskWindow_Closed(object sender, EventArgs e)
        {
            Int32.TryParse(_taskWindow.FontSize.Content.ToString(), out var size);
            Settings.Default.TaskWindowStat = new WindowStat { Height = _taskWindow.Height, Width = _taskWindow.Width, Left = _taskWindow.Left, Top = _taskWindow.Top };
            Settings.Default.TaskFontSize = size;
            Settings.Default.Save();
            _taskWindow = null;
        }

        SettingWindow _settingWindow;
        private void OpenSettingWindow(object sender, RoutedEventArgs e)
        {
            if (_settingWindow != null)
            {
                _settingWindow.WindowState = WindowState.Normal;
                _settingWindow.Activate();
                return;
            }
            _settingWindow = new SettingWindow();
            _settingWindow.HeadSpace.Text = String.Join("", Settings.Default.HeadSpaces);
            _settingWindow.HeadMark.Text = String.Join("", Settings.Default.HeadMarks);
            _settingWindow.SerchLogLimit.Text = Settings.Default.SearchLogLimit.ToString();
            _settingWindow.SaveSettingBtn.Click += SaveSetting;
            _settingWindow.Closed += SettingWindow_Closed;
            _settingWindow.CancelChangeBtn.Click += CancelChange;
            _settingWindow.ShowDialog();
        }

        private void SettingWindow_Closed(object sender, EventArgs e)
        {
            _settingWindow = null;
        }

        private void SaveSetting(object sender, RoutedEventArgs e)
        {
            Settings.Default.HeadSpaces = _settingWindow.HeadSpace.Text.ToCharArray().ToList();
            Settings.Default.HeadMarks = _settingWindow.HeadMark.Text.ToCharArray().ToList();
            var limit = int.Parse(_settingWindow.SerchLogLimit.Text);
            Settings.Default.SearchLogLimit = limit;
            if (Settings.Default.SearchLog.Count() > limit)
            {
                Settings.Default.SearchLog.RemoveFirst(Settings.Default.SearchLog.Count() - limit);
            }
            Settings.Default.Save();
            _settingWindow.Close();
        }

        private void CancelChange(object sender, RoutedEventArgs e)
        {
            _settingWindow.HeadSpace.Text = String.Join("", Settings.Default.HeadSpaces);
            _settingWindow.HeadMark.Text = String.Join("", Settings.Default.HeadMarks);
        }

        ReplaceWindow _replaceWindow;
        private void OpenReplaceWindow(object sender, RoutedEventArgs e)
        {
            var selectedTxt = DiaryTxt.SelectedText;
            if (_replaceWindow != null)
            {
                SetOrigTxt(selectedTxt);
                _replaceWindow.WindowState = WindowState.Normal;
                _replaceWindow.Activate();
                _replaceWindow.OrigTxt.SelectAll();
                _replaceWindow.OrigTxt.Focus();
                return;
            }
            else
            {
                _replaceWindow = new ReplaceWindow();
                SetOrigTxt(selectedTxt);
                _replaceWindow.Closed += ReplaceWindow_Closed;
                _replaceWindow.ReplaceBtn.Click += ReplaceTxt;
                _replaceWindow.ReplaceAllBtn.Click += ReplaceAllTxt;
                _replaceWindow.Show();
                _replaceWindow.OrigTxt.SelectAll();
                _replaceWindow.OrigTxt.Focus();
            }
        }

        private void SetOrigTxt(string selectedTxt)
        {
            if (string.IsNullOrEmpty(selectedTxt))
                return;
            _replaceWindow.OrigTxt.Text = selectedTxt;
        }

        private void ReplaceWindow_Closed(object sender, EventArgs e)
        {
            _replaceWindow = null;
            Activate();
        }
        private void ReplaceTxt(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_replaceWindow.OrigTxt.Text) || string.IsNullOrEmpty(DiaryTxt.SelectedText)) return;

            var startidx = DiaryTxt.CaretIndex;
            var endIdx = startidx + DiaryTxt.SelectedText.Length;
            var foreTxt = DiaryTxt.Text.Substring(0, startidx);
            var rearTxt = DiaryTxt.Text.Substring(endIdx, DiaryTxt.Text.Length - endIdx);

            var selected= DiaryTxt.SelectedText.Replace(_replaceWindow.OrigTxt.Text, _replaceWindow.ReplaceTxt.Text);
            var replaced = foreTxt + selected + rearTxt;
            //var replaced = DiaryTxt.Text.Replace(DiaryTxt.SelectedText, selected);

            DiaryTxt.Clear();
            DiaryTxt.AppendText(replaced);
            DiaryTxt.Select(startidx, replaced.Length - startidx - rearTxt.Length);
        }
        private void ReplaceAllTxt(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_replaceWindow.OrigTxt.Text)) return;
            var replaced = DiaryTxt.Text.Replace(_replaceWindow.OrigTxt.Text, _replaceWindow.ReplaceTxt.Text);
            //_mwvm.IndicatedDiary = replaced;
            //DiaryTxt.Text = replaced;

            //DiaryTxt.ClearValue(TextBox.TextProperty);
            DiaryTxt.Clear();
            DiaryTxt.AppendText(replaced);
            //DiaryTxt.SetValue(TextBox.TextProperty, replaced);
        }
    }
}
