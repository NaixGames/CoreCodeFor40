#if TOOLS
using Godot;
using System;

namespace  CoreCode.Docker
{
    [Tool]
    public partial class DockerExpandingButton : Button
    {
        [Export] private NodePath mNotePathOfMenu;

		private Control mNodeMenu;

        public override void _EnterTree()
        {
            Pressed += Clicked;
            mNodeMenu = this.GetNode<Control>(mNotePathOfMenu);
        }

        public void Clicked()
        {
            mNodeMenu.Visible = !mNodeMenu.Visible;
        }
    }
}
#endif