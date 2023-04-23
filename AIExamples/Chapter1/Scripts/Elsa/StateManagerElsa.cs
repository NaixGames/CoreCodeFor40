using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Else{
    public partial class StateManagerElsa : StateManagerAbstract
    {
        public readonly PeeState StatePee = new PeeState();
        public readonly HouseworkState StateHousework = new HouseworkState();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(LogObject mLogObject = null)
        {
            return StatePee;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
            StatePee.InitializeState(FSMNode, mMemoryBlackboard);
            StateHousework.InitializeState(FSMNode, mMemoryBlackboard);
        }

            //----------------------------------- Event system ------------------------------------------------

        [Signal] public delegate void FoodIsReadyEventHandler();

    }
}