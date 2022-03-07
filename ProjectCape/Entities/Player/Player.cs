using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using HonasGame.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

        private const float GRAVITY = 1000.0f;
        private const float JUMP_SPEED = 200.0f;
        private const double JUMP_TIMER = 0.15;
        private const double COYOTE_TIME = 0.2;

        public Player()
        {
            _transform = new Transform2D(this); // position
            _collider = new Collider2D(this) { Shape = new BoundingRectangle(10, 14) { Offset = new Vector2(3, 4) }, Transform = _transform };
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprPlayer"), Animation = "idle" };
            _mover = new Mover2D(this);
            _velocity = new Velocity2D();
            _jmpTimer = 0.0;
            _coyoteTimer = 0.0;
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
            }

            if(_jmpTimer > 0.0)
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
            }

            Vector2 newVel = _velocity.CalculateVelocity(gameTime);

            _mover.MoveX(newVel.X, Globals.TAG_SOLID);
            if (_mover.MoveY(newVel.Y, Globals.TAG_SOLID)) { newVel.Y = 0.0f; _velocity.Y = 0.0f; };

            // Check collision with enemy
            if (_collider.CollidesWith(Globals.TAG_ENEMY, out e))
            {
                if (_collider.Shape.Bottom < e.GetComponent<Collider2D>().Shape.Top + 2.0f || newVel.Y > 0.0f)
                {
                    e.Destroy();
                    _velocity.Y = -JUMP_SPEED;
                    _jmpTimer = JUMP_TIMER;
                    _coyoteTimer = 0.0;
                    _mover.MoveY(e.GetComponent<Collider2D>().Shape.Top - _collider.Shape.Bottom, Globals.TAG_SOLID);
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

            // Sprite Renderer
            if (MathF.Abs(_velocity.X) > 10f)
            {
                _renderer.Animation = "walk";
                _renderer.SpriteEffects = (MathF.Sign(_velocity.X) == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            }
            else _renderer.Animation = "idle";

            Camera.Position += ((_transform.Position - Camera.CameraSize / 2.0f) - Camera.Position) / 15.0f;

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {

        }
    }
}
