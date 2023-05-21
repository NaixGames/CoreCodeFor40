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
			return (IsInputForEditor(@object) || IsSceneDatabase(@object));
		}

		public override void _ParseBegin(GodotObject @object)
		{
			if (IsInputForEditor(@object)){
				InputReaderAbstract myObject = @object as InputReaderAbstract;
				AddPropertyEditor("Fix input map", new InputReaderAbstractInspectorPluging ());
				return;
			}
			else if (IsSceneDatabase(@object)){
				SceneDatabase myObject = @object as SceneDatabase;
				AddPropertyEditor("Generate Scene data mappings", new SceneDatabaseInspectorPluging ());
				return;
			}
		}

		//Parsing type methods
		private bool IsInputForEditor(GodotObject @object){
			return @object is InputReaderPlayer || @object is StateMachineAIInput;
		}

		private bool IsSceneDatabase(GodotObject @object){
			return @object is SceneDatabase;
		}
	}
}
#endif