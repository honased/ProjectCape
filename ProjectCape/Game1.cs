using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.JSON;
using HonasGame.Tiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ProjectCape.Entities;
using ProjectCape.Entities.Enemies;
using ProjectCape.Entities.Environment;
using ProjectCape.Entities.Player;
using ProjectCape.Particles;

namespace ProjectCape
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            //_graphics.IsFullScreen = true;

            Camera.CameraSize = new Vector2(320, 180);
            Camera.Bounds = new Rectangle(0, 0, 1000000, 1000000);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            AssetLibrary.AddAsset("player", Content.Load<Texture2D>("sprites/player/player"));
            AssetLibrary.AddAsset("spider", Content.Load<Texture2D>("sprites/enemies/Spider"));
            AssetLibrary.AddAsset("orc", Content.Load<Texture2D>("sprites/enemies/Orc"));
            AssetLibrary.AddAsset("mountains", Content.Load<Texture2D>("sprites/bg/mountains"));
            AssetLibrary.AddAsset("bushes", Content.Load<Texture2D>("sprites/bg/bushes"));
            AssetLibrary.AddAsset("sky", Content.Load<Texture2D>("sprites/bg/Sky"));
            AssetLibrary.AddAsset("circle", Content.Load<Texture2D>("sprites/particles/circle"));
            AssetLibrary.AddAsset("square", Content.Load<Texture2D>("sprites/particles/square"));
            AssetLibrary.AddAsset("jewel", Content.Load<Texture2D>("sprites/environment/Jewel"));
            AssetLibrary.AddAsset("portal", Content.Load<Texture2D>("sprites/environment/Portal"));
            AssetLibrary.AddAsset("tileGrass", Content.Load<Texture2D>("maps/tilesets/tileGrass"));

            // Tilesets
            AssetLibrary.AddAsset("tilesetGrass", new TiledTileset(JSON.FromFile("Content/maps/tilesets/tilesetGrass.json") as JObject));

            // Rooms
            AssetLibrary.AddAsset("room_0_0", new TiledMap(JSON.FromFile("Content/maps/rooms/room_0_0.json") as JObject));
            AssetLibrary.AddAsset("room_0_1", new TiledMap(JSON.FromFile("Content/maps/rooms/room_0_1.json") as JObject));

            var sprite = new Sprite(AssetLibrary.GetAsset<Texture2D>("player"));
            sprite.Animations.Add("idle", SpriteAnimation.FromSpritesheet(1, 0.0, 0, 0, 16, 18));
            sprite.Animations.Add("walk", SpriteAnimation.FromSpritesheet(3, 0.1, 0, 0, 16, 18));
            AssetLibrary.AddAsset("sprPlayer", sprite);

            sprite = new Sprite(AssetLibrary.GetAsset<Texture2D>("spider"));
            sprite.Animations.Add("idle", SpriteAnimation.FromSpritesheet(3, 0.1, 0, 0, 32, 16));
            AssetLibrary.AddAsset("sprSpider", sprite);

            sprite = new Sprite(AssetLibrary.GetAsset<Texture2D>("orc"));
            sprite.Animations.Add("idle", SpriteAnimation.FromSpritesheet(1, 0.0, 0, 0, 16, 18));
            sprite.Animations.Add("walk", SpriteAnimation.FromSpritesheet(3, 0.1, 0, 0, 16, 18));
            AssetLibrary.AddAsset("sprOrc", sprite);

            sprite = new Sprite(AssetLibrary.GetAsset<Texture2D>("jewel"));
            sprite.Animations.Add("default", SpriteAnimation.FromSpritesheet(1, 0.0, 0, 0, 16, 16));
            AssetLibrary.AddAsset("sprJewel", sprite);

            sprite = new Sprite(AssetLibrary.GetAsset<Texture2D>("portal"));
            sprite.Animations.Add("default", SpriteAnimation.FromSpritesheet(1, 0.0, 0, 0, 16, 16));
            AssetLibrary.AddAsset("sprPortal", sprite);

            TiledManager.AddSpawnerDefinition("Player", obj => { return new Player(obj.X, obj.Y); });
            TiledManager.AddSpawnerDefinition("Spider", obj => { return new Spider(obj.X, obj.Y); });
            TiledManager.AddSpawnerDefinition("Orc", obj =>
            {
                var orc = new Orc(obj.X, obj.Y);
                if(orc.GetComponent<SpriteRenderer>(out var renderer))
                {
                    renderer.SpriteEffects = obj.FlippedHorizontally ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                }
                return orc;
            });

            TiledManager.AddSpawnerDefinition("Collision", obj => { return new CollisionBox(obj.X, obj.Y, obj.Width, obj.Height, false); });
            TiledManager.AddSpawnerDefinition("CollisionOneWay", obj => { return new CollisionBox(obj.X, obj.Y, obj.Width, obj.Height, true); });
            TiledManager.AddSpawnerDefinition("Jewel", obj => { return new Jewel(obj.X, obj.Y); });
            TiledManager.AddSpawnerDefinition("Portal", obj => { return new Portal(obj.X, obj.Y); });

            TiledManager.Goto(AssetLibrary.GetAsset<TiledMap>("room_0_0"));

            Scene.AddParticleSystem(new Dust());
            Scene.AddParticleSystem(new Blood());
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(Input.IsKeyPressed(Keys.R))
            {
                RoomManager.GotoLevel();
            }

            // TODO: Add your update logic here
            Scene.Update(gameTime);

            Camera.Position += 2 * Vector2.UnitY;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Scene.Draw(gameTime, _spriteBatch, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));

            base.Draw(gameTime);
        }
    }
}
