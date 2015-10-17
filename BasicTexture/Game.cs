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

namespace BasicTexture
{
    class Game : GameWindow
    {
        float[] vertData;
        int shaderProgramID, vertShaderID, fragShaderID;
        int positionAttrib, colorAttrib;
        int vertexVBO;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "Basic Tests";
            GL.ClearColor(Color.CornflowerBlue);
            
            _LoadData();
            _LoadShaders();
            _LoadBuffers();
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

            _DrawTriangles();

            SwapBuffers();
        }

        private void _ProcessInput()
        {
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            if (keyboardState[Key.Escape])
                Exit();
        }

        private void _LoadData()
        {
            vertData = new float[]{
                    //Position            //Color
                -0.5f, -0.5f, 0.0f,     0.3f, 0.0f, 0.0f, 1.0f,
                -0.5f, +0.5f, 0.0f,     0.6f, 0.0f, 0.0f, 1.0f,
                +0.5f, +0.5f, 0.0f,     0.9f, 0.0f, 0.0f, 1.0f,

                +0.5f, +0.5f, 0.0f,     0.0f, 0.9f, 0.0f, 1.0f,
                +0.5f, -0.5f, 0.0f,     0.0f, 0.6f, 0.0f, 1.0f,
                -0.5f, -0.5f, 0.0f,     0.0f, 0.3f, 0.0f, 1.0f
            };
        }

        private void _LoadShaders()
        {
            shaderProgramID = GL.CreateProgram();
            _CreateShader("vs.glsl", ShaderType.VertexShader, out vertShaderID);
            _CreateShader("fs.glsl", ShaderType.FragmentShader, out fragShaderID);
            GL.AttachShader(shaderProgramID, vertShaderID);
            GL.AttachShader(shaderProgramID, fragShaderID);
            GL.LinkProgram(shaderProgramID);
            Console.WriteLine(GL.GetProgramInfoLog(shaderProgramID));

            positionAttrib = GL.GetAttribLocation(shaderProgramID, "position");
            colorAttrib = GL.GetAttribLocation(shaderProgramID, "color");

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
        
        private void _LoadBuffers()
        {
            GL.GenBuffers(1, out vertexVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexVBO);

            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertData.Length * sizeof(float)), vertData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(positionAttrib, 3, VertexAttribPointerType.Float, false, sizeof(float) * 7, 0);
            GL.VertexAttribPointer(colorAttrib, 4, VertexAttribPointerType.Float, false, sizeof(float) * 7, 3 * sizeof(float));            

            GL.UseProgram(shaderProgramID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void _DrawTriangles()
        {
            GL.EnableVertexAttribArray(positionAttrib);
            GL.EnableVertexAttribArray(colorAttrib);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.DisableVertexAttribArray(positionAttrib);
            GL.DisableVertexAttribArray(colorAttrib);
        }
    }
}
