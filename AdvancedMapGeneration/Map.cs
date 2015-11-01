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

        int vertexVBO;
        Bitmap grassMidBMP, grassMidTopBMP, grassLeftTopBMP, grassRightTopBMP, grassLeftMidBMP, grassRightMidBMP, grassMidBottomBMP, grassLeftBottomBMP, grassRightBottomBMP;
        BitmapData grassMidBMPData, grassMidTopBMPData, grassLeftTopBMPData, grassRightTopBMPData, grassLeftMidBMPData, grassRightMidBMPData, grassMidBottomBMPData, grassLeftBottomBMPData, grassRightBottomBMPData;
        int textureCount = 9;

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

            Utilities.TextureLoader.LoadTexture(@"Content\Textures\grass_mid.png", out grassMidBMP, out grassMidBMPData);
            Utilities.TextureLoader.LoadTexture(@"Content\Textures\grass_top_mid.png", out grassMidTopBMP, out grassMidTopBMPData);
            Utilities.TextureLoader.LoadTexture(@"Content\Textures\grass_top_left.png", out grassLeftTopBMP, out grassLeftTopBMPData);
            Utilities.TextureLoader.LoadTexture(@"Content\Textures\grass_top_right.png", out grassRightTopBMP, out grassRightTopBMPData);
            Utilities.TextureLoader.LoadTexture(@"Content\Textures\grass_mid_left.png", out grassLeftMidBMP, out grassLeftMidBMPData);
            Utilities.TextureLoader.LoadTexture(@"Content\Textures\grass_mid_right.png", out grassRightMidBMP, out grassRightMidBMPData);
            Utilities.TextureLoader.LoadTexture(@"Content\Textures\grass_bottom_mid.png", out grassMidBottomBMP, out grassMidBottomBMPData);
            Utilities.TextureLoader.LoadTexture(@"Content\Textures\grass_bottom_left.png", out grassLeftBottomBMP, out grassLeftBottomBMPData);
            Utilities.TextureLoader.LoadTexture(@"Content\Textures\grass_bottom_right.png", out grassRightBottomBMP, out grassRightBottomBMPData);

            int tile_0_Bytes = (grassMidBMPData.Stride * grassMidBMP.Height);
            int tile_1_Bytes = (grassMidTopBMPData.Stride * grassMidTopBMP.Height);
            int tile_2_Bytes = (grassLeftTopBMPData.Stride * grassLeftTopBMP.Height);
            int tile_3_Bytes = (grassRightTopBMPData.Stride * grassRightTopBMP.Height);
            int tile_4_Bytes = (grassLeftMidBMPData.Stride * grassLeftMidBMP.Height);
            int tile_5_Bytes = (grassRightMidBMPData.Stride * grassRightMidBMP.Height);
            int tile_6_Bytes = (grassMidBottomBMPData.Stride * grassMidBottomBMP.Height);
            int tile_7_Bytes = (grassLeftBottomBMPData.Stride * grassLeftBottomBMP.Height);
            int tile_8_Bytes = (grassRightBottomBMPData.Stride * grassRightBottomBMP.Height);

            int bytes = (tile_0_Bytes + tile_1_Bytes + tile_2_Bytes + tile_3_Bytes + tile_4_Bytes + tile_5_Bytes + tile_6_Bytes + tile_7_Bytes + tile_8_Bytes);
            byte[] texelBytes = new byte[bytes];
            Marshal.Copy(grassMidBMPData.Scan0, texelBytes, 0, tile_0_Bytes);
            Marshal.Copy(grassMidTopBMPData.Scan0, texelBytes, tile_0_Bytes, tile_1_Bytes);
            Marshal.Copy(grassLeftTopBMPData.Scan0, texelBytes, tile_0_Bytes + tile_1_Bytes, tile_2_Bytes);
            Marshal.Copy(grassRightTopBMPData.Scan0, texelBytes, tile_0_Bytes + tile_1_Bytes + tile_2_Bytes, tile_3_Bytes);
            Marshal.Copy(grassLeftMidBMPData.Scan0, texelBytes, tile_0_Bytes + tile_1_Bytes + tile_2_Bytes + tile_3_Bytes, tile_4_Bytes);
            Marshal.Copy(grassRightMidBMPData.Scan0, texelBytes, tile_0_Bytes + tile_1_Bytes + tile_2_Bytes + tile_3_Bytes + tile_4_Bytes, tile_5_Bytes);
            Marshal.Copy(grassMidBottomBMPData.Scan0, texelBytes, tile_0_Bytes + tile_1_Bytes + tile_2_Bytes + tile_3_Bytes + tile_4_Bytes + tile_5_Bytes, tile_6_Bytes);
            Marshal.Copy(grassLeftBottomBMPData.Scan0, texelBytes, tile_0_Bytes + tile_1_Bytes + tile_2_Bytes + tile_3_Bytes + tile_4_Bytes + tile_5_Bytes + tile_6_Bytes, tile_7_Bytes);
            Marshal.Copy(grassRightBottomBMPData.Scan0, texelBytes, tile_0_Bytes + tile_1_Bytes + tile_2_Bytes + tile_3_Bytes + tile_4_Bytes + tile_5_Bytes + tile_6_Bytes + tile_7_Bytes, tile_8_Bytes);
            int textureWidth = grassMidBMPData.Width;
            int textureHeight = grassMidBMPData.Height;

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
