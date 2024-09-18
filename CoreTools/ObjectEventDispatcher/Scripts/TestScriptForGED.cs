using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.Example{
	public partial class TestScriptForGED : Node
	{
		public override void _Ready()
		{
			GlobalEventDispatcher.Instance.Connect("ObjectWasPooledSignal", new Callable(this, nameof(this.PrintNameOfObjectPooled)));
		}

		private void PrintNameOfObjectPooled(string name){
			GD.Print("Object was pooled with name" + name); //Normally wouldnt use this because it is an example on how to setup the event dispatcher. Remember to add
		}

		//If want to re-test, add:
		//GlobalEventDispatcher.Instance.EmitSignal(nameof(GlobalEventDispatcher.Instance.ObjectWasPooledSignal), ObjectToPool.Name);
		//To Object Pooler method ReturnToPool();

	}
}