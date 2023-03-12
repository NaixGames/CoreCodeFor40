using Godot;
using System;

public partial class AutomaticAIStateManager : StateManagerAbstract
{
	public readonly WaitingState StateWaiting = new WaitingState();


    //----------------------------------- Initial State ------------------------------------------------

    public override StateAbstract GiveInitialState(LogObject mLogObject = null)
    {
        return StateWaiting;
    }
	
    public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
        StateWaiting.InitializeState(FSMNode, mMemoryBlackboard);
    }
}
