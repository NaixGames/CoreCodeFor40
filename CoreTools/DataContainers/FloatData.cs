using Godot;
using System;

namespace CoreCode.Scripts{
    [GlobalClass]
    public partial class FloatData : Resource
    {
        [Export] private float mValue=0;

        public float Value{
            get{return mValue;}
        }
    }
}
