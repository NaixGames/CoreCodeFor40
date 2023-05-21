using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.Example.DummyPlayerFSM{
	public partial class PlayerStateMoving : StateAbstract
	{
		// -------------------------- Variables -------------------------------------

		private CharacterBody2D mCharacterBody;
		private InputReaderAbstract mInput;
		private float mJumpVelocity;
		private float mMovingVelocity;

		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["CharacterNode"].AsNodePath());
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			mJumpVelocity = (float)mMemoryBlackboardCache["JumpVelocity"].AsDouble();
			mMovingVelocity = (float)mMemoryBlackboardCache["MovingVelocity"].AsDouble();
		}
		
		protected override StateAbstract ProcessAction(double delta, LogObject mlogObject=null){
			if (mInput.IsButtonJustPressedInput("Up")){
				mCharacterBody.Velocity = new Vector2(mCharacterBody.Velocity.X, mCharacterBody.Velocity.Y - mJumpVelocity);
				mCharacterBody.MoveAndSlide();
				return ((PlayerStateManagerExample)mStateManagerCache).StateJumping;
			}
			if (!mCharacterBody.IsOnFloor()){
				return ((PlayerStateManagerExample)mStateManagerCache).StateJumping;
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta,  LogObject mlogObject=null){
			float TotalInput = mInput.GiveAxisStrength("Right")-mInput.GiveAxisStrength("Left");

			mCharacterBody.Velocity = new Vector2(TotalInput*mMovingVelocity, mCharacterBody.Velocity.Y);
			mCharacterBody.MoveAndSlide();
			return this;
		}
	}
}