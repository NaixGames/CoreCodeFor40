using Godot;
using System;

public partial class PlayerStateManagerExample : StateManagerAbstract
{
	public readonly PlayerStateJumping StateJumping = new PlayerStateJumping();
	public readonly PlayerStateMoving StateMoving = new PlayerStateMoving();
    public readonly PlayerStateFalling StateFalling = new PlayerStateFalling();


    //----------------------------------- Initial State ------------------------------------------------

    public override StateAbstract GiveInitialState(LogObject mLogObject = null)
    {
        return StateFalling;
    }
	
    public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
        StateJumping.InitializeState(FSMNode, mMemoryBlackboard);
        StateMoving.InitializeState(FSMNode, mMemoryBlackboard);
        StateFalling.InitializeState(FSMNode, mMemoryBlackboard);
    }

}
