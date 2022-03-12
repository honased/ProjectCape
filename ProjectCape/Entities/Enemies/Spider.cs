using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using ProjectCape.Entities.Player;

namespace ProjectCape.Entities.Enemies
{
    public class Spider : Entity
    {
        private SpriteRenderer _renderer;
        private Collider2D _collider;
        private Mover2D _mover;
        private Velocity2D _velocity;
        private const float GRAVITY = 930.0f;

        public Spider(float x, float y)
        {
            var transform = new Transform2D(this) { Position = new Vector2(x, y) };
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprSpider"), Animation = "idle" };
            _collider = new Collider2D(this) { Shape = new BoundingRectangle(32, 15) { Offset = Vector2.UnitY }, Transform = transform, Tag = Globals.TAG_ENEMY };
            _mover = new Mover2D(this);
            _velocity = new Velocity2D();

            var routine = new Coroutine(this, Routine());
            routine.Start();
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _velocity.Y += GRAVITY * t;

            Vector2 newVel = _velocity.CalculateVelocity(gameTime);

            if(_mover.MoveX(newVel.X, Globals.TAG_SOLID)) _velocity.X *= -1;
            if(_velocity.Y < 0.0f) _mover.MoveY(newVel.Y, Globals.TAG_SOLID | Globals.TAG_PLAYER, oneWayTag: Globals.TAG_ONE_WAY);
            else _mover.MoveY(newVel.Y, Globals.TAG_SOLID, oneWayTag: Globals.TAG_ONE_WAY);

            if (_collider.CollidesWith(Globals.TAG_SOLID | Globals.TAG_ONE_WAY, Vector2.UnitY, out Entity e))
            {
                _velocity.Set(Vector2.Zero);
            }

            base.Update(gameTime);
        }

        private IEnumerator<double> Routine()
        {
            while(true)
            {
                yield return Globals.Random.NextDouble() + 1.0f * 2.0f;
                _renderer.Enabled = false;
                yield return 0.5;
                _renderer.Enabled = true;
                _velocity.Y = -300;

                if(Scene.GetEntity<Player.Player>(out var player))
                {
                    float theirX, ourX;

                    if(player.GetComponent<Transform2D>(out var comp)) theirX = comp.Position.X;
                    else theirX = 0.0f;

                    if(GetComponent<Transform2D>(out comp)) ourX = comp.Position.X;
                    else ourX = 0.0f;

                    _velocity.X = MathF.Sign(theirX - ourX) * MathHelper.Clamp(MathHelper.Distance(theirX, ourX) * 1.25f, 30, 180);
                }
            }
        }

        protected override void Cleanup()
        {

        }
    }
}
