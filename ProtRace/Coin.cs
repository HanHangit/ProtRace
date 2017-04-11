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
    class Coin
    {
        Model model;
        Vector3 pos;
        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("coin");

        }
        public Vector3 getPos()
        {
            return pos;
        }
        public Coin(Vector3 position)
        {
            pos = position;
        }

        public void Draw(Matrix view)
        {

            if(pos != new Vector3(0, -50, 0)) // nicht mehr draw wenn eingesammelt
            //draw box
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = Matrix.CreateScale(0.15f) * Matrix.CreateTranslation(pos);

                    effect.View = view;

                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 1f, 0.1f, 200f);

                }

                mesh.Draw();
            }
        }
public int Kollision(Vector3 position)
        {
            if (System.Math.Abs(position.X - pos.X) < 2.05 &&
      System.Math.Abs(position.Y - pos.Y) < 2.05 &&
      System.Math.Abs(position.Z - pos.Z) < 2.05) { 
                pos = new Vector3(0, -50, 0);//unter der Map
                
                return 1;
            }
            return 0;
        }
    }
}
