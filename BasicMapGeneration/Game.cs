using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Input;

namespace BasicMapGeneration
{
    class Game : GameWindow
    {
        float[] vertData;
        int[] indexData;
        Matrix4[] modelViewData;
        int shaderProgramID, vertShaderID, fragShaderID, textureID;
        int positionAttrib, colorAttrib, textureAttrib;
        int modelViewUniform;
        int vertexVBO;
        int indexVBO;

        Bitmap bmp;
        BitmapData bmpData;

        Camera cam = new Camera();
        Map map = new Map();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "Basic Tests";
            GL.ClearColor(Color.CornflowerBlue);
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);

            _LoadData();
            _LoadShaders();
            _LoadTextures();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            _GetViewMatrix();
            _LoadUniforms();

            _LoadBuffers();

            _ProcessInput();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _DrawTriangles();

            SwapBuffers();
        }

        private void _ProcessInput()
        {
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            if (keyboardState[Key.Escape])
                Exit();
            if(keyboardState[Key.D])
                cam.Move(0.1f, 0f);
            if(keyboardState[Key.A])
                cam.Move(-0.1f, 0f);
            if(keyboardState[Key.W])
                cam.Move(0f, 0.1f);
            if (keyboardState[Key.S])
                cam.Move(0f, -0.1f);
            if (keyboardState[Key.Q])
            {
                modelViewData[0][3, 0] = 1;
                Console.WriteLine(modelViewData[0][3,0]+"\n");
            }
        }

        private void _LoadData()
        {
            vertData = map.GetVertexData();
            indexData = map.GetIndexData();

            modelViewData = new Matrix4[]{
                Matrix4.Identity
            };
        }

        private void _LoadShaders()
        {
            shaderProgramID = GL.CreateProgram();
            _CreateShader(@"Content\Shaders\vs.glsl", ShaderType.VertexShader, out vertShaderID);
            _CreateShader(@"Content\Shaders\fs.glsl", ShaderType.FragmentShader, out fragShaderID);
            GL.AttachShader(shaderProgramID, vertShaderID);
            GL.AttachShader(shaderProgramID, fragShaderID);
            GL.LinkProgram(shaderProgramID);
            Console.WriteLine(GL.GetProgramInfoLog(shaderProgramID));

            positionAttrib = GL.GetAttribLocation(shaderProgramID, "position");
            colorAttrib = GL.GetAttribLocation(shaderProgramID, "color");
            textureAttrib = GL.GetAttribLocation(shaderProgramID, "texture");
            modelViewUniform = GL.GetUniformLocation(shaderProgramID, "modelView");

            if(positionAttrib == -1 || colorAttrib == -1 || textureAttrib == -1 || modelViewUniform == -1)
            {
                Console.WriteLine("Error binding attributes");
                Console.WriteLine("positionAttrib: " + positionAttrib);
                Console.WriteLine("colorAttrib: " + colorAttrib);
                Console.WriteLine("textureAttrib: " + textureAttrib);
                Console.WriteLine("modelView: " + modelViewUniform);
            }
        }

        private void _CreateShader(string filePath, ShaderType type, out int id)
        {
            id = GL.CreateShader(type);
            using (StreamReader reader = new StreamReader(filePath))
            {
                GL.ShaderSource(id, reader.ReadToEnd());
            }
            GL.CompileShader(id);
            Console.WriteLine(GL.GetShaderInfoLog(id));
        }

        private void _LoadTextures()
        {
            textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            bmp = new Bitmap(@"Content\Textures\Floor.png");
            bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D,
               0,
               PixelInternalFormat.Rgba,
               bmpData.Width,
               bmpData.Height,
               0,
               OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
               PixelType.UnsignedByte,
               bmpData.Scan0);

            bmp.UnlockBits(bmpData);
        }

        private void _LoadBuffers()
        {
            GL.GenBuffers(1, out vertexVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexVBO);

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertData.Length * sizeof(float)), vertData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(positionAttrib, 3, VertexAttribPointerType.Float, false, sizeof(float) * 9, 0);
            GL.VertexAttribPointer(colorAttrib, 4, VertexAttribPointerType.Float, false, sizeof(float) * 9, 3 * sizeof(float));
            GL.VertexAttribPointer(textureAttrib, 2, VertexAttribPointerType.Float, false, sizeof(float) * 9, 7 * sizeof(float));

            GL.GenBuffers(1, out indexVBO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexVBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indexData.Length * sizeof(float)), indexData, BufferUsageHint.StaticDraw);

            GL.UseProgram(shaderProgramID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        private void _LoadUniforms()
        {
            GL.UniformMatrix4(modelViewUniform, false, ref modelViewData[0]);
        }

        private void _GetViewMatrix()
        {
            modelViewData[0] = cam.GetViewMatrix();
            modelViewData[0][3, 3] = 3;
        }

        private void _DrawTriangles()
        {
            GL.EnableVertexAttribArray(positionAttrib);
            GL.EnableVertexAttribArray(colorAttrib);
            GL.EnableVertexAttribArray(textureAttrib);

            GL.DrawElements(PrimitiveType.Triangles, indexData.Length, DrawElementsType.UnsignedInt, indexData);

            GL.DisableVertexAttribArray(positionAttrib);
            GL.DisableVertexAttribArray(colorAttrib);
            GL.DisableVertexAttribArray(textureAttrib);
        }
    }
}
