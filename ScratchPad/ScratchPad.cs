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

namespace ScratchPad
{
    class ScratchPad : GameWindow
    {
        int program, vertShaderID, fragShaderID;
        int vao, vbo;
        float[] vertexBufferData;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.CornflowerBlue);
            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);

            vertexBufferData = new float[]{
                -1.0f, -1.0f, 0.0f,
                +1.0f, -1.0f, 0.0f,
                +0.0f, +1.0f, 0.0f
            };

            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexBufferData.Length * sizeof(float)), vertexBufferData, BufferUsageHint.StaticDraw);

            _LoadShaders();
            
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
            GL.UseProgram(program);

            GL.EnableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            GL.DisableVertexAttribArray(0);

            SwapBuffers();
        }

        private void _LoadShaders()
        {
            program = GL.CreateProgram();
            _CreateShader(@"vs.glsl", ShaderType.VertexShader, out vertShaderID);
            _CreateShader(@"fs.glsl", ShaderType.FragmentShader, out fragShaderID);
            GL.AttachShader(program, vertShaderID);
            GL.AttachShader(program, fragShaderID);
            GL.LinkProgram(program);
            Console.WriteLine(GL.GetProgramInfoLog(program));

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

        private void _ProcessInput()
        {
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            if (keyboardState[Key.Escape])
                Exit(); 
        }
    }
}
