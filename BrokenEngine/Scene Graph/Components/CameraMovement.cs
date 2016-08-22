using System;
using OpenTK;
using OpenTK.Input;

namespace OpenGLTest
{
    public class CameraMovement : Component
    {

        public enum Type
        {
            FirstPerson,
            MouseOrbit
        }

        public Type CurrentType;
        public Vector2 MouseSensitivity = new Vector2(0.005f, 0.005f);
        public float Speed = 0.12f;

        public Vector3 LookAtTarget = Vector3.Zero;
        public float Distance = 5;
        public Vector3 RotationAxis = new Vector3(0, 1, 0);

        public CameraMovement(Type type = Type.FirstPerson)
        {
            CurrentType = type;
        }

        public override void OnStart()
        {
            base.OnStart();

            eyeVector = GameObject.Position;
        }

        private Vector2 oldMousePos;
        public override void OnUpdate()
        {
            var mouse = Mouse.GetState();
            var mousePos = new Vector2(mouse.X, mouse.Y);

            switch (CurrentType)
            {
                case Type.FirstPerson:
                    DoFirstPerson(mousePos);
                    break;
                case Type.MouseOrbit:
                    DoMouseOrbit(mousePos);
                    break;
            }

            oldMousePos = mousePos;
        }

        private float yaw, pitch;
        private Vector3 eyeVector;
        private void DoFirstPerson(Vector2 mousePos)
        {
            var delta = mousePos - oldMousePos;

            yaw += delta.X * MouseSensitivity.X;
            pitch += delta.Y * MouseSensitivity.Y;

            int dx = 0, dz = 0, speedMult = 1;
            var key = Keyboard.GetState();
            if (key[Key.W])
                dz = 2;
            if (key[Key.S])
                dz = -2;
            if (key[Key.A])
                dx = -2;
            if (key[Key.D])
                dx = 2;
            if (key[Key.ShiftLeft])
                speedMult = 2;

            var mat = GameObject.ModelMatrix;
            var forward = new Vector3(mat[0, 2], mat[1, 2], mat[2, 2]);
            var strafe = new Vector3(mat[0, 0], mat[1, 0], mat[2,0]);

            eyeVector += (-dz*forward + dx*strafe)*Speed* speedMult;

            var pitchMat = Matrix4.CreateFromAxisAngle(new Vector3(1f, 0f, 0f), pitch);
            var yawMat = Matrix4.CreateFromAxisAngle(new Vector3(0f, 1f, 0f), yaw);
            Matrix4 rotate = yawMat * pitchMat;
            Matrix4 translate = Matrix4.CreateTranslation(-eyeVector);

            GameObject.ModelMatrix = translate * rotate;
        }

        private float theta, phi;
        private void DoMouseOrbit(Vector2 mousePos)
        {
            theta += (mousePos.X - oldMousePos.X) * MouseSensitivity.X;
            phi += (mousePos.Y - oldMousePos.Y) * MouseSensitivity.Y;

            var pos = new Vector3(
                Distance * (float)-Math.Sin(theta) * (float)Math.Cos(phi),
                Distance * (float)-Math.Sin(phi),
                -Distance * (float)Math.Cos(theta) * (float)Math.Cos(phi)
            );

            GameObject.ModelMatrix = Matrix4.LookAt(pos, LookAtTarget, RotationAxis);
        }

    }
}