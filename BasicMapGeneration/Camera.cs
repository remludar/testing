using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;


namespace BasicMapGeneration
{
    class Camera
    {
        public Vector3 position = Vector3.Zero;
        public float moveSpeed = 0.03f;

        public Matrix4 GetViewMatrix()
        {
            Vector3 lookAt = new Vector3(0, 0, -1);

            return Matrix4.LookAt(position, position + lookAt, Vector3.UnitY);
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

            Console.WriteLine(position);
        }

    }
}
