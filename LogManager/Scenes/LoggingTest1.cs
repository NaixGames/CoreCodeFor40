using Godot;
using System;

public partial class LoggingTest1 : Node
{
	private LogObject mDummyLogReference;
	public override void _Ready(){
		mDummyLogReference = LogManager.Instance.RequestLogReference("Dummy", 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		mDummyLogReference.AddToLogString("This is a test!");
		mDummyLogReference.AddToLogString("The test continues!");
	}
}
