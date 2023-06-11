using System;
using CoreCode.Scripts;
using CoreCode.FSM;
using Godot;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AITestActorPlayerFSM{
    public partial class ActorPlayerFSMStateManager : StateManagerAbstract
    {
        public readonly Moving moving = new Moving();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(LogObject mLogObject = null)
        {
            return moving;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
            moving.InitializeState(FSMNode, this, mMemoryBlackboard);
        }


    }
}

