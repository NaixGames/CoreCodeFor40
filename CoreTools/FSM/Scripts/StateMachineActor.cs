using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.FSM{
	[Tool]	
	public partial class StateMachineActor : Node, IStateMachine, IControlableByInput, IResetable
	{
		// Information
		/*This is a script to create instances of state machines to use in Godot.*/
		
		// Use
		/* The Machine have access to a StateAbstract element, and each Update and FixedUpdate will delegate 
		to the State any execution of instructions. It will also pass a BlackboardMemory, which is a dictionary
		of elements that contains the variables of the actor. Note: this model with Memory is Turing-Complete,
		so it should be able to do everything with enough effort put into it.*/

		/* To instantiate and reference State it uses a StateManagerAbstract, which is basically a container of states. */

		/*Note the input is in this class to avoid depending on node path and the scene tree. That way every request for the player
		input can be done for the InputManager, and if an AI is a child node of the actor this can be hooked without extra setup */

		// Variables

		private StateManagerAbstract mStateManager;
		[Export] public StateManagerPointer StateManagerPointerResource;
		[Export] private Godot.Collections.Dictionary mMemoryBlackboard = new Godot.Collections.Dictionary();
		private StateAbstract mActualState;

		

		// Variable for logging

		[Export] protected bool mShouldLog;
		protected ILogObject mLogObject;

		// Variable for input requesting

		[Export] protected int mRequestInputChannel=0;

		[Export] protected InputReaderAbstract mInputReader;


		// Methods for IControlableByInput interface
		
		public InputReaderAbstract ReturnInputReader(){
			if (mInputReader == null){
				mInputReader = InputManager.Instance.NullInputReader;
			}
			return mInputReader;
		}


		public void RecieveInputReader(InputReaderAbstract mInputReaderNew){
			mInputReader = mInputReaderNew;
		}


		public void ClearInputReader(){
			mInputReader = InputManager.Instance.NullInputReader;
		}


		public void DeactivateInputReader(){
			mInputReader.IsActive = false;
		}


		public void ActivateInputReader(){
			mInputReader.IsActive = true;
		}

		// Functions 

		// Godot overrides


		public override void _Ready()
		{
			if (Engine.IsEditorHint()){
				return;
			}

			mLogObject = LogManager.Instance.RequestLog("FSM", mShouldLog);
			mLogObject.Assert(StateManagerPointerResource != null, "Had a StateMachineActor without a StateManagerPointer. You forgot to set a reference!");
			
			mStateManager = StateManagerPointerResource.GiveStateManagerInstance();
			mLogObject.Print("Intiliazing FSM of: "  + this.Name + " with state manager " + mStateManager.GetType()); 
			
			if (mRequestInputChannel>0){
				RecieveInputReader(InputManager.Instance.GiveInputByPlayerChannel(this, mRequestInputChannel));
			}
			if (mInputReader == null){
				ClearInputReader();
			}

			mStateManager.InitializeStates(this, mMemoryBlackboard);
			mActualState = mStateManager.GiveInitialState(mLogObject); 
			
			mLogObject.Print("Starting FSM of " + this.Name + " with state " + mActualState.GetType());
		}


		public override void _Process(double delta)
		{
			if (Engine.IsEditorHint()){
				return;
			}

			mActualState = mActualState.ExecuteProcess(delta, mLogObject); 
			mActualState = mActualState.ExecuteQueuedDelegatedEvent(mLogObject);
		}


		public override void _PhysicsProcess(double delta)
		{
			if (Engine.IsEditorHint()){
				return;
			}
			
			mActualState = mActualState.ExecutePhysicsProcess(delta, mLogObject); 
		}

		// Interface methods

		public StateManagerAbstract GiveStateManager() //This will be useful if I ever want to do a visualisation tool. I can just request the StateManager and get information from that.
		{
			return mStateManager;
		}


		public StateAbstract GiveActualState(){
			return mActualState;
		}

		public void InjectVariables(Godot.Collections.Dictionary payload){
			mMemoryBlackboard.Merge(payload);
		}


		//THIS SHOULD ONLY BE USED WHEN SPAWNING ACTORS TO FORCE A THE INITIAL STATE FOR RESET
		public void DoReset(){
			mActualState = mStateManager.GiveInitialState();
			mStateManager.DoReset();
		}
	}
}
