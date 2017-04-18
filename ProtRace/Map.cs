using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtRace
{
    class Map
    {
        public Vector3 size = new Vector3(64, 64, 64);
        int[,] data = {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1},
            {1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 2, 0, 2, 0, 0, 0, 3, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 1},
            {0, 2, 0, 2, 3, 3, 0, 0, 1, 1, 1, 1, 0, 0, 1, 0, 0, 3, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 0, 2, 0, 1, 1, 0, 0, 2, 0, 0, 1},
            {1, 1, 1, 1, 1, 2, 0, 2, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 2, 0, 0, 0, 0, 1, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1},
            {0, 0, 0, 0, 0, 1, 0, 0, 3, 0, 3, 0, 0, 2, 0, 3, 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 2, 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        };
        BasicEffect basicEffect;
        VertexBuffer vertexBuffer;
        List<VertexPositionColor> vertices;

        public Map(GraphicsDevice graphDevice, ContentManager content, List<Coin> CoinList, List<Box> boxList)
        {
            vertices = new List<VertexPositionColor>();

            basicEffect = new BasicEffect(graphDevice);

            for (int i = 0; i < data.GetLength(0) ; ++i)
                for (int j = 0; j < data.GetLength(1); ++j)
                {
                    FillVerticesBot(vertices, new Vector3(i * size.X, -3f, j * -size.Z), Color.OliveDrab);


                    if (data[i,j] == 1)
                        FillVerticesFullBlock(vertices, new Vector3(i * size.X, -3f, j * -size.Z), Color.SaddleBrown);
                    if (data[i, j] == 2)
                        CoinList.Add(new Coin(new Vector3(i * size.X, 0, j * -size.Z) + new Vector3(size.X / 2, 0, 0)));
                    if (data[i, j] == 3)
                        boxList.Add(new Box(new Vector3(i * size.X, 0, j * -size.Z) + new Vector3(size.X / 2, 0, 0)));
                }


            vertexBuffer = new VertexBuffer(graphDevice, typeof(VertexPositionColor), vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices.ToArray());
        }

        public bool Walkable(Vector3 position)
        {
            int x = Math.Max(0, (int)position.X / (int)size.X);
            int y = Math.Max(0, (int)position.Z / (int)size.Z);
            return data[(int)position.X / (int)size.X, (int)-position.Z / (int)size.Z] != 1;
        }


        void FillVerticesBot(List<VertexPositionColor> vertices, Vector3 vect, Color clr)
        {
            vertices.Add(new VertexPositionColor(vect, clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(size.X, 0, 0), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, 0, -size.Z), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(size.X, 0, 0), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(size.X, 0, -size.Z), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, 0, -size.Z), clr));
        }

        void FillVerticesFullBlock(List<VertexPositionColor> vertices, Vector3 vect, Color clr)
        {
            FillVerticesWallStraight(vertices, vect, clr);
            FillVerticesWallStraight(vertices, vect + new Vector3(size.X,0,0), clr);
            FillVerticesWallBlock(vertices, vect, clr);
            FillVerticesWallBlock(vertices, vect + new Vector3(0, 0, -size.Z), clr);
        }

        void FillVerticesWallStraight(List<VertexPositionColor> vertices, Vector3 vect, Color clr)
        {
            vertices.Add(new VertexPositionColor(vect, clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, size.Y, 0), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, 0, -size.Z), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, size.Y, 0), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, size.Y, -size.Z), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, 0, -size.Z), clr));
        }

        void FillVerticesWallBlock(List<VertexPositionColor> vertices, Vector3 vect, Color clr)
        {
            vertices.Add(new VertexPositionColor(vect, clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(size.X, 0, 0), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, size.Y, 0), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(size.X, 0, 0), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(size.X, size.Y, 0), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, size.Y, 0), clr));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GraphicsDevice graphDevice, Matrix view, Matrix proj, Matrix world)
        {
            basicEffect.World = Matrix.CreateTranslation(Vector3.Zero);
            basicEffect.View = view;
            basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 500f);
            basicEffect.VertexColorEnabled = true;

            graphDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, vertices.Count);
            }
        }
    }
}
