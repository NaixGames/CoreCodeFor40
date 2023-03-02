using Godot;
using System;

public partial class AudioPhasingTest : Node, IControlableByInput
{

	//Variables for test
	[Export] private AdaptativeMusicTrack mFirstTrack;

	[Export] private AdaptativeMusicTrack mSecondTrack;

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
		//USING GD.PRINT AS THIS IS A TEST BUT THIS SHOULDN'T BE THE DEFAULT!

		GD.Print("ACTUAL TRACK:" + AudioManager.Instance.MusicManager.ActualMusicTrack.Name);
		
		if (mInputReader.IsButtonJustPressedInput("Up")){
			AudioManager.Instance.TransitionToOtherTrack(2);
			GD.Print("GO TO TRACK 2");
		}
		if (mInputReader.IsButtonJustPressedInput("Down")){
			AudioManager.Instance.TransitionToOtherTrack(0);
			GD.Print("GO TO TRACK 0");
		}
	}
}
