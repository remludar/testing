using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using System.IO;

namespace BasicMapGeneration
{
    class Map
    {
        const int WIDTH = 10;
        const int HEIGHT = 5;

        Tile[] tiles = new Tile[WIDTH * HEIGHT];
        float[] vertData = new float[Tile.VERTEX_COUNT * Vertex.FLOATS_PER_VERTEX * (WIDTH * HEIGHT)];
        int[] indexData = new int[6 * (WIDTH * HEIGHT)];

        public Map()
        {
            _ReadMapFromFile();
            _ConvertVertexDataToFloats();
        }

        private void _ConvertVertexDataToFloats()
        {
            var vertexDataBuffer = new float[Tile.VERTEX_COUNT * Vertex.FLOATS_PER_VERTEX];
            for (int i = 0; i < tiles.Length; i++)
            {
                vertexDataBuffer = tiles[i].GetFloats();
                vertexDataBuffer.CopyTo(vertData, i * Tile.VERTEX_COUNT * Vertex.FLOATS_PER_VERTEX);

                indexData[i * 6 + 0] = i * 4 + 0;
                indexData[i * 6 + 1] = i * 4 + 1;
                indexData[i * 6 + 2] = i * 4 + 2;
                indexData[i * 6 + 3] = i * 4 + 2;
                indexData[i * 6 + 4] = i * 4 + 3;
                indexData[i * 6 + 5] = i * 4 + 0;
            }
        }

        private void _ReadMapFromFile()
        {
            using (StreamReader reader = new StreamReader(@"Content\Map.txt"))
            {
                List<float> tileArray = new List<float>();
                while (!reader.EndOfStream)
                {
                    var tmpArray = reader.ReadLine().Split(',');
                    for (int i = 0; i < tmpArray.Length; i++)
                    {
                        if (!tmpArray[i].Equals("\r\n"))
                        {
                            tileArray.Add(Convert.ToSingle(tmpArray[i]));
                        }
                    }
                }
                tileArray.Reverse();
                for (int col = 0; col < HEIGHT; col++)
                {
                    for (int row = 0; row < WIDTH; row++)
                    {
                        tiles[row + col * WIDTH] = new Tile(row, col, tileArray[row + col * WIDTH]);
                    }
                }


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
