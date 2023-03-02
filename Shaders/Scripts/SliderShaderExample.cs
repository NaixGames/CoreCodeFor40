using Godot;
using System;

[Tool]
public partial class SliderShaderExample : Sprite2D
{
	private ShaderMaterial mShaderMaterial;


	public override void _Ready(){
		mShaderMaterial = this.Material as ShaderMaterial;
	}
	public void _OnHSliderValueChanged(float newValue){
		if (mShaderMaterial == null){
			mShaderMaterial = Material as ShaderMaterial;
		}
		Vector2 newAmplitude = new Vector2(newValue, newValue);
		mShaderMaterial.SetShaderParameter("amplitude", newAmplitude);
	}
}
