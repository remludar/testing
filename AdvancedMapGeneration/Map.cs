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
        const int WIDTH = 10;
        const int HEIGHT = 5;

        int vertexVBO;
        Bitmap floorBMP, wallBMP;
        BitmapData floorBMPData, wallBMPData;
        int textureCount = 2;

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

            Utilities.TextureLoader.LoadTexture(@"Content\Textures\floor.png", out floorBMP, out floorBMPData);
            Utilities.TextureLoader.LoadTexture(@"Content\Textures\wall.jpg", out wallBMP, out wallBMPData);

            int floorbytes = (floorBMPData.Stride * floorBMP.Height);
            int wallbytes = (wallBMPData.Stride * wallBMP.Height);
            int bytes = (floorbytes + wallbytes);
            byte[] texelBytes = new byte[bytes];
            Marshal.Copy(floorBMPData.Scan0, texelBytes, 0, floorbytes);
            Marshal.Copy(wallBMPData.Scan0, texelBytes, floorbytes, wallbytes);
            int textureWidth = floorBMPData.Width;
            int textureHeight = floorBMPData.Height;

            GL.TexImage3D(TextureTarget.Texture2DArray,
                          0,
                          PixelInternalFormat.Rgba8,
                          textureWidth,
                          textureHeight,
                          textureCount,
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
