using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.Example.DummyPlayerFSM{
	public partial class PlayerStateFalling : StateAbstract
	{

		// -------------------------- Variables -------------------------------------
		private float gravity;
		private CharacterBody2D mCharacterBody;
		private InputReaderAbstract mInput;
		private float mMovingVelocity;


		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["CharacterNode"].AsNodePath());
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
			mMovingVelocity = (float)mMemoryBlackboardCache["MovingVelocity"].AsDouble();
		}
		
		protected override StateAbstract ProcessAction(double delta, LogObject mlogObject=null){
			if (mCharacterBody.IsOnFloor()){
				return ((PlayerStateManagerExample)mStateManagerCache).StateMoving;
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, LogObject mlogObject=null){
			float TotalInput = mInput.GiveAxisStrength("Right")-mInput.GiveAxisStrength("Left");

			mCharacterBody.Velocity = new Vector2(TotalInput*mMovingVelocity, mCharacterBody.Velocity.Y + gravity*(float)delta);
			mCharacterBody.MoveAndSlide();
			
			return this;
		}
	}
}