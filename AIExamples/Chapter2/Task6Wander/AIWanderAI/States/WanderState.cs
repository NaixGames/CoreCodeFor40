using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAIWanderAI{
	public partial class WanderState : StateAbstract
	{
		Godot.Collections.Dictionary<string, float> AxisCollection;
		private CharacterBody2D mCharacterBody;

		private float mWanderRadius;
		private float mWanderDistance;
		private float mWanderJitter;
		private Vector2 mWanderTarget;
		
		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			AxisCollection = (Godot.Collections.Dictionary<string, float>)mMemoryBlackboardCache["AxisContainer"];
			mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["CharacterNode"].AsNodePath());
			mWanderRadius =  (float)mMemoryBlackboardCache["WanderRadius"].AsDouble();
			mWanderDistance =  (float)mMemoryBlackboardCache["WanderDistance"].AsDouble();
			mWanderJitter =  (float)mMemoryBlackboardCache["WanderJitter"].AsDouble();
			mWanderTarget = -mCharacterBody.Transform.Y.Normalized();
		}

		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			//Put any action to be performed on update here.
			float deltaf = (float)delta;
			Vector2 input = SteeringBehaviour.WanderForce2D(-mCharacterBody.Transform.Y, mWanderRadius, mWanderDistance, mWanderJitter*deltaf, mWanderTarget,  out mWanderTarget);
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
