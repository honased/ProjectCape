using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape
{
    public static class SongManager
    {
        public static float MasterVolume = 1.0f;

        private static Song _nextSong = null;
        private static bool _nextLoop;
        private static double _timer = 0.0;
        private static double _duration = 0.0;

        public static void PlaySong(Song song, bool loop = false)
        {
            MediaPlayer.Volume = MasterVolume;
            MediaPlayer.IsRepeating = loop;
            MediaPlayer.Play(song);
        }

        public static void TransitionSong(Song song, double duration, bool loop = false)
        {
            _nextLoop = loop;
            _duration = duration;
            _nextSong = song;
            _timer = duration;
        }

        public static void FadeSong(double duration)
        {
            TransitionSong(null, duration, false);
        }

        public static void Update(GameTime gameTime)
        {
            if (_timer > 0)
            {
                _timer -= gameTime.ElapsedGameTime.TotalSeconds;
                MediaPlayer.Volume = MathHelper.Clamp((float)(_timer / _duration), 0.0f, 1.0f) * MasterVolume;
                if (_timer <= 0 && _nextSong != null)
                {
                    PlaySong(_nextSong, _nextLoop);
                    _nextSong = null;
                }
            }
        }
    }
}
