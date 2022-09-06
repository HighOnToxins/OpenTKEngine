#version 330 core

layout (location = 0) in vec2 vPos;
layout (location = 1) in vec2 vTexPos;
layout (location = 2) in float vTexNum;
	
out vec2 fTexPos;
flat out float fTexNum;

void main(){
	gl_Position = vec4(vPos, 0, 1);
	fTexPos = vTexPos;
	fTexNum = vTexNum;
}