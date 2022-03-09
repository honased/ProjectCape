using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape
{
    public static class Globals
    {
        public static Random Random { get; private set; } = new Random();

        public const uint TAG_SOLID         = 1;
        public const uint TAG_ENEMY         = 2;
        public const uint TAG_MOVE          = 4;
        public const uint TAG_MOVE_ENEMY    = TAG_ENEMY | TAG_MOVE;
        public const uint TAG_JEWEL         = 8;
        public const uint TAG_PLAYER        = 16;
    }
}
