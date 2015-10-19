using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;

namespace BasicMapGeneration
{
    class Map
    {
        const int WIDTH = 1;
        const int HEIGHT = 1;

        Tile tile = new Tile();
        public float[] data = new float[54];

        public Map()
        {
            FillData();
        }

        public void FillData()
        {
            for (int i = 0; i < 6; i++)
            {
                data[(i*9)+0] = tile.verts[i].position.X;
                data[(i*9)+1] = tile.verts[i].position.Y;
                data[(i*9)+2] = tile.verts[i].position.Z;
                data[(i*9)+3] = tile.verts[i].color.X;
                data[(i*9)+4] = tile.verts[i].color.Y;
                data[(i*9)+5] = tile.verts[i].color.Z;
                data[(i*9)+6] = tile.verts[i].color.W;
                data[(i*9)+7] = tile.verts[i].uv.X;
                data[(i*9)+8] = tile.verts[i].uv.Y;
            }
        }

        public float[] GetMap()
        {
            return data;
        }
    }
}
