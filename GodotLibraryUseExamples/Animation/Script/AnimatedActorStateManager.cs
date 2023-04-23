using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.Example.AnimationExampleFSM{
    public partial class AnimatedActorStateManager : StateManagerAbstract
    {
        [Export] private AnimationPlayer mAnimator;

        public readonly AnimatedActorAnimateState AnimateState = new AnimatedActorAnimateState();


        //----------------------------------- Initial State ------------------------------------------------

        public override StateAbstract GiveInitialState(LogObject mLogObject = null)
        {
            return AnimateState;
        }
        
        public override void InitializeStates(Node FSMNode, Godot.Collections.Dictionary mMemoryBlackboard, LogObject mLogObject = null){
            AnimateState.InitializeState(FSMNode, mMemoryBlackboard);
        }
    }
}
