using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ProtRace
{
    public class Box
    {
        Model model;
        Vector3 pos;
        float scale;
        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("box");

        }
        public Vector3 getPos()
        {
            return pos;
        }
        public float getScale()
        {
            return scale;
        }


        public Box(Vector3 position)
        {
            pos = position;
            scale = 1f;
        }

        public Box(Vector3 position, float scalefaktor)
        {
            pos = position;
            scale = scalefaktor;
        }

        public void Draw(Matrix view)
        {
    

            //draw box
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                  
                    effect.World =Matrix.CreateScale(scale)* Matrix.CreateTranslation(pos);
  
                    effect.View = view;

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 500f);

                }

                mesh.Draw();
            }
        }
    }

}