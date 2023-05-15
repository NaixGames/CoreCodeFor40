using Godot;
using System;
using System.Collections.Generic;
using CoreCode.Scripts;

namespace CoreCode.Example{
	public partial class ParticleSystemController3D : Node
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is an example on how to use the PoolableParticleSystem3D class and take adventage of it.
		
		// ------------------------------------ Use -------------------------------------------------------
		/*  Pretty explinatory from the Process method
		*/

		// ------------------------------------ Variables -------------------------------------------------------
		[Export] private Vector3 LocationOne;
		[Export] private Vector3 LocationTwo;
		[Export] private Vector3 LocationThree;
		[Export] private InputReaderAbstract mInputReference;
		private Stack<PoolableParticleSystem3D> mParticleReferences = new Stack<PoolableParticleSystem3D>();
		private GameObjectPooler3D gameObjectPooler;


		// ------------------------------------ Methods -------------------------------------------------------

		public void EmitParticleSystemAtLocation(string tag, Vector3 newPosition, Vector3 Rotation){
			Node3D mParticleObject = gameObjectPooler.InstantiateGameObjectIn3D(tag, newPosition, Rotation);
			mParticleReferences.Push((PoolableParticleSystem3D)mParticleObject);
		}

		public void StopParticleSystem(PoolableParticleSystem3D particles){
			particles.ReturnToPool();
		}

		public override void _Process(double delta)
		{
			if (mInputReference.IsButtonJustPressedInput("Up")){
				EmitParticleSystemAtLocation("PSExample1",LocationOne, new Vector3(0,0,0));
			}
			if (mInputReference.IsButtonJustPressedInput("Left")){
				EmitParticleSystemAtLocation("PSExample2",LocationTwo, new Vector3(0,0,0));
			}
			if (mInputReference.IsButtonJustPressedInput("Right")){
				EmitParticleSystemAtLocation("PSExample3",LocationThree, new Vector3(0,0,0));
			}
			if (mInputReference.IsButtonJustPressedInput("Down")){
				while (true){
					if (mParticleReferences.Count==0){
						return;
					}
					PoolableParticleSystem3D PSObject = mParticleReferences.Pop();
					if (PSObject.ParticleSystemReference.Emitting==true){
						StopParticleSystem(PSObject);
					}
				}
			}
		}

		public override void _Ready(){
			gameObjectPooler = GameObjectPooler.Instance as GameObjectPooler3D;
		}
	}
}