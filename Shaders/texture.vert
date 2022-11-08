﻿#version 330 core

//Mesh variables Matrix
in vec3 vMeshPos;
in vec2 vMeshTexPos;
in int vMeshTexNum;

//Instance Matrix
in vec4 vInstMatrix0;
in vec4 vInstMatrix1;
in vec4 vInstMatrix2;
in vec4 vInstMatrix3;

//Camera Uniforms
uniform mat4 uView;
uniform mat4 uProj;

//Texturing for Fragment
out vec2 fTexPos;
flat out int fTexNum;

void main(){
	inst = mat4(vInstMatrix0, vInstMatrix1, vInstMatrix2, vInstMatrix3);
	gl_Position = uView * uProj * inst * vec4(vPos, 1);
	fTexPos = vTexPos;
	fTexNum = vTexNum;
}

