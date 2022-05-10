using HonasGame.Assets;
using HonasGame.Tiled;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape
{
    public static class RoomManager
    {
        private static string[] _levels = new string[] { "room_madeby", "room_menu", "room_intro", "room_0_0", "room_inbetween", "room_0_1", "room_inbetween", "room_0_2", "room_inbetween", "room_outro" };

        public static int CurrentLevel { get; private set; } = 0;

        public static void Goto(string room)
        {
            AssetLibrary.GetAsset<TiledMap>(room).Goto();
        }

        public static void GotoLevel()
        {
            Goto(_levels[CurrentLevel]);
        }

        public static void GotoNextLevel()
        {
            CurrentLevel = (CurrentLevel + 1) % _levels.Length;
            Goto(_levels[CurrentLevel]);
        }
    }
}
