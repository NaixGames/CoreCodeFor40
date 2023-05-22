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

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			mPocketSize = mMemoryBlackboardCache["PocketSize"].AsInt32();
			mMaxThirst = mMemoryBlackboardCache["MaxThirst"].AsInt32();
			mMaxFatigue = mMemoryBlackboardCache["MaxFatigue"].AsInt32();
		}
		
		protected override StateAbstract ProcessAction(double delta, LogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			int gold = mMemoryBlackboardCache["Gold"].AsInt32();
			gold++;
			mMemoryBlackboardCache["Gold"]=gold;
			//Using GD Print just for the example
			GD.Print("Ay I have gotten some gold. Now got " + gold);
			int thirst = mMemoryBlackboardCache["Thirst"].AsInt32();
			thirst ++;
			mMemoryBlackboardCache["Thirst"]=thirst;
			int fatigue = mMemoryBlackboardCache["Fatigue"].AsInt32();
			fatigue++;
			mMemoryBlackboardCache["Fatigue"]=fatigue;
			if (gold == mPocketSize){
				return ((StateManagerBob)mStateManagerCache).StateBanking;
			}
			if (thirst >= mMaxThirst){
				return ((StateManagerBob)mStateManagerCache).StateDrinking;
			}
			if (fatigue >= mMaxFatigue){
				return ((StateManagerBob)mStateManagerCache).StateResting;
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, LogObject mlogObject=null){
			return this;
		}

		protected override void EnterState(){
			//Using GD Print just for the example
			GD.Print("Oi going to get me some more gold!");
			return;
		}
	}
}
