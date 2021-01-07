using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Dialy.Commands
{
    public class MyApplicationCommands
    {
        public static RoutedUICommand NextRecord
                            = new RoutedUICommand("Load Next Record",
                                                  "NextRecord",
                                                  typeof(MyApplicationCommands));
        public static RoutedUICommand PrevRecord
                            = new RoutedUICommand("Load Prev Record",
                                                  "PrevRecord",
                                                  typeof(MyApplicationCommands));
        public static RoutedUICommand NextDate
                            = new RoutedUICommand("Load Next Date",
                                                  "NextDate",
                                                  typeof(MyApplicationCommands));
        public static RoutedUICommand PrevDate
                            = new RoutedUICommand("Load Prev Date",
                                                  "PrevDate",
                                                  typeof(MyApplicationCommands));
        public static RoutedUICommand Today
                            = new RoutedUICommand("Indicate Today",
                                                  "Today",
                                                  typeof(MyApplicationCommands));
        public static RoutedUICommand TaskWindow
                            = new RoutedUICommand("Open Task Window",
                                                  "TaskWindow",
                                                  typeof(MyApplicationCommands));
        public static RoutedUICommand ReplaceWindow
                            = new RoutedUICommand("Open Replace Window",
                                                  "ReplaceWindow",
                                                  typeof(MyApplicationCommands));
    }
}
