using Godot;
using System;


public abstract partial class InputReaderAbstract : Node
{
	// ----------------------------------- Information ------------------------------------------------
	/*This is a script to manage player input manually, including buffers and button combinations. 
	This should serve to abstract a bit the interface from Godot, while also gaining more control.*/
	
	// ------------------------------------ Use -------------------------------------------------------
	/* When want to give input to an object, an instance of this class should be reference. Button presses/combinations
	will be stored in variables that are either float or bools (depending on if it is an axis or button).

	For setting up Input for Player in a concentrete class, see InputManager. For setting Input for Enemies (ie, AI), see the InputAIAbstract class or its childrens. (TO DO)
	*/


	// ------------------------------------- Pending -----------------------------------------------
	/*
	Related; connection to logs classes, so I can dump there input values. Logs are still not implemented, that will be next for my core.
	*/

	// ------------------------------------ Variables ------------------------------------------------

	
	[Export] protected bool mAreInputsUpdated = false;
	[Export] protected int mPlayerID = -1; //Note the convention that -1 is AI/Engine input and 1 and beyond are players.


	[Export] protected string[] mAxis = new string[0]; //Remember that godot deals with Axis differently than Unity! Left/Right must be different values!
	public string[] AxisNames{
		get{return mAxis;}
	}
	[Export] protected string[] mButtons = new string[0];
	public string[] ButtonNames{
		get{return mButtons;}
	}


	[Export] protected Godot.Collections.Dictionary<string, float> mAxisValues;
	[Export] protected Godot.Collections.Dictionary<string, bool> mButtonsValues;


	[Export] protected Godot.Collections.Dictionary<string, double> mTimeSinceLastPressAxis;
	[Export] protected Godot.Collections.Dictionary<string, double> mTimeSinceLastPressButton;

	[Export] protected Godot.Collections.Dictionary<string, double> mTimeHeldAxis;
	[Export] protected Godot.Collections.Dictionary<string, double> mTimeHeldButton;

	protected bool mIsFirstFrame=true;


	// ------------------------------------ Variable for logging-----------------------------------------

	[Export] protected bool mShouldLog;

	protected LogObject mLogObject;


	// ------------------------------------ Functions ------------------------------------------------	

	// ------------------------------------- Godot overrides ---------------------------------------

