using System;
using CoreCode.Scripts;
using CoreCode.FSM;
using Godot;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAIFleeAI{
    public partial class AIFleeAIStateManager : StateManagerAbstract
    {
		//REPLACE THIS FOR THE INITIAL STATE.REMEMBER TO CREATE THE STATE WITH NEW()!
        //REMEMBER TO GIVE THE REFERENCE TO THE STATE MACHINE!
        public readonly FleeState flee = new FleeState();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(LogObject mLogObject = null)
        {
            return flee;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
            flee.InitializeState(FSMNode, this, mMemoryBlackboard);
        }


    }
}

