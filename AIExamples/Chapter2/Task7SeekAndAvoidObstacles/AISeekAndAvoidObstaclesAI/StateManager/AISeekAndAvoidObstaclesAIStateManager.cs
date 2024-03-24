using System;
using CoreCode.Scripts;
using CoreCode.FSM;
using Godot;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAISeekAndAvoidObstaclesAI{
    public partial class AISeekAndAvoidObstaclesAIStateManager : StateManagerAbstract
    {
		//REPLACE THIS FOR THE INITIAL STATE.REMEMBER TO CREATE THE STATE WITH NEW()!
        //REMEMBER TO GIVE THE REFERENCE TO THE STATE MACHINE!
        public readonly SeekAndAvoidObstaclesState seekAndAvoidObstaclesState = new SeekAndAvoidObstaclesState();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(ILogObject mLogObject = null)
        {
            return seekAndAvoidObstaclesState;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, ILogObject mLogObject = null){
            seekAndAvoidObstaclesState.InitializeState(FSMNode, this, mMemoryBlackboard);
        }


    }
}

