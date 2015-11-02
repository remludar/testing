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
using BasicMapGeneration;

namespace Utilities
{
    class ShaderLoader
    {
        public static void LoadShaders(out int program, string vertFilePath, string fragFilePath)
        {
            int vertShaderID, fragShaderID;
            program = GL.CreateProgram();
            _CreateShader(vertFilePath, ShaderType.VertexShader, out vertShaderID);
            _CreateShader(fragFilePath, ShaderType.FragmentShader, out fragShaderID);
            GL.AttachShader(program, vertShaderID);
            GL.AttachShader(program, fragShaderID);
            GL.LinkProgram(program);
            GL.DeleteShader(vertShaderID);
            GL.DeleteShader(fragShaderID);
            Console.WriteLine(GL.GetProgramInfoLog(program));
            GL.UseProgram(program);
        }

        private static void _CreateShader(string filePath, ShaderType type, out int id)
        {
            id = GL.CreateShader(type);
            using (StreamReader reader = new StreamReader(filePath))
            {
                GL.ShaderSource(id, reader.ReadToEnd());
            }
            GL.CompileShader(id);
            Console.WriteLine(GL.GetShaderInfoLog(id));
        }

        
    }

    class TextureLoader
    {
        public static void LoadTextures(out int textureID, string filePath, out Bitmap bmp, out BitmapData bmpData)
        {
            textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            bmp = new Bitmap(filePath);
            bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        public static void LoadTexture(string filePath, out Bitmap bmp, out BitmapData bmpData)
        {
            bmp = new Bitmap(filePath);
            //bmp.MakeTransparent(Color.White);
            bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
    }
}
