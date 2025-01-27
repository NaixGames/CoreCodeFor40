using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.Example{
	public partial class SceneTransitionManagerTest : Node, IControlableByInput
	{
		// Called when the node enters the scene tree for the first time.

		[Export] private bool mHeavyLoad=false;

		// ------------------------------------ Methods for IControlableByInput interface-----------------------------------------
		
		private InputReaderAbstract mInputReader;

		public InputReaderAbstract ReturnInputReader(){
			if (mInputReader==null){
				mInputReader = InputManager.Instance.NullInputReader;
			}
			return mInputReader;
		}

		public void RecieveInputReader(InputReaderAbstract mInputReaderNew){
			mInputReader = mInputReaderNew;
		}

		public void ClearInputReader(){
			mInputReader = InputManager.Instance.NullInputReader;
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			RecieveInputReader(InputManager.Instance.GiveInputByPlayerChannel(this, 1));
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (mInputReader.IsButtonJustPressedInput("Up")){
				if (mHeavyLoad){
					SceneTransitionManager.Instance.TransitionToNewScene("ExampleScene1", true);
					return;
				}
				else{
					SceneTransitionManager.Instance.TransitionToNewScene("ExampleScene1", false);
					return;
				}
			}
			if (mInputReader.IsButtonJustPressedInput("Down")){
				if (mHeavyLoad){
					SceneTransitionManager.Instance.TransitionToNewScene("ExampleScene2", true);
					return;
				}
				else{
					SceneTransitionManager.Instance.TransitionToNewScene("ExampleScene2", false);
					return;
				}
			}
		}
	}
}