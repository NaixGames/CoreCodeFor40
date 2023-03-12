using Godot;
using System;

public partial class GoingToBankState : StateAbstract
{
	// -------------------------- Variables -------------------------------------
	private InputReaderAbstract mInput;

	private int mMaxThirst;

	private int mMaxFatigue;


	// -------------------------- Abstract overrides -------------------------------------

	public override void InitializeState(Node mNodeRef, Godot.Collections.Dictionary mMemoryBlackboard = null){
		mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
		mMaxThirst = mMemoryBlackboard["MaxThirst"].AsInt32();
		mMaxFatigue = mMemoryBlackboard["MaxFatigue"].AsInt32();
	}
	
	protected override StateAbstract ProcessAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager, LogObject mlogObject=null){
		if (!mInput.IsButtonJustPressedInput("Up")){
			return this;
		}
		int gold = mMemoryBlackboard["Gold"].AsInt32();
		int money = mMemoryBlackboard["Money"].AsInt32();
		money +=gold;
		mMemoryBlackboard["Money"]=money;
		mMemoryBlackboard["Gold"]=0;
		GD.Print("Oi, sold all my gold and now I got " + money + " of money!");
		int thirst = mMemoryBlackboard["Thirst"].AsInt32();
		int fatigue = mMemoryBlackboard["Fatigue"].AsInt32();

		if (thirst >= mMaxThirst){
			return ((StateManagerBob)mStateManager).StateDrinking;
		}
		if (fatigue >= mMaxFatigue){
			return ((StateManagerBob)mStateManager).StateResting;
		}

		return ((StateManagerBob)mStateManager).StateWorking;
	}

	protected override StateAbstract ProcessPhysicsAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,  LogObject mlogObject=null){
		return this;
	}

	protected override void EnterState(){
		//Using GD Print just for the example
		GD.Print("Oi going to sell this gold to the bank to get me some more money!");
		return;
	}
}
