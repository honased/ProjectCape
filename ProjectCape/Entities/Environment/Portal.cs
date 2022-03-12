using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using Microsoft.Xna.Framework;
using System;

namespace ProjectCape.Entities.Environment
{
    public class Portal : Entity
    {
        private SpriteRenderer _renderer;
        private Transform2D _transform;
        private Vector2 _originalPosition;

        public Portal(float x, float y)
        {
            _transform = new Transform2D(this) { Position = new Vector2(x, y) };
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprPortal"), Animation = "default" };
            _transform.Position += new Vector2(_renderer.Sprite.Texture.Width / 2.0f, _renderer.Sprite.Texture.Height / 2.0f);
            _renderer.CenterOrigin();
            var collider = new Collider2D(this) { Shape = new BoundingCircle(8.0f), Transform = _transform, Tag = Globals.TAG_PORTAL };
            _originalPosition = _transform.Position;
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            float tt = (float)gameTime.TotalGameTime.TotalSeconds;
            _renderer.Rotation = (_renderer.Rotation + (MathHelper.TwoPi * t * 4.0f)) % MathHelper.TwoPi;
            _transform.Position = _originalPosition + new Vector2(0.0f, 4.0f * MathF.Sin(tt));

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {

        }
    }
}
