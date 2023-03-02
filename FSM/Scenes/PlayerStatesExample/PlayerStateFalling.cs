using Godot;
using System;

public partial class PlayerStateFalling : StateAbstract
{

	// -------------------------- Variables -------------------------------------
	private float gravity;
	private CharacterBody2D mCharacterBody;
	private InputReaderAbstract mInput;
	private float mMovingVelocity;


	// -------------------------- Abstract overrides -------------------------------------

	public override void InitializeState(Node mNodeRef, Godot.Collections.Dictionary mMemoryBlackboard = null){
		mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboard["CharacterNode"].AsNodePath());
		mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
		gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
		mMovingVelocity = (float)mMemoryBlackboard["MovingVelocity"].AsDouble();
	}
	
	protected override StateAbstract ProcessAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager, LogObject mlogObject=null){
		if (mCharacterBody.IsOnFloor()){
			return ((PlayerStateManagerExample)mStateManager).StateMoving;
		}
		return this;
	}

	protected override StateAbstract ProcessPhysicsAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,  LogObject mlogObject=null){
		float TotalInput = mInput.GiveAxisStrength("Right")-mInput.GiveAxisStrength("Left");

		mCharacterBody.Velocity = new Vector2(TotalInput*mMovingVelocity, mCharacterBody.Velocity.Y + gravity*(float)delta);
		mCharacterBody.MoveAndSlide();
		
		return this;
	}

}
