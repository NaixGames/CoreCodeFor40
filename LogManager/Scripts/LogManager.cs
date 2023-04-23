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


		// ------------------------------------- Variable to turn off all logging.
		[Export] private bool mAllowLogging = true;

		// Variable for printing the logs.
		private LogObject[] mVariableForPrintCategoryLogArray;


		// ------------------------------------- Logs and their methods -----------------------------------------------

		//-------------------------------------- Method for giving logs

		//TO ADD NEW CATEGORIES OF LOGS ADD THEM IN THE CATEGORIES BELOW AND IN THE INITIALIZATION OF THE LOGS DICTIONARY, NUMBERS DICTRIONARY AND OPTIONS DICTIONARY!

		private Dictionary<string, LogObject[]> mLogsDictionary = new Dictionary<string, LogObject[]>();
		private Dictionary<string, int> mNumbersDictionary = new Dictionary<string, int>();
		Dictionary<string, bool> mOptionsDictionary = new Dictionary<string, bool>();
		
		public LogObject[] RequestLogCategory(string tag){
			if (!mLogsDictionary.ContainsKey(tag)){
				GD.PushError("Attempting to get unexisting Log category");
				return new LogObject[0];
			}
			return mLogsDictionary[tag];
		}

		public LogObject RequestLogReference(string tag, int channel){
			if (!mAllowLogging){
				return new LogObject();
			}
			LogObject[] CategoryLogArray = RequestLogCategory(tag);
			if (channel >= CategoryLogArray.Length){
				GD.PushError("Attempting to get unexisting channel of log category " + tag);
				return new LogObject();
			}
			return CategoryLogArray[channel];
		}

		// ------------------------------------- Logs Catogories here ------------------------------------------------

		// ------------------------------------- Dummy Log. Mostly for testing and to set up the pattern -------------

		private LogObject[] mDummyLogs;
		[Export] private bool mDisplayDummyChannels;
		[Export] private int mNumberOfDummyChannels;

		// ------------------------------------- Log for player input. ------------------------------------------------
	
		private LogObject[] mInputLogs;
		[Export] private bool mDisplayInputChannels;	
		[Export] private int mNumberOfInputChannels;


		
		// ------------------------------------- Log for object pooler. ----------------------------------------

		private LogObject[] mGameObjectPoolerLogs;

		[Export] private bool mDisplayPoolerChannels;	
		[Export] private int mNumberOfPoolerChannels;

		// ------------------------------------- Log for global event dispatcher. ----------------------------------------

		private LogObject[] mEventDispatcherLogs;
		[Export] private bool mDisplayEventDispatcherChannels;	
		[Export] private int mNumberOfEventDispatcherChannels;


		// ------------------------------------- Log for global event dispatcher. ----------------------------------------

		private LogObject[] mFSMLogs;
		[Export] private bool mDisplayFSMChannels;	
		[Export] private int mNumberOfFSMChannels;

		// ------------------------------------- Log for Achivement Manager ----------------------------------------

		private LogObject[] mAchivementLogs;
		[Export] private bool mDisplayAchivementChannels;	
		[Export] private int mNumberOfAchivementChannels;

		// ------------------------------------- Log for Audio Manager ----------------------------------------

		private LogObject[] mAudioLogs;
		[Export] private bool mDisplayAudioChannels;	
		[Export] private int mNumberOfAudioChannels;

		// ------------------------------------- Log for Scene Transition Manager ----------------------------------------

		private LogObject[] mSceneLogs;
		[Export] private bool mDisplaySceneChannels;	
		[Export] private int mNumberOfSceneChannels;

		// ------------------------------------- Methods --------------------------------------------------------------

		public override void _Ready(){
			if (!mAllowLogging){
				return;
			}
			//ADD DIFFERENT DICTIONARIES SETTINGS HERE
			mLogsDictionary.Add("Dummy", mDummyLogs);
			mNumbersDictionary.Add("Dummy", mNumberOfDummyChannels);
			mOptionsDictionary.Add("Dummy", mDisplayDummyChannels);

			mLogsDictionary.Add("Input", mInputLogs);
			mNumbersDictionary.Add("Input", mNumberOfInputChannels);
			mOptionsDictionary.Add("Input", mDisplayInputChannels);

			mLogsDictionary.Add("GameObjectPooler", mGameObjectPoolerLogs);
			mNumbersDictionary.Add("GameObjectPooler",  mNumberOfPoolerChannels);
			mOptionsDictionary.Add("GameObjectPooler", mDisplayPoolerChannels);
			
			mLogsDictionary.Add("GlobalEventDispatcher", mEventDispatcherLogs);
			mNumbersDictionary.Add("GlobalEventDispatcher", mNumberOfEventDispatcherChannels);
			mOptionsDictionary.Add("GlobalEventDispatcher", mDisplayEventDispatcherChannels);

			mLogsDictionary.Add("FSM", mFSMLogs);
			mNumbersDictionary.Add("FSM", mNumberOfFSMChannels);
			mOptionsDictionary.Add("FSM", mDisplayFSMChannels);

			mLogsDictionary.Add("Achievements", mAchivementLogs);
			mNumbersDictionary.Add("Achievements", mNumberOfAchivementChannels);
			mOptionsDictionary.Add("Achievements", mDisplayAchivementChannels);

			mLogsDictionary.Add("Audio", mAudioLogs);
			mNumbersDictionary.Add("Audio", mNumberOfAudioChannels);
			mOptionsDictionary.Add("Audio", mDisplayAudioChannels);

			mLogsDictionary.Add("SceneTransitions", mSceneLogs);
			mNumbersDictionary.Add("SceneTransitions", mNumberOfSceneChannels);
			mOptionsDictionary.Add("SceneTransitions", mDisplaySceneChannels);


			// -----------------------------------------------------------------------------------------------


			//Initialization of logs with the correct size and setting. This is automatized once the dictionaries above are set correctly.
			foreach (string typeLog in mLogsDictionary.Keys){
				mLogsDictionary[typeLog] = new LogObject[mNumbersDictionary[typeLog]];
				for (int i=0; i<mLogsDictionary[typeLog].Length;i ++){
					mLogsDictionary[typeLog][i] = new LogObject();
					mLogsDictionary[typeLog][i].AllowLogging = mOptionsDictionary[typeLog];
				}
			}		
			
			foreach (string typeLog in mLogsDictionary.Keys){
				GD.Print(typeLog);
				GD.Print("Length: " + mLogsDictionary[typeLog].Length);
			}		
		}

		public override void _Process(double delta)
		{
			if (!mAllowLogging){
				return;
			}
			//Print each log that the manager has access too. 
			foreach (string typeLog in mLogsDictionary.Keys){
				mVariableForPrintCategoryLogArray = RequestLogCategory(typeLog);
				for (int i=0; i<mVariableForPrintCategoryLogArray.Length; i++){
					mVariableForPrintCategoryLogArray[i].PrintLastLogString();
				}
			}
		}
		
	}
}
