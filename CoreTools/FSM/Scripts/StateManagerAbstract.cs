using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.FSM{

	public abstract partial class StateManagerAbstract : Node, IResetable
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to provide an abstract interface for a StateManager for the state machine class.*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/* The State Machine uses this manager as a container of states. This container will be pass to the abstract
		states for execution, where they can ask this manager for new states (ie, transitions will be requested from this class)

		Note that mulitple actors can use the same states to execute their instructions, so they potentially could be static states.
		You might want this to save memory and write easier code. You might not want this to to paralize execution. So prefer static states with a small number of actors,
		for a bigger number of one you want to be more careful with the manager (ie, having different instances of the same manager, and manage requested for states directly).*/

		
		// ------------------------------------ Abstract Functions ------------------------------------------------	

		public abstract StateAbstract GiveInitialState(ILogObject mLogObject=null);

		public abstract void InitializeStates(Node mFinisteStatemachineNode, Godot.Collections.Dictionary mMemoryBlackboard, ILogObject mLogObject=null);

		public virtual void DoReset(){
			return;
		}

	}
}