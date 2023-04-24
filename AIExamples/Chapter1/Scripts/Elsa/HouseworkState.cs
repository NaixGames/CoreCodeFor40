using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Elsa{
	public partial class HouseworkState : StateAbstract
	{
		// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;
		private Godot.Collections.Dictionary mMemory;


		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			mMemory=mMemoryBlackboardCache;
			mStateManagerCache = (mNodeRef as StateMachineActor).GiveStateManager();
		}
		
		protected override StateAbstract ProcessAction(double delta, LogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			GD.Print("I am doing housework!!");
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
				return (mStateManagerCache as StateManagerElsa).StatePee;
			}
			return this;
		}
	}
}