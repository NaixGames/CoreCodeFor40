using Godot;
using System;

public partial class InputReaderTestNumberTwo : Node
{

	//Simple class for testing inputs. Could do a better job by actually doing a for, that would be my next test.
	[Export] private InputReaderAbstract testInput;
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	//Will use GD.Print as this is intended to be editor only.
	public override void _Process(double delta)
	{
		for (int i = 0; i< testInput.AxisNames.Length; i++){
			GD.Print("Axi: " + testInput.AxisNames[i] + " value " + testInput.GiveAxisStrength(testInput.AxisNames[i])  + " last pressed "  + testInput.TimeSinceLastAxisInput(testInput.AxisNames[i]) + "held for " + testInput.TimeAxisHeldInput(testInput.AxisNames[i]));
		}
		for (int i = 0; i< testInput.ButtonNames.Length; i++){
			GD.Print("Button: " + testInput.ButtonNames[i] + " value " + testInput.IsButtonPressed(testInput.ButtonNames[i])  + " last pressed "  + testInput.TimeSinceLastButtonInput(testInput.ButtonNames[i])+ "held for " + testInput.TimeButtonHeldInput(testInput.ButtonNames[i]));
		}
	}
}