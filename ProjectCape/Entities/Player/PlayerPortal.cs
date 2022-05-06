using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using HonasGame.Helper;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ProjectCape.Entities.Environment;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Entities.Player
{
    public class PlayerPortal : Entity
    {
        private Transform2D _transform;
        private SpriteRenderer _renderer;
        private Entity _portal;
        private bool _gotoNextRoom;

        private const float GRAVITY = 1000.0f;
        private const float JUMP_SPEED = 300.0f;

        public PlayerPortal(float x, float y, SpriteEffects effects, Entity portal)
        {
            _transform = new Transform2D(this) {Position = new Vector2(x + 8, y + 9)};
            _renderer = new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprPlayer"), Animation = "idle", SpriteEffects = effects };
            _renderer.CenterOrigin();
            _portal = portal;
            _gotoNextRoom = true;
            Globals.AddToTotalJewels = true;
        }

        public override void Update(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(_portal.GetComponent<Transform2D>(out var portalTransform))
            {
                _transform.Position = HonasMathHelper.LerpDelta(_transform.Position, portalTransform.Position, 0.2f, gameTime);
            }

            _renderer.Scale = HonasMathHelper.LerpDelta(_renderer.Scale, Vector2.Zero, 0.05f, gameTime);
            _renderer.Rotation = (_renderer.Rotation + (MathHelper.TwoPi * t)) % MathHelper.TwoPi;

            if(_renderer.Scale.X <= .1f && _gotoNextRoom)
            {
                Scene.AddEntity(new RoomTransition(true));
                _gotoNextRoom = false;

                if (RoomManager.CurrentLevel == 3) SongManager.TransitionSong(AssetLibrary.GetAsset<Song>("musMenu"), 0.5, false);
            }

            base.Update(gameTime);
        }

        protected override void Cleanup()
        {

        }
    }
}
