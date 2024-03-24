// meta-name: State Template
// meta-description: Template for creating a state class

using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;

//This should be PROJECTNAME.ACTORNAME
namespace _STATENAMESPACE_{
	public partial class _CLASS_ : StateAbstract
	{
		
		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			
		}
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			//Put any action to be performed on update here.
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
			//Put any action to be performed on physics update here.
			return this;
		}
	}
}