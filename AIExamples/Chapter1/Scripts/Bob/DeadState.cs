using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Bob{
	public partial class DeadState : StateAbstract
	{
	// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;

		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
		}
		
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			GD.Print("I am dead :()");
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
			return this;
		}

		protected override void EnterState(){
			//Using GD Print just for the example
			GD.Print("Oi I couldn't drink or eat! I died uwu.");
			return;
		}
	}
}
