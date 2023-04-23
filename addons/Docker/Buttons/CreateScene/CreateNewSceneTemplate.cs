#if TOOLS
using Godot;
using System;

namespace  CoreCode.Docker
{
    [Tool]

    public partial class CreateNewSceneTemplate : Button
    {
        public override void _EnterTree()
        {
            Pressed += Clicked;
        }

        public void Clicked()
        {
            GD.Print("This should create a new scene");
        }
    }
}
#endif

