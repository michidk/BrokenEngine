using System;
using BrokenEngine.Utils;
using OpenTK;

namespace BrokenEngine.Scene_Graph.Components
{
    public class Camera : Component
    {

        private float aspectRatio, fov, nearPlane, farPlane;

        private Matrix4 projectionMatrix;


        public Camera(float width, float height, float fov = 60f, float nearPlane = 0.3f, float farPlane = 1000f)
        {
            this.aspectRatio = width/height;
            this.fov = fov;
            this.nearPlane = nearPlane;
            this.farPlane = farPlane;

            Calculate();
        }

        public override void OnStart()
        {
            base.OnStart();

            GameObject.BehaveAsCamera = true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            GameObject.BehaveAsCamera = false;
        }

        private void Calculate()
        {
            this.projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fov * MathUtils.DEG_TO_RAD, aspectRatio, nearPlane, farPlane);
        }

        public void Resize(float width, float height)
        {
            this.aspectRatio = width / height;

            Calculate();
        }

        public void Render(GameObject sceneGraph)
        {
            var viewMatrix = GameObject.GetView();
            var viewProjectionMatrix = viewMatrix * projectionMatrix;
            sceneGraph.Render(viewMatrix, viewProjectionMatrix);
        }

    }
}