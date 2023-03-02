using Godot;
using System;

public partial class TweenUseExample : Node
{
	//Example on how to use Tweens. Really nice! Be sure to check other Tweeners if needed (as the CallbackTween or the MethodTween)
	[Export] private Vector2 mObjectivePoint;
	[Export] private int mTransitionChoice;
	private Tween mTween;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mTween = CreateTween();
		switch (mTransitionChoice){
			case (1):
				mTween.TweenProperty(GetNode(this.GetPath()),"position", mObjectivePoint,2f).AsRelative().SetTrans(Tween.TransitionType.Linear);
				break;
			case (2):
				mTween.TweenProperty(GetNode(this.GetPath()),"position", mObjectivePoint,2f).AsRelative().SetTrans(Tween.TransitionType.Cubic);
				break;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
