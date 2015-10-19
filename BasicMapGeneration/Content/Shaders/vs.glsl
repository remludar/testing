#version 330

uniform mat4 modelView;

in vec4 position;
in vec4 color;
in vec2 texture;

out vec4 fragColor;
out vec2 fragTexture;

void main()
{
	gl_Position = modelView * position;
	fragColor = color;
	fragTexture = texture;
}