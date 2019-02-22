using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace Dialy
{
    /// <summary>
    /// SearchWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SearchWindow : MetroWindow
    {
        public SearchWindowViewModel _swvm;
        public SearchWindow(SortedDictionary<DateTime, string> allDiaries,int fontSize)
        {
            InitializeComponent();
            _swvm = new SearchWindowViewModel(allDiaries, fontSize);
            this.DataContext = _swvm;
        }

        private void ForcusTxt(object sender, EventArgs e)
        {
            TargetTxt.Focus();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TargetTxt.Text)) return;
            InvokeSearch();
        }

        private void TargetTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (string.IsNullOrEmpty(TargetTxt.Text)) return;
            InvokeSearch();
        }

        private void InvokeSearch()
        {
            _swvm.SearchFunc(TargetTxt.Text, OrSearch.IsChecked == true);
        }

        Dictionary<DateTime, SecondWindow> SecondWindows=new Dictionary<DateTime, SecondWindow>();
        SecondWindow _secondWindow;
        private void ShowSecondWindow(object sender, RoutedEventArgs e)
        {
            if (HitListBox.SelectedIndex == -1) return;
            var date = (DateTime)HitListBox.SelectedItem;
            if (DateTime.Today == date.Date || SecondWindows.Keys.Contains(date)) return;
            _secondWindow = new SecondWindow(date, _swvm._allDiaries[date], _swvm._fontSize);
            _secondWindow.Save.Click += SaveRecord;
            SecondWindows[date] = _secondWindow;
            _secondWindow.Show();
        }

        private void SaveRecord(object sender, RoutedEventArgs e)
        {
            _swvm._allDiaries[(DateTime)HitListBox.SelectedItem] = _secondWindow.DiaryTxt.Text;
            FileManager.SaveFile(Dialy.Settings.Default.FolderPath, _secondWindow._swvm.IndicateDate, _secondWindow.DiaryTxt.Text);
        }
    }
}
