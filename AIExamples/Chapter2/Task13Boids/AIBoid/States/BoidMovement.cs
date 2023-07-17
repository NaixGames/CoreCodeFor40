using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;
using CoreCode.MathUtils;
using System.Collections.Generic;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AIAIBoid{
	public partial class BoidMovement : StateAbstract
	{
		private Godot.Collections.Dictionary<string, float> AxisCollection;
		private CharacterBody2D mCharacterBody;
		private Smoother<Vector2, Vector2Operations> mAverageInput;
		private BoidActorsManager2D mBoidActorsManager;
		private List<Node2D> ActorsBoid; 

		//Parameters for boid

		private float mToleranceMagnitudeSeparation = 0;
		private float mWeightSeparation;
		private float mWeightAlignment;
		private float mWeightCohesion;
		private float mRangeCohesion;

		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			AxisCollection = (Godot.Collections.Dictionary<string, float>)mMemoryBlackboardCache["AxisContainer"];
			mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["CharacterNode"].AsNodePath());
			mBoidActorsManager = mNodeRef.GetNode<BoidActorsManager2D>(mMemoryBlackboardCache["BoidActorsManager"].AsNodePath());

			mToleranceMagnitudeSeparation = (float)mMemoryBlackboardCache["ToleranceMagnitudeSeparation"].AsDouble();
			mWeightSeparation = (float)mMemoryBlackboardCache["WeightSeparation"].AsDouble();
			mWeightAlignment = (float)mMemoryBlackboardCache["WeightAlignment"].AsDouble();
			mWeightCohesion = (float)mMemoryBlackboardCache["WeightCohesion"].AsDouble();
			mRangeCohesion = (float)mMemoryBlackboardCache["RangeCohesion"].AsDouble();

			mAverageInput = new Smoother<Vector2, Vector2Operations>(25);
		}
		protected override StateAbstract ProcessAction(double delta, LogObject mlogObject=null){
			//Put any action to be performed on update here.s
			ActorsBoid = mBoidActorsManager.ActorsForBoid;

			Vector2 acumulatedInput = Vector2.Zero;
			
			if (ActorsBoid.Count!=0){
				acumulatedInput += mWeightSeparation*BoidSteeringBehaviour.SeparationForce2D(mCharacterBody.GlobalPosition, ActorsBoid, mToleranceMagnitudeSeparation);
				acumulatedInput += mWeightAlignment*BoidSteeringBehaviour.AlignmentForce2D(-mCharacterBody.Transform.Y, ActorsBoid);
				acumulatedInput += mWeightCohesion*BoidSteeringBehaviour.CohesionForce2D(mCharacterBody.GlobalPosition, ActorsBoid, mCharacterBody.Velocity, mRangeCohesion);
			}

			acumulatedInput = mAverageInput.Smooth(acumulatedInput);
			
			AxisCollection.Add("Up",Mathf.Max(-acumulatedInput.Y,0));
			AxisCollection.Add("Down",Mathf.Max(acumulatedInput.Y,0));
			AxisCollection.Add("Left",Mathf.Max(-acumulatedInput.X,0));
			AxisCollection.Add("Right",Mathf.Max(acumulatedInput.X,0));
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta, LogObject mlogObject=null){
			//Put any action to be performed on physics update here.
			return this;
		}
	}
}
