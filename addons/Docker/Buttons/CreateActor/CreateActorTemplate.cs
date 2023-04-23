#if TOOLS
using Godot;
using System;

namespace  CoreCode.Docker
{
    [Tool]
    public partial class CreateActorTemplate : Button
    {
        public override void _EnterTree()
        {
            Pressed += Clicked;
        }

        public void Clicked()
        {
            GD.Print("This should create a new actor");
        }
    }
}
#endif