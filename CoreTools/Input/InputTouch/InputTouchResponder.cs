using Godot;
using System;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	public abstract partial class InputTouchResponder : Area3D
	{
		// Information 
		/* This is a base class to have an object interact with an InputReaderTouch. This is done
		by supplying template methods that answer to touch, release and drag events.*/
		
		// Use 
		/* As a template pattern; have a class inherit from it and replace the answer methods */


		[Export]
		public bool RespondToTouch = true;
		[Export]
		public bool RespondToDrag = true;
		[Export]
		public bool RespondToRelease = true;
		protected InputReaderTouch mInputReaderTouch;
		public virtual bool NeedConsistentDrag() => true;


		// Using a template method to deal with different responses

		public void TouchRequest(InputReaderTouch inputReaderTouch){
			if (!RespondToTouch){
				return;
			}

			mInputReaderTouch = inputReaderTouch;
			TouchAnswer();
		}
	

		public void ReleaseRequest(InputReaderTouch inputReaderTouch){
			if (!RespondToRelease){
				return;
			}

			mInputReaderTouch = inputReaderTouch;
			ReleaseAnswer();
		}


		public void DragStartedRequest(InputReaderTouch inputReaderTouch){
			if (!RespondToDrag){
				return;
			}

			mInputReaderTouch = inputReaderTouch;
			DragStartedAnswer();
		}


		public void DragEndedRequest(InputReaderTouch inputReaderTouch, InputTouchResponder touchResponderBelow = null){
			if (!RespondToDrag){
				return;
			}

			mInputReaderTouch = inputReaderTouch;
			DragEndedAnswer(touchResponderBelow);
		}


		public void DragUpdatedRequest(InputTouchResponder touchResponderBelow = null){
			if (!RespondToDrag){
				return;
			}
			DragUpdateAnswer(touchResponderBelow);
		}


		// Template methods

		protected virtual void TouchAnswer(){}


		protected virtual void DragStartedAnswer(){}


		protected virtual void DragEndedAnswer(InputTouchResponder touchResponderBelow = null){}


		protected virtual void ReleaseAnswer(){}


		protected virtual void DragUpdateAnswer(InputTouchResponder touchResponderBelow = null){}
		
	}
}