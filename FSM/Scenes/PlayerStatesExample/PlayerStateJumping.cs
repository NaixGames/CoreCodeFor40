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

		public override void InitializeState(Node mNodeRef, Godot.Collections.Dictionary mMemoryBlackboard = null){
			mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboard["CharacterNode"].AsNodePath());
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			mMovingVelocity = (float)mMemoryBlackboard["MovingVelocity"].AsDouble();
		}
		
		protected override StateAbstract ProcessAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager, LogObject mlogObject=null){
			mTimeJumping +=delta;
			if (mTimeJumping>=2){
				mTimeJumping=0;
				return ((PlayerStateManagerExample)mStateManager).StateFalling;
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,  LogObject mlogObject=null){
			float TotalInput = mInput.GiveAxisStrength("Right")-mInput.GiveAxisStrength("Left");

			mCharacterBody.Velocity = new Vector2(TotalInput*mMovingVelocity, mCharacterBody.Velocity.Y);
			mCharacterBody.MoveAndSlide();

			return this;
		}
	}
}