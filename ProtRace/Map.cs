using Microsoft.Xna.Framework;
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
        Vector3 size = new Vector3(16, 16, 16);
        int[,,] data = new int[2, 1, 10];
        BasicEffect basicEffect;
        VertexBuffer vertexBuffer;
        List<VertexPositionColor> vertices;

        public Map(GraphicsDevice graphDevice)
        {
            vertices = new List<VertexPositionColor>();

            basicEffect = new BasicEffect(graphDevice);

            for (int i = 0; i < data.GetLength(0); ++i)
                for (int j = 0; j < data.GetLength(2); ++j)
                {
                    FillVerticesBot(vertices, new Vector3(i * size.X, -3f, j * -size.Z) + new Vector3(-size.X * data.GetLength(0) / 2, 0, 0), Color.OliveDrab);

                    if(i == 0)
                        FillVerticesWallStraight(vertices, new Vector3(i * size.X, -3f, j * -size.Z) + new Vector3(-size.X * data.GetLength(0) / 2, 0,0), Color.SaddleBrown);
                    else if(i == data.GetLength(0) - 1)
                        FillVerticesWallStraight(vertices, new Vector3((i + 1) * size.X, -3f, j * -size.Z) + new Vector3(-size.X * data.GetLength(0) / 2, 0, 0), Color.SaddleBrown);
                }

            vertexBuffer = new VertexBuffer(graphDevice, typeof(VertexPositionColor), vertices.Count, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices.ToArray());
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

        void FillVerticesWallStraight(List<VertexPositionColor> vertices, Vector3 vect, Color clr)
        {
            vertices.Add(new VertexPositionColor(vect, clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, size.Y, 0), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, 0, -size.Z), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, size.Y, 0), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, size.Y, -size.Z), clr));
            vertices.Add(new VertexPositionColor(vect + new Vector3(0, 0, -size.Z), clr));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GraphicsDevice graphDevice, Matrix view, Matrix proj, Matrix world)
        {
            basicEffect.World = Matrix.Identity;
            basicEffect.View = view;
            basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 1f, 0.1f, 200f);
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
