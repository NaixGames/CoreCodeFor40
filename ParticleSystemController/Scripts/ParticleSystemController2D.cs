using Godot;
using System;
using System.Collections.Generic;
using CoreCode.Scripts;

namespace CoreCode.Example{
	public partial class ParticleSystemController2D : Node
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is an example on how to use the PoolableParticleSystem2D class and take adventage of it.
		
		// ------------------------------------ Use -------------------------------------------------------
		/*  Pretty explinatory from the Process method
		*/

		// ------------------------------------ Variables -------------------------------------------------------
		[Export] private Vector2 LocationOne;
		[Export] private Vector2 LocationTwo;
		[Export] private Vector2 LocationThree;
		[Export] private InputReaderAbstract mInputReference;
		private Stack<PoolableParticleSystem2D> mParticleReferences = new Stack<PoolableParticleSystem2D>();

		private GameObjectPooler2D gameObjectPooler;


		// ------------------------------------ Methods -------------------------------------------------------

		public void EmitParticleSystemAtLocation(string tag, Vector2 newPosition, float Rotation=0){
			Node2D mParticleObject = gameObjectPooler.InstantiateGameObjectIn2D(tag, newPosition, Rotation);
			mParticleReferences.Push((PoolableParticleSystem2D)mParticleObject);
		}

		public void StopParticleSystem(PoolableParticleSystem2D particles){
			particles.ReturnToPool();
		}

		public override void _Ready(){
			gameObjectPooler = (GameObjectPooler.Instance as GameObjectPooler2D);
		}

		public override void _Process(double delta)
		{
			if (mInputReference.IsButtonJustPressedInput("Up")){
				EmitParticleSystemAtLocation("PSExample1",LocationOne);
			}
			if (mInputReference.IsButtonJustPressedInput("Left")){
				EmitParticleSystemAtLocation("PSExample2",LocationTwo);
			}
			if (mInputReference.IsButtonJustPressedInput("Right")){
				EmitParticleSystemAtLocation("PSExample3",LocationThree);
			}
			if (mInputReference.IsButtonJustPressedInput("Down")){
				while (true){
					if (mParticleReferences.Count==0){
						return;
					}
					PoolableParticleSystem2D PSObject = mParticleReferences.Pop();
					if (PSObject.ParticleSystemReference.Emitting==true){
						StopParticleSystem(PSObject);
					}
				}
			}
		}
	}
}