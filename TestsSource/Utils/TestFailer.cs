using CoreCode.Scripts;
using Godot;
using System;

public partial class TestFailer : Node
{
	[Export] private Resource myTestResource;
	private float data;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		data = ((FloatData) myTestResource).Value;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
