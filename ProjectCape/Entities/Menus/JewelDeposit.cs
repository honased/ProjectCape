using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.Helper;
using HonasGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ProjectCape.Entities.GUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Entities.Menus
{
    class JewelParticle : Entity
    {
        private Transform2D _transform;
        private SpriteRenderer _renderer;
        private Vector2 _velocity;
        private const float GRAVITY = 220.0f;

        public JewelParticle(float x, float y)
        {
            _transform = new Transform2D(this) { Position = new Vector2(x, y) };
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprJewel") };
            _renderer.CenterOrigin();
            _velocity = Vector2.UnitX * RandomHelper.NextFloat(-5, 5);
            _velocity.Y = 50.0f;
        }

        public override void Update(GameTime gameTime)
        {
            _velocity += Vector2.UnitY * GRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _renderer.Rotation += _velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.25f;
            _transform.Position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_transform.Position.Y > Camera.CameraSize.Y + 16) Destroy();

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {

        }
    }

    public class JewelDeposit : Entity
    {
        private SpriteFont _font;
        private int jewelCount;

        public JewelDeposit()
        {
            _font = AssetLibrary.GetAsset<SpriteFont>("fntText");
            if(Scene.GetEntity<JewelCounter>(out var jc))
            {
                jewelCount = jc.GetJewelCount();
                jc.Destroy();
            }

            var routine = new Coroutine(this, Routine());
            routine.Start();
        }

        private IEnumerator<double> Routine()
        {
            yield return 1.0;
            while(jewelCount > 0)
            {
                jewelCount--;
                Vector2 pos = Vector2.UnitX * (RandomHelper.NextFloat(Camera.CameraSize.X / 2.0f) + (Camera.CameraSize.X / 4.0f));
                pos.Y = -8;
                Scene.AddEntity(new JewelParticle(pos.X, pos.Y));
                AssetLibrary.GetAsset<SoundEffect>("sndJewel").Play();
                yield return 0.1;
            }

            while(Scene.GetEntity<JewelParticle>(out var ignore))
            {
                yield return 0.5;
            }

            yield return 0.2f;
            Scene.AddEntity(new RoomTransition(true));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawFilledRectangle(Vector2.Zero, Camera.CameraSize, Color.Black);

            string str = $"Jewels Collected: {jewelCount}";
            var origin = _font.MeasureString(str) / 2.0f;

            spriteBatch.DrawString(_font, str, Camera.CameraSize / 2.0f, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {

        }
    }
}
