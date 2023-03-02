using Godot;
using System;

//Should change Node2D to Node3D if doing a new game. This is anoying but seems a constrain from godot, as casting from Node to Node2D does not work well :()
public partial class PoolableObject : Node2D, IPoolableObject
{
	// ----------------------------------- Information ------------------------------------------------
	/*This is a script to allow nodes to be administrated by the game object poooler. This was originally a based class, but now it is an implementation of the IPoolableObject interface*/
	
	// ------------------------------------ Use -------------------------------------------------------
	/*  Each object that will be initialized or destroyed at runtime should be an IPoolableObject, and through the pooler for this.
	To intialize, call the corresponding method of the GameObjectPooler. To destroy use ReturnToPool().
	Each implementation should adapt those for those need.
	*/

	// ------------------------------------Variables

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
