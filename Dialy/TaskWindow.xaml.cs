using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Dialy
{
    /// <summary>
    /// TaskWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TaskWindow : MetroWindow
    {
        public TaskWindow(int fontSize,TaskRecord taskdata)
        {
            InitializeComponent();
            _twvm = new TaskWindowViewModel(fontSize, taskdata);
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

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _twvm.Task.SaveTaskFile();
        }
    }
}
