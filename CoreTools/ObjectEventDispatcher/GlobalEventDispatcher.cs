using Godot;
using System;

namespace CoreCode.Scripts{
	public partial class GlobalEventDispatcher : Node
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to have a Global Event dispatcher. This is a way of dispatching information of events,
		without having spagetti code all over the place. */

		/* Communication between different component a single entities should be manage by ethier direct reference
		or by using Godot signals*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/*  Add new signals in this code to have game objects all accross the game to comunicate. This can,
		for example, be when the player takes damage, and we want to update a player data file and a UI.

		This should NOT be used for handlings signals between different entities of a single scene. (See above).

		As some tips for triggering signals, recall you can add other methods in setters, so it can communicate to the
		GlobalEventDispatcher. Also recall other classes connect to the GlobelEventDispatcher by doing, 
		GlobalEventDispatcher.Instance.Connect("MySignal", new Callable(this, nameof(this.MethodToCall)));
		Could also use the inspector, but the idea of this method making this a singleton is to avoid that.

		Recall to add signals they need to end with "EventHandler"
		*/


		// ------------------------------------- Singleton instantiation -------------------------------

		[Export] private static GlobalEventDispatcher instance;
		public static GlobalEventDispatcher Instance{
			get {return TryToReturnInstance();}
		}

		public static GlobalEventDispatcher TryToReturnInstance(){
			if (instance == null){
				GD.PushWarning("Instance of Global Event Dispatcher called before the instance was ready! This will create undesired behaviour.");
				instance = new GlobalEventDispatcher();
			}
			return instance;
		}

		public GlobalEventDispatcher(){
			instance = this;
		}

		// ------------------------------------- Signals ------------------------------
		

		[Signal] public delegate void MySignalEventHandler();

		[Signal] public delegate void WithParametersSignalEventHandler(string parameter);

		[Signal] public delegate void ObjectWasPooledSignalEventHandler(string objectsName);


		//---------------------Variables for loging
		
		[Export]
		private bool mShouldLog;

		private ILogObject mLogObject;

		// -----------------------------------------------------------------------------


		// ------------------------- Initialization

		public override void _Ready()
		{
			mLogObject = LogManager.Instance.RequestLog("GlobalEventDispatcher", mShouldLog); //This should be use to send logs in certain events if needed.
		}
	}
}