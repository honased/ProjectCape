using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
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

        public override void Update(GameTime gameTime)
        {
            if (RoomManager.CurrentLevel == 0) Destroy();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, "Hello", Camera.Position, Color.White);

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {

        }
    }
}
