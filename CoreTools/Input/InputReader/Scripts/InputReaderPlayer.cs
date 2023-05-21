using Godot;
using System;

namespace CoreCode.Scripts{
	//Tool for casting editor pluggins
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




		// -----------------------------------Overriden method to update input---------------------------------------


		protected override void UpdateInput(double delta)
		{
			for( int i=0; i< mAxis.Length ; i++){
				ProcessAxisValue(mAxis[i], Input.GetActionStrength(mAxis[i]+PlayerIdAndP()), (float)delta);
			}
			for( int i=0; i< mButtons.Length ; i++){
				ProcessButtonPressValue(mButtons[i], Input.IsActionPressed(mButtons[i]+PlayerIdAndP()), (float)delta);
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
}