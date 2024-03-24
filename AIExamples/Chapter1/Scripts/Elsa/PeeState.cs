using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Elsa{
	public partial class PeeState : StateAbstract
	{
		// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;

		private int mTimeForPee;

		private int TimeSpentPeeing;


		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();

			mTimeForPee = mMemoryBlackboardCache["TimeForPee"].AsInt32();
			TimeSpentPeeing=0;
		}
		
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			GD.Print("I am in the bathroom maity!!");
			TimeSpentPeeing+=1;
			if(TimeSpentPeeing==mTimeForPee){
				mMemoryBlackboardCache["BladerLevel"]=0;
				return ((StateManagerElsa)mStateManagerCache).StateHousework;
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
			return this;
		}

		protected override void EnterState(){
			//Using GD Print just for the example
			GD.Print("Oi I really need to pee!");
			TimeSpentPeeing=0;
			return;
		}
	}
}