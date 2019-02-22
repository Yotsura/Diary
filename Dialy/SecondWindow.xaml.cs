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
using MahApps.Metro.Controls;

namespace Dialy
{
    /// <summary>
    /// SecondWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SecondWindow : MetroWindow
    {
        public SecondWindow(DateTime date, string diaryTxt, int fontSize)
        {
            InitializeComponent();
            _swvm = new SecondWindowViewModel(date, diaryTxt, fontSize);
            this.DataContext = _swvm;
        }
        public SecondWindowViewModel _swvm;

        private void FontZoom(object sender, RoutedEventArgs e)
        {
            var btn = ((Button)sender).Content.ToString();
            _swvm.IndicateSize = btn == "+" ? _swvm.IndicateSize + 3 : _swvm.IndicateSize - 3;
        }
    }
}
