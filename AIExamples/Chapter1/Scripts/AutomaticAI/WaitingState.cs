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

		protected override void InitializeStateParams(Node mNodeRef){
			mMaxWaitTime = (int)mMemoryBlackboardCache["WaitingTime"].AsInt32();
			ButtonsCollections = (Godot.Collections.Array<string>)mMemoryBlackboardCache["ButtonsContainer"];
		}
		
		protected override StateAbstract ProcessAction(double delta, LogObject mlogObject=null){
			mActualWaitingTime+=delta;
			if (mActualWaitingTime>=mMaxWaitTime){
				mActualWaitingTime-=mMaxWaitTime;
				ButtonsCollections.Add("Up");
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, LogObject mlogObject=null){
			return this;
		}
	}
}
