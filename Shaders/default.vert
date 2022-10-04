#version 330 core

layout (location = 0) in vec3 vPos;
layout (location = 1) in vec4 vColor;
	
uniform mat4 uScr;

out vec4 fColor;

void main(){
	gl_Position = vec4(vPos, 1) * uScr;
	fColor = vColor;
}