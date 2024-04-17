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
		private ElsaReferenceHandler mElsaReferenceHandler;
		
		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();

			mNeededCookTime = mMemoryBlackboardCache["TimeForCooking"].AsInt32();

			TimeSpentCooking=0;

			mElsaReferenceHandler = mNodeRef.GetParent() as ElsaReferenceHandler;
		}
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			//Put any action to be performed on update here.
			if (!mInput.IsButtonJustPressedInput("Up")){
				return this;
			}
			GD.Print("I am cooking for my hosband!!");
			TimeSpentCooking+=1;
			if(TimeSpentCooking==mNeededCookTime){
				StateManagerElsa managerElsa = mStateManagerCache as StateManagerElsa;
				mElsaReferenceHandler.EmitSignal(ElsaReferenceHandler.SignalName.FoodIsReady);
				return ((StateManagerElsa)mStateManagerCache).StateHousework;
			}
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
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
