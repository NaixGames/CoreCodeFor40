using System;
using CoreCode.Scripts;
using CoreCode.FSM;
using Godot;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.ActorActorCreateFSM{
    public partial class ActorCreateFSMStateManager : StateManagerAbstract
    {
		//REPLACE THIS FOR THE INITIAL STATE.REMEMBER TO CREATE THE STATE WITH NEW()!
        //REMEMBER TO GIVE THE REFERENCE TO THE STATE MACHINE!
        public readonly TestStateActor testStateActor = new TestStateActor();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(LogObject mLogObject = null)
        {
            return testStateActor;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
            testStateActor.InitializeState(FSMNode, this, mMemoryBlackboard);
        }


    }
}

