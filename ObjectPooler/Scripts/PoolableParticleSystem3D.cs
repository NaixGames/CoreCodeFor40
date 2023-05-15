using Godot;
using System;

namespace CoreCode.Scripts{
	public partial class PoolableParticleSystem3D : GpuParticles3D, IPoolableObject
	{
		// ----------------------------------- Information ------------------------------------------------
		/*This is a script to control particle system with the object pooler.
		
		// ------------------------------------ Use -------------------------------------------------------
		/*  For more information, see PoolableObject class. The only difference is that this class also takes
		care of the emision of particles.
		*/

		// ------------------------------------Variables

		[Export] private string mTagObject = "";
		public string TagObject{
			get{return mTagObject;}
		}

		public bool IsObjectActive{
			get{return mIsObjectActive;}
		}
		[Export] private bool mIsObjectActive = true; //For default I assumed elements will be active. When initiliazed in the pool objects should have this turn to false.
		// Called when the node enters the scene tree for the first time.
		
		private bool mHasPoolReference = false;

		public bool HasPoolReference{
			get{return mHasPoolReference;}
			set{mHasPoolReference = value;}
		} 

		//------------------------------------Methods

		public void ReturnToPool(){
			if (HasPoolReference==false){
				AddReferenceInPool();	
			}
			Emitting=false;
			mIsObjectActive = false; 
			this.SetProcess(false);
			this.SetPhysicsProcess(false);
		}

		public void ActivatePooledObject(){
			mIsObjectActive = true;
			Emitting=true;
			this.SetProcess(GetTree().Paused); 
			this.SetPhysicsProcess(GetTree().Paused);
		}

		public void AddReferenceInPool(){
			GameObjectPooler.Instance.AddObjectReferenceToPool(this);
			HasPoolReference = true;
		}
		

		public GpuParticles3D ParticleSystemReference{
			get{ return this;}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (this.Emitting==false){
				this.ReturnToPool();
			}
		}
	}
}