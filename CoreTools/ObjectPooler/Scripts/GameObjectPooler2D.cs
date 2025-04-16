using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	public partial class GameObjectPooler2D : GameObjectPooler
	{
		// Information 
		/*Actual instantiable class of GameObjectPooler to use in 2D scenes. */


		public static Node2D FetchOrInstanceObjectIn3D(PoolableObjectReference objectRef, Vector2 Position, float Rotation, Node Parent = null)
		{
			GameObjectPooler2D gameObjectPooler = GameObjectPooler.Instance as GameObjectPooler2D;
	
			if (gameObjectPooler != null && gameObjectPooler.IsInsideTree()){
				Node2D fetchNode = gameObjectPooler.InstantiateGameObjectIn2D(objectRef.Tag, Position, Rotation, Parent);
				(fetchNode as IPoolableObject).ActivatePooledObject();
				return fetchNode;
			}
			else{
				GD.PushWarning("Trying to fetch object " + objectRef.Tag + ", but pooler is not registered. ");
				Node2D instancedNode = objectRef.Object.Instantiate() as Node2D;
				instancedNode.GlobalPosition = Position;
				instancedNode.GlobalRotation = Rotation;
				Parent.AddChild(instancedNode);
				(instancedNode as IPoolableObject).ActivatePooledObject();
				return instancedNode;
			}
		}
		

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