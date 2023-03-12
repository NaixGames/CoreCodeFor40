using Godot;
using System;

public partial class DeadState : StateAbstract
{
// -------------------------- Variables -------------------------------------
	private InputReaderAbstract mInput;

	// -------------------------- Abstract overrides -------------------------------------

	public override void InitializeState(Node mNodeRef, Godot.Collections.Dictionary mMemoryBlackboard = null){
		mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
	}
	
	protected override StateAbstract ProcessAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager, LogObject mlogObject=null){
		if (!mInput.IsButtonJustPressedInput("Up")){
			return this;
		}
		GD.Print("I am dead :()");
		return this;
	}

	protected override StateAbstract ProcessPhysicsAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,  LogObject mlogObject=null){
		return this;
	}

	protected override void EnterState(){
		//Using GD Print just for the example
		GD.Print("Oi I couldn't drink or eat! I died uwu.");
		return;
	}
}
