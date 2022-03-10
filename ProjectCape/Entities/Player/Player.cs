using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using HonasGame.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectCape.Particles;
using System;

namespace ProjectCape.Entities.Player
{
    public class Player : Entity
    {
        private Transform2D _transform;
        private SpriteRenderer _renderer;
        private Velocity2D _velocity;
        private Collider2D _collider;
        private Mover2D _mover;
        private double _jmpTimer;
        private double _coyoteTimer;
        private double _floatTimer;
        private bool _canFloat;

        private const float GRAVITY = 1000.0f;
        private const float JUMP_SPEED = 200.0f;
        private const double JUMP_TIMER = 0.15;
        private const double COYOTE_TIME = 0.2;
        private const double FLOAT_TIME = 0.5;

        public Player()
        {
            _transform = new Transform2D(this); // position
            _collider = new Collider2D(this) { Shape = new BoundingRectangle(10, 14) { Offset = new Vector2(3, 4) }, Transform = _transform, Tag = Globals.TAG_PLAYER };
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprPlayer"), Animation = "idle" };
            _mover = new Mover2D(this);
            _velocity = new Velocity2D();
            _jmpTimer = 0.0;
            _coyoteTimer = 0.0;
            _floatTimer = 0.0;
            _canFloat = false;
        }

        public override void Update(GameTime gameTime)
        {
            Entity e;
            bool onGround = false;
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float newX = ((Input.IsKeyDown(Keys.Right) || Input.IsKeyDown(Keys.D)) ?  1 : 0) +
                       ((Input.IsKeyDown(Keys.Left)  || Input.IsKeyDown(Keys.A)) ? -1 : 0);

            _velocity.X = HonasMathHelper.LerpDelta(_velocity.X, newX * (Input.IsKeyDown(Keys.LeftShift) ? 120.0f : 60.0f), 0.2f, gameTime);
            _velocity.Y += GRAVITY * t;

            if (_collider.CollidesWith(Globals.TAG_SOLID, Vector2.UnitY, out e))
            {
                onGround = true;
                _floatTimer = 0.0;
            }

            if (_jmpTimer > 0.0)
            {
                if (Input.IsKeyDown(Keys.Space))
                {
                    _velocity.Y = -JUMP_SPEED;
                    _jmpTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    _jmpTimer = 0.0;
                }

            }

            if(onGround)
            {
                _coyoteTimer = COYOTE_TIME;
                _canFloat = false;
            }
            else if(_coyoteTimer > 0.0)
            {
                _coyoteTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (Input.IsKeyPressed(Keys.Space) && _coyoteTimer > 0.0)
            {
                _velocity.Y = -JUMP_SPEED;
                _jmpTimer = JUMP_TIMER;
                _coyoteTimer = 0.0;
                _canFloat = true;
            }

            if (Input.IsKeyPressed(Keys.Space) && !onGround && _canFloat && _jmpTimer <= 0.0)
            {
                _floatTimer = FLOAT_TIME;
                _canFloat = false;
            }

            if(_floatTimer > 0.0)
            {
                if (Input.IsKeyDown(Keys.Space))
                {
                    _floatTimer -= t;
                    _velocity.Y = 0.0f;
                }
            }

            if(Input.IsKeyPressed(Keys.Enter))
            {
                Scene.GetParticleSystem<Dust>().PlaceFirework(new Vector2((_collider.Shape.Left + _collider.Shape.Right) / 2.0f, _collider.Shape.Bottom));
            }

            Vector2 newVel = _velocity.CalculateVelocity(gameTime);

            _mover.MoveX(newVel.X, Globals.TAG_SOLID);
            if (_mover.MoveY(newVel.Y, Globals.TAG_SOLID)) { newVel.Y = 0.0f; _velocity.Y = 0.0f; };

            // Check collision with enemy
            e = _collider.CollidesWithAnything(out uint collideTag);
            if ((collideTag & Globals.TAG_ENEMY) > 0)
            {
                if (_collider.Shape.Bottom < e.GetComponent<Collider2D>().Shape.Top + 2.0f || newVel.Y > 0.0f)
                {
                    e.Destroy();
                    _velocity.Y = -JUMP_SPEED;
                    _jmpTimer = JUMP_TIMER;
                    _coyoteTimer = 0.0;
                    _mover.MoveY(e.GetComponent<Collider2D>().Shape.Top - _collider.Shape.Bottom, Globals.TAG_SOLID);
                    _canFloat = true;
                }
                else
                {
                    Destroy();
                    foreach(Entity en in Scene.GetEntities())
                    {
                        if (en != this) en.Enabled = false;
                    }
                    var pd = new PlayerDead();
                    pd.GetComponent<Transform2D>().Position = _transform.Position;
                    pd.GetComponent<SpriteRenderer>().SpriteEffects = _renderer.SpriteEffects;
                    Scene.AddEntity(pd, "Player");
                }
            }
            else if((collideTag & Globals.TAG_JEWEL) > 0)
            {
                e.Destroy();
            }

            Vector2 camOffset = Vector2.UnitX * MathHelper.Clamp(_velocity.X / 1.5f, -40.0f, 40.0f);
            camOffset = new Vector2(8, 9);
            // Sprite Renderer
            if (MathF.Abs(_velocity.X) > 10f)
            {
                _renderer.Animation = "walk";
                _renderer.SpriteEffects = (MathF.Sign(_velocity.X) == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
            else _renderer.Animation = "idle";

            Camera.Position = HonasMathHelper.LerpDelta(Camera.Position, _transform.Position - Camera.CameraSize / 2.0f + camOffset, 1.0f, gameTime);

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {

        }
    }
}
