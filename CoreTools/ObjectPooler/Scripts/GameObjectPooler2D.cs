using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	public partial class GameObjectPooler2D : GameObjectPooler
	{
		// ----------------------------------- Information ------------------------------------------------
		/*Actual instantiable class of GameObjectPooler to use in 2D scenes. */

		// -----------------------------------------------------------------------------



		public Node2D InstantiateGameObjectIn2D(string tag, Vector2 Position, float Rotation=0f, Node Parent = null){
			Node2D gameObject = (Node2D)GiveObject(tag, Parent);
			gameObject.GlobalPosition = Position;
			gameObject.GlobalRotation = Rotation;
			(gameObject as IPoolableObject).ActivatePooledObject();
			return gameObject;

		}

		public Node2D InstantiateGameObjectIn2D(IPoolableObject ObjectToGive, Vector2 Position, float Rotation = 0f, Node Parent = null){
			return InstantiateGameObjectIn2D(ObjectToGive.TagObject, Position, Rotation, Parent);
		}
		

	}
}