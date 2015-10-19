using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace BasicMapGeneration
{
    class Vertex
    {

        public Vector3 position;
        public Vector4 color;
        public Vector2 uv;

        public Vertex(Vector3 p, Vector4 c, Vector2 u)
        {
            position = p;
            color = c;
            uv = u;
        }

    }
}
