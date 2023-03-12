using Godot;
using System;

public partial class StateManagerBob : StateManagerAbstract
{
    public readonly WorkingState StateWorking = new WorkingState();
    public readonly GoingToBankState StateBanking = new GoingToBankState();
    public readonly DrinkingState StateDrinking = new DrinkingState();
    public readonly RestingState StateResting = new RestingState();
    public readonly DeadState StateDead = new DeadState();


    //----------------------------------- Initial State ------------------------------------------------

    public override StateAbstract GiveInitialState(LogObject mLogObject = null)
    {
        return StateWorking;
    }
	
    public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
        StateWorking.InitializeState(FSMNode, mMemoryBlackboard);
        StateBanking.InitializeState(FSMNode, mMemoryBlackboard);
        StateDrinking.InitializeState(FSMNode, mMemoryBlackboard);
        StateResting.InitializeState(FSMNode,mMemoryBlackboard);
    }

}
