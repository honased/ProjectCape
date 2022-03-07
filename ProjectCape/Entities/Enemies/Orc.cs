﻿using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ProjectCape.Entities.Enemies
{
    public class Orc : Entity
    {
        private const float GRAVITY     = 930.0f;
        private const float MOVE_SPD    = 30.0f;

        private SpriteRenderer _renderer;
        private Collider2D _collider;
        private Mover2D _mover;
        private Velocity2D _velocity;
        private float _widthDiv2;

        public Orc()
        {
            new Transform2D(this);
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprOrc"), Animation = "walk" };
            _collider = new Collider2D(this) { Shape = new BoundingRectangle(16, 16) { Offset = new Vector2(0, 2) }, Transform = GetComponent<Transform2D>(), Tag = Globals.TAG_MOVE_ENEMY | Globals.TAG_ENEMY };
            _mover = new Mover2D(this);
            _velocity = new Velocity2D();
            _widthDiv2 = _renderer.Sprite.Animations["walk"].Frames[0].Width / 2.0f;
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _velocity.Y += GRAVITY * t;
            _velocity.X = MOVE_SPD * ((_renderer.SpriteEffects == SpriteEffects.None) ? 1 : -1);

            Vector2 newVel = _velocity.CalculateVelocity(gameTime);

            if(_mover.MoveX(newVel.X, Globals.TAG_SOLID)) InvertDirection();
            if(_mover.MoveY(newVel.Y, Globals.TAG_SOLID)) _velocity.Y = 0.0f;

            if(!_collider.CollidesWith(Globals.TAG_SOLID, new Vector2(((_renderer.SpriteEffects == SpriteEffects.None) ? 1 : -1) * _widthDiv2, 1), out Entity e))
            {
                InvertDirection();
            }

            if(_collider.CollidesWith(Globals.TAG_MOVE_ENEMY, Vector2.UnitX * ((_renderer.SpriteEffects == SpriteEffects.None) ? 1 : -1), out e))
            {
                if(e is Orc o)
                {
                    var c = e.GetComponent<SpriteRenderer>();
                    if (c != null && c.SpriteEffects != _renderer.SpriteEffects)
                    {
                        InvertDirection();
                        o.InvertDirection();
                    }
                }
            }

            base.Update(gameTime);
        }
        
        private void InvertDirection()
        {
            _renderer.SpriteEffects ^= SpriteEffects.FlipHorizontally;
        }

        protected override void Cleanup()
        {

        }
    }
}
