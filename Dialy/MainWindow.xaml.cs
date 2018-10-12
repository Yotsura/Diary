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
                case "<<":
                    day = mwvm.NextRecord(date, "<<");
                    break;
                case ">>":
                    day = mwvm.NextRecord(date, ">>");
                    break;
            }
            DatePick.Text = day.ToString();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var searchWindow = new SearchWindow();
            searchWindow.Show();
        }

        private void DialyTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageLabel.Visibility = mwvm.AllDiaries.ContainsKey(DatePick.SelectedDate.Value) ?
                mwvm.AllDiaries[DatePick.SelectedDate.Value] == DialyTxt.Text ? Visibility.Collapsed : Visibility.Visible :
                String.IsNullOrEmpty(DialyTxt.Text) ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
