using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
//X-positiv links
//Y-positiv oben
//Z-positiv hinten
namespace ProtRace
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont CoinFont;
        Map map;
        Model model;
        List<Box> boxofboxes = new List<Box>();
        List<Coin> CoinList= new List<Coin>();
        float rotate; //rotate wheels
        private Vector3 position;
        private Matrix world= Matrix.Identity;
        private Matrix view = Matrix.Identity;
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 500f);
        float v; //Geschwindigkeit
        float a;//Beschleunigung
        float maxv; // Max-Geschwindigkeit
        bool up,down,left,right;
        int CoinCounter;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
                //graphics.IsFullScreen = true;
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
            rotate = 0;CoinCounter = 0;
            v = 0f; a = 0.0075f; maxv = 3f; up = false; down = false; left = false; right = false;
           boxofboxes.Add(new Box(new Vector3(0, 0, -20), 2f));
            boxofboxes.Add(new Box(new Vector3(10, 0,-100 )));
           boxofboxes.Add(new Box(new Vector3(-8, 0, -20)));
            boxofboxes.Add(new Box(new Vector3(0, 0, -100),3f));
            foreach (Box box in boxofboxes)
                box.Initialize(Content);

            map = new Map(GraphicsDevice);

            CoinList.Add(new Coin(new Vector3(0, 0, -50)));
            foreach (Coin coin in CoinList)
                coin.Initialize(Content);

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
            model = Content.Load<Model>("robot");
            CoinFont = Content.Load<SpriteFont>("Score");
            //  box = Content.Load<Model>("box");
            // TODO: use this.Content to load your game content here
          
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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

            map.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState state = Keyboard.GetState();
            Keys[] pressed = state.GetPressedKeys();
            down = false;
            up = false;
            left = false;
            right = false;
            foreach (Keys key in pressed)
            {
                if (key.Equals(Keys.W) || key.Equals(Keys.Up)) up = true;
                if (key.Equals(Keys.S) || key.Equals(Keys.Down)) down = true;
                if (key.Equals(Keys.A) || key.Equals(Keys.Left)) left = true;
                if (key.Equals(Keys.D) || key.Equals(Keys.Right)) right = true;
            }
            if (up == true)
            {

                if (v < maxv) v += a;

                position -= new Vector3(-rotate * 2 * v, 0, v);
            }


            if (down == true)
            {
                //invertieren?
                if (right == true)
                    rotate = 0.1f;
                else if (left == true)
                    rotate = -0.1f;
                else
                    rotate = 0f;

                if (v > 0)
                {
                    v -= 3 * a;
                
                    position -= new Vector3(rotate * 2 * v, 0, v);//1f

                }
                else
                {


        
                    position += new Vector3(rotate, 0, 0.25f);
                 
                }
            }

            if (right == true)
            {
                rotate = 0.2f;
            }
            else if (left == true)
            {
                rotate = -0.2f;

            }
            else rotate = 0f;

            //nicht gedrückt             
            //werde langsamer
            if (v > 0 + a && up == false && down == false)
            {
                v -= a;

                position -= new Vector3(-rotate * 2 * v, 0, v);//1f
            }

            foreach (Coin coin in CoinList)
                CoinCounter+=coin.Kollision(position);
            //if (position.Z==-100f) Exit();

            foreach (Box box in boxofboxes)
            {

             

                //Frontkollision

                if ((position.Z - box.getPos().Z - 3f * box.getScale() < 0.5) && (position.Z - box.getPos().Z - 3f * box.getScale() > 0) &&
      (position.X - box.getPos().X) < 2.1f+(box.getScale()-1)*1.4 && (position.X - box.getPos().X) > -2.5f - (box.getScale() - 1) * 1.8)
                {
                    position += new Vector3(0, 0, v);
                    v = 0;
                }

                //Rechte Seite Kollision

                if ((position.X - box.getPos().X < 1.75 + (box.getScale() - 1) * 1.6) && (position.X - box.getPos().X > 0) &&
      (position.Z - box.getPos().Z) < 2.1f + (box.getScale() - 1) * 3 && (position.Z - box.getPos().Z) > -2.5f - (box.getScale() - 1) * 1.55)
                {
                    position += new Vector3(-rotate * 2 * v, 0, v);
                    v = 0;
                }
        

                  //Linke Seite Kollision

                  if ((position.X - box.getPos().X > -2.5 - (box.getScale() - 1) * 1.9) && (position.X - box.getPos().X < 0) &&
      (position.Z - box.getPos().Z) < 2.1f + (box.getScale() - 1) * 4 && (position.Z - box.getPos().Z) > -2.5f- (box.getScale() - 1) * 1.55)
                  {
                  position -= new Vector3(rotate * 2 * v, 0, v);
                  v = 0;
              }
              
              //Backkollision

              if ((position.Z - box.getPos().Z + 3f > -0.5 - (box.getScale() - 1) * 3) && (position.Z - box.getPos().Z + 3f + (box.getScale() - 1) * 2.5 < 0) &&
    (position.X - box.getPos().X) < 1.6f +(box.getScale() - 1) * 1.6 && (position.X - box.getPos().X) > -2.0f -(box.getScale() - 1) * 2.5)
              {
                
                position += new Vector3(rotate, 0, -0.25f);
                    v = 0;
                }
              
                view = Matrix.CreateLookAt(new Vector3(0 + position.X, position.Y + 2, position.Z + 8), new Vector3(0 + position.X, position.Y, position.Z - 5), Vector3.UnitY);
            base.Update(gameTime);
        }
    }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            // A model is composed of "Meshes" which are
            // parts of the model which can be positioned
            // independently, which can use different textures,
            // and which can have different rendering states
            // such as lighting applied.
         

            float aspectRatio =
           graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;

          
            //draw player

            map.Draw(GraphicsDevice, view, projection, world);
          

            DrawModel(model, world, view, projection);
            //draw box
            foreach (Box box in boxofboxes)
                box.Draw(view);
            foreach (Coin coin in CoinList)
                coin.Draw(view);


            // GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
           //  GraphicsDevice.BlendState = BlendState.Opaque;
            // GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            spriteBatch.Begin();
            spriteBatch.DrawString(CoinFont, "Score: " + CoinCounter, new Vector2(50, 50), Color.Black);
            spriteBatch.End();
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            base.Draw(gameTime);
          
        }
 
        private void DrawModel(Model model, Matrix world, Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = effect.World = Matrix.CreateTranslation(position);
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }

    }
}
