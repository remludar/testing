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

        List<Tile> tiles = new List<Tile>();
        public float[] data = new float[270];

        public Map()
        {
            ReadData();
            FillData();
        }

        public void ReadData()
        {
            for (int i = 0; i < 4; i++)
            {
                Vector3[] pos = new Vector3[]{
                    new Vector3(+0.0f + i, +0.0f, 0.0f),
                    new Vector3(+0.0f + i, +1.0f, 0.0f),
                    new Vector3(+1.0f + i, +1.0f, 0.0f),
                    new Vector3(+1.0f + i, +1.0f, 0.0f),
                    new Vector3(+1.0f + i, +0.0f, 0.0f),
                    new Vector3(+0.0f + i, +0.0f, 0.0f),
                };

                Vector4[] col = new Vector4[]{
                    new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                    new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                    new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                    new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                    new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                    new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                };

                Vector2[] uv = new Vector2[]{
                    new Vector2(1.0f, 1.0f),
                    new Vector2(1.0f, 0.0f),
                    new Vector2(0.0f, 0.0f),
                    new Vector2(0.0f, 0.0f),
                    new Vector2(0.0f, 1.0f),
                    new Vector2(1.0f, 1.0f),
                };

                tiles.Add(new Tile(pos, col, uv));
            }
            
        }

        public void FillData()
        {
            for (int j = 0; j < tiles.Count; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    data[j * 54 + (i * 9) + 0] = tiles[j].verts[i].position.X;
                    data[j * 54 + (i * 9) + 1] = tiles[j].verts[i].position.Y;
                    data[j * 54 + (i * 9) + 2] = tiles[j].verts[i].position.Z;
                    data[j * 54 + (i * 9) + 3] = tiles[j].verts[i].color.X;
                    data[j * 54 + (i * 9) + 4] = tiles[j].verts[i].color.Y;
                    data[j * 54 + (i * 9) + 5] = tiles[j].verts[i].color.Z;
                    data[j * 54 + (i * 9) + 6] = tiles[j].verts[i].color.W;
                    data[j * 54 + (i * 9) + 7] = tiles[j].verts[i].uv.X;
                    data[j * 54 + (i * 9) + 8] = tiles[j].verts[i].uv.Y;

                    //Console.WriteLine(j * 54 + (i * 9));
                }
            }
        }

        public float[] GetMap()
        {
            return data;
        }
    }
}
