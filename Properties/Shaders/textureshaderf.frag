#version 330

in vec2 fTexPos;
flat in int fTexNum;

uniform sampler2D tex[16];

out vec4 outputColor;

void main()
{
    outputColor = texture(tex[fTexNum], fTexPos);
}