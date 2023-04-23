#if TOOLS
using Godot;
using System;

namespace  CoreCode.Docker
{
    [Tool]
    public partial class DockerButtonExample : Button
    {
        public override void _EnterTree()
        {
            Pressed += Clicked;
        }

        public void Clicked()
        {
            GD.Print("This is indeed working");
        }
    }
}
#endif
