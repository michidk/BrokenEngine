using System.Xml;
using System.Xml.Serialization;
using BrokenEngine.SceneGraph;
using BrokenEngine.Utils;
using OpenTK;

namespace BrokenEngine.Components
{
    public class Camera : Component
    {

        [XmlElement]
        private float aspectRatio, fov, nearPlane, farPlane;

        private Matrix4 projectionMatrix;


        private Camera() { }

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
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
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
            var viewMatrix = this.GameObject.GetView();
            var viewProjectionMatrix = viewMatrix * projectionMatrix;
            sceneGraph.Render(viewMatrix, viewProjectionMatrix);
        }

    }
}
