using System;

namespace OpenGLTest
{
    public abstract class Component
    {
        public GameObject GameObject;

        public virtual void OnStart()
        {
            Console.WriteLine($"{this.GetType().Name} of {GameObject.Name} started!");
        }

        public virtual void OnUpdate()
        {
            
        }

        public virtual void OnDestroy()
        {
            
        }
    }
}