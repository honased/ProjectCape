using HonasGame;
using HonasGame.Assets;
using HonasGame.ECS;
using HonasGame.ECS.Components;
using HonasGame.JSON;
using HonasGame.Tiled;
using HonasGame.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ProjectCape.Entities;
using ProjectCape.Entities.Enemies;
using ProjectCape.Entities.Environment;
using ProjectCape.Entities.GUI;
using ProjectCape.Entities.Menus;
using ProjectCape.Entities.Player;
using ProjectCape.Particles;
using System.Diagnostics;

namespace ProjectCape
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Stopwatch _watch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Window.Title = "Forgotten Wisp";
            //_graphics.IsFullScreen = true;

            Camera.CameraSize = new Vector2(320, 180);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _watch = new Stopwatch();

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
            AssetLibrary.AddAsset("room_0_2", new TiledMap(JSON.FromFile("Content/maps/rooms/room_0_2.json") as JObject));
            AssetLibrary.AddAsset("room_menu", new TiledMap(JSON.FromFile("Content/maps/rooms/room_menu.json") as JObject));
            AssetLibrary.AddAsset("room_intro", new TiledMap(JSON.FromFile("Content/maps/rooms/room_intro.json") as JObject));
            AssetLibrary.AddAsset("room_outro", new TiledMap(JSON.FromFile("Content/maps/rooms/room_outro.json") as JObject));
            AssetLibrary.AddAsset("room_inbetween", new TiledMap(JSON.FromFile("Content/maps/rooms/room_inbetween.json") as JObject));
            AssetLibrary.AddAsset("room_madeby", new TiledMap(JSON.FromFile("Content/maps/rooms/room_madeby.json") as JObject));

            // Music
            AssetLibrary.AddAsset("musMenu", Content.Load<Song>("sfx/musMenu"));
            AssetLibrary.AddAsset("musForest", Content.Load<Song>("sfx/musForest"));
            AssetLibrary.AddAsset("musVoices", Content.Load<Song>("sfx/musVoices"));

            // SFX
            AssetLibrary.AddAsset("sndJump", Content.Load<SoundEffect>("sfx/sndJump"));
            AssetLibrary.AddAsset("sndJewel", Content.Load<SoundEffect>("sfx/sndJewel"));
            AssetLibrary.AddAsset("sndKill", Content.Load<SoundEffect>("sfx/sndKill"));
            AssetLibrary.AddAsset("sndPortal", Content.Load<SoundEffect>("sfx/sndPortal"));
            AssetLibrary.AddAsset("sndDeath", Content.Load<SoundEffect>("sfx/sndDeath"));
            AssetLibrary.AddAsset("sndTalk", Content.Load<SoundEffect>("sfx/sndTalk"));
            AssetLibrary.AddAsset("sndAlien1", Content.Load<SoundEffect>("sfx/sndAlien1"));
            AssetLibrary.AddAsset("sndAlien2", Content.Load<SoundEffect>("sfx/sndAlien2"));

            // Fonts
            AssetLibrary.AddAsset("fntText", Content.Load<SpriteFont>("fonts/fntText"));

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
            TiledManager.AddSpawnerDefinition("Menu", obj => { return new Menu(); });
            TiledManager.AddSpawnerDefinition("Intro", obj => { return new Intro(); });
            TiledManager.AddSpawnerDefinition("Outro", obj => { return new Outro(); });
            TiledManager.AddSpawnerDefinition("MadeBy", obj => { return new MadeBy(); });
            TiledManager.AddSpawnerDefinition("JewelDeposit", obj => { return new JewelDeposit(); });

            Scene.AddParticleSystem(new Dust());
            Scene.AddParticleSystem(new Blood());

            RoomManager.Goto("room_madeby");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Scene.Update(gameTime);
            SongManager.Update(gameTime);

            if(Scene.GetEntity<Menu>(out var menu))
            {
                if (menu.Quit) Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _watch.Restart();
            _watch.Start();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Scene.Draw(gameTime, _spriteBatch, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));

            _watch.Stop();
            _spriteBatch.Begin();
            //_spriteBatch.DrawFilledRectangle(new Rectangle(8, 8, 48, 16), Color.Black);
            //_spriteBatch.DrawString(AssetLibrary.GetAsset<SpriteFont>("fntText"), $"{(1000000000.0 * (double)_watch.ElapsedTicks / Stopwatch.Frequency) / 1000000.0}", new Vector2(10, 10), Color.Yellow);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
