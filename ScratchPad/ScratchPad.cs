using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using System.IO;

using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ScratchPad
{
    class ScratchPad : GameWindow
    {
        int program;
        int vao, vbo;

        float[] vertexData;

        int floorTexID, wallTexID;
        int width = 2;
        int height = 2;
        int layerCount = 2;
        int mipLevelCount = 1;
        float[] texels;
        byte[] texelBytes;

        Bitmap atlasBMP, floorBMP, wallBMP;
        BitmapData atlasBMPData, floorBMPData, wallBMPData;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.CornflowerBlue);

            GL.GenBuffers(1, out vao);
            GL.BindVertexArray(vao);

            GL.GenBuffers(1, out vbo);
            
            _LoadVertexData();
            //_LoadTexelData();
            _LoadTexelDataFromBitmap();

            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2DArray, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            

            Utilities.ShaderLoader.LoadShaders(out program, @"Content\Shaders\vs.glsl", @"Content\Shaders\fs.glsl");
                    
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _ProcessInput();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _DrawWithTexture();

            GL.Flush();
            SwapBuffers();
        }


        private void _LoadVertexData()
        {
            vertexData = new float[]
            {

                -1.0f, +0.0f,   +0.0f, +1.0f, +0.0f,
                +0.0f, +0.0f,   +1.0f, +1.0f, +0.0f,
                +0.0f, +1.0f,   +1.0f, +0.0f, +0.0f,                                            
                +0.0f, +1.0f,   +1.0f, +0.0f, +0.0f,
                -1.0f, +1.0f,   +0.0f, +0.0f, +0.0f,
                -1.0f, +0.0f,   +0.0f, +1.0f, +0.0f,
                                              
                +0.0f, +0.0f,   +0.0f, +1.0f, +1.0f,
                +1.0f, +0.0f,   +1.0f, +1.0f, +1.0f,
                +1.0f, +1.0f,   +1.0f, +0.0f, +1.0f,                                            
                +1.0f, +1.0f,   +1.0f, +0.0f, +1.0f,
                +0.0f, +1.0f,   +0.0f, +0.0f, +1.0f,
                +0.0f, +0.0f,   +0.0f, +1.0f, +1.0f,
                
                -1.0f, -1.0f,   +0.0f, +1.0f, +1.0f,
                +0.0f, -1.0f,   +1.0f, +1.0f, +1.0f,
                +0.0f, +0.0f,   +1.0f, +0.0f, +1.0f,                                            
                +0.0f, +0.0f,   +1.0f, +0.0f, +1.0f,
                -1.0f, +0.0f,   +0.0f, +0.0f, +1.0f,
                -1.0f, -1.0f,   +0.0f, +1.0f, +1.0f,
              
                +0.0f, -1.0f,   +0.0f, +1.0f, +0.0f,
                +1.0f, -1.0f,   +1.0f, +1.0f, +0.0f,
                +1.0f, +0.0f,   +1.0f, +0.0f, +0.0f,                                            
                +1.0f, +0.0f,   +1.0f, +0.0f, +0.0f,
                +0.0f, +0.0f,   +0.0f, +0.0f, +0.0f,
                +0.0f, -1.0f,   +0.0f, +1.0f, +0.0f,
            };
        }
        private void _LoadTexelData()
        {
            //Read you texels here. In the current example, we have 2*2*2 = 8 texels, with each texel being 4 GLubytes.
            texels = new float[]
            {
                 //Texels for first image.
                 0.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 1.0f, 0.0f, 0.0f, 0.0f,
                 //Texels for second image.
                 0.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 1.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 //Texels for third image.
                 0.0f, 0.0f, 0.0f, 0.0f,
                 1.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 //Texels for fourth image.
                 1.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
            };

            GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba8, width, height, layerCount, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float, texels);
        }
        private void _LoadTexelDataFromBitmap()
        {
            //This code loads the whole bitmap
            //Utilities.TextureLoader.LoadTextureAtlas(@"Content\Textures\SampleAtlas.png", out atlasBMP, out atlasBMPData);
            //GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba, atlasBMPData.Width, atlasBMPData.Height, 1, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, atlasBMPData.Scan0);

            //Attempt to load 2 small bitmaps, convert to ubyte[], and render 
            //Utilities.TextureLoader.LoadTexture(@"Content\Textures\floor.png", out floorBMP, out floorBMPData);
            //GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba8, floorBMPData.Width, floorBMPData.Height, 1, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, floorBMPData.Scan0);

            //Utilities.TextureLoader.LoadTexture(@"Content\Textures\floor.png", out floorBMP, out floorBMPData);
            //int bytes = floorBMPData.Stride * floorBMP.Height;
            //byte[] floorBytes = new byte[bytes];
            //Marshal.Copy(floorBMPData.Scan0, floorBytes, 0, bytes);
            //GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba8, floorBMPData.Width, floorBMPData.Height, layerCount, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, floorBytes);

            Utilities.TextureLoader.LoadTexture(@"Content\Textures\floor.png", out floorBMP, out floorBMPData);
            Utilities.TextureLoader.LoadTexture(@"Content\Textures\wall.jpg", out wallBMP, out wallBMPData);
            int floorbytes = (floorBMPData.Stride * floorBMP.Height);
            int wallbytes =(wallBMPData.Stride * wallBMP.Height);
            int bytes = floorbytes + wallbytes;
            byte[] texelBytes = new byte[floorbytes + wallbytes];
            Marshal.Copy(floorBMPData.Scan0, texelBytes, 0, floorbytes); 
            Marshal.Copy(wallBMPData.Scan0, texelBytes, floorbytes, wallbytes); 
            int textureWidth = floorBMPData.Width;
            int textureHeight = floorBMPData.Height;
            

            GL.TexImage3D(TextureTarget.Texture2DArray, 0, PixelInternalFormat.Rgba8, textureWidth, textureHeight, layerCount, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, texelBytes);



        }
        private void _DrawWithTexture()
        {
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * sizeof(float)), vertexData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 2 * sizeof(float));

            GL.DrawArrays(PrimitiveType.Triangles, 0, 24);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
        }
        private void _ProcessInput()
        {
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            if (keyboardState[Key.Escape])
                Exit();
        }
    }
}
