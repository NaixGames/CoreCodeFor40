using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.FSM{
	//Tool for casting editor pluggins
	[Tool]
	public partial class StateMachineAIInput : InputReaderAbstract, IStateMachine
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to create instances of AI state machines to use in Godot.*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/* The states use for this State machine should always recieve a mMemoryblackboard of input variables,
		that are later use to update the values inside of this AI.  */

		// ------------------------------------ Variables ------------------------------------------------


		[Export] private StateManagerAbstract mStateManager;
		[Export] private Godot.Collections.Dictionary mMemoryBlackboard = new Godot.Collections.Dictionary();
		private StateAbstract mActualState;

		//Have a list of inputs key and buttons that can be called.

		// ------------------------------------ Variable for logging-----------------------------------------
		[Export] protected int mLogChannel;

		//Recall the InputRead itself already has the Log object.	

		// ------------------------------------ Functions ------------------------------------------------	

		// ------------------------------------- Godot overrides ---------------------------------------

		protected override void UpdateInput(double delta){
			mActualState = mActualState.ExecuteProcess(delta, mLogObject); 
			//Check the lsit of "processed" things and update value of input reader accordingly.
			Godot.Collections.Array<string> ButtonsThisFrame = (Godot.Collections.Array<string>)mMemoryBlackboard["ButtonsContainer"];
			UpdateButtonValues(ButtonsThisFrame, delta);

			Godot.Collections.Dictionary<string,float> AxisValuesThisFrame = (Godot.Collections.Dictionary<string,float>)mMemoryBlackboard["AxisContainer"];
			UpdateAxisValues(AxisValuesThisFrame, delta);

			ButtonsThisFrame.Clear();
			AxisValuesThisFrame.Clear();
		} 


		public override void _Ready()
		{
			base._Ready();
			if (Engine.IsEditorHint()){ //Using prints here because this will only be called from the Editor!
				return;
			}
			if (mShouldLog){
				mLogObject = LogManager.Instance.RequestLogReference("FSM", mLogChannel);
				mLogObject.AddToLogString("Intiliazing FSM of: "  + this.Name + " with state manager " + mStateManager.GetType()); 
			}
			if (mShouldLog){
				mLogObject.AddToLogString("Starting FSM of " + this.Name + " with state " + mActualState.GetType());
			}
			//Add a BBV container list to the Blackboard. This is what will have the inputs pressed on that frame.
			Godot.Collections.Array<string> ButtonsPressedThisFrame = new Godot.Collections.Array<string>();
			if (mMemoryBlackboard.ContainsKey("ButtonsContainer")){
				mMemoryBlackboard["ButtonsContainer"] = ButtonsPressedThisFrame;
			}
			else{
				mMemoryBlackboard.Add("ButtonsContainer", ButtonsPressedThisFrame);
			}
			Godot.Collections.Dictionary<string,float> AxisNonZeroThisFrame = new Godot.Collections.Dictionary<string,float>();
			if (mMemoryBlackboard.ContainsKey("AxisContainer")){
				mMemoryBlackboard["AxisContainer"] = AxisNonZeroThisFrame;
			}
			else{
				mMemoryBlackboard.Add("AxisContainer", AxisNonZeroThisFrame);
			}

			mStateManager.InitializeStates(this, mMemoryBlackboard);
			mActualState = mStateManager.GiveInitialState(mLogObject); 
		}


		// ------------------------------------- Update input logic ------------------------------------

		private void UpdateButtonValues(Godot.Collections.Array<string> ButtonPressedThisFrame, double deltaTime){
			for( int i=0; i< mButtons.Length ; i++){
				ProcessButtonPressValue(mButtons[i], ButtonPressedThisFrame.Contains(mButtons[i]), (float)deltaTime);
			}
			return;
		}

		private void UpdateAxisValues(Godot.Collections.Dictionary<string, float> AxisValuesThisFrame, double deltaTime){
			for( int i=0; i< mAxis.Length ; i++){
				float axisStrength = AxisValuesThisFrame.ContainsKey(mAxis[i]) ? AxisValuesThisFrame[mAxis[i]] : 0;
				ProcessAxisValue(mAxis[i], axisStrength , (float)deltaTime);
			}
			return;
		}

		// ------------------------------------- Interface methods ---------------------------------------

		public StateManagerAbstract GiveStateManager() //This will be useful if I ever want to do a visualisation tool. I can just request the StateManager and get information from that.
		{
			return mStateManager;
		}
		public StateAbstract GiveActualState(){
			return mActualState;
		}

		public void ChangeStateAfterEventChange(StateAbstract newState){
			mActualState = newState;
		}

	}
}