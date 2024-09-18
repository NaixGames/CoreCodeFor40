using Godot;
using System;

namespace CoreCode.Scripts{
	public partial class GlobalEventDispatcher : SingleNode<GlobalEventDispatcher>
	{
		// Information
		/*This is a script to have a Global Event dispatcher. This is a way of dispatching information of events,
		without having spagetti code all over the place. */

		/* Communication between different component a single entities should be manage by ethier direct reference
		or by using Godot signals*/
		
		// Use
		/*  Add new signals in this code to have game objects all accross the game to comunicate. This can,
		for example, be when the player takes damage, and we want to update a player data file and a UI.

		This should NOT be used for handlings signals between different entities of a single scene. (See above).

		As some tips for triggering signals, recall you can add other methods in setters, so it can communicate to the
		GlobalEventDispatcher. Also recall other classes connect to the GlobelEventDispatcher by doing, 
		GlobalEventDispatcher.Instance.Connect("MySignal", new Callable(this, nameof(this.MethodToCall)));
		Could also use the inspector, but the idea of this method making this a singleton is to avoid that.

		Recall to add signals they need to end with "EventHandler"
		*/


		// Signals
		

		[Signal] public delegate void MySignalEventHandler();

	}
}