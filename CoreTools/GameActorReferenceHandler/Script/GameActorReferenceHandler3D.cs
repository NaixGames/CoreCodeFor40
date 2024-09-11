using Godot;
using System;
using CoreCode.FSM;

namespace CoreCode.Scripts{
	[Tool]
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
		[Export] public string mTagObject = "";
		public string TagObject{
			get{return mTagObject;}
			set{mTagObject = value;}
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

		[Export] public StateMachineActor StateMachine;

		[Export] private Godot.Collections.Dictionary mMemoryBlackboardInject = new Godot.Collections.Dictionary();


        //------------------------------------Methods


        public override void _EnterTree()
        {
            base._EnterTree();
			StateMachine.InjectVariables(mMemoryBlackboardInject);
        }

        //THIS SHOULD NEVER BE CALLED MANUALLY. USE GAME OBJECT POOLER TO POOL OBJECTS!
        public void ReturnToPool(){
			if (HasPoolReference==false){
				AddReferenceInPool();	
			}
			mIsObjectActive = false; 
			EmitSignal(nameof(ReturnedToPool));
			Position=Vector3.Zero;
			Rotation=Vector3.Zero;
			this.ProcessMode=ProcessModeEnum.Disabled;
		}

		public void ActivatePooledObject(){
			mIsObjectActive = true;
			this.ProcessMode=ProcessModeEnum.Inherit;
			EmitSignal(nameof(SpawnedFromPool));
		}

		public void AddReferenceInPool(){
			GameObjectPooler.Instance.AddObjectReferenceToPool(this);
			HasPoolReference = true;
		}

		//---------- Signals

		[Signal] public delegate void ReturnedToPoolEventHandler();
		[Signal] public delegate void SpawnedFromPoolEventHandler(); 
	}
}