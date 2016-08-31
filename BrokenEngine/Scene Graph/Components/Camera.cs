using System;
using BrokenEngine.Utils;
using OpenTK;

namespace BrokenEngine.Scene_Graph.Components
{
    public class Camera : Component
    {

        private readonly Matrix4 projectionMatrix;

        public Camera(float fov, float aspectRatio, float nearPlane = 0.3f, float farPlane = 1000f)
        {
            this.projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(fov * MathUtils.DEG_TO_RAD, aspectRatio, nearPlane, farPlane);
        }

        public void Render(GameObject sceneGraph)
        {
            sceneGraph.Render(GameObject.LocalToWorldMatrix, projectionMatrix);
        }

    }
}