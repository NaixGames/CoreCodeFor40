using Godot;
using System;

public partial class RestingState : StateAbstract
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
		mMemoryBlackboard["Fatigue"]=0;
		return ((StateManagerBob)mStateManager).StateWorking;
	}

	protected override StateAbstract ProcessPhysicsAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,  LogObject mlogObject=null){
		return this;
	}

	protected override void EnterState(){
		//Using GD Print just for the example
		GD.Print("Oi I am tired. Going home to rest!");
		return;
	}
}