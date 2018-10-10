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

        private void SaveDialy(object sender, RoutedEventArgs e)
        {
            SaveInvoke();
        }

        private void SaveCommand(object sender, ExecutedRoutedEventArgs e)
        {
            SaveInvoke();
        }

        private void SaveInvoke()
        {
            DateTime.TryParse(DatePick.Text, out var day);
            mwvm.AllDiaries[day] = DialyTxt.Text;
            FileManager.SaveFile(mwvm.FolderPath, DatePick.Text.Replace("/", "_"), DialyTxt.Text);
        }

        private void DatePick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DialyTxt.Text = string.Empty;
            DateTime.TryParse(DatePick.Text, out var request);
            //var request = DatePick.Text.Replace("/", "_");
            if (!mwvm.AllDiaries.ContainsKey(request)) return;
            DialyTxt.Text = mwvm.AllDiaries[request];
        }

        private void DateChangeButton(object sender, RoutedEventArgs e)
        {
            DateTime.TryParse(DatePick.Text, out var date);
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
                    day = date.AddDays(-7);
                    //mwvm.NextRecord(day, "<<");
                    break;
                case ">>":
                    day = date.AddDays(7);
                    break;
            }
            DatePick.Text = day.ToString();
        }
    }
}
