#if TOOLS
using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.Inspector{
	[Tool]
	public partial class NaixEditorInspector : EditorInspectorPlugin
	{
		public override bool _CanHandle(GodotObject @object)
		{
			return IsStateManagerPointer(@object);
		}

		public override void _ParseBegin(GodotObject @object)
		{
			if (IsStateManagerPointer(@object)){
				StateManagerPointer myObject = @object as StateManagerPointer;
				AddPropertyEditor("State Manager Drawer", new StateManagerPointerInspectorPluging ());
			}
		}

		private bool IsStateManagerPointer(GodotObject @object){
			return @object is StateManagerPointer;
		}
	}
}
#endif