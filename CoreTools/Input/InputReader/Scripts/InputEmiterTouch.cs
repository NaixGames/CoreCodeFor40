using Godot;
using System;

namespace CoreCode.Scripts{
	//Tool for casting editor pluggins
	[Tool]
	public partial class InputEmiterTouch : Node
	{
		// ----------------------------------- Information ------------------------------------------------
		/*Script to emit InputMap for touch controls.*/

		// -----------------------------------

		private Vector2 mFirstPosition;   //First touch position
    	private Vector2 mLastPosition;   //Last touch position
    	[Export] private float mDragDistance=5f;  //minimum distance for a swipe to be registered

		private bool mRecording = false;
		private string mInputProcessed = "";

        public override void _Process(double delta)
        {
            base._Process(delta);
			if (mInputProcessed==""){
				return;
			}
			Input.ActionRelease(mInputProcessed);
			mInputProcessed="";
        }

        public override void _Input(InputEvent @event)
		{
			if (@event is InputEventScreenTouch eventKey){
				if (eventKey.Index != 0) // user is touching the screen with a single touch
				{
					return;
				}
				GD.Print("HERE");
				GD.Print(eventKey.Position);
				if (eventKey.Pressed == true && mRecording==false){
					mFirstPosition = eventKey.Position;
              		mLastPosition = mFirstPosition;
					mRecording = true;
					return;
				}
				if (eventKey.Pressed == true && mRecording==true){
					mLastPosition = eventKey.Position;
				}
				if (eventKey.Pressed == false && mRecording == true){
					mLastPosition = eventKey.Position;
					mRecording = false;

					Vector2 difVector = mLastPosition-mFirstPosition;

					if (difVector.Length() <= mDragDistance){
						return; 
					}

					//If we get here we actually gave a drag event

					if (Mathf.Abs(difVector.X) > Mathf.Abs(difVector.Y)){
						if (difVector.X > 0){
							//Trigger a right swipe event
							Input.ActionPress("RightP1", 1);
							mInputProcessed="RightP1";
						}
						else{
							//Trigger a left swipe event
							Input.ActionPress("LeftP1", 1);
							mInputProcessed="LeftP1";
						}
					}
					else{
						if (difVector.Y > 0){
							//Trigger a up swipe event
							Input.ActionPress("DownP1", 1);
							mInputProcessed="DownP1";
						}
						else{
							//Trigger a down swipe event
							Input.ActionPress("UpP1", 1);
							mInputProcessed="UpP1";
						}
					}
				}
			}
		}
	}
}