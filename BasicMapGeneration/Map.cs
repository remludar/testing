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

        Tile[] tiles = new Tile[WIDTH * HEIGHT];
        float[] data = new float[36 * (WIDTH * HEIGHT)];

        public Map()
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = new Tile(0, 0); // TODO: hard coded to 0,0 temporarily. future will need to represent world x,y coords
            }

            _ConvertToFloats();
        }

        private void _ConvertToFloats()
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                data = tiles[i].GetFloats();
            }
        }

        public float[] GetData()
        {
            return data;
        }

        public int GetTileCount()
        {
            return tiles.Length;
        }
    }
}
