using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.Example.AnimationExampleFSM{
    public partial class AnimatedActorStateManager : StateManagerAbstract
    {
        public readonly AnimatedActorAnimateState AnimateState = new AnimatedActorAnimateState();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(ILogObject mLogObject = null)
        {
            return AnimateState;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, ILogObject mLogObject = null){
            AnimateState.InitializeState(FSMNode, this, mMemoryBlackboard);
        }
    }
}
