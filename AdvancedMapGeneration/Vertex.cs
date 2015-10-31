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
        public static int FLOATS_PER_VERTEX = 10;

        public struct Position
        {
            public float x, y, z;
        }

        public struct Color
        {
            public float r, g, b, a;
        }

        public struct UV
        {
            public float u, v;
            public void SetUV(float newU, float newV)
            {
                u = newU;
                v = newV;
            }
        }

        public struct TileType
        {
            public float type;
            public void SetTileType(float newType)
            {
                type = newType;
            }
        }

        public Position pos;
        public Color col;
        public UV uv;
        public TileType tile;

        public Vertex(float newX, float newY, float newType)
        {
            pos.x = newX;
            pos.y = newY;
            pos.z = 0;

            col.r = 1.0f;
            col.g = 1.0f;
            col.b = 1.0f;
            col.a = 1.0f;

            tile.type = newType;
        }
    }
}
