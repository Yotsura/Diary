using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Dialy
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        internal Dialy.MainWindowViewModel mwvm;
        public MainWindow()
        {
            InitializeComponent();
            mwvm = new MainWindowViewModel();
            this.DataContext = mwvm;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DatePick.Text = DateTime.Today.ToString();
            DialyTxt.FontSize = mwvm.FontSize;
        }

        private void FontZoom(object sender, RoutedEventArgs e)
        {
            mwvm.Zoom(((Button)sender).Content.ToString());
            DialyTxt.FontSize = mwvm.FontSize;
        }

        private void SaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            DateTime.TryParse(DatePick.Text, out var day);
            mwvm.AllDiaries[day] = DialyTxt.Text;
            FileManager.SaveFile(mwvm.FolderPath, DatePick.Text.Replace("/", "_"), DialyTxt.Text);
            MessageLabel.Visibility = Visibility.Collapsed;
        }

        private void DatePick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DialyTxt.Text = string.Empty;
            if (!mwvm.AllDiaries.ContainsKey(DatePick.SelectedDate.Value)) return;
            DialyTxt.Text = mwvm.AllDiaries[DatePick.SelectedDate.Value];
        }

        private void NextFile(object sender, ExecutedRoutedEventArgs e)
        {//←と→を区別して<<か>>を渡したい
            DateChangeButton(sender, e);
        }

        private void DateChangeButton(object sender, RoutedEventArgs e)
        {
            //DateTime.TryParse(DatePick.Text, out var date);
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
                    day = mwvm.NextRecord(date, "<<");
                    break;
                case ">>":
                    day = mwvm.NextRecord(date, ">>");
                    break;
            }
            DatePick.Text = day.ToString();
        }

        SearchWindow searchWindow;
        private void OpenSearchWindow(object sender, ExecutedRoutedEventArgs e)
        {
            if (searchWindow != null)
            {
                searchWindow.Activate();
                return;
            }
            searchWindow = new SearchWindow(mwvm.AllDiaries);
            searchWindow.HitListBox.MouseDoubleClick += ReflectSearch;
            searchWindow.Closed += SearchWindow_Closed;
            searchWindow.Show();
        }

        private async void ReflectSearch(object sender, MouseButtonEventArgs e)
        {
            var s = (ListBox)sender;
            if (s.ItemsSource == null) return;
            var result = searchWindow._swvm.IndicateList[s.SelectedIndex];
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
            searchWindow = null;
        }

        private void DialyTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageLabel.Visibility = mwvm.AllDiaries.ContainsKey(DatePick.SelectedDate.Value) ?
                mwvm.AllDiaries[DatePick.SelectedDate.Value] == DialyTxt.Text ? Visibility.Collapsed : Visibility.Visible :
                String.IsNullOrEmpty(DialyTxt.Text) ? Visibility.Collapsed : Visibility.Visible;
        }

        private async void ShowMessageDialog(string title, string message)
        {
            await this.ShowMessageAsync(title, message);
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            Settings.Default.FontSize = mwvm.FontSize;
            Settings.Default.Save();
        }

        private async void DelRecord(object sender, RoutedEventArgs e)
        {
            var day = DatePick.SelectedDate.Value;
            if (!mwvm.AllDiaries.ContainsKey(day)) return;
            var metroDialogSettings = new MetroDialogSettings() { AffirmativeButtonText = "Yes", NegativeButtonText = "No" };
            var select = await this.ShowMessageAsync("確認", "本当に削除しますか？",
                MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
            if (select == MessageDialogResult.Negative) return;
            mwvm.AllDiaries.Remove(day);
            FileManager.DeleteFile(mwvm.FolderPath, DatePick.Text.Replace("/", "_"));
            DialyTxt.Text = string.Empty;
        }
    }
}
