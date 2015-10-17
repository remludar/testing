#version 330

uniform sampler2D sampler;

in vec4 fragColor;
in vec2 fragTexture;

out vec4 OutputColor;

void main()
{
	OutputColor = texture(sampler, fragTexture);
}