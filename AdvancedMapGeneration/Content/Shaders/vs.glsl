#version 330

uniform mat4 modelView;

layout(location=0)in vec4 position;
layout(location=1)in vec4 color;
layout(location=2)in vec3 vertTexCoord;

out vec4 fragColor;
out vec3 fragTexCoord;

void main()
{
	gl_Position = modelView * position;
	fragColor = color;
	fragTexCoord = vertTexCoord;
}