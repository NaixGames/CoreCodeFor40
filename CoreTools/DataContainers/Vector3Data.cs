using Godot;
using System;

namespace CoreCode.Scripts{
    [GlobalClass]
    public partial class Vector3Data : Resource
    {
        [Export] private Vector3 mValue=Vector3.Zero;

        public Vector3 Value{
            get{return mValue;}
        }
    }
}
