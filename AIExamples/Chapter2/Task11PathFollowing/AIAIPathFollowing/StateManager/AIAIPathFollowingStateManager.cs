using System;
using CoreCode.Scripts;
using CoreCode.FSM;
using Godot;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAIAIPathFollowing{
    public partial class AIAIPathFollowingStateManager : StateManagerAbstract
    {
		//REPLACE THIS FOR THE INITIAL STATE.REMEMBER TO CREATE THE STATE WITH NEW()!
        //REMEMBER TO GIVE THE REFERENCE TO THE STATE MACHINE!
        public readonly PathFollowing pathFollowing = new PathFollowing();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(LogObject mLogObject = null)
        {
            return pathFollowing;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
            pathFollowing.InitializeState(FSMNode, this, mMemoryBlackboard);
        }


    }
}

