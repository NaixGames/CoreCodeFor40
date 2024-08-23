using Godot;
using System;

namespace CoreCode.Scripts{
	public partial class Camera3DInstance : Camera3D
	{
		// Information
		/* This is a script for easily manage camera with the camera3dmanager. */
		
		// Use 
		/* Add this to a Camera3D node to register it to the camera manager. */


		[Export] public int CameraPriority;

		[Export] public SubViewportContainer ViewportContainer;
		

		public override void _Ready(){
			Camera3DManager.Instance.RegisterCameraInstance(this);
		}


		public int GetStretchFactor(){
			return ViewportContainer != null? ViewportContainer.StretchShrink : 1;
		}

	}
}
