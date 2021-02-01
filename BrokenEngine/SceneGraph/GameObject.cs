﻿using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using BrokenEngine.Components;
using BrokenEngine.Utils;
using OpenTK.Mathematics;

namespace BrokenEngine.SceneGraph
{
    public sealed class GameObject : IEnumerable<GameObject>
    {

        public string Name { get; set; }

        #region Local Space Properties
        [XmlElement("Position")]
        public Vector3 LocalPosition
        {
            get { return localPosition; }
            set {
                localPosition = value;
                SetDirty();
                cachedWorldPosition = null;
            }
        }

        [XmlIgnore]
        public Quaternion LocalRotation
        {
            get { return localRotation; }
            set
            {
                localRotation = value;
                SetDirty();
                cachedWorldRotation = null;
            }
        }

        [XmlElement("Scale")]
        public Vector3 LocalScale
        {
            get { return localScale; }
            set
            {
                localScale = value;
                SetDirty();
                cachedWorldScale = null;
            }
        }

        // angles are in degrees
        [XmlElement("Rotation")]
        public Vector3 LocalEulerRotation
        {
            get { return QuaternionUtils.ToEuler(LocalRotation); }
            set { LocalRotation = QuaternionUtils.FromEuler(value); }
        }
        #endregion

        #region World Space Properties
        [XmlIgnore]
        public Vector3 Position
        {
            get { return (Vector3) (cachedWorldPosition ?? CalculateWorldPosition()); }
            set { LocalPosition = parent?.InverseTransformPoint(value) ?? value; }
        }

        [XmlIgnore]
        public Quaternion Rotation
        {
            get { return (Quaternion) (cachedWorldRotation ?? CalculateWorldRotation()); }
            set { LocalRotation = parent?.InverseTransformRotation(value) ?? value; }
        }

        [XmlIgnore]
        public Vector3 Scale
        {
            get { return (Vector3) (cachedWorldScale ?? CalculateWorldScale()); }
            set { LocalScale = parent?.InverseTransformScale(value) ?? value; }
        }
        #endregion

        #region Matrice Properties
        [XmlIgnore]
        public Matrix4 LocalToWorldMatrix
        {
            get
            {
                if (localToWorldDirty)
                {
                    if (parent == null)
                        localToWorldMatrix = CalculateLocal();
                    else
                        localToWorldMatrix = parent.LocalToWorldMatrix * CalculateLocal();

                    localToWorldDirty = false;
                }

                return localToWorldMatrix;
            }
        }

        [XmlIgnore]
        public Matrix4 WorldToLocalMatrix
        {
            get
            {
                if (worldToLocalDirty)
                {
                    worldToLocalMatrix = LocalToWorldMatrix.Inverted();

                    worldToLocalDirty = false;
                }

                return worldToLocalMatrix;
            }
        }

        [XmlIgnore]
        public Matrix4 NormalMatrix
        {
            get
            {
                if (normalDirty)
                {
                    normalMatrix = Matrix4.Transpose(Matrix4.Invert(LocalToWorldMatrix));

                    normalDirty = false;
                }

                return normalMatrix;
            }
        }
        #endregion

        [XmlIgnore]
        public ReadOnlyCollection<GameObject> Children => childrenList.AsReadOnly();
        [XmlIgnore]
        public ReadOnlyCollection<Component> Components => componentsList.AsReadOnly();

        private Vector3 localPosition;
        private Quaternion localRotation = Quaternion.Identity;
        private Vector3 localScale = Vector3.One;

        private Matrix4 localToWorldMatrix;
        private Matrix4 worldToLocalMatrix;
        private Matrix4 normalMatrix;

        private GameObject parent;

        [XmlElement("Children")]
        private readonly List<GameObject> childrenList = new List<GameObject>();

        [XmlElement("Components")]
        private readonly List<Component> componentsList = new List<Component>();


        #region Cache
        private bool localToWorldDirty = true, worldToLocalDirty = true, normalDirty = true;

        private Vector3? cachedWorldPosition = null;
        private Quaternion? cachedWorldRotation = null;
        private Vector3? cachedWorldScale = null;
        #endregion


        // empty ctor for xml
        private GameObject()
        {

        }

        public GameObject(string name, Vector3 position = default(Vector3), GameObject parent = null) : this()
        {
            Name = name;
            LocalPosition = position;

            if (parent != null)
                this.SetParent(parent);
        }

        private void SetDirty()
        {
            localToWorldDirty = true;
            worldToLocalDirty = true;

            foreach (var child in Children)
                child.SetDirty();
        }

        public void Translate(Vector3 vector, bool worldSpace = false)
        {
            if (worldSpace)
                // add world translation to the GameObject's world position
                Position += vector;
            else
                // add local translation (transformed on the GameObject) to our local position
                LocalPosition += TransformDirection(vector);
        }

