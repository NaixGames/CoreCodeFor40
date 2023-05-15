using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	public partial class GameObjectPooler3D : GameObjectPooler
	{
		// ----------------------------------- Information ------------------------------------------------
		/*Actual instantiable class of GameObjectPooler to use in 2D scenes. */

		// -----------------------------------------------------------------------------



		public Node3D InstantiateGameObjectIn3D(string tag, Vector3 Position, Vector3 Rotation){
			Node3D gameObject = (Node3D)GiveObject(tag);
			gameObject.Position = Position;
			gameObject.Rotation = Rotation;
			return gameObject;

		}

		public Node3D InstantiateGameObjectIn3D(IPoolableObject ObjectToGive,Vector3 Position, Vector3 Rotation){
			return InstantiateGameObjectIn3D(ObjectToGive.TagObject, Position, Rotation);
		}
		

	}
}