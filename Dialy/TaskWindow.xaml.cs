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
using MahApps.Metro.Controls.Dialogs;

namespace Dialy
{
    /// <summary>
    /// TaskWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TaskWindow : MetroWindow
    {
        public TaskWindow(string taskTxt,int fontSize)
        {
            _twvm = new TaskWindowViewModel(taskTxt, fontSize);
            this.DataContext = _twvm;
            InitializeComponent();
        }

        TaskWindowViewModel _twvm;

        private void FontZoom(object sender, RoutedEventArgs e)
        {
            var btn = ((Button)sender).Content.ToString();
            _twvm.IndicateSize = btn == "+" ? _twvm.IndicateSize + 3 : _twvm.IndicateSize - 3;
        }
    }
}
