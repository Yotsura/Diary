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

        private void ShowSecondWindow(object sender, RoutedEventArgs e)
        {
            if (HitListBox.SelectedIndex == -1) return;
            var s = (ListBox)sender;
            if (s.ItemsSource == null) return;
            var result = _swvm.IndicateList[s.SelectedIndex];
            SecondWindow secondWindow = new SecondWindow(result, _swvm._allDiaries[result], _swvm._fontSize);
        }

        private void CreateContextMenu_MouseRightButton(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton != MouseButtonState.Pressed) return;

            HitListBox.ContextMenu = null;

            if (!HitCheck(HitListBox, e)) return;

            //ContextMenuを作成する。
            MenuItem menuitem = new MenuItem { Header = "別ウィンドウで開く" };
            menuitem.PreviewMouseLeftButtonDown += ShowSecondWindow;;

            ContextMenu contextmenu = new ContextMenu();
            contextmenu.Items.Add(menuitem);
            HitListBox.ContextMenu = contextmenu;
        }

        public static bool HitCheck(ListBox listBox, MouseButtonEventArgs e)
        {
            if (!(listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem) is DataGridRow ctrl)) return false;
            if (null == ctrl.InputHitTest(e.GetPosition(ctrl))) return false;
            return true;
        }
    }
}
