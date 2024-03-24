using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAIArriveAI{
	public partial class ArriveState : StateAbstract
	{
		
		Godot.Collections.Dictionary<string, float> AxisCollection;
		private CharacterBody2D mObjective;
		private CharacterBody2D mCharacterBody;

		private float mRange;
		private float mVelocityTolerance;

		
		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			AxisCollection = (Godot.Collections.Dictionary<string, float>)mMemoryBlackboardCache["AxisContainer"];
			mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["CharacterNode"].AsNodePath());
			mObjective = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["Objective"].AsNodePath()); //in a better setting this would be set each time we enter the state.
			mRange = (float)mMemoryBlackboardCache["Range"].AsDouble();
			mVelocityTolerance = (float)mMemoryBlackboardCache["VelocityTolerance"].AsDouble();
		}
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			//Put any action to be performed on update here.
			//When wanting to process input use something like AxisCollections.Add("Up", 0.5);
			Vector2 input = SteeringBehaviour.ArriveDirectionForce2D(mCharacterBody.Position, mObjective.Position, mCharacterBody.Velocity, mRange, mVelocityTolerance);
			AxisCollection.Add("Up",Mathf.Max(-input.Y,0));
			AxisCollection.Add("Down",Mathf.Max(input.Y,0));
			AxisCollection.Add("Left",Mathf.Max(-input.X,0));
			AxisCollection.Add("Right",Mathf.Max(input.X,0));
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
			//Put any action to be performed on physics update here.
			return this;
		}
	}
}
