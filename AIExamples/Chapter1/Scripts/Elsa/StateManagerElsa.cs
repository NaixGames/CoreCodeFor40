using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Elsa{
    public partial class StateManagerElsa : StateManagerAbstract
    {
        private StateMachineActor ElsaStateMachine;
        public readonly PeeState StatePee = new PeeState();
        public readonly HouseworkState StateHousework = new HouseworkState();
        public readonly CookingState StateCooking = new CookingState();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(ILogObject mLogObject = null)
        {
            return StateHousework;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, ILogObject mLogObject = null){
            StatePee.InitializeState(FSMNode, this, mMemoryBlackboard);
            StateHousework.InitializeState(FSMNode, this, mMemoryBlackboard);
            StateCooking.InitializeState(FSMNode, this, mMemoryBlackboard);

            ElsaStateMachine = FSMNode as StateMachineActor;
        }

    }
}