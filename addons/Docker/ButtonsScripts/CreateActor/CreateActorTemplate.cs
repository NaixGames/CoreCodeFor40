#if TOOLS
using Godot;
using System;

namespace  CoreCode.Docker
{
    [Tool]
    public partial class CreateActorTemplate : Button
    {
        private LineEdit mPathStringInputNode;
        public override void _EnterTree()
        {
            Pressed += Clicked;
            mPathStringInputNode = this.GetParent<Node>().GetNode<LineEdit>("InputBox");
        }

        public void Clicked()
        {
            string mPath = mPathStringInputNode.Text;
            GD.Print("This should create a new actor on path: " + mPath);
        }
    }
}
#endif