using Godot;
using System;
using System.Collections.Generic;

namespace CoreCode.Scripts{

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
		// ------------------------------------ Variables ------------------------------------------------

		
		[Export] protected int mPlayerID = -1; //Note the convention that -1 is AI/Engine input and 1 and beyond are players.


		[Export] protected string[] mAxis = new string[0]; //Remember that godot deals with Axis differently than Unity! Left/Right must be different values!
		public string[] AxisNames{
			get{return mAxis;}
		}

		[Export] protected string[] mButtons = new string[0];
		public string[] ButtonNames{
			get{return mButtons;}
		}


		protected Dictionary<string, InputActionInfo> mAxisValues = new Dictionary<string, InputActionInfo>();
		protected Dictionary<string, InputActionInfo> mButtonsValues = new Dictionary<string, InputActionInfo>();
		protected bool mIsFirstFrame = true;

		public bool IsActive = true;


		// ------------------------------------ Variable for logging-----------------------------------------

		[Export] protected bool mShouldLog;

		protected ILogObject mLogObject;


		// ------------------------------------ Functions ------------------------------------------------	

		// ------------------------------------- Godot overrides ---------------------------------------

		public override void _Ready(){
			GenerateInputValuesArrays();
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
				mLogObject.Print("Axi: " + AxisNames[i] + " value " + GiveAxisStrength(AxisNames[i])  + " last pressed "  + TimeSinceLastAxisInput(AxisNames[i]) + " held for " + TimeAxisHeldInput(AxisNames[i]));
			}
			for (int i = 0; i< ButtonNames.Length; i++){
				mLogObject.Print("Button: " + ButtonNames[i] + " value " + IsButtonPressed(ButtonNames[i])  + " last pressed "  + TimeSinceLastButtonInput(ButtonNames[i])+ " held for " + TimeButtonHeldInput(ButtonNames[i]));
			}
		}
		
		// ------------------------------------- Abstract methods ---------------------------------------
		protected abstract void UpdateInput(double delta);

		// ------------------------------------- Auxiliary functions for setups ---------------------------------------
		
		//This function should be executed in the inspector. Using the Set method for a bool for that.


		protected void GenerateInputValuesArrays(){
			for( int i=0; i< mAxis.Length ; i++){
				InputActionInfo mActionInfo = new InputActionInfo(mAxis[i]);
				mAxisValues.Add(mAxis[i], mActionInfo);
			}
			for( int i=0; i< mButtons.Length ; i++){
				InputActionInfo mActionInfo = new InputActionInfo(mButtons[i]);
				mButtonsValues.Add(mButtons[i], mActionInfo);
			}
		}

		protected void ProcessButtonPressValue(string ButtonName, bool pressValue, float deltaTime){
			float previousValue = mButtonsValues[ButtonName].Value;
			InputActionInfo mActioInfoRef = mButtonsValues[ButtonName];
			mActioInfoRef.Value = pressValue ? 1f : 0f;
				if (pressValue){
					mActioInfoRef.TimeSinceLastPressed=0;
					if (previousValue>0.5f){
						mActioInfoRef.TimeHeld+=deltaTime;
					}
					else{
						mActioInfoRef.TimeHeld=0;
					}
				}
				else{
					mActioInfoRef.TimeHeld=0;
					mActioInfoRef.TimeSinceLastPressed +=deltaTime;
				}
		}

		protected void ProcessAxisValue(string AxisName, float AxisStrength, float deltaTime){
			float previousValue = mAxisValues[AxisName].Value;
			InputActionInfo mActioInfoRef = mAxisValues[AxisName];
			mActioInfoRef.Value = AxisStrength;
				if (AxisStrength>0){
					mActioInfoRef.TimeSinceLastPressed=0;
					if (previousValue>0){
						mActioInfoRef.TimeHeld += deltaTime;
					}
					else{
						mActioInfoRef.TimeHeld = 0;
					}
				}
				else{
					mActioInfoRef.TimeHeld = 0;
					mActioInfoRef.TimeSinceLastPressed +=deltaTime;
				}
		}

		// ------------------------------------- Primary functions ---------------------------------------
		public float GiveAxisStrength(string AxisName){
			if (!mAxisValues.ContainsKey(AxisName)){
				GD.PushError("Input key " + AxisName + " does not exist in dictionary. Check your input names/generate the input keys!");
				return 0;
			}
			return mAxisValues[AxisName].Value;
		}

		//--------------------------------------

		public bool IsButtonPressed(string ButtonName){
			if (!mButtonsValues.ContainsKey(ButtonName)){
				GD.PushError("Input key " + ButtonName + " does not exist in dictionary. Check your input names/generate the input keys!");
				return false;
			}
			return mButtonsValues[ButtonName].Value > 0.5f;
		}

		//--------------------------------------
	
		public double TimeSinceLastAxisInput(string AxisName){
			if (!mAxisValues.ContainsKey(AxisName)){
				GD.PushError("Input key " + AxisName + " does not exist in dictionary. Check your input names/generate the input keys!");
				return 0;
			}
			return  mAxisValues[AxisName].TimeSinceLastPressed;
		}

		//--------------------------------------

		public double TimeSinceLastButtonInput(string ButtonName){
			if (!mButtonsValues.ContainsKey(ButtonName)){
				GD.PushError("Input key " + ButtonName + " does not exist in dictionary. Check your input names/generate the input keys!");
				return 0;
			}
			return  mButtonsValues[ButtonName].TimeSinceLastPressed;
		}

		//--------------------------------------

		public double TimeAxisHeldInput(string AxisName){
			if (!mAxisValues.ContainsKey(AxisName)){
				GD.PushError("Input key " + AxisName + " does not exist in dictionary. Check your input names/generate the input keys!");
				return 0;
			}
			return  mAxisValues[AxisName].TimeHeld;
		}

		//--------------------------------------

		public double TimeButtonHeldInput(string ButtonName){
			if (!mButtonsValues.ContainsKey(ButtonName)){
				GD.PushError("Input key " + ButtonName + " does not exist in dictionary. Check your input names/generate the input keys!");
				return 0;
			}
			return  mButtonsValues[ButtonName].TimeHeld;
		}

		//--------------------------------------
		public bool IsAxisJustPressedInput(string AxisName){
			if (!mAxisValues.ContainsKey(AxisName)){
				GD.PushError("Input key " + AxisName + " does not exist in dictionary. Check your input names/generate the input keys!");
				return false;
			}
			return  (mAxisValues[AxisName].TimeHeld==0 && mAxisValues[AxisName].TimeSinceLastPressed==0 && (!mIsFirstFrame));
		}

		//--------------------------------------
		public bool IsButtonJustPressedInput(string ButtonName){
			if (!mButtonsValues.ContainsKey(ButtonName)){
				GD.PushError("Input key " + ButtonName + " does not exist in dictionary. Check your input names/generate the input keys!");
				return false;
			}
			return  (mButtonsValues[ButtonName].TimeHeld==0 && mButtonsValues[ButtonName].TimeSinceLastPressed==0 && (!mIsFirstFrame));
		}

		//--------------------------------------

		public virtual string PlayerIdAndP(){
			return "";
		}

	}
}