#version 330 core

in vec3 pos;
in mat4 model;
in vec4 vColor;

uniform mat4 view;
uniform mat4 proj;

out vec4 fColor;

void main(void)
{
    gl_Position = proj * view * model * vec4(pos, 1.0);
    fColor = vColor;
}
