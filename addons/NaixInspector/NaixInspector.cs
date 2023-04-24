#if TOOLS
using Godot;
using System;
namespace CoreCode.Inspector{
	[Tool]
	public partial class NaixInspector : EditorPlugin
	{
		private NaixEditorInspector editorInspector;
		public override void _EnterTree()
		{
			// Initialization of the plugin goes here.
			editorInspector = new NaixEditorInspector();
			AddInspectorPlugin(editorInspector);
		}

		public override void _ExitTree()
		{
			// Clean-up of the plugin goes here.
			RemoveInspectorPlugin(editorInspector);
		}
	}
}
#endif