using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace BasicMapGeneration
{
    class Tile
    {

        public Vertex[] verts = new Vertex[4];

        public Tile(float x, float y)
        {
            verts[0] = new Vertex(x, y);            verts[0].uv.SetUV(1.0f, 1.0f);
            verts[1] = new Vertex(x, y + 1);        verts[1].uv.SetUV(1.0f, 0.0f);
            verts[2] = new Vertex(x + 1, y + 1);    verts[2].uv.SetUV(0.0f, 0.0f);
            verts[3] = new Vertex(x + 1, y);        verts[3].uv.SetUV(0.0f, 1.0f);
        }

        public float[] GetFloats()
        {
            float[] retVal = new float[36];

            for (int i = 0; i < verts.Length; i++)
            {
                retVal[0 + i * 9] = verts[i].pos.x;
                retVal[1 + i * 9] = verts[i].pos.y;
                retVal[2 + i * 9] = verts[i].pos.z;
                                   
                retVal[3 + i * 9] = verts[i].col.r;
                retVal[4 + i * 9] = verts[i].col.g;
                retVal[5 + i * 9] = verts[i].col.b;
                retVal[6 + i * 9] = verts[i].col.a;
                                  
                retVal[7 + i * 9] = verts[i].uv.u;
                retVal[8 + i * 9] = verts[i].uv.v;

                Console.WriteLine(0 + i * 9);
                Console.WriteLine(1 + i * 9);
                Console.WriteLine(2 + i * 9);
                                          
                Console.WriteLine(3 + i * 9);
                Console.WriteLine(4 + i * 9);
                Console.WriteLine(5 + i * 9);
                Console.WriteLine(6 + i * 9);
                                          
                Console.WriteLine(7 + i * 9);
                Console.WriteLine(8 + i * 9);

            }

            return retVal;
        }
    }
}
