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

        public override StateAbstract GiveInitialState(ILogObject mLogObject = null)
        {
            return StateWorking;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, ILogObject mLogObject = null){
            StateWorking.InitializeState(FSMNode, this, mMemoryBlackboard);
            StateBanking.InitializeState(FSMNode,this,  mMemoryBlackboard);
            StateDrinking.InitializeState(FSMNode, this, mMemoryBlackboard);
            StateResting.InitializeState(FSMNode, this, mMemoryBlackboard);
            StateDead.InitializeState(FSMNode, this, mMemoryBlackboard);

            BobStateMachine = FSMNode as StateMachineActor;
        }
    }
}