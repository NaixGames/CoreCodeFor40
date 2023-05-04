#if TOOLS
using Godot;
using System;

namespace  CoreCode.Docker
{
    [Tool]
    public partial class CreateNewSceneTemplate : Button
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
            GD.Print("This should create a new scene on path: " + mPath);
        }
    }
}
#endif

