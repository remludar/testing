#version 330

in vec4 position;
in vec4 color;
in vec2 texture;

out vec4 fragColor;
out vec2 fragTexture;

void main()
{
	gl_Position = position;
	fragColor = color;
	fragTexture = texture;
}