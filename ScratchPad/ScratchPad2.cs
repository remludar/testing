using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;

namespace ScratchPad
{
    class ScratchPad2 : GameWindow
    {
        int program;
        int vao, vbo1, vbo2;
        int floorTexID, wallTexID;
        float[] vertexData1, vertexData2;
        int[] indexData1, indexData2;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.CornflowerBlue);

            GL.GenBuffers(1, out vao);
            GL.BindVertexArray(vao);
           
            GL.GenBuffers(1, out vbo1);
            vertexData1 = new float[]{
                -1.0f, -0.5f, 0.0f, 0.0f,
                +0.0f, -0.5f, 1.0f, 0.0f,
                +0.0f, +0.5f, 1.0f, 1.0f,
                -1.0f, +0.5f, 0.0f, 1.0f
            };

            indexData1 = new int[]{
                0,1,2,
                2,3,0
            };

            GL.GenBuffers(1, out vbo2);
            vertexData2 = new float[]{
                +0.0f, -0.5f, 0.0f, 0.0f,
                +1.0f, -0.5f, 1.0f, 0.0f,
                +1.0f, +0.5f, 1.0f, 1.0f,
                +0.0f, +0.5f, 0.0f, 1.0f

            };

            indexData2 = new int[]{
                0,1,2,
                2,3,0
            };

            
           
            Utilities.ShaderLoader.LoadShaders(out program, "vs.glsl", "fs.glsl");

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

            //Utilities.TextureLoader.LoadTextures(out floorTexID, "floor.png");
            //_Draw(vbo1, vertexData1, indexData1);
            //Utilities.TextureLoader.LoadTextures(out wallTexID, "wall.jpg");
            //_Draw(vbo2, vertexData2, indexData2);

            GL.Flush();
            SwapBuffers();
        }

        private void _Draw(int vbo, float[] vertexData, int[] indexData)
        {
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexData1.Length * sizeof(float)), vertexData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, indexData);

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
