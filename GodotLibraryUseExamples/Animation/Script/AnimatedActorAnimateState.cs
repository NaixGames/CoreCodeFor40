using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.Example.AnimationExampleFSM{
	public partial class AnimatedActorAnimateState : StateAbstract
	{
		// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;
		
		private AnimationPlayer mAnimator;


		// -------------------------- Abstract overrides -------------------------------------

		public override void InitializeState(Node mNodeRef, Godot.Collections.Dictionary mMemoryBlackboard = null){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			NodePath animationPath = mMemoryBlackboard["AnimationPlayer"].AsNodePath();
			mAnimator = mNodeRef.GetNode<AnimationPlayer>(animationPath);
		}
		
		protected override StateAbstract ProcessAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,  LogObject mlogObject=null){
			if (mInput.IsButtonJustPressedInput("Up")){
				mAnimator.Play("HappyFace");
				return this;
			}
			if (mInput.IsButtonJustPressedInput("Down")){
				mAnimator.Play("SadFace");
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,   LogObject mlogObject=null){
			return this;
		}
	}
}