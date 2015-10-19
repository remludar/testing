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

        public Vertex[] verts = new Vertex[6];

        public Tile(Vector3[] pos, Vector4[] col, Vector2[] uv)
        {
            for (int i = 0; i < 6; i++)
            {
                verts[i] = new Vertex(pos[i], col[i], uv[i]);
            }
            
        }
    }
}
