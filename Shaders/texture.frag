#version 330 core

//Inputs from Vertex
in vec2 fTexPos;
flat in int fTexNum;

//All current Textures
uniform sampler2D[2] uTexs;

out vec4 Color;

void main(){
	Color = texture(uTexs[fTexNum], fTexPos);
}
