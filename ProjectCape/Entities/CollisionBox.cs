using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectCape.Entities
{
    public class CollisionBox : Entity
    {
        public CollisionBox(float x, float y, float w, float h, bool oneWay)
        {
            var transform = new Transform2D(this) { Position = new Microsoft.Xna.Framework.Vector2(x, y) };
            var collider = new Collider2D(this) { Shape = new BoundingRectangle(w, h), Tag = Globals.TAG_SOLID, Transform = transform };
            if (oneWay) collider.Tag = Globals.TAG_ONE_WAY;
        }

        protected override void Cleanup()
        {
            
        }
    }
}
