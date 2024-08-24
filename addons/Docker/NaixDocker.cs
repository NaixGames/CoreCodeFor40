#if TOOLS
using Godot;
using System;

namespace  CoreCode.Docker
{
	[Tool]
	public partial class NaixDocker : EditorPlugin
	{
		Control DockLayout;

		public override void _EnterTree()
		{
			// Initialization of the plugin goes here.

			//Load the layout
			DockLayout = (Control)GD.Load<PackedScene>("res://addons/Docker/Layout/NaixDock.tscn").Instantiate();
			AddControlToDock(DockSlot.LeftBr, DockLayout);
		}

		public override void _ExitTree()
		{
			// Clean-up of the plugin goes here.
			// Remove the dock.
			RemoveControlFromDocks(DockLayout);
			//Erase the control from the memory.
			DockLayout.Free();
		}
	}	
}
#endif
