using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.Example.DummyPlayerFSM{
	public partial class PlayerStateJumping : StateAbstract
	{

		// -------------------------- Variables -------------------------------------
		private CharacterBody2D mCharacterBody;
		private InputReaderAbstract mInput;
		private float mMovingVelocity;

		private double mTimeJumping = 0;



		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["CharacterNode"].AsNodePath());
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			mMovingVelocity = (float)mMemoryBlackboardCache["MovingVelocity"].AsDouble();
		}
		
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			mTimeJumping +=delta;
			if (mTimeJumping>=2){
				mTimeJumping=0;
				return ((PlayerStateManagerExample)mStateManagerCache).StateFalling;
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
			float TotalInput = mInput.GiveAxisStrength("Right")-mInput.GiveAxisStrength("Left");

			mCharacterBody.Velocity = new Vector2(TotalInput*mMovingVelocity, mCharacterBody.Velocity.Y);
			mCharacterBody.MoveAndSlide();

			return this;
		}
	}
}