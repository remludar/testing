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

namespace ScratchPad
{
    class ScratchPad : GameWindow
    {
        int program;
        int vao, vbo;

        int textureID;
        int width = 2;
        int height = 2;
        int layerCount = 4;
        int mipLevelCount = 2;
        float[] texels;

        float[] vertexData;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.CornflowerBlue);

            GL.GenBuffers(1, out vao);
            GL.BindVertexArray(vao);

            GL.GenBuffers(1, out vbo);

            vertexData = new float[]
            {
                0.0f, 0.0f,     0.0f, 1.0f, 0.0f,
                1.0f, 0.0f,     1.0f, 1.0f, 0.0f,
                1.0f, 1.0f,     1.0f, 0.0f, 0.0f,                                            
                1.0f, 1.0f,     1.0f, 0.0f, 0.0f,
                0.0f, 1.0f,     0.0f, 0.0f, 0.0f,
                0.0f, 0.0f,     0.0f, 1.0f, 0.0f,

                0.0f, -1.0f,    0.0f, 1.0f, 2.0f,
                1.0f, -1.0f,    1.0f, 1.0f, 2.0f,
                1.0f, 0.0f,     1.0f, 0.0f, 2.0f,                                            
                1.0f, 0.0f,     1.0f, 0.0f, 2.0f,
                0.0f, 0.0f,     0.0f, 0.0f, 2.0f,
                0.0f, -1.0f,    0.0f, 1.0f, 2.0f,
            };

            //Read you texels here. In the current example, we have 2*2*2 = 8 texels, with each texel being 4 GLubytes.
            texels = new float[]
            {
                 //Texels for first image.
                 1.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 //Texels for second image.
                 1.0f, 0.0f, 0.0f, 0.0f,
                 1.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 //Texels for third image.
                 0.0f, 0.0f, 0.0f, 0.0f,
                 1.0f, 0.0f, 0.0f, 0.0f,
                 1.0f, 0.0f, 1.0f, 0.0f,
                 0.0f, 0.0f, 0.0f, 0.0f,
                 //Texels for fourth image.
                 1.0f, 0.0f, 0.0f, 0.0f,
                 1.0f, 0.0f, 0.0f, 0.0f,
                 1.0f, 0.0f, 0.0f, 0.0f,
                 1.0f, 0.0f, 0.0f, 0.0f,
                 
            };

            GL.GenTextures(1, out textureID);
            GL.BindTexture(TextureTarget.Texture2DArray, textureID);

            //Allocate the storage.
            //GL.TexStorage3D(TextureTarget3d.Texture2DArray, mipLevelCount, SizedInternalFormat.Rgba8, width, height, layerCount);
            

            //Upload pixel data.
            //The first 0 refers to the mipmap level (level 0, since there's only 1)
            //The following 2 zeroes refers to the x and y offsets in case you only want to specify a subrectangle.
            //The final 0 refers to the layer index offset (we start from index 0 and have 2 levels).
            //Altogether you can specify a 3D box subset of the overall texture, but only one mip level at a time.
            //GL.TexSubImage3D(TextureTarget.Texture2DArray, 0, 0, 0, 0, width, height, layerCount, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float, texels);
            
            //Always set reasonable texture parameters

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

            //_DrawWithColor();
            _DrawWithTexture();

            GL.Flush();
            SwapBuffers();
        }

        private void _DrawWithColor()
        {
            GL.EnableVertexAttribArray(0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * sizeof(float)), vertexData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DisableVertexAttribArray(0);
        }

        private void _DrawWithTexture()
        {
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * sizeof(float)), vertexData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 2 * sizeof(float));
            GL.TexImage3D(TextureTarget.Texture2DArray, 0 , PixelInternalFormat.Rgba8, width, height, layerCount, 0, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.Float, texels);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 12);

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
