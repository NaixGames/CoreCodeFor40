using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;
using CoreCode.MathUtils;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAIOffsetPirsuit{
	public partial class Pirsuit : StateAbstract
	{
		private Godot.Collections.Dictionary<string, float> AxisCollection;
		private CharacterBody2D mObjective;
		private CharacterBody2D mCharacterBody;
		private float mRange;
		private float mVelocityTolerance;
		private Vector2 mOffsetVector;

		private Smoother<Vector2, Vector2Operations> mAverageInput;


		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			AxisCollection = (Godot.Collections.Dictionary<string, float>)mMemoryBlackboardCache["AxisContainer"];
			mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["CharacterNode"].AsNodePath());
			mObjective = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["Objective"].AsNodePath()); //in a better setting this would be set each time we enter the state.
			mRange = (float)mMemoryBlackboardCache["Range"].AsDouble();
			mVelocityTolerance = (float)mMemoryBlackboardCache["VelocityTolerance"].AsDouble();
			mOffsetVector = mMemoryBlackboardCache["OffsetVector"].AsVector2();

			mAverageInput = new Smoother<Vector2, Vector2Operations>(2);
		}
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			//Put any action to be performed on update here.
			Vector2 localOffsetPosition = mObjective.ToGlobal(mOffsetVector);
			Vector2 input =  mAverageInput.Smooth(SteeringBehaviour.ArriveDirectionForce2D(mCharacterBody.Position, localOffsetPosition, mCharacterBody.Velocity, mRange, mVelocityTolerance));
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
