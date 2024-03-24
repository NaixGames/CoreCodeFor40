using System;
using CoreCode.Scripts;
using CoreCode.FSM;
using Godot;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAIOffsetPirsuit{
    public partial class AIOffsetPirsuitStateManager : StateManagerAbstract
    {
		//REPLACE THIS FOR THE INITIAL STATE.REMEMBER TO CREATE THE STATE WITH NEW()!
        //REMEMBER TO GIVE THE REFERENCE TO THE STATE MACHINE!
        public readonly Pirsuit pirsuit = new Pirsuit();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(ILogObject mLogObject = null)
        {
            return pirsuit;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, ILogObject mLogObject = null){
            pirsuit.InitializeState(FSMNode, this, mMemoryBlackboard);
        }


    }
}

