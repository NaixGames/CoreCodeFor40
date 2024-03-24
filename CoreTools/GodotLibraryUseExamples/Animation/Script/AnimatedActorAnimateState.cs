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

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			NodePath animationPath = mMemoryBlackboardCache["AnimationPlayer"].AsNodePath();
			mAnimator = mNodeRef.GetNode<AnimationPlayer>(animationPath);
		}
		
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			if (mInput.IsButtonJustPressedInput("Up")){
				mAnimator.Play("HappyFace");
				return this;
			}
			if (mInput.IsButtonJustPressedInput("Down")){
				mAnimator.Play("SadFace");
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
			return this;
		}
	}
}