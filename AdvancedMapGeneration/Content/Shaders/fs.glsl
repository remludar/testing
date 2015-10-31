#version 330

uniform sampler2DArray texSampler;

in vec4 fragColor;
in vec3 fragTexCoord;

out vec4 OutputColor;

void main()
{
	OutputColor = fragColor * texture(texSampler, fragTexCoord);
}