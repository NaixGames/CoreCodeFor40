using Godot;
using System;

namespace CoreCode.Scripts{
	public partial class InputManager : SingleNode<InputManager>
	{

		// Information
		/*This is a script to allow distribution of player input to different parts of the game.
		This include menus, characters and so on.
		
		// Use
		/* Put inputs reader as child nodes of this singleton. Then assign them to the mPlayerInputs array.
		Then actors of the game can just request their inputs or being assign by other means using this
		singleton methods*/
		

		// Variables

		[Export] private Godot.Collections.Array<NodePath> mPlayerInputs = new Godot.Collections.Array<NodePath>();

		public InputReaderVoid NullInputReader{
			get{return this.GetNode<InputReaderVoid>(mPlayerInputs[0]);}
		}

		// Methods

		public void AssignInputByPlayerChannel(IControlableByInput controlable, int channel){
			if (channel > mPlayerInputs.Count){
				mLogObject.Print("Unexisting player channel requested! " + channel + " by " + controlable);
				return;
			}
			mLogObject.Print("Assigning player input " + channel + " to " + controlable);

			controlable.RecieveInputReader(this.GetNode<InputReaderPlayer>(mPlayerInputs[channel]));
		}


		public InputReaderAbstract GiveInputByPlayerChannel(IControlableByInput controlable, int channel){
			if (channel > mPlayerInputs.Count){
				mLogObject.Print("Unexisting player channel requested! " + channel + " by " + controlable);
				return this.GetNode<InputReaderAbstract>(mPlayerInputs[0]);
			}
			
			mLogObject.Print("Player input " + channel + " was requested");

			return this.GetNode<InputReaderPlayer>(mPlayerInputs[channel]);
		}
		

		public void FreeInputFromControlable(IControlableByInput controlable){
			controlable.RecieveInputReader(this.GetNode<InputReaderAbstract>(mPlayerInputs[0]));
		}

	}
}
