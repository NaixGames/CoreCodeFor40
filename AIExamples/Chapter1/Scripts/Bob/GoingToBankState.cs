using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Bob{
	public partial class GoingToBankState : StateAbstract
	{
		// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;

		private int mMaxThirst;

		private int mMaxFatigue;


		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			mMaxThirst = mMemoryBlackboardCache["MaxThirst"].AsInt32();
			mMaxFatigue = mMemoryBlackboardCache["MaxFatigue"].AsInt32();
		}
		
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			int gold = mMemoryBlackboardCache["Gold"].AsInt32();
			int money = mMemoryBlackboardCache["Money"].AsInt32();
			money +=gold;
			mMemoryBlackboardCache["Money"]=money;
			mMemoryBlackboardCache["Gold"]=0;
			GD.Print("Oi, sold all my gold and now I got " + money + " of money!");
			int thirst = mMemoryBlackboardCache["Thirst"].AsInt32();
			int fatigue = mMemoryBlackboardCache["Fatigue"].AsInt32();

			if (thirst >= mMaxThirst){
				return ((StateManagerBob)mStateManagerCache).StateDrinking;
			}
			if (fatigue >= mMaxFatigue){
				return ((StateManagerBob)mStateManagerCache).StateResting;
			}

			return ((StateManagerBob)mStateManagerCache).StateWorking;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
			return this;
		}

		protected override void EnterState(){
			//Using GD Print just for the example
			GD.Print("Oi going to sell this gold to the bank to get me some more money!");
			return;
		}
	}
}