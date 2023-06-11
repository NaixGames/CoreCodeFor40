using System;
using CoreCode.Scripts;
using CoreCode.FSM;
using Godot;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAISeekAI{
    public partial class AISeekAIStateManager : StateManagerAbstract
    {
		//REPLACE THIS FOR THE INITIAL STATE.REMEMBER TO CREATE THE STATE WITH NEW()!
        //REMEMBER TO GIVE THE REFERENCE TO THE STATE MACHINE!
        public readonly SeekState seek = new SeekState();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(LogObject mLogObject = null)
        {
            return seek;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
            seek.InitializeState(FSMNode, this, mMemoryBlackboard);
        }


    }
}

