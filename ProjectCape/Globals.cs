using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape
{
    public static class Globals
    {
        public static Random Random { get; private set; } = new Random();

        public const uint TAG_SOLID         = (0x1 << 0);
        public const uint TAG_ENEMY         = (0x1 << 1);
        public const uint TAG_MOVE          = (0x1 << 2);
        public const uint TAG_MOVE_ENEMY    = TAG_ENEMY | TAG_MOVE;
        public const uint TAG_JEWEL         = (0x1 << 3);
        public const uint TAG_PLAYER        = (0x1 << 4);
        public const uint TAG_PORTAL        = (0x1 << 5);
        public const uint TAG_ONE_WAY       = (0x1 << 6);
    }
}
