namespace BrokenEngine.Scene_Graph.Components
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