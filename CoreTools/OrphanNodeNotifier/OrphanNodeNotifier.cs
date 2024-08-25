using Godot;
using System;

namespace CoreCode.Scripts{
	public partial class OrphanNodeNotifier : Node
	{
        public override void _Notification(int what)
        {
            if (what == NotificationWMCloseRequest){
                GD.Print("Printing Orphan nodes on exit");
			    PrintOrphanNodes();
                GetTree().Quit(); // default behavior
            }
        }
        
    }
}
