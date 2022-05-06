using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using HonasGame.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectCape.Entities.Environment;
using ProjectCape.Entities.GUI;
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

        private const float GRAVITY = 1000.0f;
        private const float JUMP_SPEED = 200.0f;
        private const double JUMP_TIMER = 0.15;
        private const double COYOTE_TIME = 0.2;

        public Player(float x, float y)
        {
            _transform = new Transform2D(this) { Position = new Vector2(x, y) }; // position
            _collider = new Collider2D(this) { Shape = new BoundingRectangle(10, 14) { Offset = new Vector2(3, 4) }, Transform = _transform, Tag = Globals.TAG_PLAYER };
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprPlayer"), Animation = "idle" };
            _mover = new Mover2D(this);
            _velocity = new Velocity2D();
            _jmpTimer = 0.0;
            _coyoteTimer = 0.0;
            Camera.Position = _transform.Position;
            if (Scene.GetEntity<JewelCounter>(out var jc)) jc.Destroy();
            Scene.AddEntity(new JewelCounter(), "GUI");

            if(Globals.AddToTotalJewels)
            {
                Globals.AddToTotalJewels = false;
                foreach(Entity e in Scene.GetEntities())
                {
                    if (e is Jewel j) Globals.TotalJewels += 1;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            Entity e;
            bool onGround = false;
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float newX = ((Input.IsKeyDown(Keys.Right) || Input.IsKeyDown(Keys.D) || Input.CheckAnalogDirection(true, true, 1) || Input.IsButtonDown(Buttons.DPadRight)) ?  1 : 0) +
                       ((Input.IsKeyDown(Keys.Left)  || Input.IsKeyDown(Keys.A) || Input.CheckAnalogDirection(true, true, -1) || Input.IsButtonDown(Buttons.DPadLeft)) ? -1 : 0);

            _velocity.X = HonasMathHelper.LerpDelta(_velocity.X, newX * ((Input.IsKeyDown(Keys.LeftShift) || Input.IsButtonDown(Buttons.RightTrigger)) ? 120.0f : 60.0f), 0.2f, gameTime);
            _velocity.Y += GRAVITY * t;

            if (_collider.CollidesWith(Globals.TAG_SOLID | Globals.TAG_ONE_WAY, Vector2.UnitY, out e))
            {
                onGround = true;
                
                if(e.GetComponent<Collider2D>(out var eCollider))
                {
                    if((eCollider.Tag & Globals.TAG_ONE_WAY) > 0)
                    {
                        var down = Input.IsKeyPressed(Keys.S) || Input.IsKeyPressed(Keys.Down) || Input.IsButtonPressed(Buttons.DPadDown) || Input.CheckAnalogPressed(true, false, 1);
                        if (down)
                        {
                            onGround = false;
                            _coyoteTimer = 0.0f;
                            _transform.Position += Vector2.UnitY;
                        }
                    }
                }
            }

            if (_jmpTimer > 0.0)
            {
                if (Input.IsKeyDown(Keys.Space) || Input.IsButtonDown(Buttons.A))
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

            if ((Input.IsKeyPressed(Keys.Space) || Input.IsButtonPressed(Buttons.A)) && _coyoteTimer > 0.0)
            {
                _velocity.Y = -JUMP_SPEED;
                _jmpTimer = JUMP_TIMER;
                _coyoteTimer = 0.0;
                AssetLibrary.GetAsset<SoundEffect>("sndJump").Play();
                //Scene.GetParticleSystem<Blood>().PlaceBlood(_transform.Position);
            }

            Vector2 newVel = _velocity.CalculateVelocity(gameTime);

            _mover.MoveX(newVel.X, Globals.TAG_SOLID);
            if (_mover.MoveY(newVel.Y, Globals.TAG_SOLID, oneWayTag: Globals.TAG_ONE_WAY))
            { 
                // Place dust particle impact
                if(_velocity.Y > 360.0f)
                {
                    Scene.GetParticleSystem<Blood>().PlaceBlood(new Vector2((_collider.Shape.Right + _collider.Shape.Left) / 2.0f, _collider.Shape.Bottom), Color.FromNonPremultiplied(138, 72, 54, 255));
                }

                newVel.Y = 0.0f; 
                _velocity.Y = 0.0f; 
                _jmpTimer = 0.0; 
            }

            // Check collision with enemy
            e = _collider.CollidesWithAnything(out uint collideTag);
            if ((collideTag & Globals.TAG_ENEMY) > 0)
            {
                if(e.GetComponent<Collider2D>(out var eCollider))
                {
                    if (_collider.Shape.Bottom < eCollider.Shape.Top + 2.0f || newVel.Y > 0.0f)
                    {
                        e.Destroy();
                        _velocity.Y = -JUMP_SPEED;
                        _jmpTimer = JUMP_TIMER;
                        _coyoteTimer = 0.0;
                        _mover.MoveY(eCollider.Shape.Top - _collider.Shape.Bottom, Globals.TAG_SOLID);
                        Scene.GetParticleSystem<Blood>().PlaceBlood(new Vector2((_collider.Shape.Right + _collider.Shape.Left) / 2.0f, eCollider.Shape.Top), Color.Red);
                        AssetLibrary.GetAsset<SoundEffect>("sndKill").Play();
                    }
                    else
                    {
                        Destroy();
                        foreach(Entity en in Scene.GetEntities())
                        {
                            if (en != this) en.Enabled = false;
                        }
                        var pd = new PlayerDead(_transform.Position.X, _transform.Position.Y, _renderer.SpriteEffects);
                        Scene.AddEntity(pd, "Player");
                        AssetLibrary.GetAsset<SoundEffect>("sndDeath").Play();
                    }
                }
            }
            else if((collideTag & Globals.TAG_JEWEL) > 0)
            {
                e.Destroy();
                AssetLibrary.GetAsset<SoundEffect>("sndJewel").Play();
                if (Scene.GetEntity<JewelCounter>(out var jc)) jc.AddJewel();
            }
            else if((collideTag & Globals.TAG_PORTAL) > 0)
            {
                Destroy();
                Scene.AddEntity(new PlayerPortal(_transform.Position.X, _transform.Position.Y, _renderer.SpriteEffects, e), "Player");
                AssetLibrary.GetAsset<SoundEffect>("sndPortal").Play();
            }

            Vector2 camOffset = Vector2.UnitX * MathHelper.Clamp(_velocity.X / 1.5f, -40.0f, 40.0f);
            camOffset = new Vector2(8, 9);
            // Sprite Renderer
            if (MathF.Abs(_velocity.X) > 10f)
            {
                _renderer.Animation = "walk";
                _renderer.SpriteEffects = (MathF.Sign(_velocity.X) == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                if(onGround)
                {
                    Vector2 spawnPos = RandomHelper.RandomPosition(new Rectangle((int)_collider.Shape.Left, (int)_collider.Shape.Bottom - 2, (int)_collider.Shape.Right - (int)_collider.Shape.Left, 2));
                    Scene.GetParticleSystem<Dust>().PlaceDust(spawnPos);
                }
            }
            else _renderer.Animation = "idle";

            Camera.Position = HonasMathHelper.LerpDelta(Camera.Position, _transform.Position - new Vector2(Camera.CameraSize.X / 2.0f, Camera.CameraSize.Y / 1.8f) + camOffset, 0.2f, gameTime);
            Camera.Position = new Vector2(MathF.Round(Camera.Position.X), MathF.Round(Camera.Position.Y));

            if(_transform.Position.Y - 24.0f > Camera.Bounds.Height)
            {
                Destroy();
                Scene.AddEntity(new RoomTransition(false));
                AssetLibrary.GetAsset<SoundEffect>("sndDeath").Play();
            }

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {

        }
    }
}
