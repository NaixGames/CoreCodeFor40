using System;
using CoreCode.Scripts;
using CoreCode.FSM;
using Godot;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAIBoid{
    public partial class AIBoidStateManager : StateManagerAbstract
    {
		//REPLACE THIS FOR THE INITIAL STATE.REMEMBER TO CREATE THE STATE WITH NEW()!
        //REMEMBER TO GIVE THE REFERENCE TO THE STATE MACHINE!
        public readonly BoidMovement boidMovement = new BoidMovement();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(ILogObject mLogObject = null)
        {
            return boidMovement;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, ILogObject mLogObject = null){
            boidMovement.InitializeState(FSMNode, this, mMemoryBlackboard);
        }


    }
}

