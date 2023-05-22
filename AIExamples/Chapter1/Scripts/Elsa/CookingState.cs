using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIExamples.Elsa{
	public partial class CookingState : StateAbstract
	{

		private InputReaderAbstract mInput;

		private int mNeededCookTime; 

		private int TimeSpentCooking=0;
		
		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();

			mNeededCookTime = mMemoryBlackboardCache["TimeForCooking"].AsInt32();

			TimeSpentCooking=0;
		}
		protected override StateAbstract ProcessAction(double delta, LogObject mlogObject=null){
			//Put any action to be performed on update here.
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			GD.Print("I am cooking for my hosband!!");
			TimeSpentCooking+=1;
			if(TimeSpentCooking==mNeededCookTime){
				StateManagerElsa managerElsa = mStateManagerCache as StateManagerElsa;
				mStateManagerCache.EmitSignal(nameof(managerElsa.FoodIsReady));
				return ((StateManagerElsa)mStateManagerCache).StateHousework;
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, LogObject mlogObject=null){
			//Put any action to be performed on physics update here.
			return this;
		}

		protected override void EnterState(){
			TimeSpentCooking=0;
			GD.Print("Time to cook for mi hosband!!");
			return;
		}
	}
}
