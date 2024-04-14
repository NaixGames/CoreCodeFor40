using Godot;
using System;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	public partial class LogManager : Node
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to have a administrate different log instances in the game. This should help manage different logging options.*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/* This is a service locator that give different log instances to game objects for them to put log, and then this class prints their information.

		Note that I put specific name for the methods and variables to have control on which variables are actually logging information.
		New tools imply making new methods and similar variables. While this may seem like a code overhead, this should not be modified frequently.

		Note that the order in the hierarchy is important to have a correct use of _Ready()!

		Update: Added new dictionary based approach. Makes setting dictionaries a bit easier.
		*/


		// ------------------------------------- Singleton instantiation -------------------------------
		private static LogManager instance;
		public static LogManager Instance{
			get {return TryToReturnInstance();}
		}

		public static LogManager TryToReturnInstance(){
			if (instance == null){
				GD.PushWarning("instance of LogManager called before the instance was ready!");
				instance = new LogManager();
			}
			return instance;
		}


		public LogManager(){
			instance=this; 
		}


		// ------------------------------------- Variable to turn logging by channels.
		
		[Export] private Godot.Collections.Dictionary<string, bool> mChannels = new Godot.Collections.Dictionary<string, bool>();

		//TO ADD NEW CATEGORIES OF LOGS ADD THEM IN THE MChannels VARIABLE.
		//THE START METHOD CREATE THE LOG OBJECTS AUTOMATICALLY FROM THIS INFORMATION.
	
		private Dictionary<string, ILogObject> mLogsDictionary = new Dictionary<string, ILogObject>();

		// ------------------------------------- Logs and their methods -----------------------------------------------

		//-------------------------------------- Method for giving logs

		public ILogObject RequestLog(string channel, bool shouldLog){
			if (!shouldLog){
				return new VoidLogObject();
			}
			if (mLogsDictionary.ContainsKey(channel)){
				return mLogsDictionary[channel];
			}
			//Using an editor log here just to superseed our own system in this particular case
			GD.PushWarning("REQUESTING NON-EXISTING CHANNEL");
			return new VoidLogObject();
		}

		// ------------------------------------- Methods --------------------------------------------------------------

		private ILogObject GiveLogObject(string channel){
			//For now this just return a EditorLogObject. In the future for debug builds this should return another
			//type of log object that actually writes in some files, one for each channel.
			return new EditorLogObject(channel);
		}

		// -------------------------------------

		public override void _Ready(){
			foreach (string channel in mChannels.Keys){
				ILogObject newLog;
				if (mChannels[channel]){
					newLog = GiveLogObject(channel);
				}
				else{
					newLog = new VoidLogObject();
				}
				mLogsDictionary.Add(channel, newLog);
			}
		}

		// -------------------------------------

	}
}
