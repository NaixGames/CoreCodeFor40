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
			return true;
		}

		public override void _ParseBegin(GodotObject @object)
		{
			InputReaderAbstract myInput = @object as InputReaderPlayer;
			if (myInput!=null){
				AddPropertyEditor("Fix input map", new InputReaderAbstractInspectorPluging ());
				return;
			}
			myInput = @object as StateMachineAIInput;
			if (myInput!=null){
				AddPropertyEditor("Fix input map", new InputReaderAbstractInspectorPluging ());
				return;
			}
		}
	}
	#endif
}