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
        const int WIDTH = 5;
        const int HEIGHT = 5;

        Tile[] tiles = new Tile[WIDTH * HEIGHT];
        float[] vertData = new float[36 * (WIDTH * HEIGHT)];
        int[] indexData = new int[6 * (WIDTH * HEIGHT)];

        public Map()
        {
            for (int col = 0; col < HEIGHT; col++)
            {
                for (int row = 0; row < WIDTH; row++)
                {
                    tiles[row + col * WIDTH] = new Tile(row, col);
                }
            }

            _ConvertVertexDataToFloats();
        }

        private void _ConvertVertexDataToFloats()
        {
            var vertexDataBuffer = new float[36];
            for (int i = 0; i < tiles.Length; i++)
            {
                vertexDataBuffer = tiles[i].GetFloats();
                vertexDataBuffer.CopyTo(vertData, i * 36);

                indexData[i * 6 + 0] = i * 4 + 0;
                indexData[i * 6 + 1] = i * 4 + 1;
                indexData[i * 6 + 2] = i * 4 + 2;
                indexData[i * 6 + 3] = i * 4 + 2;
                indexData[i * 6 + 4] = i * 4 + 3;
                indexData[i * 6 + 5] = i * 4 + 0;
            }
        }



        public float[] GetVertexData()
        {
            return vertData; 
        }

        public int[] GetIndexData()
        {
            return indexData;
        }

        public int GetTileCount()
        {
            return tiles.Length;
        }
    }
}
