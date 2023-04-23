using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Bob{
	public partial class RestingState : StateAbstract
	{
	// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;
		private Godot.Collections.Dictionary mMemory;
		private StateManagerBob mStateManager;

		// -------------------------- Abstract overrides -------------------------------------

		public override void InitializeState(Node mNodeRef, Godot.Collections.Dictionary mMemoryBlackboard = null){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
		}
		
		protected override StateAbstract ProcessAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager, LogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			int fatigue = mMemoryBlackboard["Fatigue"].AsInt32()-1;
			mMemoryBlackboard["Fatigue"]=fatigue;
			if (fatigue==0){
				GD.Print("Welpy, timy to get bak to work!");
				return ((StateManagerBob)mStateManager).StateWorking;
			}
			GD.Print("Will continue to sleep a winy bit!");
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, Godot.Collections.Dictionary mMemoryBlackboard, StateManagerAbstract mStateManager,  LogObject mlogObject=null){
			return this;
		}

		protected override void EnterState(){
			//Using GD Print just for the example
			GD.Print("Oi I am tired. Going home to rest!");
			GD.Print("Honey I am home!");
			mStateManager.EmitSignal(nameof(mStateManager.BobIsHome));
			return;
		}

		protected override StateAbstract ProcessDelegatedEvent(string EventName, Godot.Collections.Dictionary mParameters = null, LogObject mlogObject=null){
			if (EventName == "FoodIsReady"){
				mMemory["Fatigue"]=0;
				GD.Print("With my darlin fud I can work again!");
				return (mStateManager).StateWorking;
			}
			return this;
		}
	}
}