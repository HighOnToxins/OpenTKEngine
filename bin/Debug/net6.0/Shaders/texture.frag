#version 330 core

in vec2 fTexPos;
flat in float fTexNum;
	
uniform sampler2D[2] uTexes;

out vec4 Color;

void main(){
	int index = int(fTexNum);
	Color = texture(uTexes[index], fTexPos);
}
