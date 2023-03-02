shader_type canvas_item;

uniform vec2 TiledVector = vec2(10.0,5.0);
uniform float AspectRatio = 0.5f;
uniform vec2 WaveAmplitude= vec2(0.1f,0.2f);
uniform vec2 WaveSpeed = vec2(1.0f,2.0f);

void fragment() {
	// Place fragment code here.
	vec2 TiledUV = UV*TiledVector;
	TiledUV.y*=AspectRatio;
	
	vec2 WaveUVOffset;
	WaveUVOffset.x = cos(TIME*WaveSpeed.x+TiledUV.x+TiledUV.y);
	WaveUVOffset.y = sin(TIME*WaveSpeed.y+TiledUV.x+TiledUV.y); 
	//COLOR = vec4(TiledUV,0.0f,1.0f);
	COLOR = texture( TEXTURE , TiledUV + WaveAmplitude*WaveUVOffset);
}
