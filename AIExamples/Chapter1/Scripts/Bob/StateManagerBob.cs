using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Bob{
    public partial class StateManagerBob : StateManagerAbstract
    {
        private StateMachineActor BobStateMachine;
        public readonly WorkingState StateWorking = new WorkingState();
        public readonly GoingToBankState StateBanking = new GoingToBankState();
        public readonly DrinkingState StateDrinking = new DrinkingState();
        public readonly RestingState StateResting = new RestingState();
        public readonly DeadState StateDead = new DeadState();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(LogObject mLogObject = null)
        {
            return StateWorking;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
            StateWorking.InitializeState(FSMNode, mMemoryBlackboard);
            StateBanking.InitializeState(FSMNode, mMemoryBlackboard);
            StateDrinking.InitializeState(FSMNode, mMemoryBlackboard);
            StateResting.InitializeState(FSMNode,mMemoryBlackboard);
            StateDead.InitializeState(FSMNode, mMemoryBlackboard);

            BobStateMachine = FSMNode as StateMachineActor;
        }

        //----------------------------------- Event system ------------------------------------------------

        [Signal] public delegate void BobIsHomeEventHandler();

        private void FoodIsReady(){
            BobStateMachine.GiveActualState().ExecuteDelegatedEvent("FoodIsReady");
        }
    }
}