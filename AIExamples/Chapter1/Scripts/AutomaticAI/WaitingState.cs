using Godot;
using System;
using CoreCode.FSM;
using CoreCode.Scripts;

namespace CoreCode.Example.AutomaticAI{
	public partial class WaitingState : StateAbstract
	{
		// -------------------------- Variables -------------------------------------
		private int mMaxWaitTime;

		Godot.Collections.Array<string> ButtonsCollections;

		private double mActualWaitingTime=0;

		// -------------------------- Abstract overrides -------------------------------------

		public override void InitializeState(Node mNodeRef, Godot.Collections.Dictionary mMemoryBlackboard = null){
			mMaxWaitTime = (int)mMemoryBlackboard["WaitingTime"].AsInt32();
			ButtonsCollections = (Godot.Collections.Array<string>)mMemoryBlackboard["ButtonsContainer"];
		}
		
		protected override StateAbstract ProcessAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,  LogObject mlogObject=null){
			mActualWaitingTime+=delta;
			if (mActualWaitingTime>=mMaxWaitTime){
				mActualWaitingTime-=mMaxWaitTime;
				ButtonsCollections.Add("Up");
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,   LogObject mlogObject=null){
			return this;
		}
	}
}
