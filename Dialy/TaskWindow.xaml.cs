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
            InitializeComponent();
            _twvm = new TaskWindowViewModel(taskTxt, fontSize);
            this.DataContext = _twvm;

            if (Settings.Default.TaskWindowStat == null) return;
            this.Top = Settings.Default.TaskWindowStat.Top;
            this.Left = Settings.Default.TaskWindowStat.Left;
            this.Width = Settings.Default.TaskWindowStat.Width;
            this.Height = Settings.Default.TaskWindowStat.Height;
        }

        TaskWindowViewModel _twvm;
        private void ForcusTxt(object sender, EventArgs e)
        {
            TaskTxt.Focus();
        }
        private void FontZoom(object sender, RoutedEventArgs e)
        {
            var btn = ((Button)sender).Content.ToString();
            _twvm.IndicateSize = btn == "+" ? _twvm.IndicateSize + 3 : _twvm.IndicateSize - 3;
        }
    }
}
