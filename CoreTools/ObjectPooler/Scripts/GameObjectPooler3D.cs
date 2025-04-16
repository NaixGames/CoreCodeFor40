using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreCode.Scripts{
	public partial class GameObjectPooler3D : GameObjectPooler
	{
		// Information
		/*Actual instantiable class of GameObjectPooler to use in 3D scenes. */
		
		public static Node3D FetchOrInstanceObjectIn3D(PoolableObjectReference objectRef, Vector3 Position, Vector3 Rotation, Node Parent = null)
		{
			GameObjectPooler3D gameObjectPooler = GameObjectPooler.Instance as GameObjectPooler3D;
	
			if (gameObjectPooler != null && gameObjectPooler.IsInsideTree()){
				Node3D fetchNode = gameObjectPooler.InstantiateGameObjectIn3D(objectRef.Tag, Position, Rotation, Parent);
				(fetchNode as IPoolableObject).ActivatePooledObject();
				return fetchNode;
			}
			else{
				GD.PushWarning("Trying to fetch object " + objectRef.Tag + ", but pooler is not registered. ");
				Node3D instancedNode = objectRef.Object.Instantiate() as Node3D;
				instancedNode.GlobalPosition = Position;
				instancedNode.GlobalRotation = Rotation;
				Parent.AddChild(instancedNode);
				(instancedNode as IPoolableObject).ActivatePooledObject();
				return instancedNode;
			}
		}


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