using Godot;
using System;

public partial interface IPoolableObject
{
	// ----------------------------------- Information ------------------------------------------------
	/*This is a script to allow an implimentation for object to be administrated by the game object poooler. 
	This was originally a based class, but now it was moved to be an interface to allow more flexibility with diferent type of nodes.*/
	
	// ------------------------------------ Use -------------------------------------------------------
	/*  Each objet that will be initialized or destroyed at runtime should be an IPoolableObject, and through the pooler for this.
	To intialize, call the corresponding method of the GameObjectPooler. To destroy use ReturnToPool().
	Each implementation should adapt those for those need.
	*/
	public string TagObject{
		get;
	}

	public bool IsObjectActive{
		get;
	}

	public bool HasPoolReference{
		get;
		set;
	}

	public void ReturnToPool();

	public void ActivatePooledObject();

	public void AddReferenceInPool();
}
