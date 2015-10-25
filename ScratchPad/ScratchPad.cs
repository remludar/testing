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
        int vao, vbo1;
        int floorTexID, wallTexID;
        float[] vertexData;
        int[] indexData;
        Bitmap floorBMP, wallBMP;
        BitmapData floorBMPData, wallBMPData;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            GL.ClearColor(Color.CornflowerBlue);

            GL.GenBuffers(1, out vao);
            GL.BindVertexArray(vao);

            GL.GenBuffers(1, out vbo1);
            vertexData = new float[]{
                -1.0f, -1.0f, 0.0f, 0.0f, 0.0f,
                +0.0f, -1.0f, 1.0f, 0.0f, 0.0f,
                +0.0f, +0.0f, 1.0f, 1.0f, 0.0f,
                -1.0f, +0.0f, 0.0f, 1.0f, 0.0f,

                +0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
                +1.0f, -1.0f, 1.0f, 0.0f, 0.0f,
                +1.0f, +0.0f, 1.0f, 1.0f, 0.0f,
                +0.0f, +0.0f, 0.0f, 1.0f, 0.0f,

                +0.0f, +0.0f, 0.0f, 0.0f, 1.0f,
                +1.0f, +0.0f, 1.0f, 0.0f, 1.0f,
                +1.0f, +1.0f, 1.0f, 1.0f, 1.0f,
                +0.0f, +1.0f, 0.0f, 1.0f, 1.0f,

                -1.0f, +0.0f, 0.0f, 0.0f, 1.0f,
                +0.0f, +0.0f, 1.0f, 0.0f, 1.0f,
                +0.0f, +1.0f, 1.0f, 1.0f, 1.0f,
                -1.0f, +1.0f, 0.0f, 1.0f, 1.0f,
            };

            indexData = new int[]{
                0,1,2,
                2,3,0,

                4,5,6,
                6,7,4,

                8,9,10,
                10,11,8,

                12,13,14,
                14,15,12
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

            int lastTileDrawn = 0;
            for (int i = 4; i < 20 * 4; i += 20)
            {
               if (vertexData[i] == 0)
                {
                    Utilities.TextureLoader.LoadTextures(out floorTexID, "floor.png");
                }
                else
                {
                    Utilities.TextureLoader.LoadTextures(out wallTexID, "wall.jpg");
                }

                _Draw(vbo1, vertexData, lastTileDrawn);
                lastTileDrawn++;
            }

            GL.Flush();
            SwapBuffers();
        }

        private void _Draw(int vbo, float[] vertexData, int tileCount)
        {
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexData.Length * sizeof(float)), vertexData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 2 * sizeof(float));
            int[] indices = new int[6];
            for (int i = 0; i < 6; i++)
            {
                indices[i] = indexData[i + tileCount * 6];
            }
            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, indices);

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
