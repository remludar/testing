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
        int shaderProgramID;
        int modelViewUniformLocation;
        int vao;
        int vertexVBO;
        int floorTexID, wallTexID;

        Bitmap floorBMP, wallBMP;
        BitmapData floorBMPData, wallBMPData;

        Camera cam = new Camera();
        Map map = new Map();

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Title = "Basic Tests";
            GL.ClearColor(Color.CornflowerBlue);
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);

            GL.GenBuffers(1, out vao);
            GL.BindVertexArray(vao);

            _LoadData();
            _LoadTextures();
            Utilities.ShaderLoader.LoadShaders(out shaderProgramID, @"Content\Shaders\vs.glsl", @"Content\Shaders\fs.glsl");
            modelViewUniformLocation = GL.GetUniformLocation(shaderProgramID, "modelView");
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            _GetViewMatrix();
            _LoadUniforms();
            _ProcessInput();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            int lastTileDrawn = 0;
            for (int i = 9; i < map.GetTileCount() * Vertex.FLOATS_PER_VERTEX * Tile.VERTEX_COUNT; i += Vertex.FLOATS_PER_VERTEX * Tile.VERTEX_COUNT)
            {
                if (vertData[i] == 0)
                {
                    _UseTexture(wallBMPData);
                }
                else
                {
                    _UseTexture(floorBMPData);
                }
                _DrawTriangles(vertexVBO, vertData, lastTileDrawn);
                lastTileDrawn++;
            }
            GL.Flush();
            SwapBuffers();
        }

        private void _UseTexture(BitmapData bmpData)
        {
            GL.TexImage2D(TextureTarget.Texture2D,
                      0,
                      PixelInternalFormat.Rgba,
                      bmpData.Width,
                      bmpData.Height,
                      0,
                      OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                      PixelType.UnsignedByte,
                      bmpData.Scan0);
        }

        private void _LoadTextures()
        {
            Utilities.TextureLoader.LoadTextures(out floorTexID, @"Content\Textures\floor.png", out floorBMP, out floorBMPData);
            Utilities.TextureLoader.LoadTextures(out wallTexID, @"Content\Textures\wall.jpg", out wallBMP, out wallBMPData);
        }

        private void _ProcessInput()
        {
            var keyboardState = OpenTK.Input.Keyboard.GetState();
            if (keyboardState[Key.Escape])
                Exit();
            if(keyboardState[Key.D])
                cam.Move(0.5f, 0f);
            if(keyboardState[Key.A])
                cam.Move(-0.5f, 0f);
            if(keyboardState[Key.W])
                cam.Move(0f, 0.5f);
            if (keyboardState[Key.S])
                cam.Move(0f, -0.5f);
            if (keyboardState[Key.Q])
            {
                modelViewData[0][3, 0] = 1;
                Console.WriteLine(modelViewData[0][3,0]+"\n");
            }
        }

        private void _LoadData()
        {
            GL.GenBuffers(1, out vertexVBO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexVBO);

            vertData = map.GetVertexData();
            indexData = map.GetIndexData();

            modelViewData = new Matrix4[]{
                Matrix4.Identity
            };
        }

        private void _LoadUniforms()
        {
            GL.UniformMatrix4(modelViewUniformLocation, false, ref modelViewData[0]);
        }

        private void _GetViewMatrix()
        {
            modelViewData[0] = cam.GetViewMatrix();
            modelViewData[0][3, 3] = 3;
        }

        private void _DrawTriangles(int vbo, float[] vertData, int tileCount)
        {
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertData.Length * sizeof(float)), vertData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 10, 0);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 10, 3 * sizeof(float));
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(float) * 10, 7 * sizeof(float));
            int[] indices = new int[6];
            for (int i = 0; i < 6; i++)
            {
                indices[i] = indexData[i + tileCount * 6];
            }

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, indices);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
        }
    }
}
