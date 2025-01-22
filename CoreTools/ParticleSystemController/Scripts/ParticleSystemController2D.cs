using Godot;
using System;
using System.Collections.Generic;
using CoreCode.Scripts;

namespace CoreCode.Example{
	public partial class ParticleSystemController2D : Node, IControlableByInput
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
		private InputReaderAbstract mInputReference;
		private Stack<PoolableParticleSystem2D> mParticleReferences = new Stack<PoolableParticleSystem2D>();
		private GameObjectPooler2D mGameObjectPooler;


		// ------------------------------------ Methods -------------------------------------------------------

		public void EmitParticleSystemAtLocation(Vector2 newPosition, float Rotation=0){
			Node2D mParticleObject = mGameObjectPooler.InstantiateGameObjectIn2D("PSEmitter", newPosition, Rotation, this);
			mParticleReferences.Push((PoolableParticleSystem2D)mParticleObject);
		}

		public void StopParticleSystem(PoolableParticleSystem2D particles){
			GameObjectPooler.Instance.ReturnObjectToPool(particles);
		}

		public override void _Ready(){
			mGameObjectPooler = GameObjectPooler.Instance as GameObjectPooler2D;
			mInputReference = InputManager.Instance.GiveInputByPlayerChannel(this, 1);
		}

		public override void _Process(double delta)
		{
			if (mInputReference.IsButtonJustPressedInput("Up")){
				EmitParticleSystemAtLocation(LocationOne);
			}
			if (mInputReference.IsAxisJustPressedInput("Left")){
				EmitParticleSystemAtLocation(LocationTwo);
			}
			if (mInputReference.IsAxisJustPressedInput("Right")){
				EmitParticleSystemAtLocation(LocationThree);
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


		//IControllableByInput interface

		public InputReaderAbstract ReturnInputReader()
		{
			if (mInputReference == null){
				mInputReference = InputManager.Instance.NullInputReader;
			}
			return mInputReference;
		}
		

		public void ClearInputReader()
		{
			mInputReference = InputManager.Instance.NullInputReader;
		}

		public void RecieveInputReader(InputReaderAbstract inputReaderPath)
		{
			mInputReference = inputReaderPath;
		}

	}
}