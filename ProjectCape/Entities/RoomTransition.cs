using HonasGame;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Entities
{
    public class RoomTransition : Entity
    {
        private Transform2D _transform;
        private int _width, _height;
        private bool _goToRoom;
        private bool _nextRoom;

        public RoomTransition(bool nextRoom)
        {
            _transform = new Transform2D(this) { Position = new Vector2(Camera.CameraSize.X, 0.0f) };
            Persistent = true;
            Depth = int.MaxValue;
            _width = (int)Camera.CameraSize.X + 60;
            _height = (int)Camera.CameraSize.Y;
            _goToRoom = true;
            _nextRoom = nextRoom;
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _transform.Position -= Vector2.UnitX * 1200.0f * t;

            if(_goToRoom && _transform.Position.X <= 0.0f)
            {
                _goToRoom = false;
                if (_nextRoom) RoomManager.GotoNextLevel();
                else RoomManager.GotoLevel();
            }
            else if(_transform.Position.X <= -_width)
            {
                Destroy();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawFilledRectangle(new Rectangle((int)(Camera.Position.X + _transform.Position.X), (int)(Camera.Position.Y + _transform.Position.Y), _width, _height), Color.Black);

            base.Draw(gameTime, spriteBatch);
        }

        protected override void Cleanup()
        {

        }
    }
}
