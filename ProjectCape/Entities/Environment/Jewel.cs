using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using Microsoft.Xna.Framework;
using System;

namespace ProjectCape.Entities.Environment
{
    public class Jewel : Entity
    {
        private SpriteRenderer _renderer;
        private Transform2D _transform;

        public Jewel(float x, float y)
        {
            _transform = new Transform2D(this) { Position = new Vector2(x, y) };
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprJewel"), Animation = "default" };
            _transform.Position += new Vector2(_renderer.Sprite.Texture.Width / 2.0f, _renderer.Sprite.Texture.Height / 2.0f);
            _renderer.CenterOrigin();
            var collider = new Collider2D(this) { Shape = new BoundingRectangle(8, 10) { Offset = new Vector2(4, 3) }, Transform = _transform, Tag = Globals.TAG_JEWEL };
            collider.Shape.Offset = new Vector2(-_renderer.Sprite.Texture.Width / 4.0f, -_renderer.Sprite.Texture.Height / 4.0f);
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.TotalGameTime.TotalSeconds;
            _renderer.Rotation = MathF.Sin((t + _transform.Position.X / 32.0f) * 2.0f) / 2.5f;

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {

        }
    }
}
