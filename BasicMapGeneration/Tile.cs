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

        public Tile()
        {
            verts[0] = new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector2(1.0f, 1.0f));
            verts[1] = new Vertex(new Vector3(-0.5f, +0.5f, 0.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector2(1.0f, 0.0f));
            verts[2] = new Vertex(new Vector3(+0.5f, +0.5f, 0.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector2(0.0f, 0.0f));
            verts[3] = new Vertex(new Vector3(+0.5f, +0.5f, 0.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector2(0.0f, 0.0f));
            verts[4] = new Vertex(new Vector3(+0.5f, -0.5f, 0.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector2(0.0f, 1.0f));
            verts[5] = new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector2(1.0f, 1.0f));
        }
    }
}
