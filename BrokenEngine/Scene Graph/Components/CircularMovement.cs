using System;
using BrokenEngine.Utils;
using OpenTK;
using OpenTK.Compute.CL10;
using OpenTK.Input;

namespace BrokenEngine.Scene_Graph.Components
{
    public class CircularMovement : Component
    {

        public float Speed;
        public float Radius;

        private Vector3 initialPosition;

        public CircularMovement(float speed = 0.5f, float radius = 3f)
        {
            Speed = speed;
            Radius = radius;
        }

        public override void OnStart()
        {
            base.OnStart();

            initialPosition = this.GameObject.LocalPosition;
        }

        private float time = 0;
        public override void OnUpdate(float deltaTime)
        {
            var val = (time*Speed*2*Math.PI)%(2*Math.PI);
            this.GameObject.LocalPosition = initialPosition + new Vector3((float)Math.Cos(val), 0, (float)Math.Sin(val)) * Radius;
            time += deltaTime;
        }

    }
}