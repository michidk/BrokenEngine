using System.Xml.Serialization;
using BrokenEngine.SceneGraph;

namespace BrokenEngine.Components
{
    public abstract class Component
    {

        [XmlIgnore]
        public GameObject GameObject => gameObject;

        private GameObject gameObject;


        internal void Assign(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }
        public virtual void OnInitialize()
        {

        }

        public virtual void OnStart()
        {
            
        }

        public virtual void OnUpdate(float deltaTime)
        {
            
        }

        public virtual void OnDestroy()
        {
            
        }
    }
}