using Godot;
using System;

public partial class InputReaderVoid : InputReaderAbstract
{
	// ----------------------------------- Information ------------------------------------------------
	/*This is a script to allow a null input to be assign to actors that requir an input. 
	This script is then useful to avoid getting errors of nullInput reference.
	
	// ------------------------------------ Use -------------------------------------------------------
	/* Asign to whatever actor requires to be left "without input"
	*/


	// -----------------------------------Overriden method to update input---------------------------------------
	protected override void UpdateInput(double delta)
	{
		//This is setting by default to "nothing is pressed"
		for( int i=0; i< mAxis.Length ; i++){
			mAxisValues[mAxis[i]] =0;
			mTimeSinceLastPressAxis[mAxis[i]]+=delta;
			mTimeHeldAxis[mAxis[i]]=0;
		}
		for( int i=0; i< mButtons.Length ; i++){
			mButtonsValues[mButtons[i]]=false;
			mTimeSinceLastPressButton[mButtons[i]]+=delta;
			mTimeHeldButton[mButtons[i]]=0;
		}
	}
}

