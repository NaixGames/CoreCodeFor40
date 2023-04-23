using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.Example{
	public partial class InputReaderTestNumberOne : Node
	{

		//Simple class for testing inputs. Could do a better job by actually doing a for, that would be my next test.
		[Export] private InputReaderAbstract testInput;
		[Export] private int PlayerID;
		

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			GD.Print("Printing Axis strength:");
			GD.Print(testInput.GiveAxisStrength("RightP" + PlayerID) - testInput.GiveAxisStrength("LeftP" + PlayerID));
			GD.Print("Left last input " + testInput.TimeSinceLastAxisInput("LeftP" + PlayerID) + "Right last input: " + testInput.TimeSinceLastAxisInput("RightP" + PlayerID));
			GD.Print("Printing Button pressed:");
			GD.Print(testInput.IsButtonPressed("JumpP" + PlayerID));
			GD.Print("Time since last button press: " + testInput.TimeSinceLastButtonInput("JumpP" + PlayerID));
		}
	}
}