using Godot;
using System;

public partial class BezierCurveFollowerExample : Node2D
{
	//Example on how to use BezierCurves. Nothing fancy and not expected to actually be a part of the core library.

	[Export] private Path2D mPathReference;
	private Curve2D mCurveReference;
	
	[Export] private float mSpeed;

	private float mT=0f;

	public override void _Ready(){
		mCurveReference = mPathReference.Curve;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		mT+=mSpeed*((float)delta); 
		this.Position = mCurveReference.SampleBaked(mT*mCurveReference.GetBakedLength(),true);  //Note this is normalize
	}
}
