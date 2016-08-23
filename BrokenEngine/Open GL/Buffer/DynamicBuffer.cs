﻿using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace BrokenEngine.Open_GL
{
    public class DynamicBuffer<T> : Buffer<T> where T : struct
    {

        public List<T> Data;

        public override int Count { get { return Data.Count; } }

        public DynamicBuffer(int elementSize, List<T> data, BufferTarget target) : base(elementSize, target)
        {
            Data = data;
        }

        public override void BufferData()
        {
            base.BufferData(Data.ToArray());
        }

    }
}