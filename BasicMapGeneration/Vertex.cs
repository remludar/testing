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

        public struct Position
        {
            public float x, y;
        }

        public struct Color
        {
            public float r, g, b, a;
        }

        public struct UV
        {
            public float u, v;
        }

        public Position pos;
        public Color col;
        public UV uv;

        public Vertex(float newX, float newY)
        {
            pos.x = newX;
            pos.y = newY;

            col.r = 1.0f;
            col.g = 1.0f;
            col.b = 1.0f;
            col.a = 1.0f;
        }
    }
}
