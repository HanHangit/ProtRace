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


        Map map;
        Model model;
        List<Box> boxofboxes = new List<Box>();
        float rotate; //rotate wheels
        private Vector3 position;
        private Matrix world= Matrix.Identity;
        private Matrix view = Matrix.Identity;
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 500f);
      
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
            rotate = 0; 
            boxofboxes.Add(new Box(new Vector3(0, 1, -20), 2f));
            boxofboxes.Add(new Box(new Vector3(10, 1,-10 )));
            boxofboxes.Add(new Box(new Vector3(-10, 1, -10)));
            foreach (Box box in boxofboxes)
                box.Initialize(Content);

            map = new Map(GraphicsDevice);

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

                        if (state.IsKeyDown(Keys.Right))
                        {
                           // position += new Vector3(1f, 0, 0);
                rotate = 0.7f;
                //world = Matrix.CreateTranslation(1f, 0, 0);
            }


                        else if (state.IsKeyDown(Keys.Left))
                        {
                            //position -= new Vector3(1f, 0, 0);
                  rotate = -0.7f;
               // world = Matrix.CreateTranslation(-1f, 0, 0);
            }
                        else
                        {
                rotate = 0;
                            // if (rotate > 0) rotate = rotate - 0.0075f;
                            //  if (rotate < 0) rotate = rotate + 0.0075f;
                        }

                        if (state.IsKeyDown(Keys.Up))
                        {
                if(rotate!=0)
                            position -= new Vector3(-rotate, 0, 0.5f);
                else
                    position -= new Vector3(-rotate, 0, 1f);
                // world = Matrix.CreateTranslation(0, 0, -1f);

            }
                        if (state.IsKeyDown(Keys.Down))
                        {
                if (rotate != 0)
                    position += new Vector3(-rotate, 0, 0.5f);
                else
                    position += new Vector3(-rotate, 0, 1f);
                //   world = Matrix.CreateTranslation(0, 0, 1f);
            }

     

            //2.5f*rotate
            view = Matrix.CreateLookAt(new Vector3(0 + position.X, position.Y +2, position.Z +8), new Vector3(0 + position.X, position.Y , position.Z - 5), Vector3.UnitY);
 
            //if (position.Z==-100f) Exit();
 
            foreach (Box box in boxofboxes)
                if (System.Math.Abs(box.getPos().X-position.X)<2.05*box.getScale() &&
       System.Math.Abs(box.getPos().Y - position.Y )< 2.05* box.getScale() &&
       System.Math.Abs(box.getPos().Z - position.Z )< 2.05* box.getScale()) Exit();

            base.Update(gameTime);
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
            
          
       
          
           // GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
           // GraphicsDevice.BlendState = BlendState.Opaque;
           // GraphicsDevice.DepthStencilState = DepthStencilState.Default;

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
