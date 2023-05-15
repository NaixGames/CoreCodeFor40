using Godot;
using System;
using CoreCode.FSM;

namespace CoreCode.Scripts{
	public partial class GameActorReferenceHandler3D : CharacterBody3D, IPoolableObject
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to reference nodes in actors more easily in Godot.*/
		
		// ------------------------------------ Use -------------------------------------------------------
		/*The class have a Tag to identify Actors if needed.  Is also give methods to be able to get reference to other classes and nodes
		more easily.
		
		This script should be on the node that is detected by collision with the actor. (so a the CharacterBody. In this case 2D.)

		Will need to expand as needed.
		*/

		// ------------------------------------ Variables ------------------------------------------------
		[Export] private string mTagObject = "";
		public string TagObject{
			get{return mTagObject;}
		}

		public bool IsObjectActive{
			get{return mIsObjectActive;}
		}
		[Export] private bool mIsObjectActive = true; //For default I assumed elements will be active. When initiliazed in the pool objects should have this turn to false.
		// Called when the node enters the scene tree for the first time.
		
		private bool mHasPoolReference = false;

		public bool HasPoolReference{
			get{return mHasPoolReference;}
			set{mHasPoolReference = value;}
		} 

		[Export] private StateMachineActor mStateMachine;
		public IStateMachine StateMachine{
			get{return mStateMachine;} 
		}

		
		//------------------------------------Methods

		public void ReturnToPool(){
			if (HasPoolReference==false){
				AddReferenceInPool();	
			}
			mIsObjectActive = false; 
			this.SetProcess(false);
			this.SetPhysicsProcess(false);
			//INFORM OBJECT EVENTS DISPATCHER IN OTHER OBJECTS
		}

		public void ActivatePooledObject(){
			mIsObjectActive = true;
			this.SetProcess(GetTree().Paused); //Put the pause mode to whatever is happening in scene. Useful if we want to spawn object in paused mode.
			this.SetPhysicsProcess(GetTree().Paused);
			///INFORM OBJECT EVENTS DISPATCHER IN OTHER OBJECTS
		}

		public void AddReferenceInPool(){
			GameObjectPooler.Instance.AddObjectReferenceToPool(this);
			HasPoolReference = true;
		}
	}
}