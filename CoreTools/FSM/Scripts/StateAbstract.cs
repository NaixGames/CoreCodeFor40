using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.FSM{
	public abstract partial class StateAbstract
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
	
		//This is to execute Payload/message reception post next update
		public string EventQueued="";

		// ------------------------------------- Abstract methods ---------------------------------------
		
		protected abstract StateAbstract ProcessAction(double delta, ILogObject mlogObject=null);

		protected abstract StateAbstract ProcessPhysicsAction(double delta, ILogObject mlogObject=null);

		protected abstract void InitializeStateParams(Node mNodeReference);
		

		// ------------------------------------- Virtual functions ---------------------------------------

		protected virtual void EnterState(){
			return;
		}

		protected virtual void ExitState(){
			return;
		}

		//This is the method that queues some event to later execution. Any needed information should be cached here
		//Note this will only queue ONE event per frame execution! (To be fair, if we are queing more than one most likely
		//we are doing something wrong. Worst case can put a queue of event names and run all of them on each frame. Or just override EventQueued).
		public void QueueDelegatedEvent(string EventName, Godot.Collections.Dictionary PayloadDictionary = null, ILogObject mlogObject=null){
			EventQueued = EventName;
			mMemoryBlackboardCache["Payload"]= PayloadDictionary;
		}
	

		//This is what actually gets called on the execution cycle of the event. This should NOT be called from other states, 
		//as it is executed after the Update cycle to avoid a state eating its own signals. Note that, for default,
		//we dont do anything and return.
		protected virtual StateAbstract OnUnqueuedDelegatedEvent(string EventName, ILogObject logObject=null){
			return this;
		}

		// ------------------------------------- Functions ---------------------------------------

		public StateAbstract ExecuteProcess(double delta, ILogObject mLogObject=null){
			StateAbstract stateActionResult = ProcessAction(delta, mLogObject);
			if (stateActionResult==this){
				return stateActionResult;
			}
			this.ExitState();
			stateActionResult.EnterState();
			if (mLogObject!=null){
				mLogObject.Print("Change of state in Process from state "  + this.GetType() + " to state " + stateActionResult.GetType()); 
			}
			return stateActionResult;
		}

		public StateAbstract ExecutePhysicsProcess(double delta, ILogObject mLogObject=null){
			StateAbstract stateActionResult = ProcessPhysicsAction(delta, mLogObject);
			if (stateActionResult==this){
				return stateActionResult;
			}
			this.ExitState();
			stateActionResult.EnterState();
			if (mLogObject!=null){
				mLogObject.Print("Change of state in Physics Process from state "  + this.GetType() + " to state " + stateActionResult.GetType());
			}
			return stateActionResult;
		}

		public StateAbstract ExecuteQueuedDelegatedEvent(ILogObject mLogObject=null){
			StateAbstract stateEventResult = OnUnqueuedDelegatedEvent(EventQueued, mLogObject);
			if (stateEventResult==this){
				return stateEventResult;
			}
			this.ExitState();
			stateEventResult.EnterState();
			if (mLogObject!=null){
				mLogObject.Print("Change of state from event " + EventQueued + " from state "  + this.GetType() + " to state " + stateEventResult.GetType());
			}
			EventQueued="";
			mMemoryBlackboardCache.Remove("Payload");
			return stateEventResult;
		}

		public void InitializeState(Node mNodeReference, StateManagerAbstract stateManager, Godot.Collections.Dictionary mMemoryBlackboard = null){
			mStateManagerCache = stateManager;
			mMemoryBlackboardCache = mMemoryBlackboard;
			InitializeStateParams(mNodeReference);
		}	
	}
}