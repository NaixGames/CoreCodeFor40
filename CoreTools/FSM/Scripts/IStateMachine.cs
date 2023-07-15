using Godot;
using System;

namespace CoreCode.FSM{
	public partial interface IStateMachine
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to give a uniform interface for state machines in Godot. Note we do this an interface 
		to be able to do Input for AI that are also state machine (so they inherit from inputs, but have a State machine
		interface)
		
		// ------------------------------------ Use -------------------------------------------------------

		/* Implement the methods below in your class an "inherit" from the IStateMachine interface.*/

		// ------------------------------------ Interface methods ------------------------------------------------

		public StateManagerAbstract GiveStateManager(); //This will be useful if I ever want to do a visualisation tool. I can just request the StateManager and get information from that.

		public StateAbstract GiveActualState();
		
	}
}

