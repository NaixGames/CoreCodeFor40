using Godot;
using CoreCode.FSM;
using CoreCode.Scripts;

//This should be PROJECTNAME.ACTORNAME
namespace CoreCode.AITestActorPlayerFSM{
	public partial class Moving : StateAbstract
	{
		
		// -------------------------- Variables -------------------------------------

		private CharacterBody2D mCharacterBody;
		private InputReaderAbstract mInput;
		private float mMaxSpeed;
		private float mAcceleration;
		private float mDrag;
		
		private float mForce;
		private float mMass;
		private Vector2 mInputBuffer;

		// -------------------------- Abstract overrides -------------------------------------

		protected override void InitializeStateParams(Node mNodeRef){
			mCharacterBody = mNodeRef.GetNode<CharacterBody2D>(mMemoryBlackboardCache["CharacterNode"].AsNodePath());
			mInput = (mNodeRef as StateMachineActor).ReturnInputReader();
			mMaxSpeed = (float)mMemoryBlackboardCache["MaxSpeed"].AsDouble();
			mAcceleration = (float)mMemoryBlackboardCache["Acceleration"].AsDouble();
			mDrag = (float)mMemoryBlackboardCache["Drag"].AsDouble();
			mMass = (float)mMemoryBlackboardCache["Mass"].AsDouble();
			mForce = (float)mMemoryBlackboardCache["Force"].AsDouble();
		}
		
		protected override StateAbstract ProcessAction(double delta, LogObject mlogObject=null){
			mInputBuffer = new Vector2(mInput.GiveAxisStrength("Right")-mInput.GiveAxisStrength("Left"), mInput.GiveAxisStrength("Down")-mInput.GiveAxisStrength("Up"));
			return this;
		}

		protected override StateAbstract ProcessPhysicsAction(double delta,  LogObject mlogObject=null){
			mCharacterBody.Velocity += mAcceleration*mInputBuffer*mForce/mMass;
			if(mCharacterBody.Velocity.Length() > mMaxSpeed){
				mCharacterBody.Velocity = mCharacterBody.Velocity.Normalized()*mMaxSpeed;
			}
			if (mInputBuffer.Length() > 0.001f){
				//Using the velocity to make the rotation appear smooth, but I am not exactly convinced by it.
				mCharacterBody.Rotation = Mathf.Atan2(mCharacterBody.Velocity.X, -mCharacterBody.Velocity.Y);
			}
			else{
				mCharacterBody.Velocity-=mCharacterBody.Velocity*mDrag*(float)delta;
			}
			mCharacterBody.MoveAndSlide();
			return this;
		}
	}
}
