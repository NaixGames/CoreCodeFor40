using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.FSM{
	public abstract partial class StateAbstract : Node
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to create States for a FSM. This gives the abstract interface that the FSM uses.*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/* The FSM delegates execution of its Process method to ExecuteProcess and its PhysicsProcess to ExecutePhysicsProcess.*/

		/*As optional Virtual functions we got InitializeState and FinalizeState.This are used to Start and exit a state, respectively*/

		/*ExecuteProcess executes a ProcessAction, which can create a change of state. If that is the case it Finalizes the Actual state,
		initialize the new one, and print the change of state. ExecutePhysicsProcess works similarly. */

		// ------------------------------------- Variable
		protected StateManagerAbstract mStateManagerCache;
		protected Godot.Collections.Dictionary mMemoryBlackboardCache;

		// ------------------------------------- Abstract methods ---------------------------------------
		
		protected abstract StateAbstract ProcessAction(double delta, LogObject mlogObject=null);

		protected abstract StateAbstract ProcessPhysicsAction(double delta, LogObject mlogObject=null);

		protected abstract void InitializeStateParams(Node mNodeReference);
		

		// ------------------------------------- Virtual functions ---------------------------------------

		protected virtual void EnterState(){
			return;
		}

		protected virtual void ExitState(){
			return;
		}

		//This is the method for processing external events in state machines. By default, we ignore it by staying in the same state.
		protected virtual StateAbstract ProcessDelegatedEvent(string EventName, LogObject mlogObject=null){
			return this;
		}

		// ------------------------------------- Functions ---------------------------------------

		public StateAbstract ExecuteProcess(double delta, LogObject mLogObject=null){
			StateAbstract stateActionResult = ProcessAction(delta, mLogObject);
			if (stateActionResult==this){
				return stateActionResult;
			}
			this.ExitState();
			stateActionResult.EnterState();
			if (mLogObject!=null){
				mLogObject.AddToLogString("Change of state in Process from state "  + this.GetType() + " to state " + stateActionResult.GetType()); 
			}
			return stateActionResult;
		}

		public StateAbstract ExecutePhysicsProcess(double delta, LogObject mLogObject=null){
			StateAbstract stateActionResult = ProcessPhysicsAction(delta, mLogObject);
			if (stateActionResult==this){
				return stateActionResult;
			}
			this.ExitState();
			stateActionResult.EnterState();
			if (mLogObject!=null){
				mLogObject.AddToLogString("Change of state in Physics Process from state "  + this.GetType() + " to state " + stateActionResult.GetType());
			}
			return stateActionResult;
		}

		public StateAbstract ExecuteDelegatedEvent(string EventName,LogObject mLogObject=null){
			StateAbstract stateEventResult = ProcessDelegatedEvent(EventName, mLogObject);
			if (stateEventResult==this){
				return stateEventResult;
			}
			this.ExitState();
			stateEventResult.EnterState();
			if (mLogObject!=null){
				mLogObject.AddToLogString("Change of state from event " + EventName + " from state "  + this.GetType() + " to state " + stateEventResult.GetType());
			}
			return stateEventResult;
		}

		public void InitializeState(Node mNodeReference, StateManagerAbstract stateManager, Godot.Collections.Dictionary mMemoryBlackboard = null){
			mStateManagerCache = stateManager;
			mMemoryBlackboardCache = mMemoryBlackboard;
			InitializeStateParams(mNodeReference);
		}	
	}
}