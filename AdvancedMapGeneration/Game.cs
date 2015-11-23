using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace BasicMapGeneration
{
    class Game : GameWindow
    {
        int vao;
        int shaderProgramID;

        Camera cam;
        Map map;

        //temp variable area
        Point mousePos;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            _Init();

            map = new Map();
            
            Utilities.ShaderLoader.LoadShaders(out shaderProgramID, @"Content\Shaders\vs.glsl", @"Content\Shaders\fs.glsl");
            
            cam = new Camera(shaderProgramID);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            cam.Update();
            _ProcessInput();
            _DisplayMouseLocation();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            map.Draw();

            GL.Flush();
            SwapBuffers();
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
            if (keyboardState[Key.E])
                cam.ZoomIn();
            if (keyboardState[Key.Q])
                cam.ZoomOut();
        }

        private void _DisplayMouseLocation()
        {
            this.MouseMove += (object sender, MouseMoveEventArgs mouseEvent) =>
            {
                if (mousePos != mouseEvent.Position)
                {
                    mousePos = mouseEvent.Position;
                    Console.WriteLine(mousePos);
                }
            };
        }

        private void _Init()
        {
            Title = "Basic Tests";
            GL.ClearColor(Color.CornflowerBlue);
            GL.Viewport(0, 0, ClientRectangle.Width, ClientRectangle.Height);

            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            GL.GenBuffers(1, out vao);
            GL.BindVertexArray(vao);
        }
    }
}
