using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.FSM;

namespace CoreCode.AIExamples.Bob{
	public partial class DrinkingState : StateAbstract
	{
		// -------------------------- Variables -------------------------------------
		private InputReaderAbstract mInput;

		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
		}
		
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			//If Bob does not have enough money to buy a Drink he dies of thirst :(
			int money = mMemoryBlackboardCache["Money"].AsInt32();
			if (money == 0){
				return ((StateManagerBob)mStateManagerCache).StateDead;
			}

			int thirst = mMemoryBlackboardCache["Thirst"].AsInt32();
			int change = Mathf.Min(money,thirst);
			money -=change;
			thirst -= change;

			mMemoryBlackboardCache["Thirst"]=thirst;
			mMemoryBlackboardCache["Money"]=money;

			GD.Print("Jeesz, I wasted " + change + " on drinks! Better back to work!");


			return ((StateManagerBob)mStateManagerCache).StateWorking;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
			return this;
		}

		protected override void EnterState(){
			//Using GD Print just for the example
			GD.Print("Oi gotten thirsty. Will go to drink a winny bit!");
			return;
		}
	}
}