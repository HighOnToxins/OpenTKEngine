#version 330 core

in vec3 pos;
in vec2 vTexPos;
in mat4 model;
in int vTexNum;

uniform mat4 view;
uniform mat4 proj;

out vec2 fTexPos;
flat out int fTexNum;

void main(void)
{
    gl_Position = proj * view * model * vec4(pos, 1.0);
    fTexPos = vTexPos;
    fTexNum = vTexNum;
}