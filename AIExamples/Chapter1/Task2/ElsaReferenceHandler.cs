using Godot;
using System;
using CoreCode.Scripts;

namespace CoreCode.AIExamples.Elsa{
	public partial class ElsaReferenceHandler : GameActorReferenceHandler2D
	{
		[Signal] public delegate void FoodIsReadyEventHandler();

		private void ReceiveDelegateEvent(string eventName){
			StateMachine.GiveActualState().QueueDelegatedEvent(eventName);
		}

	}
}