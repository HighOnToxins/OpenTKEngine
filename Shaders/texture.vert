#version 330 core

in vec3 vMeshPos;
in vec2 vMeshTexPos;
in int vMeshTexNum;

in vec4 vInstMod0;
in vec4 vInstMod1;
in vec4 vInstMod2;
in vec4 vInstMod3;

uniform mat4 uView;
uniform mat4 uProj;

out vec2 fTexPos;
flat out int fTexNum;

void main(){
	mat4 model = mat4(vInstMod0, vInstMod1, vInstMod2, vInstMod3);
	gl_Position = uProj * uView * model * vec4(vMeshPos, 1);
	fTexPos = vTexPos;
	fTexNum = vTexNum;
}

