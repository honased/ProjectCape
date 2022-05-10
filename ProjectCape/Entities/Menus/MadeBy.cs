using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Entities.Menus
{
    public class MadeBy : Entity
    {
        private SpriteFont _font;
        string text;

        public MadeBy()
        {
            _font = AssetLibrary.GetAsset<SpriteFont>("fntText");

            Coroutine routine = new Coroutine(this, Routine());
            routine.Start();
            text = "";
        }

        private IEnumerator<double> Routine()
        {
            AssetLibrary.GetAsset<SoundEffect>("sndAlien1").Play();
            yield return 1.0;
            text = "A GAME BY";
            yield return 2.0;
            text = "";
            yield return 0.5;
            AssetLibrary.GetAsset<SoundEffect>("sndAlien2").Play();
            yield return 1.0;
            text = "ERIC HONAS";
            yield return 2.5;
            text = "";
            yield return 1.5;
            Scene.AddEntity(new RoomTransition(true));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawFilledRectangle(Vector2.Zero, Camera.CameraSize, Color.Black);
            Vector2 origin = _font.MeasureString(text) / 2.0f;
            spriteBatch.DrawString(_font, text, Camera.CameraSize / 2.0f, Color.White, 0.0f, origin, 1.5f, SpriteEffects.None, 0.0f);
            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {

        }
    }
}
