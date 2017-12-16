using System.Xml.Serialization;
using BrokenEngine.SceneGraph;
using BrokenEngine.Utils.Attributes;

namespace BrokenEngine.Components
{
    public abstract class Component
    {
        [XmlIgnore]
        public GameObject GameObject { get; private set; }

        
        [XmlConstructor]
        protected Component()
        {

        }

        internal void Assign(GameObject gameObject)
        {
            this.GameObject = gameObject;
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