using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSnake
{
    internal static class ConsolePositionHelper
    {
        public static int GetCorrectLeft(int pos, int offset = 0)
        {
            return (pos + 1) * 2 + offset;
        }

        public static int GetCorrectTop(int pos, int offset = 0)
        {
            return pos + 1 + offset;
        }
    }
}
