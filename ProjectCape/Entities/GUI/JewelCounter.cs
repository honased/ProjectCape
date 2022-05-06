using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Entities.GUI
{
    public class JewelCounter : Entity
    {
        private SpriteFont _font;
        private Texture2D _jewelTex;
        private int _count;

        public JewelCounter()
        {
            _font = AssetLibrary.GetAsset<SpriteFont>("fntText");
            _jewelTex = AssetLibrary.GetAsset<Texture2D>("jewel");
            Persistent = true;
            _count = 0;
        }

        public void AddJewel()
        {
            _count += 1;
        }

        public int GetJewelCount()
        {
            return _count;
        }

        public override void Update(GameTime gameTime)
        {
            if (RoomManager.CurrentLevel == 0) Destroy();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_jewelTex, Camera.Position, Color.White);
            spriteBatch.DrawFilledRectangle(new Rectangle((int)Camera.Position.X + 12, (int)Camera.Position.Y + 3, 24, 10), Color.Black);
            spriteBatch.DrawString(_font, $"x{_count}", Camera.Position + Vector2.UnitX * 12, Color.White);

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {

        }
    }
}
