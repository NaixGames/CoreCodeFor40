using Godot;
using System;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	//Tool for casting editor pluggins
	[Tool]
	public partial class InputReaderTouch : InputReaderAbstract
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to manage player input when using a touch screen / mouse.
		This also includes differentiating swipes and dragging. It also stores data
		about inputs, low how long a drag has lasted, the last time we touched the screen, etc.*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/* Use is similar to InputReaderPlayer, but we use "Touch, Drag, Release" as other type of possible
		input types. 

		When touching an element that could respond to touch (InputTouchResponder) we bridge reader and responder,
		and we delegate particular behaviour to the responder.
		*/


		// Variables
    	[Export] private float mSwipeDistance = 5f;  //minimum distance for a swipe to be registered
		[Export] private float mSwipeTimer = 0.2f; //maximum time for getting a swipe detected
		private bool mIsTouching = false;
		private bool mIsDragging = false;

		//Variables for screen coordinates
		private Vector2 mTouchPosition; //This is on screen coordinates
		private Vector2 mReleasePosition; //This is on screen coordinates
		private Vector2 mDragPosition; //This is on screen coordinates
		public Vector2 TouchPosition => mTouchPosition;
		public Vector2 ReleasePosition => mReleasePosition;
		public Vector2 DragPosition => mDragPosition;

		// Variables for World coordinates
		private Vector3 mWorldTouchPosition; 
		private Vector3 mWorldReleasePosition;
		private Vector3 mWorldDragPosition;
		public Vector3 WorldTouchPosition => mWorldTouchPosition;
		public Vector3 WorldReleasePosition => mWorldReleasePosition;
		public Vector3 WorldDragPosition => mWorldDragPosition;

		// Variables for raycasting information

		public Vector3 RaycastOrigin;
		public Vector3 RaycastDirection;

		private List<string> mEventsThisFrame = new List<string>(3);
		private InputTouchResponder mInteractingResponder;

		[Export]
		private RayCast3D mRaycastNode;
		public RayCast3D RaycastNode => mRaycastNode;
		[Export]
		private float mRaycastLength;

		// Overriden method to update input
		protected override void UpdateInput(double delta)
		{
			// This is to take into the account that while we have the mouse still but we haven't
			// released the button we are still dragging.
			if (mIsDragging && !mEventsThisFrame.Contains("Drag")){
				mEventsThisFrame.Add("Drag");
			}

			for( int i=0; i< mAxis.Length ; i++){
				ProcessAxisValue(mAxis[i], IsActive && mEventsThisFrame.Contains(mAxis[i])? 1 : 0 , (float)delta);
			}
			for( int i=0; i< mButtons.Length ; i++){
				ProcessButtonPressValue(mButtons[i], IsActive && mEventsThisFrame.Contains(mButtons[i]), (float)delta);
			}

			mEventsThisFrame.Clear();
		}


		// Methods for touch screen
		private void DetectTouch(Vector2 touchPosition){
			mTouchPosition = touchPosition;
            mDragPosition = mTouchPosition;
			mEventsThisFrame.Add("Touch");
			mIsTouching = true;
			
			InputTouchResponder touchResponder = GetObjectInClick(touchPosition, out mWorldTouchPosition);
			if (touchResponder == null){
				return;
			}

			mInteractingResponder = touchResponder;
			mInteractingResponder.TouchRequest(this);
		}


		private void DetectDrag(Vector2 dragPosition){
			mEventsThisFrame.Add("Drag");
			mIsDragging = true;
			mDragPosition = dragPosition;
			InputTouchResponder touchResponder = GetObjectInClick(dragPosition, out mWorldDragPosition);
			
			if (!IsButtonPressed("Drag")){

				if (mInteractingResponder == null && touchResponder != null){
					mInteractingResponder = touchResponder;
				}
				mInteractingResponder?.DragStartedRequest(this);
				return;
			}

			//Send request to InputResponder, if any, that drag is getting done
			if (mInteractingResponder == null){
				return;
			}

			if (touchResponder == null && mInteractingResponder.NeedConsistentDrag()){
				mInteractingResponder.DragEndedRequest(this);
				mInteractingResponder = null;
				return;
			}

			mInteractingResponder.DragUpdatedRequest();
		}


		private void DetectSwipe(Vector2 initialPos, Vector2 finalPos){
			Vector2 difVector = finalPos-initialPos;
			if (difVector.Length() <= mSwipeDistance){
				return; 
			}

			if (Mathf.Abs(difVector.X) > Mathf.Abs(difVector.Y)){
				if (difVector.X > 0){
					mEventsThisFrame.Add("Right");
				}
				else{
					mEventsThisFrame.Add("Left");
				}
			}
			else{
				if (difVector.Y > 0){
					mEventsThisFrame.Add("Down");
				}
				else{
					mEventsThisFrame.Add("Up");
				}
			}
			mEventsThisFrame.Add("Swipe");

		}


		private void DetectRelease(Vector2 releasePosition){
			mEventsThisFrame.Add("Release" + PlayerIdAndP());
			mReleasePosition = releasePosition;
			mIsTouching = false;
			
			//If no responder, we assume the player wanted to do release on other object
			//And so we check if there is any
			InputTouchResponder touchResponder = GetObjectInClick(releasePosition, out mWorldReleasePosition);

			if (mInteractingResponder != null){
				mInteractingResponder.ReleaseRequest(this);

				if (mInteractingResponder.NeedConsistentDrag() && (touchResponder != mInteractingResponder)){
					mInteractingResponder = null;
					mIsDragging = false;
					return;
				}
				
				if (mIsDragging && TimeSinceLastButtonInput("Touch") >= mSwipeTimer  && !mEventsThisFrame.Contains("Swipe")){
					mInteractingResponder.DragEndedRequest(this, touchResponder);
				}
				mIsDragging = false;
				mInteractingResponder = null;
				return;
			}

			mInteractingResponder = touchResponder;
			mInteractingResponder?.ReleaseRequest(this);
			
			if (mIsDragging && TimeSinceLastButtonInput("Touch") >= mSwipeTimer  && !mEventsThisFrame.Contains("Swipe")){
				mInteractingResponder?.DragEndedRequest(this);
			}

			mIsDragging = false;
			mInteractingResponder = null;
		}


		// Method that hooks up to Godot InputEvent system
		public override void _Input(InputEvent @event)
		{
			if (@event is InputEventScreenTouch){
				InputEventScreenTouch eventKey = @event as InputEventScreenTouch;
				if (eventKey.Index != 0) 
				{
					return;
				}

				if (eventKey.Pressed && !mIsTouching){
              		DetectTouch(eventKey.Position);
					return;
				}
				
				if (!eventKey.Pressed && mIsTouching){
					DetectRelease(eventKey.Position);
					if (TimeSinceLastButtonInput("Touch") < mSwipeTimer && !mEventsThisFrame.Contains("Drag")){
						DetectSwipe(mTouchPosition, eventKey.Position);
					}
				}
			}

			else if (@event is InputEventScreenDrag){
				InputEventScreenDrag eventKey = @event as InputEventScreenDrag;

				if (eventKey.Index != 0) 
				{
					return;
				}

				if (mIsTouching){
					if (TimeSinceLastButtonInput("Touch") >= mSwipeTimer  && !mEventsThisFrame.Contains("Swipe")){
						DetectDrag(eventKey.Position);
					}
				}
			}
		}



		public void ClearResponder(){
			mInteractingResponder = null;
		}

		// Override method to log

		public override void _Ready(){
			if (Engine.IsEditorHint()){
                return;
            }
			base._Ready();
			
			mLogObject = LogManager.Instance.RequestLog("Input", mShouldLog);
		}

		private void TransformScreenPositionToWorldCoordinates(Vector2 screenPostion){
			Camera3D camera = Camera3DManager.Instance.GiveMainCamera();
			RaycastOrigin = camera.ProjectRayOrigin(screenPostion);
			int stretchFactor = (camera as Camera3DInstance).GetStretchFactor();
			RaycastDirection = camera.ProjectRayNormal(screenPostion/stretchFactor);
		}


		private InputTouchResponder GetObjectInClick(Vector2 screenPosition, out Vector3 touchPosition){
			TransformScreenPositionToWorldCoordinates(screenPosition);
			mRaycastNode.GlobalPosition = RaycastOrigin;
			mRaycastNode.TargetPosition = RaycastDirection*mRaycastLength;
			mRaycastNode.ForceRaycastUpdate();
			touchPosition = mRaycastNode.GetCollisionPoint();
	
			if (mRaycastNode.GetCollider() != null && !(mRaycastNode.GetCollider() is InputTouchResponder)){
				return null;
			}
			return mRaycastNode.GetCollider() as InputTouchResponder;
		}
		

		// Helper method
		public override string PlayerIdAndP(){
			return "P"+mPlayerID;
		}	


	}
}