shader_type canvas_item;

uniform vec2 TiledVector = vec2(10.0,5.0);
uniform float AspectRatio = 0.5f;

//Params for base offset
uniform vec2 BaseWaveAmplitude= vec2(0.1f,0.2f);
uniform vec2 BaseWaveSpeed = vec2(1.0f,2.0f);
uniform vec2 BaseWaveOffset = vec2(1.0f,2.0);

//Parameters for texture offset
uniform sampler2D UVOffsetTexture : hint_default_black;
uniform vec2 UVOffsetScale = vec2(1.0f,1.0f);
uniform vec2 TextureWaveSize = vec2(0.1f,0.1f);
uniform float TextureOffsetTimeScale = 0.05f;

vec2 CalculateWaveOffset(vec2 TiledUV, float timeValue){
	vec2 offset;
	offset.x = cos(timeValue*BaseWaveSpeed.x+BaseWaveOffset.x*(TiledUV.x+TiledUV.y));
	offset.y = sin(timeValue*BaseWaveSpeed.y+BaseWaveOffset.y*(TiledUV.x+TiledUV.y));
	return BaseWaveAmplitude*offset;
}

vec2 CalculateTextureOffset(vec2 uv){
	vec2 offsetuv = uv*UVOffsetScale;
	offsetuv += vec2(TextureOffsetTimeScale*cos(TIME), TextureOffsetTimeScale*sin(TIME));
	vec2 offset = texture (UVOffsetTexture, offsetuv).rg;
	offset = (2.0f)*offset-1.0f;
	return offset*TextureWaveSize;
}

void fragment() {
	// Place fragment code here.
	//Base offset code
	vec2 TiledUV = UV*TiledVector;
	TiledUV.y*=AspectRatio;
	
	vec2 BaseWaveUVOffset = CalculateWaveOffset(TiledUV, TIME);
	
	//Texture sample offset
	vec2 TextureBasedOffset = CalculateTextureOffset(UV);
	
	COLOR = texture( TEXTURE , TiledUV + BaseWaveUVOffset+TextureBasedOffset);
	NORMAL_MAP = texture(NORMAL_TEXTURE, UV+(TextureBasedOffset+BaseWaveUVOffset)*(0.4f)).rgb;
}

