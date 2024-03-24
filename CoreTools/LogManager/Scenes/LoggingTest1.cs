using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.Example{
public partial class LoggingTest1 : Node
	{
		private ILogObject mDummyLogReference;
		public override void _Ready(){
			mDummyLogReference = LogManager.Instance.RequestLog("Dummy");
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			mDummyLogReference.Print("This is a test!");
			mDummyLogReference.Print("The test continues!");
		}
	}
}
