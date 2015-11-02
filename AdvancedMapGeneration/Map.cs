using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace BasicMapGeneration
{
    class Map
    {
        const int WIDTH = 5;
        const int HEIGHT = 3;
        const int TEXTURE_COUNT = 10;
        

        int vertexVBO;
        Bitmap[] bitmaps = new Bitmap[TEXTURE_COUNT];
        BitmapData[] bitmapData = new BitmapData[TEXTURE_COUNT];

        Tile[] tiles = new Tile[WIDTH * HEIGHT];
        float[] vertData = new float[Tile.VERTEX_COUNT * Vertex.FLOATS_PER_VERTEX * (WIDTH * HEIGHT)];
        int[] indexData = new int[6 * (WIDTH * HEIGHT)];

        public Map()
        {
            _LoadTextures();
            _ReadMapFromFile();
            _ConvertVertexDataToFloats();
        }

        private void _LoadTextures()
        {
            GL.GenBuffers(1, out vertexVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexVBO);

            //Texture file paths
            string[] paths = new string[]{
                @"Content\Textures\grass_top_left.png",
                @"Content\Textures\grass_top_mid.png",
                @"Content\Textures\grass_top_right.png",
                @"Content\Textures\grass_mid_left.png",
                @"Content\Textures\grass_mid.png", 
                @"Content\Textures\grass_mid_right.png",
                @"Content\Textures\grass_bottom_left.png",
                @"Content\Textures\grass_bottom_mid.png", 
                @"Content\Textures\grass_bottom_right.png",
                @"Content\Textures\grass_alt_mid.png"
            };

            //Read textures in and track the bytes.
            int[] tileBytes = new int[TEXTURE_COUNT];
            int totalBytes = 0;
            for (int i = 0; i < TEXTURE_COUNT; i++)
            {
                Utilities.TextureLoader.LoadTexture(paths[i], out bitmaps[i], out bitmapData[i]);
                tileBytes[i] = bitmapData[i].Stride * bitmaps[i].Height;
                totalBytes += tileBytes[i];
            }
           
            //Copy all the texture data for each texture into a single byte array
            byte[] texelBytes = new byte[totalBytes];
            int startingPoint = 0;
            for (int i = 0; i < TEXTURE_COUNT; i++)
            {
                startingPoint = i * tileBytes[i];
                Marshal.Copy(bitmapData[i].Scan0, texelBytes, startingPoint, tileBytes[i]);
            }
            
            int textureWidth = bitmapData[0].Width;
            int textureHeight = bitmapData[0].Height;

            GL.TexImage3D(TextureTarget.Texture2DArray,
                          0,
                          PixelInternalFormat.Rgba8,
                          textureWidth,
                          textureHeight,
                          TEXTURE_COUNT,
                          0,
                          OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                          PixelType.UnsignedByte,
                          texelBytes
                          );

            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
        }

        private void _ReadMapFromFile()
        {
            using (StreamReader reader = new StreamReader(@"Content\Map.txt"))
            {
                List<float> mapTileList = new List<float>();
                while (!reader.EndOfStream)
                {
                    var tmpArray = reader.ReadLine().Split(',');
                    for (int i = 0; i < tmpArray.Length; i++)
                    {
                        if (!tmpArray[i].Equals("\r\n"))
                        {
                            mapTileList.Add(Convert.ToSingle(tmpArray[i]));
                        }
                    }
                }

                int topDownModifier = HEIGHT - 1;
                
                for (int col = 0; col < HEIGHT; col++)
                {
                    for (int row = 0; row < WIDTH; row++)
                    {
                        tiles[row + col * WIDTH] = new Tile(row, topDownModifier - col, mapTileList[row + col * WIDTH]);
                    }
                }
            }
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


        public void Draw()
        {
            int lastTileDrawn = 0;
            for (int i = 9; i < tiles.Length * Vertex.FLOATS_PER_VERTEX * Tile.VERTEX_COUNT; i += Vertex.FLOATS_PER_VERTEX * Tile.VERTEX_COUNT)
            {
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);

                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexVBO);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertData.Length * sizeof(float)), vertData, BufferUsageHint.StaticDraw);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 10, 0);
                GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 10, 3 * sizeof(float));
                GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, sizeof(float) * 10, 7 * sizeof(float));
                int[] indices = new int[6];
                for (int index = 0; index < 6; index++)
                {
                    indices[index] = indexData[index + lastTileDrawn * 6];
                }

                GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, indices);

                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                GL.DisableVertexAttribArray(0);
                GL.DisableVertexAttribArray(1);
                GL.DisableVertexAttribArray(2);
                lastTileDrawn++;
            }
        }

    }
}
