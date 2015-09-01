using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace WindowsGame5
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Model shipModel, bulletModel, enemyModel;
        Texture2D starfield;

        Matrix shipPos;
        List<Matrix> enemies = new List<Matrix>();
        List<Matrix> bullets = new List<Matrix>();

        Random rnd = new Random();

        KeyboardState oldKState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            shipModel = Content.Load<Model>("Models\\p2_wedge");
            shipPos = Matrix.CreateRotationY(MathHelper.ToRadians(180))
                * Matrix.CreateTranslation(0,-1000,0);

            starfield = Content.Load<Texture2D>("Textures\\B1_stars");
            enemyModel = Content.Load<Model>("Models\\p1_rocket");
            bulletModel = Content.Load<Model>("Models\\mgun_proj");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Left)) shipPos *= Matrix.CreateTranslation(100, 0, 0);
            if (kstate.IsKeyDown(Keys.Right)) shipPos *= Matrix.CreateTranslation(-100, 0, 0);
            if (kstate.IsKeyDown(Keys.Up)) shipPos *= Matrix.CreateTranslation(0, 0, 100);
            if (kstate.IsKeyDown(Keys.Down)) shipPos *= Matrix.CreateTranslation(0, 0, -100);

            if (kstate.IsKeyDown(Keys.Space) && !oldKState.IsKeyDown(Keys.Space ))
                bullets.Add(shipPos);

            oldKState = kstate;

            // TODO: Add your update logic here
            for (int i = bullets.Count-1; i >= 0; i--)
            {
                bullets[i] *= Matrix.CreateTranslation(0, 0, 200);
                if (bullets[i].Translation.Z > 20000) bullets.RemoveAt(i);
            }

            if (rnd.Next(42) == 1)
                enemies.Add(Matrix.CreateTranslation(rnd.Next(-4000, +4000), -1000, 20000));

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i] *= Matrix.CreateTranslation(0, 0, -100);
            }

            BoundingSphere bs1, bs2;

            bs1 = new BoundingSphere(shipPos.Translation, shipModel.Meshes[0].BoundingSphere.Radius *.9f);

            foreach (Matrix enemy in enemies)
            {
                bs2 = new BoundingSphere(enemy.Translation, enemyModel.Meshes[0].BoundingSphere.Radius * .9f);
                if (bs2.Intersects(bs1)) this.Exit();
            }

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bs1 = new BoundingSphere(bullets[i].Translation, bulletModel.Meshes[0].BoundingSphere.Radius);
                for (int j = enemies.Count - 1; j >= 0; j--)
                {
                    bs2 = new BoundingSphere(enemies[j].Translation, enemyModel.Meshes[0].BoundingSphere.Radius * .7f);
                    if (bs1.Intersects(bs2))
                    {
                        enemies.RemoveAt(j);
                        bullets.RemoveAt(i);
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(starfield, new Rectangle(0, 0, 800, 600), Color.White);
            spriteBatch.End();

            // TODO: Add your drawing code here

            
            
            foreach (Matrix bulletPos in bullets)
                DrawModel(bulletPos, bulletModel);

            foreach (Matrix enemyPos in enemies)
                DrawModel(enemyPos, enemyModel);

            DrawModel(shipPos, shipModel);


            base.Draw(gameTime);
        }

        private void DrawModel(Matrix worldMatrix, Model myModel)
        {
            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = worldMatrix;
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                        MathHelper.ToRadians(45), 
                        GraphicsDevice.DisplayMode.AspectRatio, 
                        1.0f, 
                        20000.0f);

                    effect.View = Matrix.CreateLookAt(new Vector3(0, 0, -5000),
                        Vector3.Zero,
                        Vector3.Up);

                    effect.FogEnabled = true;
                    effect.FogColor = Vector3.Zero;
                    effect.FogStart = 15000;
                    effect.FogEnd = 20000;

                    effect.EnableDefaultLighting();
                }
                mesh.Draw();
            }
        }
    }
}
