using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.ECS.Components.Physics;
using Microsoft.Xna.Framework;

namespace ProjectCape.Entities.Environment
{
    public class Jewel : Entity
    {
        public Jewel(float x, float y)
        {
            var t = new Transform2D(this) { Position = new Vector2(x, y) };
            new SpriteRenderer(this) { Sprite = AssetLibrary.GetAsset<Sprite>("sprJewel"), Animation = "default" };
            new Collider2D(this) { Shape = new BoundingRectangle(8, 10) { Offset = new Vector2(4, 3) }, Transform = t, Tag = Globals.TAG_JEWEL };
        }

        protected override void Cleanup()
        {

        }
    }
}
