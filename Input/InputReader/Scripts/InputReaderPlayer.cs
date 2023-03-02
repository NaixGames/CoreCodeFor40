using Godot;
using System;

[Tool]
public partial class InputReaderPlayer : InputReaderAbstract
{
	// ----------------------------------- Information ------------------------------------------------
	/*This is a script to manage player input manually, including buffers and button combinations. 
	This should serve to abstract a bit the interface from Godot, while also gaining more control.*/
	
	// ------------------------------------ Use -------------------------------------------------------
	/* When want to give input to an object, an instance of this class should be reference. Button presses/combinations
	will be stored in variables that are either float or bools (depending on if it is an axis or button)

	For the correct use of this class it is important that input mappings in the project retain a consistant format.
	Ie, it should be "ACTION"P"PLAYERNUMBER". Then the player id will sort out to which player the input corresponds too. 
	(Note, this is still in development and need to be updated depending on how godot handles multiplayer input. The idea
	is that, whatever interface that is, this class should make the transition as smooth as possible.)

	For setting up Input for Enemies (ie, AI), see the InputAIAbstract class or its children.  (TO DO)

	Remember to generate input keys from the editor!
	*/

	// -----------------------------IN EDITOR METHODS-------------------------

	// THIS NEEDS TO BE IMPLEMENTED IN THE INHERITATED CLASS. Having base class that are [Tools] generate lose of default values
	[Export] public bool SetInputVariablesValues{
		get{return false;}
		set{FixFormatInputsButton();}
	}


	// -----------------------------------Overriden method to update input---------------------------------------


	protected override void UpdateInput(double delta)
	{
		for( int i=0; i< mAxis.Length ; i++){
			mAxisValues[mAxis[i]] = Input.GetActionStrength(mAxis[i]+PlayerIdAndP());
			if (Input.GetActionStrength(mAxis[i]+PlayerIdAndP())>0){
				mTimeSinceLastPressAxis[mAxis[i]]=0;
				if (Input.IsActionJustPressed(mAxis[i]+PlayerIdAndP())){
					mTimeHeldAxis[mAxis[i]] = 0;
				}
				else{
					mTimeHeldAxis[mAxis[i]] += delta;
				}
			}
			else{
				mTimeHeldAxis[mAxis[i]] = 0;
				mTimeSinceLastPressAxis[mAxis[i]] +=delta;
			}
		}
		for( int i=0; i< mButtons.Length ; i++){
			mButtonsValues[mButtons[i]] = Input.IsActionPressed(mButtons[i]+PlayerIdAndP());
			if (Input.IsActionPressed(mButtons[i]+PlayerIdAndP())){
				mTimeSinceLastPressButton[mButtons[i]]=0;
				if (Input.IsActionJustPressed(mButtons[i]+PlayerIdAndP())){
					mTimeHeldButton[mButtons[i]]=0;
				}
				else{
					mTimeHeldButton[mButtons[i]]+=delta;
				}
			}
			else{
				mTimeHeldButton[mButtons[i]]=0;
				mTimeSinceLastPressButton[mButtons[i]] +=delta;
			}
		}
	}

	// ---------------------------------- override method to log ---------------------------

	public override void _Ready(){
		base._Ready();
		if (mShouldLog){
			mLogObject = LogManager.Instance.RequestLogReference("Input", mPlayerID);
		}
	}

	// -----------------------------------Helper method---------------------------------------

	public override string PlayerIdAndP(){
		return "P"+mPlayerID;
	}
}
