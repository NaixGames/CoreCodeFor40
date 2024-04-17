using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.AIExamples.Bob{
	public partial class BobReferenceHandler : GameActorReferenceHandler2D
	{
		[Signal] public delegate void BobIsHomeEventHandler();

		private void ReceiveDelegateEvent(string eventName){
			StateMachine.GiveActualState().QueueDelegatedEvent(eventName);
		}

	}
}
