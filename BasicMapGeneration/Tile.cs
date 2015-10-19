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

        public Tile(float x, float y)
        {
            verts[0] = new Vertex(x, y);
            verts[1] = new Vertex(x, y + 1);
            verts[2] = new Vertex(x + 1, y + 1);
            verts[3] = new Vertex(x + 1, y + 1);
            verts[4] = new Vertex(x + 1, y);
            verts[5] = new Vertex(x, y);
        }
    }
}
