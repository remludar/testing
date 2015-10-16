#version 330
in vec4 position;
in vec4 color;
out vec4 Color;

void main()
{
	gl_Position = position;
	Color = color;
}