using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialy.Model
{
    public class CharInfo
    {
        public char Val { get; set; }
        public bool IsHit { get; set; } = false;
        public CharInfo(char val)
        {
            Val = val;
        }
    }
}
