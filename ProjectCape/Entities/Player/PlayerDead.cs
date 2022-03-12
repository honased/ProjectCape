using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Entities.Player
{
    public class PlayerDead : Entity
    {
        private Velocity2D _velocity;
        private Transform2D _transform;

        private const float GRAVITY = 1000.0f;
        private const float JUMP_SPEED = 300.0f;

        public PlayerDead(float x, float y, SpriteEffects effects)
        {
            _transform = new Transform2D(this) {Position = new Vector2(x, y)};
            new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprPlayer"), Animation = "idle", SpriteEffects = effects };
            _velocity = new Velocity2D();

            _velocity.Y = -JUMP_SPEED;
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _velocity.Y += GRAVITY * t;
            float addY = _velocity.CalculateVelocity(gameTime).Y;

            _transform.Position += Vector2.UnitY * addY;

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {

        }
    }
}
