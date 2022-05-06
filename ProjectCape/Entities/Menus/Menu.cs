using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Entities.Menus
{
    public class Menu : Entity
    {
        private SpriteFont _font;
        private bool _startSelected;

        public bool Quit { get; private set; } = false;

        public Menu()
        {
            Camera.Position = new Vector2(0, 0);

            _font = AssetLibrary.GetAsset<SpriteFont>("fntText");
            _startSelected = true;
        }

        public override void Update(GameTime gameTime)
        {
            var right = Input.IsKeyPressed(Keys.D) || Input.IsKeyPressed(Keys.Right) || Input.IsButtonPressed(Buttons.DPadRight) || Input.CheckAnalogPressed(true, true, 1);
            var left = Input.IsKeyPressed(Keys.A) || Input.IsKeyPressed(Keys.Left) || Input.IsKeyPressed(Keys.LeftShift) || Input.IsButtonPressed(Buttons.DPadLeft) || Input.CheckAnalogPressed(true, true, -1);
            var select = Input.IsKeyPressed(Keys.Space) || Input.IsKeyPressed(Keys.Enter) || Input.IsButtonPressed(Buttons.A);

            if (right) _startSelected = false;
            else if (left) _startSelected = true;

            if(select)
            {
                if (_startSelected)
                {
                    Scene.AddEntity(new RoomTransition(true));
                    Enabled = false;
                    SongManager.TransitionSong(AssetLibrary.GetAsset<Song>("musForest"), 0.5, true);
                }
                else Quit = true;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float t = (float)gameTime.TotalGameTime.TotalSeconds;

            Vector2 origin = _font.MeasureString("Start") / 2.0f;
            //var rect = new Rectangle((int)((Camera.CameraSize.X / 2.0f) - origin.X - 5), (int)((Camera.CameraSize.Y / 2.0f) - origin.Y), (int)(origin.X * 2) + 10, (int)(origin.Y * 2));
            //spriteBatch.DrawFilledRectangle(rect, Color.Black);
            spriteBatch.DrawString(_font, "Start", new Vector2(Camera.CameraSize.X / 3.0f, Camera.CameraSize.Y - 18.0f), _startSelected ? Color.Yellow : Color.Black, 0.0f, origin, 2.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(_font, "Quit", new Vector2(Camera.CameraSize.X / 3.0f * 2, Camera.CameraSize.Y - 18.0f), _startSelected ? Color.Black : Color.Yellow, 0.0f, origin, 2.0f, SpriteEffects.None, 0.0f);

            origin = _font.MeasureString("Forgotten Wisp") / 2.0f;
            Vector2 pos = new Vector2(Camera.CameraSize.X / 2.0f, Camera.CameraSize.Y / 3.0f) + (Vector2.UnitY * (MathF.Sin(t) * 7.0f));
            spriteBatch.DrawString(_font, "Forgotten Wisp", pos + Vector2.UnitX, Color.Black, 0.0f, origin, 2.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(_font, "Forgotten Wisp", pos - Vector2.UnitX, Color.Black, 0.0f, origin, 2.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(_font, "Forgotten Wisp", pos + Vector2.UnitY, Color.Black, 0.0f, origin, 2.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(_font, "Forgotten Wisp", pos - Vector2.UnitY, Color.Black, 0.0f, origin, 2.0f, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(_font, "Forgotten Wisp", pos, Color.White, 0.0f, origin, 2.0f, SpriteEffects.None, 0.0f);

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {

        }
    }
}