	public override void _Ready(){
		if (mAreInputsUpdated == false){
			GD.PushWarning("There are inputs that are not correctly updated! Check node " + this.Name);
		}
	}
	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint()){
			return;
		}

		UpdateInput(delta);
		if (mIsFirstFrame){
			mIsFirstFrame = false;
		}
		//Logging.
		if (!mShouldLog){
			return;
		}
		for (int i = 0; i< AxisNames.Length; i++){
			mLogObject.AddToLogString("Axi: " + AxisNames[i] + " value " + GiveAxisStrength(AxisNames[i])  + " last pressed "  + TimeSinceLastAxisInput(AxisNames[i]) + " held for " + TimeAxisHeldInput(AxisNames[i]));
		}
		for (int i = 0; i< ButtonNames.Length; i++){
			mLogObject.AddToLogString("Button: " + ButtonNames[i] + " value " + IsButtonPressed(ButtonNames[i])  + " last pressed "  + TimeSinceLastButtonInput(ButtonNames[i])+ " held for " + TimeButtonHeldInput(ButtonNames[i]));
		}
	}
	
	// ------------------------------------- Abstract methods ---------------------------------------
	protected abstract void UpdateInput(double delta);

	// ------------------------------------- Auxiliary functions for setups ---------------------------------------
	
	//This function should be executed in the inspector. Using the Set method for a bool for that.
	public void FixFormatInputsButton(){
		if (Engine.IsEditorHint()){ //Using prints here because this will only be called from the Editor!
			if (mAreInputsUpdated){
				GD.Print("Trying to Fix updated inputs for a player. Are you sure you want to do this? If so untick the mAreInputsUpdated variable.");
				return;
			}
			GenerateInputValuesArrays();
			mAreInputsUpdated=true;
			GD.Print("Inputs set up correctly!"); 
		}
		else{
			GD.PushWarning("Trying to call FixFormatInputs in gameplay! This should not happen!");
		}
	}


	protected void GenerateInputValuesArrays(){
		mAxisValues = new Godot.Collections.Dictionary<string, float>();
		mButtonsValues =  new Godot.Collections.Dictionary<string, bool>();
		mTimeSinceLastPressAxis = new Godot.Collections.Dictionary<string, double>();
		mTimeSinceLastPressButton= new Godot.Collections.Dictionary<string, double>();
		mTimeHeldAxis = new Godot.Collections.Dictionary<string, double>();
		mTimeHeldButton= new Godot.Collections.Dictionary<string, double>();
		for( int i=0; i< mAxis.Length ; i++){
			mAxisValues.Add(mAxis[i], 0);
			mTimeSinceLastPressAxis.Add(mAxis[i], 0);
			mTimeHeldAxis.Add(mAxis[i], 0);
		}
		for( int i=0; i< mButtons.Length ; i++){
			mButtonsValues.Add(mButtons[i], false);
			mTimeSinceLastPressButton.Add(mButtons[i], 0);
			mTimeHeldButton.Add(mButtons[i], 0);
		}
	}

	protected void ProcessButtonPressValue(string ButtonName, bool pressValue, double deltaTime){
		bool previousValue = mButtonsValues[ButtonName];
		mButtonsValues[ButtonName] = pressValue;
			if (pressValue){
				mTimeSinceLastPressButton[ButtonName]=0;
				if (previousValue){
					mTimeHeldButton[ButtonName]+=deltaTime;
				}
				else{
					mTimeHeldButton[ButtonName]=0;
				}
			}
			else{
				mTimeHeldButton[ButtonName]=0;
				mTimeSinceLastPressButton[ButtonName] +=deltaTime;
			}
	}

	protected void ProcessAxisValue(string AxisName, float AxisStrength, double deltaTime){
		float previousValue = mAxisValues[AxisName];
		mAxisValues[AxisName] = AxisStrength;
			if (AxisStrength>0){
				mTimeSinceLastPressAxis[AxisName]=0;
				if (previousValue>0){
					mTimeHeldAxis[AxisName] += deltaTime;
				}
				else{
					mTimeHeldAxis[AxisName] = 0;
				}
			}
			else{
				mTimeHeldAxis[AxisName] = 0;
				mTimeSinceLastPressAxis[AxisName] +=deltaTime;
			}
	}

	// ------------------------------------- Primary functions ---------------------------------------
	public float GiveAxisStrength(string AxisName){
		if (!mAxisValues.ContainsKey(AxisName)){
			GD.PushError("Input key " + AxisName + " does not exist in dictionary. Check your input names/generate the input keys!");
			return 0;
		}
		return mAxisValues[AxisName];
	}

	//--------------------------------------

	public bool IsButtonPressed(string ButtonName){
		if (!mButtonsValues.ContainsKey(ButtonName)){
			GD.PushError("Input key " + ButtonName + " does not exist in dictionary. Check your input names/generate the input keys!");
			return false;
		}
		return mButtonsValues[ButtonName];
	}

	//--------------------------------------
 
	public double TimeSinceLastAxisInput(string AxisName){
		if (!mAxisValues.ContainsKey(AxisName)){
			GD.PushError("Input key " + AxisName + " does not exist in dictionary. Check your input names/generate the input keys!");
			return 0;
		}
		return  mTimeSinceLastPressAxis[AxisName];
	}

	//--------------------------------------

	public double TimeSinceLastButtonInput(string ButtonName){
		if (!mButtonsValues.ContainsKey(ButtonName)){
			GD.PushError("Input key " + ButtonName + " does not exist in dictionary. Check your input names/generate the input keys!");
			return 0;
		}
		return  mTimeSinceLastPressButton[ButtonName];
	}

	//--------------------------------------

	public double TimeAxisHeldInput(string AxisName){
		if (!mAxisValues.ContainsKey(AxisName)){
			GD.PushError("Input key " + AxisName + " does not exist in dictionary. Check your input names/generate the input keys!");
			return 0;
		}
		return  mTimeHeldAxis[AxisName];
	}

	//--------------------------------------

	public double TimeButtonHeldInput(string ButtonName){
		if (!mButtonsValues.ContainsKey(ButtonName)){
			GD.PushError("Input key " + ButtonName + " does not exist in dictionary. Check your input names/generate the input keys!");
			return 0;
		}
		return  mTimeHeldButton[ButtonName];
	}

	//--------------------------------------
	public bool IsAxisJustPressedInput(string AxisName){
		if (!mAxisValues.ContainsKey(AxisName)){
			GD.PushError("Input key " + AxisName + " does not exist in dictionary. Check your input names/generate the input keys!");
			return false;
		}
		return  (mTimeHeldAxis[AxisName]==0 && mTimeSinceLastPressAxis[AxisName]==0);
	}

	//--------------------------------------
	public bool IsButtonJustPressedInput(string ButtonName){
		if (!mButtonsValues.ContainsKey(ButtonName)){
			GD.PushError("Input key " + ButtonName + " does not exist in dictionary. Check your input names/generate the input keys!");
			return false;
		}
		return  (mTimeHeldButton[ButtonName]==0 && mTimeSinceLastPressButton[ButtonName]==0 && (!mIsFirstFrame));
	}

	//--------------------------------------

	public virtual string PlayerIdAndP(){
		return "";
	}

}
