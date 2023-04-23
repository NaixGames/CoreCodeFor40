using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Bob{

	public partial class WorkingState : StateAbstract
	{
		// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;
		private int mPocketSize;
		private int mMaxThirst;

		private int mMaxFatigue;

		// -------------------------- Abstract overrides -------------------------------------

		public override void InitializeState(Node mNodeRef, Godot.Collections.Dictionary mMemoryBlackboard = null){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			mPocketSize = mMemoryBlackboard["PocketSize"].AsInt32();
			mMaxThirst = mMemoryBlackboard["MaxThirst"].AsInt32();
			mMaxFatigue = mMemoryBlackboard["MaxFatigue"].AsInt32();
		}
		
		protected override StateAbstract ProcessAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager, LogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			int gold = mMemoryBlackboard["Gold"].AsInt32();
				gold++;
			mMemoryBlackboard["Gold"]=gold;
			//Using GD Print just for the example
			GD.Print("Ay I have gotten some gold. Now got " + gold);
			int thirst = mMemoryBlackboard["Thirst"].AsInt32();
			thirst ++;
			mMemoryBlackboard["Thirst"]=thirst;
			int fatigue = mMemoryBlackboard["Fatigue"].AsInt32();
			fatigue++;
			mMemoryBlackboard["Fatigue"]=fatigue;
			if (gold == mPocketSize){
				return ((StateManagerBob)mStateManager).StateBanking;
			}
			if (thirst >= mMaxThirst){
				return ((StateManagerBob)mStateManager).StateDrinking;
			}
			if (fatigue >= mMaxFatigue){
				return ((StateManagerBob)mStateManager).StateResting;
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,  LogObject mlogObject=null){
			return this;
		}

		protected override void EnterState(){
			//Using GD Print just for the example
			GD.Print("Oi going to get me some more gold!");
			return;
		}
	}
}
