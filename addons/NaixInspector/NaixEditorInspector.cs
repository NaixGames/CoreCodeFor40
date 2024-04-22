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
			return (IsSceneDatabase(@object)) || (IsStateManagerPointer(@object));
		}

		public override void _ParseBegin(GodotObject @object)
		{
			if (IsSceneDatabase(@object)){
				SceneDatabase myObject = @object as SceneDatabase;
				AddPropertyEditor("Generate Scene data mappings", new SceneDatabaseInspectorPluging ());
				return;
			}
			if (IsStateManagerPointer(@object)){
				StateManagerPointer myObject = @object as StateManagerPointer;
				AddPropertyEditor("State Manager Drawer", new StateManagerPointerInspectorPluging ());
			}
		}

		//Parsing type methods
		private bool IsSceneDatabase(GodotObject @object){
			return @object is SceneDatabase;
		}

		private bool IsStateManagerPointer(GodotObject @object){
			return @object is StateManagerPointer;
		}
	}
}
#endif