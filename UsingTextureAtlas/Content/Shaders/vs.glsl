#version 330

uniform mat4 modelView;

layout(location=0)in vec4 position;
layout(location=1)in vec4 color;
layout(location=2)in vec2 texture;

out vec4 fragColor;
out vec2 fragTexture;

void main()
{
	gl_Position = modelView * position;
	fragColor = color;
	fragTexture = texture;
}