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
            InvokeSearch();
        }

        private void TargetTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            InvokeSearch();
        }

        private void InvokeSearch()
        {
            _swvm.SearchFunc(TargetTxt.Text, OrSearch.IsChecked == true);
            if (_swvm.IndicateList.Count < 1) return;
            HitListBox.SelectedIndex = 1;
            HitListBox.SelectedIndex = 0;
            HitListBox.Focus();
        }

        private void HitListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HitListBox.SelectedValue == null)
            {
                _swvm.RecordTxt = String.Empty;
                return;
            }
            var date = (DateTime)HitListBox.SelectedValue;
            var txt = _swvm._allDiaries[date];
            _swvm.RecordTxt = txt;
        }

        private void FontZoom(object sender, RoutedEventArgs e)
        {
            var btn = ((Button)sender).Content.ToString();
            _swvm.IndicateSize = btn == "+" ? _swvm.IndicateSize + 3 : _swvm.IndicateSize - 3;
        }

        private void FocusSearchWindow(object sender, ExecutedRoutedEventArgs e)
        {
            TargetTxt.Focus();
        }
    }
}
