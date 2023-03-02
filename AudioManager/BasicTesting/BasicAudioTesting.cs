using Godot;
using System;

public partial class BasicAudioTesting : Node, IControlableByInput
{

	private InputReaderAbstract mInputReader;

	[Export] private AudioStreamPlayer mAudioStream;
	
	//Interface methods
	public InputReaderAbstract ReturnInputReader()
	{
		return mInputReader;
	}

	public void ClearInputReader(){
		mInputReader = InputManager.Instance.NullInputReader;
	}

	public void RecieveInputReader(InputReaderAbstract inputReaderPath){
		mInputReader = inputReaderPath;
	}


	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		RecieveInputReader(InputManager.Instance.GiveInputByPlayerChannel(this, 1));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (mInputReader.IsAxisJustPressedInput("Right")){
			mAudioStream.Play();
		}
		if (mInputReader.IsAxisJustPressedInput("Left")){
			mAudioStream.Stop();
		}
	}
}
