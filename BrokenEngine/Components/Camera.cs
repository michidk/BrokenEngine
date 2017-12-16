using System.Xml;
using System.Xml.Serialization;
using BrokenEngine.SceneGraph;
using BrokenEngine.Utils;
using BrokenEngine.Utils.Attributes;
using OpenTK;

namespace BrokenEngine.Components
{
    public class Camera : Component
    {

        public float AspectRatio { get; set; }
        public float Fov { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }

        private Matrix4 projectionMatrix;


        [XmlConstructor]
        private Camera() { }

        public Camera(float width, float height, float fov = 60f, float nearPlane = 0.3f, float farPlane = 1000f)
        {
            this.AspectRatio = width/height;
            this.Fov = fov;
            this.NearPlane = nearPlane;
            this.FarPlane = farPlane;

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
            this.projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(Fov * MathUtils.DEG_TO_RAD, AspectRatio, NearPlane, FarPlane);
        }

        public void Resize(float width, float height)
        {
            this.AspectRatio = width / height;

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
