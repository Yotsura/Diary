using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialy.Funcs
{
    public static class ListExtensions
    {
        public static void DropRight<T>(this List<T> self, int count)
        {
            self.RemoveRange(self.Count - count, count);
        }
        public static void RemoveFirst<T>(this List<T> self, int count)
        {
            self.RemoveRange(0, count);
        }
    }
}
