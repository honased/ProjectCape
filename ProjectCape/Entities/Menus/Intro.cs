using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.Helper;
using HonasGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Entities.Menus
{
    public class Intro : Entity
    {
        private const double TEXT_TIMER = 0.06f;

        private SpriteFont _font;
        private string _text;
        private int _textIndex;
        private double timer;

        public Intro()
        {
            _font = AssetLibrary.GetAsset<SpriteFont>("fntText");
            _text = "";
            _textIndex = 0;
            timer = TEXT_TIMER;
            var routine = new Coroutine(this, Routine());
            routine.Start();
            SongManager.TransitionSong(AssetLibrary.GetAsset<Song>("musVoices"), 0.5, true);
        }

        private IEnumerator<double> Routine()
        {
            yield return 1.0;
            _textIndex = 0;
            _text = "Wake up...";
            yield return 2.0;
            _textIndex = 0;
            _text = "";
            yield return 1.0;
            _textIndex = 0;
            _text = "There is work to be done, wisp...";
            yield return 5.0;
            _textIndex = 0;
            _text = "";
            yield return 1.0;
            _textIndex = 0;
            _text = "Wake up...";
            yield return 2.0;
            _textIndex = 0;
            _text = "";
            yield return 1.0;
            Scene.AddEntity(new RoomTransition(true));
            SongManager.PlaySong(AssetLibrary.GetAsset<Song>("musForest"), true);
        }

        public override void Update(GameTime gameTime)
        {
            if (timer <= 0)
            {
                if (_textIndex < _text.Length)
                {
                    _textIndex += 1;
                    if (_textIndex % 2 == 1)
                    {
                        var inst = AssetLibrary.GetAsset<SoundEffect>("sndTalk").CreateInstance();
                        inst.Pitch = RandomHelper.NextFloat(-0.2f, 0.4f);
                        inst.Play();
                    }
                }
                timer = TEXT_TIMER;
            }
            else timer -= gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawFilledRectangle(Vector2.Zero, Camera.CameraSize, Color.Black);
            string strSubset = _text.Substring(0, _textIndex);
            var origin = _font.MeasureString(_text) / 2.0f;

            spriteBatch.DrawString(_font, strSubset, Camera.CameraSize / 2.0f, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {
        }
    }
}
