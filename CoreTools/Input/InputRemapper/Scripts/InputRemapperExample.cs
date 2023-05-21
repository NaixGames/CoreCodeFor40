using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.Example{
	public partial class InputRemapperExample : Node, IControlableByInput
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to make an example on how to remap inputs on Godot. To use in the future for
		remapping input menus */


		// ---------------------------------- Methods

		public void AddInputKeyToAction(InputEvent newKey, string ActionToAddTo){
			InputMap.ActionAddEvent(ActionToAddTo, newKey);
		}

		public void RemoveInputKeyFromAction(InputEvent oldKey, string ActionToRemoveFrom){
			if (InputMap.ActionHasEvent(ActionToRemoveFrom, oldKey)){
				InputMap.ActionEraseEvent(ActionToRemoveFrom, oldKey);
			}
		}

		public void RemapInputKeyAction(InputEvent oldKey, InputEvent newKey, string ActionToChange){
			AddInputKeyToAction(newKey,ActionToChange);
			RemoveInputKeyFromAction(oldKey, ActionToChange);
		}

		public InputEvent FromKeyToInputEvent(Godot.Key keyForEvent){
			InputEventKey newEvent = new InputEventKey();
			newEvent.PhysicalKeycode = keyForEvent;
			return newEvent;
		}

		public void RestoreInputToDefault(){
			InputMap.LoadFromProjectSettings();
		}

		// ------------------------------------ Variable for logging-----------------------------------------

		[Export] protected bool mShouldLog;
		protected LogObject mLogObject;

		// ------------------------------------ Variable for Input-----------------------------------------


		public InputReaderAbstract ReturnInputReader(){
			return mInputReader;
		}

		public void ClearInputReader(){
			mInputReader = InputManager.Instance.NullInputReader;
		}

		public void RecieveInputReader(InputReaderAbstract inputReaderPath){
			mInputReader = inputReaderPath;
		}

		//Code to execute for example in a scene.

		private InputReaderAbstract mInputReader;

		public override void _Ready()
		{
			RecieveInputReader(InputManager.Instance.GiveInputByPlayerChannel(this,1));
			
			if (mShouldLog){
				mLogObject = LogManager.Instance.RequestLogReference("Input", 0);
			}
		}

		public override void _Process(double delta)
		{
			base._Process(delta);

			if (mInputReader.IsButtonPressed("Up")){
				RemapInputKeyAction(FromKeyToInputEvent(Godot.Key.A), FromKeyToInputEvent(Godot.Key.Z), "LeftP1");
				RemapInputKeyAction(FromKeyToInputEvent(Godot.Key.D), FromKeyToInputEvent(Godot.Key.C), "RightP1");
				if (mShouldLog){
					mLogObject.AddToLogString("Remap code executed!");
				}
			}
			if (mInputReader.IsButtonPressed("Down")){
				RestoreInputToDefault();
				if (mShouldLog){
					mLogObject.AddToLogString("Remap code executed!");
				}
			}

		}

	}
}