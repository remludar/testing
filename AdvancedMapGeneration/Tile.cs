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
        public static int VERTEX_COUNT = 4;

        public Vertex[] verts = new Vertex[VERTEX_COUNT];

        public Tile(float x, float y, float type)
        {
            verts[0] = new Vertex(x, y, type);            verts[0].uv.SetUV(1.0f, 1.0f);
            verts[1] = new Vertex(x, y + 1, type);        verts[1].uv.SetUV(1.0f, 0.0f);
            verts[2] = new Vertex(x + 1, y + 1, type);    verts[2].uv.SetUV(0.0f, 0.0f);
            verts[3] = new Vertex(x + 1, y, type);        verts[3].uv.SetUV(0.0f, 1.0f);
        }

        public float[] GetFloats()
        {
            float[] retVal = new float[VERTEX_COUNT * Vertex.FLOATS_PER_VERTEX];

            for (int i = 0; i < verts.Length; i++)
            {
                retVal[0 + i * 10] = verts[i].pos.x;
                retVal[1 + i * 10] = verts[i].pos.y;
                retVal[2 + i * 10] = verts[i].pos.z;
                                  
                retVal[3 + i * 10] = verts[i].col.r;
                retVal[4 + i * 10] = verts[i].col.g;
                retVal[5 + i * 10] = verts[i].col.b;
                retVal[6 + i * 10] = verts[i].col.a;
                                 
                retVal[7 + i * 10] = verts[i].uv.u;
                retVal[8 + i * 10] = verts[i].uv.v;

                retVal[9 + i * 10] = verts[i].tile.type;
            }

            return retVal;
        }
    }
}
