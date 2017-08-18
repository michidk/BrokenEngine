using BrokenEngine.SceneGraph;

namespace BrokenEngine.Components
{
    public abstract class Component
    {
        public GameObject GameObject;

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