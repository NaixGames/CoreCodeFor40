using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Else{
	public partial class PeeState : StateAbstract
	{
		// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;

		private StateManagerElsa mStateManager; //THE STATE MANAGER CAN BE USED TO FIRE/SUBSCRIBE TO SIGNALS

		// -------------------------- Abstract overrides -------------------------------------

		public override void InitializeState(Node mNodeRef, Godot.Collections.Dictionary mMemoryBlackboard = null){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			mStateManager = ((mNodeRef as StateMachineActor).GiveStateManager() as StateManagerElsa);
		}
		
		protected override StateAbstract ProcessAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager, LogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			GD.Print("I am in the bathroom maity!!");
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,  LogObject mlogObject=null){
			return this;
		}

		protected override void EnterState(){
			//Using GD Print just for the example
			GD.Print("Oi I really need to pee!");
			return;
		}
	}
}