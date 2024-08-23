using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	public partial class GameObjectPooler3D : GameObjectPooler
	{
		// ----------------------------------- Information ------------------------------------------------
		/*Actual instantiable class of GameObjectPooler to use in 3D scenes. */

		// -----------------------------------------------------------------------------



		public Node3D InstantiateGameObjectIn3D(string tag, Vector3 Position, Vector3 Rotation, Node Parent = null){
			Node3D gameObject = (Node3D)GiveObject(tag, Parent);
			gameObject.GlobalPosition = Position;
			gameObject.GlobalRotation = Rotation;
			(gameObject as IPoolableObject).ActivatePooledObject();
			return gameObject;
		}

		public Node3D InstantiateGameObjectIn3D(IPoolableObject ObjectToGive,Vector3 Position, Vector3 Rotation, Node Parent = null){
			return InstantiateGameObjectIn3D(ObjectToGive.TagObject, Position, Rotation, Parent);
		}
		

	}
}