using Godot;
using System;

namespace CoreCode.Scripts{
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
				InputActionInfo mActionInfoRef = mAxisValues[mAxis[i]];
				mActionInfoRef.Value =0;
				mActionInfoRef.TimeSinceLastPressed+=(float)delta;
				mActionInfoRef.TimeHeld=0;
			}
			for( int i=0; i< mButtons.Length ; i++){
				InputActionInfo mActionInfoRef = mButtonsValues[mButtons[i]];
				mActionInfoRef.Value=0;
				mActionInfoRef.TimeSinceLastPressed+=(float)delta;
				mActionInfoRef.TimeHeld=0;
			}
		}
	}
}

