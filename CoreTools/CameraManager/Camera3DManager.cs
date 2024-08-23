using Godot;
using System;
using System.Collections.Generic;


namespace CoreCode.Scripts{
	public partial class Camera3DManager : Node
	{
		// Information
		/* This is a Manager to easily get references to cameras. In case no camera is registered,
		it looks for the the default viewport. 
		Note this is a simple class right now, but we need to expand this if we want to have
		more uses with multiple cameras or camera blends. */
		
		// Use 
		/*  This has a list of Camera Instances which are inserted in a priority order. The
		highest priority order one should be the active one. */

		// Singleton instantiation 
		private static Camera3DManager instance;

		public static Camera3DManager Instance{
			get {return TryToReturnInstance();}
		}

		public static Camera3DManager TryToReturnInstance(){
			if (Engine.IsEditorHint()){
				return new Camera3DManager();
			}
			if (instance == null){
				GD.PushWarning("Instance of CameraManager called before the instance was ready!");
				instance = new Camera3DManager();
			}
			return instance;
		}


		public Camera3DManager(){
			if (Engine.IsEditorHint()){
				return;
			}
			if (instance==null){
				instance=this; 
			}
			else{
				GD.PushWarning("Instance of CameraManager created when there is an existing instance!");
			}
		}


		// Variables

		List<Camera3DInstance> mCameraInstances = new List<Camera3DInstance>(1);


		// Variable for logging

		[Export] protected bool mShouldLog;

		protected ILogObject mLogObject; 


		// Methods 

		public Camera3DInstance GiveHighPriorityCamera(){
			return mCameraInstances[0];
		}


		public void RegisterCameraInstance(Camera3DInstance instance){
			for (int i = 0; i < mCameraInstances.Count; i++){
				if (mCameraInstances[i] == null || instance.CameraPriority > mCameraInstances[i].CameraPriority){
					mCameraInstances.Insert(i, instance);
					return;
				}
			}
			mCameraInstances.Add(instance);
		}


		public Camera3D GiveMainCamera(){
			if (mCameraInstances.Count > 0 && mCameraInstances[0] != null){
				return mCameraInstances[0];
			}
			return GetViewport().GetCamera3D();
		}


	}
}
