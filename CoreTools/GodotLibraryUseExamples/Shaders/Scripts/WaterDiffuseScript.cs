using Godot;
using System;

namespace CoreCode.Shaders{
	[Tool]
	public partial class WaterDiffuseScript : Sprite2D
	{
		private ShaderMaterial mShaderMaterial;
		private void AdjustAspectRatio(){
			float aspectRatio = Scale.Y/Scale.X;
			mShaderMaterial = Material as ShaderMaterial;
			mShaderMaterial.SetShaderParameter("AspectRatio", aspectRatio);
		}
	}
}

