using Godot;
using System;
using CoreCode.Scripts;
using CoreCode.AudioSystem;

namespace CoreCode.Example{
	public partial class AudioPhasingTest : Node, IControlableByInput
	{

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

		// ------------------------------------ Normal Overrides -----------------------------------------
		


		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			RecieveInputReader(InputManager.Instance.GiveInputByPlayerChannel(this, 1));
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			
			if (mInputReader.IsButtonJustPressedInput("Up")){
				AudioManager.Instance.TransitionToOtherTrack(2);
			}
			if (mInputReader.IsButtonJustPressedInput("Down")){
				AudioManager.Instance.TransitionToOtherTrack(0);
			}
		}
	}
}