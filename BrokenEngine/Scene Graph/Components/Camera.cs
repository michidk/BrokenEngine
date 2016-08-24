using System;
using OpenTK;

namespace BrokenEngine.Scene_Graph.Components
{
    public class Camera : Component
    {

        private readonly Matrix4 projectionMatrix;

        public Camera(Matrix4 projectionMatrix)
        {
            this.projectionMatrix = projectionMatrix;
        }

        public void Render(GameObject sceneGraph)
        {
            sceneGraph.Render(GameObject.ModelMatrix, projectionMatrix);
        }

    }
}