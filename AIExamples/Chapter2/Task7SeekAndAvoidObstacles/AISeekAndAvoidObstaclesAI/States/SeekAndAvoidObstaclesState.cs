using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;
using CoreCode.MathUtils;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAISeekAndAvoidObstaclesAI{
	public partial class SeekAndAvoidObstaclesState : StateAbstract
	{
		Godot.Collections.Dictionary<string, float> AxisCollection;
		Node2D mObjective;
		private CharacterBody2D mCharacterBody;
		private RayCast2D mLeftRaycast;
		private RayCast2D mRightRaycast;
		private float mAvoidanceRange;

		//I guess this is wrong as I also need to provide operations?
		private Smoother<Vector2, Vector2Operations> mAverageInput;

		private int mAverageWindow = 10;

		private float mSeekWeight = 0.2f;
	
		private float mAvoidanceWeight = 0.8f;

		private float mLateralAvoidance = 0.7f;

		private float mBreakingAvoidance = 0.3f;

		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			AxisCollection = (Godot.Collections.Dictionary<string, float>)mMemoryBlackboardCache["AxisContainer"];
			mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["CharacterNode"].AsNodePath());
			mObjective = mNodeRef.GetNode<Node2D>(mMemoryBlackboardCache["Objective"].AsNodePath()); //in a better setting this would be set each time we enter the state.
			mLeftRaycast = mNodeRef.GetNode<RayCast2D>(mMemoryBlackboardCache["LeftRaycast"].AsNodePath());
			mRightRaycast = mNodeRef.GetNode<RayCast2D>(mMemoryBlackboardCache["RightRaycast"].AsNodePath());
			mAverageWindow = (int)mMemoryBlackboardCache["InputAverageWindow"].AsInt32();
			mSeekWeight = (float)mMemoryBlackboardCache["SeekWeight"].AsDouble();
			mAvoidanceWeight = (float)mMemoryBlackboardCache["AvoidanceWeight"].AsDouble();
			mLateralAvoidance = (float)mMemoryBlackboardCache["LateralAvoidanceWeight"].AsDouble();
			mBreakingAvoidance =  (float)mMemoryBlackboardCache["BreakingAvoidanceWeight"].AsDouble();

			//I guess this is wrong as I also need to provide operations?
			mAverageInput = new Smoother<Vector2, Vector2Operations>(mAverageWindow);
			mAvoidanceRange = Mathf.Max(mLeftRaycast.TargetPosition.Length(), mRightRaycast.TargetPosition.Length());
		}
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			Vector2 inputThisFrame;
			Vector2 inputSeek = SteeringBehaviour.SeekDirectionForce2D(mCharacterBody.Position, mObjective.Position, mCharacterBody.Velocity);

			float leftDistance = mLeftRaycast.IsColliding()? (mLeftRaycast.GetCollisionPoint()-mLeftRaycast.Position -mCharacterBody.Position).Length() : Mathf.Inf;
			float rightDistance = mRightRaycast.IsColliding()?(mRightRaycast.GetCollisionPoint()-mRightRaycast.Position-mCharacterBody.Position).Length() : Mathf.Inf;
			float correctionWeight = Mathf.Min(leftDistance, rightDistance)/mAvoidanceRange;

			if (correctionWeight <= 1){
				//Doing obstacle avoidance here, since it seems quite particular of a certain situation.
				//Could move into the library if that is no the case in the long term.
				//Also, in a real seek with obstacle avoidance I would instead use Pathfinding + the Seek class, so not really this :)
				Vector2 hitNormal; float distance;
				if (leftDistance < rightDistance){ 
					hitNormal = mLeftRaycast.GetCollisionNormal();
					distance = leftDistance;
				}
				else{
					hitNormal = mRightRaycast.GetCollisionNormal();
					distance = rightDistance;
				}
				Vector2 velocityDirection = mCharacterBody.Velocity.Normalized();
				Vector2 inputAvoidance = GetLateralInputCorrection(velocityDirection, hitNormal, distance).Normalized();
				
				//How we deal with this input could be improve by averraging over the last X frames.
				inputThisFrame = mAverageInput.Smooth(inputSeek*mSeekWeight + inputAvoidance*mAvoidanceWeight);
				
			}
			else{
				inputThisFrame = mAverageInput.Smooth(inputSeek);
			}

			AxisCollection.Add("Up",Mathf.Max(-inputThisFrame.Y,0));
			AxisCollection.Add("Down",Mathf.Max(inputThisFrame.Y,0));
			AxisCollection.Add("Left",Mathf.Max(-inputThisFrame.X,0));
			AxisCollection.Add("Right",Mathf.Max(inputThisFrame.X,0));
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
			//Put any action to be performed on physics update here.
			return this;
		}

		//--------------------------

		private Vector2 GetLateralInputCorrection(Vector2 velocity, Vector2 normal, float distance){
			Vector2 perpVelocity = new Vector2(velocity.Y, -velocity.X);
			float slopeRaycast = perpVelocity.Dot(normal);
			return -mCharacterBody.Transform.X*slopeRaycast*mLateralAvoidance+mBreakingAvoidance*mCharacterBody.Transform.Y*(mAvoidanceRange-distance)/mAvoidanceRange;
		}
	}
}
