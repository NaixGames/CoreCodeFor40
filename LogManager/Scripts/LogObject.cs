using Godot;
using System;

public partial class LogObject : Node
{
	// ----------------------------------- Information ------------------------------------------------
	/*This is a script to have a separate Log for different part of the game.*/
	
	// ------------------------------------ Use -------------------------------------------------------
	/* Different instances of this object are dedicated to different part of the game (input, FSM, object pooler, etc.)
	There is a Log Manager script, that works as a Service Locator (plus manager) to this instances.
	*/


	// ------------------------------------- Variables -----------------------------------------------

	private string mLogHistory = "";
	private string mLogString= "";
	private bool mSaveLoggingEntries= false;

	public bool AllowLogging=false;

	public bool SaveLogginEntries{
		get {return mSaveLoggingEntries;}
	}

	// ------------------------------------- Methods -----------------------------------------------

	public void PrintLastLogString(){
		if (!AllowLogging){
			return;
		}
		if (mLogString == ""){
			return;
		}
		GD.Print(mLogString);
		if (mSaveLoggingEntries){
			mLogHistory += mLogString;
			mLogHistory += "\n";
		}
		mLogString = "";
	}

	// ------------------------------------- 

	public void PrintAllStringLog(){
		if (!AllowLogging){
			return;
		}
		GD.Print(mLogHistory);
	}

	// ------------------------------------- 

	public void LogString(string information){
		if (!AllowLogging){
			return;
		}
		mLogString = information;
	}

	public void AddToLogString(string information){
		if (!AllowLogging){
			return;
		}
		mLogString += "\n";
		mLogString += information;
	}
	
}
