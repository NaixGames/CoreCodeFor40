using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.ActorActorTestFSM{
	public partial class TestState : StateAbstract
	{
		
		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			
		}
		protected override StateAbstract ProcessAction(double delta, LogObject mlogObject=null){
			//Put any action to be performed on update here.
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, LogObject mlogObject=null){
			//Put any action to be performed on physics update here.
			return this;
		}
	}
}