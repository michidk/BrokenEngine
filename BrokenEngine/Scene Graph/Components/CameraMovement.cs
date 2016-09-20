using System;
using BrokenEngine.Utils;
using OpenTK;
using OpenTK.Compute.CL10;
using OpenTK.Input;

namespace BrokenEngine.Scene_Graph.Components
{
    public class CameraMovement : Component
    {

        public enum Type
        {
            FirstPerson,
            DebugMovement
        }

        public Type CurrentType;


        public CameraMovement(Type type = Type.FirstPerson)
        {
            CurrentType = type;
        }

        public override void OnStart()
        {
            base.OnStart();
        }

        private Vector2 oldMousePos;
        public override void OnUpdate(float deltaTime)
        {
            var mouse = Mouse.GetState();
            var mousePos = new Vector2(mouse.X, mouse.Y);

            switch (CurrentType)
            {
                case Type.FirstPerson:
                    DoFirstPerson(mousePos);
                    break;
                case Type.DebugMovement:
                    DoDebugMovement(mousePos);
                    break;
            }

            oldMousePos = mousePos;
        }

        private float speed = 0.25f;
        private float sensitivity = 0.009f;
        private float smoothing = 1.25f;
        private float clampX = 360;
        private float clampY = 180;

        private Vector2 _mouseAbsolute;
        private Vector2 _smoothMouse;
        
        private void DoFirstPerson(Vector2 mousePos)
        {
            Vector3 vel = Vector3.Zero;
            if (Globals.Game.Keyboard[Key.W])
            {
                vel += new Vector3(0, 0, -1);   // camera looks backwards
            }
            if (Globals.Game.Keyboard[Key.S])
            {
                vel += new Vector3(0, 0, 1);
            }
            if (Globals.Game.Keyboard[Key.A])
            {
                vel += new Vector3(-1, 0, 0);
            }
            if (Globals.Game.Keyboard[Key.D])
            {
                vel += new Vector3(1, 0, 0);
            }
            vel.NormalizeFast();

            if (Globals.Game.Keyboard[Key.ShiftLeft])
                vel *= 4;
            
            var mouseDelta = mousePos - oldMousePos;

            // unity screen coordinate system
            mouseDelta = new Vector2(-mouseDelta.Y, -mouseDelta.X) * sensitivity * smoothing;

            _smoothMouse.X = MathUtils.Lerp(_smoothMouse.X, mouseDelta.X, 1f / smoothing);
            _smoothMouse.Y = MathUtils.Lerp(_smoothMouse.Y, mouseDelta.Y, 1f / smoothing);
            
            // find the absolute mouse movement value from point zero
            _mouseAbsolute += _smoothMouse;

            if (clampX < 360)
                _mouseAbsolute.X = MathUtils.Clamp(_mouseAbsolute.X, -clampX * 0.5f, clampX * 0.5f);

            if (clampY < 360)
                _mouseAbsolute.Y = MathUtils.Clamp(_mouseAbsolute.Y, -clampY * 0.5f, clampY * 0.5f);

            var xRotation = Quaternion.FromAxisAngle(Vector3.UnitX, _mouseAbsolute.X);
            var yRotation = Quaternion.FromAxisAngle(Vector3.UnitY, _mouseAbsolute.Y);

            GameObject.LocalRotation = yRotation * xRotation;
            GameObject.Translate(vel * speed, false);
        }

        private void DoDebugMovement(Vector2 mousePos)
        {
            Vector3 vel = Vector3.Zero;
            if (Globals.Game.Keyboard[Key.W])
            {
                vel += new Vector3(0, 0, 1);
            }
            if (Globals.Game.Keyboard[Key.S])
            {
                vel += new Vector3(0, 0, -1);
            }
            if (Globals.Game.Keyboard[Key.A])
            {
                vel += new Vector3(-1, 0, 0);
            }
            if (Globals.Game.Keyboard[Key.D])
            {
                vel += new Vector3(1, 0, 0);
            }
            
            GameObject.Translate(vel * speed);
            
            Console.WriteLine(this.GameObject.LocalPosition + " - " + this.GameObject.Position);
        }

    }
}