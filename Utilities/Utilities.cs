using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace Utilities
{
    public static class ShaderLoader
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
}
