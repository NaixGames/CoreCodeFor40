using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.Example.DummyPlayerFSM{
    public partial class PlayerStateManagerExample : StateManagerAbstract
    {
        public readonly PlayerStateJumping StateJumping = new PlayerStateJumping();
        public readonly PlayerStateMoving StateMoving = new PlayerStateMoving();
        public readonly PlayerStateFalling StateFalling = new PlayerStateFalling();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(ILogObject mLogObject = null)
        {
            return StateFalling;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, ILogObject mLogObject = null){
            StateJumping.InitializeState(FSMNode, this, mMemoryBlackboard);
            StateMoving.InitializeState(FSMNode, this,  mMemoryBlackboard);
            StateFalling.InitializeState(FSMNode, this, mMemoryBlackboard);
        }

    }
}