using Godot;
using System;

namespace CoreCode.Scripts{
    [GlobalClass]
    public partial class Vector2Data : Resource
    {
        [Export] private Vector2 mValue=Vector2.Zero;

        public Vector2 Value{
            get{return mValue;}
        }
    }
}
