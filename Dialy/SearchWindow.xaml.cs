using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Dialy
{
    /// <summary>
    /// SearchWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SearchWindow : MetroWindow
    {
        public SearchWindowViewModel _swvm;
        public SearchWindow(SortedDictionary<DateTime, string> allDiaries)
        {
            InitializeComponent();
            _swvm = new SearchWindowViewModel(allDiaries);
            this.DataContext = _swvm;

            if (Settings.Default.SearchWindowStat == null) return;
            this.Top = Settings.Default.SearchWindowStat.Top;
            this.Left = Settings.Default.SearchWindowStat.Left;
            this.Width = Settings.Default.SearchWindowStat.Width;
            this.Height = Settings.Default.SearchWindowStat.Height;
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
            HitListBox.SelectedIndex = -1;
            _swvm.SearchFunc();
            if (_swvm.IndicateList == null || _swvm.IndicateList.Count < 1) return;
            HitListBox.SelectedIndex = 0;
            HitListBox.Focus();
        }

        private void HitListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextScroll.ScrollToHome();
            if (HitListBox.SelectedValue == null)
            {
                _swvm.IndicateRecord(DateTime.Parse("1900/1/1"));
            }
            else
            {
                var date = (DateTime)HitListBox.SelectedValue;
                _swvm.IndicateRecord(date);
            }
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
