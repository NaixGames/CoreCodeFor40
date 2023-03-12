shader_type canvas_item;

uniform vec2 TiledVector = vec2(10.0,5.0);
uniform float AspectRatio = 0.5f;

uniform sampler2D UVOffsetTexture : hint_default_black;
uniform vec2 UVOffsetScale = vec2(1.0f,1.0f);
uniform vec2 WaveSize = vec2(0.1f,0.1f);

uniform float TimeScale = 0.05f;

void fragment() {
	//Do a sample texture defformation for sampling waves
	vec2 OffsetTextureUV = UV*UVOffsetScale; 
	OffsetTextureUV += vec2(TimeScale*cos(TIME), TimeScale*sin(TIME));
	vec2 TextureBasedOffset = texture (UVOffsetTexture, OffsetTextureUV).rg;
	TextureBasedOffset = (2.0f)*TextureBasedOffset-1.0f;
	// Place fragment code here.
	vec2 TiledUV = UV*TiledVector;
	TiledUV.y*=AspectRatio;
	//COLOR = vec4(TiledUV,0.0f,1.0f);
	COLOR = texture( TEXTURE , TiledUV + TextureBasedOffset*WaveSize);
	//COLOR = texture (UVOffsetTexture, OffsetTextureUV);
}
