using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.Example{
	public partial class AudioPhasingTest : Node, IControlableByInput
	{

		private IAudioPlayer mAudioPlayer;
		private AudioStreamPlaybackInteractive mAudioStreamPlaybackInteractive;

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
			mAudioPlayer  = AudioPlayersRegister.Instance.GetAudioPlayer("MainMusic");
			mAudioStreamPlaybackInteractive = (mAudioPlayer as AudioStreamPlayer).GetStreamPlayback() as AudioStreamPlaybackInteractive;
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			
			if (mInputReader.IsButtonJustPressedInput("Up")){
				IAudioPlayer player = AudioPlayersRegister.Instance.GetAudioPlayer("MainMusic");
				mAudioStreamPlaybackInteractive.SwitchToClip(1);				
			}
			if (mInputReader.IsButtonJustPressedInput("Down")){
				IAudioPlayer player = AudioPlayersRegister.Instance.GetAudioPlayer("MainMusic");
				mAudioStreamPlaybackInteractive.SwitchToClip(0);		
			}
		}
	}
}