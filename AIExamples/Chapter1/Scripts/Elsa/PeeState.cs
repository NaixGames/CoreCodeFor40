using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Elsa{
	public partial class PeeState : StateAbstract
	{
		// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;


		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			mStateManagerCache = (mNodeRef as StateMachineActor).GiveStateManager();
		}
		
		protected override StateAbstract ProcessAction(double delta, LogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			GD.Print("I am in the bathroom maity!!");
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, LogObject mlogObject=null){
			return this;
		}

		protected override void EnterState(){
			//Using GD Print just for the example
			GD.Print("Oi I really need to pee!");
			return;
		}
	}
}