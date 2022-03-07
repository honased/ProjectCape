using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape
{
    public static class Globals
    {
        public static Random Random { get; private set; } = new Random();

        public const int TAG_SOLID          = 1;
        public const int TAG_ENEMY          = 2;
        public const int TAG_MOVE_ENEMY     = 4;
    }
}