        #region World Space Helpers
        private Vector3? CalculateWorldPosition()
        {
            if (parent == null)
                cachedWorldPosition = LocalPosition;
            else
                cachedWorldPosition = parent.TransformPoint(LocalPosition);
            return cachedWorldPosition;
        }

        private Quaternion? CalculateWorldRotation()
        {
            if (parent == null)
                cachedWorldRotation = LocalRotation;
            else
                cachedWorldRotation = parent.TransformRotation(LocalRotation);
            return cachedWorldRotation;
        }

        private Vector3? CalculateWorldScale()
        {
            if (parent == null)
                cachedWorldScale = LocalScale;
            else
                cachedWorldScale = TransformScale(LocalScale);
            return cachedWorldScale;
        }

        // calculates the local matrix (aka LocalToParent)
        private Matrix4 CalculateLocal()
        {
            return MatrixUtils.CreateTRS(LocalPosition, LocalRotation, LocalScale);
        }

        public Matrix4 GetView()
        {
            return WorldToLocalMatrix;
        }
        #endregion

        #region Matrix Helpers
        // point local -> world
        public Vector3 TransformPoint(Vector3 pos)
        {
            return MatrixUtils.TransformPoint(LocalToWorldMatrix, pos);
        }

        // direction local -> world
        public Vector3 TransformDirection(Vector3 dir)
        {
            return MatrixUtils.TransformDirection(LocalToWorldMatrix, dir);
        }

        // rotation local -> world
        public Quaternion TransformRotation(Quaternion rot)
        {
            return MatrixUtils.TransformRotation(LocalToWorldMatrix, rot);
        }

        // scale local -> world
        public Vector3 TransformScale(Vector3 scale)
        {
            return MatrixUtils.TransformScale(LocalToWorldMatrix, scale);
        }

        // point world -> local
        public Vector3 InverseTransformPoint(Vector3 pos)
        {
            return MatrixUtils.TransformPoint(WorldToLocalMatrix, pos);
        }

        // direction world -> local
        public Vector3 InverseTransformDirection(Vector3 dir)
        {
            return MatrixUtils.TransformDirection(WorldToLocalMatrix, dir);
        }

        // rotation world -> local
        public Quaternion InverseTransformRotation(Quaternion rot)
        {
            return MatrixUtils.TransformRotation(WorldToLocalMatrix, rot);
        }

        // scale local -> world
        public Vector3 InverseTransformScale(Vector3 scale)
        {
            return MatrixUtils.TransformScale(WorldToLocalMatrix, scale);
        }
        #endregion

        #region Scene Graph Helpers
        public void SetParent(GameObject go)
        {
            // remove old parent
            if (this.parent != null)
                this.parent.childrenList.Remove(this);

            // set parent
            this.parent = go;
            go.childrenList.Add(this);

            SetDirty();
        }

        public void AddChild(GameObject go)
        {
            // remove old parent
            if (this.parent != null)
                this.parent.childrenList.Remove(go);

            // set parent
            this.childrenList.Add(go);
            go.parent = this;

            SetDirty();
        }
        #endregion

        #region Component Helpers
        // set initialize to false if building a scene graph before initializing it
        public GameObject AddComponent(Component comp, bool initialize = true)
        {
            componentsList.Add(comp);
            comp.Assign(this);

            if (initialize)
            {
                comp.OnInitialize();
                comp.OnStart();
            }

            return this;
        }

        public GameObject RemoveComponent(Component comp)
        {
            componentsList.Remove(comp);
            comp.Assign(null);
            comp.OnDestroy();

            return this;
        }

        public T FindComponentOfType<T>() where T : Component
        {
            return (T) componentsList.Find(e => e.GetType() == typeof (T));
        }
        #endregion

        #region Component Calls
        public void Initialize()
        {
            foreach (var child in Children)
                child.Initialize();
            foreach (var comp in Components)
            {
                //comp.Assign(this); is now done in serilization step and when parenting a component
                comp.OnInitialize();
            }
        }

        public void Start()
        {
            foreach (var child in Children)
                child.Start();
            foreach (var comp in Components)
                comp.OnStart();
        }

        public void Update(float deltaTime)
        {
            foreach (var child in Children)
                child.Update(deltaTime);
            foreach (var comp in Components)
                comp.OnUpdate(deltaTime);
        }

        public void Destroy()
        {
            foreach (var child in Children)
                child.Destroy();
            foreach (var comp in Components)
                comp.OnDestroy();
        }

        public void Render(Matrix4 viewMatrix, Matrix4 vpMatrix)
        {
            // render current object first
            for (int i = 0; i < Components.Count; i++)
            {
                var comp = Components[i];
                if (comp is MeshRenderer)
                    ((MeshRenderer) comp).Render(viewMatrix, vpMatrix);
            }

            // render children
            for (int i = 0; i < Children.Count; i++)
            {
                Children[i].Render(viewMatrix, vpMatrix);
            }
        }
        #endregion

        #region Interfaces
        public IEnumerator<GameObject> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

    }
}
