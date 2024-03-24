using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.Example.AutomaticAI{
    public partial class AutomaticAIStateManager : StateManagerAbstract
    {
        public readonly WaitingState StateWaiting = new WaitingState();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(ILogObject mLogObject = null)
        {
            return StateWaiting;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, ILogObject mLogObject = null){
            StateWaiting.InitializeState(FSMNode, this, mMemoryBlackboard);
        }
    }
}