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



		public Node2D InstantiateGameObjectIn2D(string tag, Vector2 Position, float Rotation=0f){
			Node2D TwoDimObject = (Node2D)GiveObject(tag);
			TwoDimObject.Position = Position;
			TwoDimObject.Rotation = Rotation;
			return TwoDimObject;

		}

		public Node2D InstantiateGameObjectIn2D(IPoolableObject ObjectToGive, Vector2 Position, float Rotation = 0f){
			return InstantiateGameObjectIn2D(ObjectToGive.TagObject, Position, Rotation);
		}
		

	}
}