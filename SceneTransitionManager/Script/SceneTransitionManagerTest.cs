using Godot;
using System;

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
				SceneTransitionManager.Instance.HeavyTransitionToNewScene("SceneOne");
				return;
			}
			else{
				SceneTransitionManager.Instance.LightTransitionToNewScene("SceneOne");
				return;
			}
		}
		if (mInputReader.IsButtonJustPressedInput("Down")){
			if (mHeavyLoad){
				SceneTransitionManager.Instance.HeavyTransitionToNewScene("SceneTwo");
				return;
			}
			else{
				SceneTransitionManager.Instance.LightTransitionToNewScene("SceneTwo");
				return;
			}
		}
	}
}
