using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Elsa{
	public partial class HouseworkState : StateAbstract
	{
		// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;

		private int mMaxBladerTime; 


		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();

			mMaxBladerTime = mMemoryBlackboardCache["BladerFillTime"].AsInt32();
		}
		
		protected override StateAbstract ProcessAction(double delta, LogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			GD.Print("I am doing housework!!");
			int blader = mMemoryBlackboardCache["BladerLevel"].AsInt32();
			blader++;
			mMemoryBlackboardCache["BladerLevel"]=blader;
			if (blader >= mMaxBladerTime){
				return (mStateManagerCache as StateManagerElsa).StatePee;
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, LogObject mlogObject=null){
			return this;
		}

		protected override void EnterState(){
			//Using GD Print just for the example
			GD.Print("Welp, will do some shore rround the huse!!");
			return;
		}


		protected override StateAbstract ProcessDelegatedEvent(string EventName, LogObject mlogObject=null){
			if (EventName == "BobIsHome"){
				GD.Print("My hosband is hom. Will go and mak fud!!");
				return (mStateManagerCache as StateManagerElsa).StateCooking;
			}
			return this;
		}
	}
}