#version 330 core

//Mesh variables Matrix
in vec3 vMeshPos;
in vec4 vMeshColor;

//Instance Matrix
in vec4 vInstMatrix0;
in vec4 vInstMatrix1;
in vec4 vInstMatrix2;
in vec4 vInstMatrix3;

//Camera Uniforms
uniform mat4 uView;
uniform mat4 uProj;

//Coloring for Fragment
out vec4 fColor;

void main(){
	mat4 inst = mat4(vInstMatrix0, vInstMatrix1, vInstMatrix2, vInstMatrix3);
	gl_Position = uView * uProj * inst * vec4(vPos, 1);
	fColor = vColor;
}