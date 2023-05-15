using System;
using CoreCode.Scripts;
using CoreCode.FSM;
using Godot;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.ActorAITestAI{
    public partial class AITestAIStateManager : StateManagerAbstract
    {
		//REPLACE THIS FOR THE INITIAL STATE.REMEMBER TO CREATE THE STATE WITH NEW()!
        //REMEMBER TO GIVE THE REFERENCE TO THE STATE MACHINE!
        public readonly TestStateAI testStateAI = new TestStateAI();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(LogObject mLogObject = null)
        {
            return testStateAI;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
            testStateAI.InitializeState(FSMNode, this, mMemoryBlackboard);
        }


    }
}

