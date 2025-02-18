using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Bob{
	public partial class RestingState : StateAbstract
	{
	// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;
		private BobReferenceHandler mBobReferenceHandler;

		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			mStateManagerCache = (mNodeRef as StateMachineActor).GiveStateManager();
			mBobReferenceHandler = mNodeRef.GetParent() as BobReferenceHandler; 
		}
		
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			int fatigue = mMemoryBlackboardCache["Fatigue"].AsInt32()-1;
			mMemoryBlackboardCache["Fatigue"]=fatigue;
			if (fatigue==0){
				GD.Print("Welpy, timy to get bak to work!");
				return ((StateManagerBob)mStateManagerCache).StateWorking;
			}
			GD.Print("Will continue to sleep a winy bit!");
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
			return this;
		}

		protected override void EnterState(){
			//Using GD Print just for the example
			GD.Print("Oi I am tired. Going home to rest!");
			GD.Print("Honey I am home!");
			mBobReferenceHandler.EmitSignal(BobReferenceHandler.SignalName.BobIsHome);
			return;
		}

		protected override StateAbstract OnUnqueuedDelegatedEvent(string EventName, ILogObject mlogObject=null){
			if (EventName == "FoodIsReady"){
				mMemoryBlackboardCache["Fatigue"]=0;
				GD.Print("With my darlin fud I can work again!");
				return (mStateManagerCache as StateManagerBob).StateWorking;
			}
			return this;
		}
	}
}