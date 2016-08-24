using System.Collections;
using System.Collections.Generic;
using BrokenEngine.Scene_Graph.Components;
using BrokenEngine.Utils;
using OpenTK;

namespace BrokenEngine.Scene_Graph
{
    public sealed class GameObject : IEnumerable<GameObject>
    {
        public string Name;
        public Matrix4 ModelMatrix = Matrix4.Identity;

        private GameObject parent;
        private readonly List<GameObject> children = new List<GameObject>();

        private readonly List<Component> components = new List<Component>();

        public List<Component> Components { get { return components; } } 

        public GameObject(string name, Vector3 position = default(Vector3), GameObject parent = null)
        {
            Name = name;
            Position = position;

            if (parent != null)
                this.SetParent(parent);
        }

        #region Transformation Helpers
        public Vector3 Position
        {
            get { return ModelMatrix.ExtractTranslation(); }
            set
            {
                ModelMatrix.ClearTranslation();
                ModelMatrix *= Matrix4.CreateTranslation(value);
            }
        }

        public Quaternion Rotation
        {
            get { return ModelMatrix.ExtractRotation(); }
            set
            {
                ModelMatrix.ClearRotation();
                ModelMatrix *= Matrix4.CreateFromQuaternion(value);
            }
        }


        public Vector3 EulerRotation
        {
            get { return ModelMatrix.ExtractRotation().Xyz; }
            set
            {
                ModelMatrix.ClearRotation();
                ModelMatrix *= Matrix4.CreateFromQuaternion(MathUtils.FromEuler(value));
            }
        }

        public Vector3 Scale
        {
            get { return ModelMatrix.ExtractScale(); }
            set
            {
                ModelMatrix.ClearTranslation();
                ModelMatrix *= Matrix4.CreateScale(value);
            }
        }

        public void Translate(Vector3 vector)
        {
            ModelMatrix *= Matrix4.CreateTranslation(vector);
        }
        #endregion

        #region Scene Graph Helpers
        public void SetParent(GameObject go)
        {
            // remove old parent
            if (this.parent != null)
                this.parent.children.Remove(this);

            // set parent
            this.parent = go;
            go.children.Add(this);
        }

        public void AddChild(GameObject go)
        {
            // remove old parent
            if (this.parent != null)
                this.parent.children.Remove(go);

            // set parent
            this.children.Add(go);
            go.parent = this;
        }
        #endregion

        #region Component Helpers
        // set callStart to false if building a scene graph before initializing it
        public void AddComponent(Component comp, bool callStart = true)
        {
            components.Add(comp);
            comp.GameObject = this;

            if (callStart)
                comp.OnStart();

            Globals.Game.BuildSceneGraphUI();
        }

        public void RemoveComponent(Component comp)
        {
            components.Remove(comp);
            comp.GameObject = null;
            comp.OnDestroy();

            Globals.Game.BuildSceneGraphUI();
        }

        public T FindComponentOfType<T>() where T : Component
        {
            return (T) components.Find(e => e.GetType() == typeof (T));
        }
        #endregion

        public void Start()
        {
            foreach (var child in children)
                child.Start();
            foreach (var comp in components)
                comp.OnStart();

            Globals.Game.BuildSceneGraphUI();
        }

        public void Update()
        {
            foreach (var child in children)
                child.Update();
            foreach (var comp in components)
                comp.OnUpdate();
        }

        public void Destroy()
        {
            foreach (var child in children)
                child.Destroy();
            foreach (var comp in components)
                comp.OnDestroy();

            Globals.Game.BuildSceneGraphUI();
        }

        public void Render(Matrix4 viewMatrix, Matrix4 projMatrix)
        {
            // render current object first
            for (int i = 0; i < components.Count; i++)
            {
                var comp = components[i];
                if (comp is MeshRenderer)
                    ((MeshRenderer) comp).Render(viewMatrix, projMatrix);
            }

            // render children
            for (int i = 0; i < children.Count; i++)
            {
                children[i].Render(viewMatrix, projMatrix);
            }
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}