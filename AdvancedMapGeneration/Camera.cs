using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace BasicMapGeneration
{
    class Camera
    {
        public Vector3 position = Vector3.Zero;
        public float moveSpeed = 0.09f;
        public float zoomSpeed = 0.10f;

        int modelViewUniformLocation;
        Matrix4 modelViewData;

        float zoom = 5;

        public Camera(int shaderProgramID)
        {

            modelViewUniformLocation = GL.GetUniformLocation(shaderProgramID, "modelView");
            modelViewData = Matrix4.Identity;
            position = new Vector3(3.5f, 5, 0);
        }

        public void Move(float x, float y)
        {
            Vector3 offset = new Vector3();

            Vector3 right = new Vector3(1, 0, 0);
            Vector3 up = new Vector3(0, 1, 0);

            offset += x * right;
            offset += y * up;

            offset.NormalizeFast();
            offset = Vector3.Multiply(offset, moveSpeed);

            position += offset;

        }

        public void ZoomOut()
        {
            zoom += zoomSpeed;
        }

        public void ZoomIn()
        {
            zoom -= zoomSpeed;
        }

        public void Update()
        {
            GL.UniformMatrix4(modelViewUniformLocation, false, ref modelViewData);
            Vector3 lookAt = new Vector3(0, 0, -1);
            modelViewData = Matrix4.LookAt(position, position + lookAt, Vector3.UnitY);
            modelViewData[3, 3] = zoom; //zoom

            modelViewData[0, 0] *= 0.75f; //x aspect
            modelViewData[1, 1] *= 1.0f; //y aspect


        }
    }
}
