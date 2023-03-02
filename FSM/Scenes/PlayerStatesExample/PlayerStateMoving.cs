using Godot;
using System;

public partial class PlayerStateMoving : StateAbstract
{
	// -------------------------- Variables -------------------------------------

	private CharacterBody2D mCharacterBody;
	private InputReaderAbstract mInput;
	private float mJumpVelocity;
	private float mMovingVelocity;

	// -------------------------- Abstract overrides -------------------------------------

	public override void InitializeState(Node mNodeRef, Godot.Collections.Dictionary mMemoryBlackboard = null){
		mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboard["CharacterNode"].AsNodePath());
		mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
		mJumpVelocity = (float)mMemoryBlackboard["JumpVelocity"].AsDouble();
		mMovingVelocity = (float)mMemoryBlackboard["MovingVelocity"].AsDouble();
	}
	
	protected override StateAbstract ProcessAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,  LogObject mlogObject=null){
		if (mInput.IsButtonJustPressedInput("Up")){
			mCharacterBody.Velocity = new Vector2(mCharacterBody.Velocity.X, mCharacterBody.Velocity.Y - mJumpVelocity);
			mCharacterBody.MoveAndSlide();
			return ((PlayerStateManagerExample)mStateManager).StateJumping;
		}
		if (!mCharacterBody.IsOnFloor()){
			return ((PlayerStateManagerExample)mStateManager).StateJumping;
		}
		return this;
	}

	protected override StateAbstract ProcessPhysicsAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,   LogObject mlogObject=null){
		float TotalInput = mInput.GiveAxisStrength("Right")-mInput.GiveAxisStrength("Left");

		mCharacterBody.Velocity = new Vector2(TotalInput*mMovingVelocity, mCharacterBody.Velocity.Y);
		mCharacterBody.MoveAndSlide();
		return this;
	}
}
