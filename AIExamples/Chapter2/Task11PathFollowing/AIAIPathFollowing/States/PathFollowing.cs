using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAIAIPathFollowing{
	public partial class PathFollowing : StateAbstract
	{

		Godot.Collections.Dictionary<string, float> AxisCollection;
		private CharacterBody2D mCharacterBody;
		private float mSplineRuningSpeed;
		private PathFollow2D mPathFollower;
		private float mToleranceForNextPoint;
		
		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			AxisCollection = (Godot.Collections.Dictionary<string, float>)mMemoryBlackboardCache["AxisContainer"];
			mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["CharacterNode"].AsNodePath());
			mPathFollower = mNodeRef.GetNode<PathFollow2D>(mMemoryBlackboardCache["PathFollower"].AsNodePath());
			mToleranceForNextPoint = (float)mMemoryBlackboardCache["NextPointTolerance"].AsDouble();
			mSplineRuningSpeed = (float)mMemoryBlackboardCache["SplineRuningSpeed"].AsDouble();
			
		}
		protected override StateAbstract ProcessAction(double delta, ILogObject mlogObject=null){
			//Put any action to be performed on update here.
			Vector2 splineObjectivePosition = mPathFollower.GlobalPosition;

			//This works fine if the tolerance for the point and the spline runing speed are good enough.
			//Could potentially be more tolerance if this was a while with a break instead of just one if.
			if ((splineObjectivePosition-mCharacterBody.GlobalPosition).Length() < mToleranceForNextPoint){
				mPathFollower.Progress += (float)delta*mSplineRuningSpeed;
				splineObjectivePosition = mPathFollower.GlobalPosition;
			}
			//This will loop through the spline. If one wants it to end at the last point needs to detect if we are close to the end
			// and use arrive on that last point.
			Vector2 input = SteeringBehaviour.SeekDirectionForce2D(mCharacterBody.GlobalPosition, splineObjectivePosition, mCharacterBody.Velocity);
			AxisCollection.Add("Up",Mathf.Max(-input.Y,0));
			AxisCollection.Add("Down",Mathf.Max(input.Y,0));
			AxisCollection.Add("Left",Mathf.Max(-input.X,0));
			AxisCollection.Add("Right",Mathf.Max(input.X,0));
			GD.Print("Input:" + input);
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null){
			//Put any action to be performed on physics update here.
			return this;
		}
	}
}
