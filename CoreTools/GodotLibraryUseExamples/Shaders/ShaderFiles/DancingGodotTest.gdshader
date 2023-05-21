shader_type canvas_item;

uniform vec2 timeFactor = vec2(1.0f, 2.0f);
uniform vec2 amplitude = vec2(15.0f, 25.0f);

void vertex() {
	VERTEX.x +=(amplitude.x)*cos(TIME*timeFactor.x+VERTEX.x+VERTEX.y);
	VERTEX.y +=(amplitude.y)*sin(TIME*timeFactor.y+VERTEX.y+VERTEX.x);
}
