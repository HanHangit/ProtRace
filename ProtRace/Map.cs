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
        Vector3 size = new Vector3(64, 64, 64);
        int[,,] data = new int[5,5,1];
        BasicEffect basicEffect;
        VertexBuffer vertexBuffer;
        VertexPositionColor[] vertices;

        public Map(GraphicsDevice graphDevice)
        {
            vertices = new VertexPositionColor[6 * data.GetLength(0) * data.GetLength(1)];

            basicEffect = new BasicEffect(graphDevice);

            for(int i = 0; i < data.GetLength(1); ++i)
                for(int j = 0; j < data.GetLength(0); ++j)
            {
                    FillVerticesBot(vertices, i * data.GetLength(0) * 6 + j * 6, new Vector3(i / 6 * size.X,-3f,0) + new Vector3(0,0,0),Color.OliveDrab);
            }

            vertexBuffer = new VertexBuffer(graphDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData(vertices);
        }


        void FillVerticesBot(VertexPositionColor[] vertices, int start, Vector3 vect, Color clr)
        {
            vertices[start] = new VertexPositionColor(vect, Color.OliveDrab);
            vertices[start + 1] = new VertexPositionColor(vect + new Vector3(size.X, 0, 0), clr);
            vertices[start + 2] = new VertexPositionColor(vect + new Vector3(0, 0, -size.Z), clr);
            vertices[start + 3] = new VertexPositionColor(vect + new Vector3(size.X, 0, 0), clr);
            vertices[start + 4] = new VertexPositionColor(vect + new Vector3(size.X, 0, -size.Z), clr);
            vertices[start + 5] = new VertexPositionColor(vect + new Vector3(0, 0, -size.Z), clr);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GraphicsDevice graphDevice, Matrix view, Matrix proj, Matrix world)
        {
            basicEffect.World = world;
            basicEffect.View = view;
            basicEffect.Projection = proj;
            basicEffect.VertexColorEnabled = true;

            graphDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphDevice.RasterizerState = rasterizerState;

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }
        }
    }
}
